using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine
{
    class Background_Ingame
    {
        Rectangle r = new Rectangle(0, 80, 480, 600);

        float _timer = 0;
        string temps_best = (string)IsolatedStorageSettings.ApplicationSettings["high_score"];
        float[] scores;
        int _nombre_carre;

        bool mode_high_score = false;
        float _vitesse = 0.12f;

        public List<Porte_Couleur> liste_porte_blanche = new List<Porte_Couleur>();
        List<Diagonale> liste_diagonale = new List<Diagonale>();
        GameScreen screen;
        Color color_bordure = Color.White;
        Color color_bordure_base = Color.White;

        Color color_carre_fond = Color.Black;
        Color color_carre_fond_restaure = Color.Black;

        int R_value = 0;
        int G_value = 0;
        int B_value = 0;
        int mode = 0;

        Rectangle base_rectangle_fond;

        private List<Color> _color_annimation = new List<Color>() { new Color(255, 45, 255), new Color(45, 255, 255), new Color(255, 255, 45) };
        int _frame_interface_annim;
        int _frame_interface_annim_max = 10;
        Random rand = new Random();

        List<Particule_Mini> liste_particule = new List<Particule_Mini>();

        public Background_Ingame(GameScreen _screen, int nombre_carre)
        {
            _nombre_carre = nombre_carre;
            screen = _screen;
            Initialization();
        }

        public void Initialization()
        {
            string[] k = temps_best.Split(new char[] { '-' });
            scores = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                scores[i] = float.Parse(k[i]);
            }

            for (int i = 0; i < 100; i++)
            {
                liste_particule.Add(new Particule_Mini(screen, new Vector2(rand.Next(r.X, r.Width), rand.Next(r.Y, r.Height + r.Y))));
            }

            base_rectangle_fond = new Rectangle((int)(r.Width / 2 - (r.Width * 0.1)), (int)(r.Height / 2 - (r.Height * 0.1) + r.Y), (int)(r.Width * 0.2), (int)(r.Height * 0.2));

            int distance = (int)Math.Sqrt(Math.Pow(base_rectangle_fond.X, 2) + Math.Pow(base_rectangle_fond.Y, 2));
            liste_diagonale.Add(new Diagonale(r, new Vector2(r.Width, r.Y), screen, new Vector2(base_rectangle_fond.Right - 2, base_rectangle_fond.Top), distance));
            liste_diagonale.Add(new Diagonale(r, new Vector2(r.Width, r.Height + r.Y), screen, new Vector2(base_rectangle_fond.Right, base_rectangle_fond.Bottom), distance));
            liste_diagonale.Add(new Diagonale(r, new Vector2(r.X, r.Height + r.Y), screen, new Vector2(base_rectangle_fond.Left + 2, base_rectangle_fond.Bottom - 2), distance));
            liste_diagonale.Add(new Diagonale(r, new Vector2(r.X, r.Y), screen, new Vector2(base_rectangle_fond.Left, base_rectangle_fond.Top + 2), distance));

        }

        public void Update_Background(float timer)
        {
            _timer += timer;
            foreach (Diagonale caca in liste_diagonale)
            {
                caca.UpdateDiagonale(_timer);
            }

            for (int i = 0; i < liste_porte_blanche.Count(); i++)
            {
                liste_porte_blanche[i].Update_Porte(timer, _vitesse, r);
            }
            Gestion_Porte();
            if (mode_high_score)
            {
                AnnimColor();
            }
            TestBestScore();
        }

        private void TestBestScore()
        {
            if ((scores.Length >= _nombre_carre - 1) && (scores[_nombre_carre - 2] > 1) && (_timer > scores[_nombre_carre - 2]))
            {
                mode_high_score = true;
                color_bordure_base = Color.Black;
                foreach (Porte_Couleur caca in liste_porte_blanche)
                {
                    caca.color_bordure = Color.Black;
                    caca.color = Color.White;
                }
            }
        }

        private void AnnimColor()
        {
            _frame_interface_annim++;
            if (_frame_interface_annim >= _frame_interface_annim_max)
            {
                Color caca = color_carre_fond;
                while (caca == color_carre_fond)
                {
                    int r = rand.Next(0, _color_annimation.Count);
                    color_carre_fond = _color_annimation[r];
                }
                _frame_interface_annim = 0;
            }

        }

        public void Level_Increase()
        {
            _vitesse += 0.02f;
            if (!mode_high_score)
            {
                liste_porte_blanche.Add(new Porte_Couleur(base_rectangle_fond, Color.White, screen, mode_high_score));
            }
        }

        public void Change_Couleur_Annimation(Color caca, bool restauration)
        {
            if(restauration)
            {
                color_bordure = color_bordure_base;
                color_carre_fond = color_carre_fond_restaure;
                foreach (Diagonale pd in liste_diagonale)
                {
                    pd.color = color_bordure_base;
                    pd.InitializeBordure(screen);
                }
                foreach (Porte_Couleur pd in liste_porte_blanche)
                {
                    pd.color_bordure = color_bordure;
                }
            }
            else
            {
                color_bordure = caca;
                color_carre_fond = caca;
                foreach (Diagonale pd in liste_diagonale)
                {
                    pd.color = caca;
                    pd.InitializeBordure(screen);
                }
                foreach (Porte_Couleur pd in liste_porte_blanche)
                {
                    pd.color_bordure = caca;
                }
            }
        }

        private void Gestion_Porte()
        {
            if (liste_porte_blanche.Count() > 2 && liste_porte_blanche[1].r.Width >= r.Width)
            {
                liste_porte_blanche.RemoveAt(0);
            }

            Rectangle k = new Rectangle(0, 0, 5000, 5000);
            foreach (Porte_Couleur pd in liste_porte_blanche)
            {
                if (pd.r.Width < k.Width)
                {
                    k = pd.r;
                }
            }
            if (k.Width > 210)
            {
                liste_porte_blanche.Add(new Porte_Couleur( base_rectangle_fond,SetColor(),screen, mode_high_score));
            }

        }

        private Color SetColor()
        {
            int incrementation = 10;
            if (mode == 0)
            {
                R_value += 15;
                if (R_value >= 255)
                {
                    R_value = 255;
                    mode++;
                }
            }
            else if (mode == 1)
            {
                G_value += incrementation;
                if (G_value >= 255)
                {
                    G_value = 255;
                    mode++;
                }
            }
            else if (mode == 2)
            {
                R_value -= incrementation;
                if (R_value <= 0)
                {
                    R_value = 0;
                    mode++;
                }
            }
            else if (mode == 3)
            {
                B_value += incrementation;
                if (B_value >= 255)
                {
                    B_value = 255;
                    mode++;
                }
            }
            else if (mode == 4)
            {
                G_value -= incrementation;
                if (G_value <= 0)
                {
                    G_value = 0;
                    mode++;
                }
            }
            else if (mode == 5)
            {
                R_value += incrementation;
                if (R_value >= 255)
                {
                    R_value = 255;
                    mode++;
                }
            }
            else if (mode == 6)
            {
                B_value -= incrementation;
                if (B_value <= 0)
                {
                    B_value = 0;
                    mode = 1;
                }
            }
            return new Color(R_value, G_value, B_value);
        }

        public void Draw_Background()
        {
            screen.ScreenManager.SpriteBatch.Draw(screen.ScreenManager.BlankTexture, r, Color.Black);

            Draw_Porte_Blanche();
            Draw_Diagonales();

            /*foreach (Particule_Mini caca in liste_particule)
            {
                caca.DrawParticule(screen);
            }*/
        }


        private void Draw_Diagonales()
        {
            foreach (Diagonale caca in liste_diagonale)
            {
                caca.DrawDiagonale(screen);
            }
        }

        private void Draw_Porte_Blanche()
        {
            int BorderThickness = 2;
            SpriteBatch spritebatch = screen.ScreenManager.SpriteBatch;
            Texture2D blank = screen.ScreenManager.BlankTexture;

            foreach (Porte_Couleur caca in liste_porte_blanche)
            {
                caca.Draw_Porte();
            }

            Rectangle p1 = new Rectangle(base_rectangle_fond.Left, base_rectangle_fond.Top, base_rectangle_fond.Width, BorderThickness);
            Rectangle p2 = new Rectangle(base_rectangle_fond.Left, base_rectangle_fond.Top, BorderThickness, base_rectangle_fond.Height);
            Rectangle p3 = new Rectangle(base_rectangle_fond.Right - BorderThickness, base_rectangle_fond.Top, BorderThickness, base_rectangle_fond.Height);
            Rectangle p4 = new Rectangle(base_rectangle_fond.Left, base_rectangle_fond.Bottom - BorderThickness, base_rectangle_fond.Width, BorderThickness);

            spritebatch.Draw(blank, base_rectangle_fond, color_carre_fond);
            spritebatch.Draw(blank, p1, color_bordure);
            spritebatch.Draw(blank, p2, color_bordure);
            spritebatch.Draw(blank, p3, color_bordure);
            spritebatch.Draw(blank, p4, color_bordure);

            
        }
    }

    class Porte_Couleur
    {
        public Rectangle r;
        public Color color;
        int BorderThickness = 2;
        public Color color_bordure;
        GameScreen screen;

        public Porte_Couleur(Rectangle _r, Color _color, GameScreen _screen, bool high_score)
        {
            r = _r;
            screen = _screen;
            if (high_score)
            {
                color_bordure = Color.Black;
                color = Color.White;
            }
            else
            {
                color_bordure = Color.White;
                color = _color;
            }
        }

        public void Update_Porte(float timer, float _vitesse, Rectangle caca)
        {
            r.Width += (int)(_vitesse * timer);
            r.Height = r.Width * caca.Height / caca.Width;
            r.X = caca.Width / 2 - (r.Width / 2);
            r.Y = ((caca.Height / 2) + caca.Y) - (r.Height / 2);
        }

        public void Draw_Porte()
        {
            Texture2D blank = screen.ScreenManager.BlankTexture;
            SpriteBatch spritebatch = screen.ScreenManager.SpriteBatch;

            Rectangle r1 = new Rectangle(r.Left, r.Top, r.Width, BorderThickness);
            Rectangle r2 = new Rectangle(r.Left, r.Top, BorderThickness, r.Height);
            Rectangle r3 = new Rectangle(r.Right - BorderThickness, r.Top, BorderThickness, r.Height);
            Rectangle r4 = new Rectangle(r.Left, r.Bottom - BorderThickness, r.Width, BorderThickness);

            spritebatch.Draw(blank, r, color);

            spritebatch.Draw(blank, r1, color_bordure);
            spritebatch.Draw(blank, r2, color_bordure);
            spritebatch.Draw(blank, r3, color_bordure);
            spritebatch.Draw(blank, r4, color_bordure);
        }
    }

    class Diagonale
    {
        const float TEMPS_AUGMENTATION = 4000f;

        Texture2D texture;

        Rectangle ecran;
        Rectangle r;
        float rotation;
        Vector2 pos_milieu;
        public Color color;
        int distance;
        Vector2 coin;
        GameScreen _screen;
        Vector2 _position;

        public Diagonale(Rectangle _r, Vector2 _coin, GameScreen screen, Vector2 position, int _distance)
        {
            _position = position;
            _screen = screen;
            ecran = _r;
            coin = _coin;
            r = new Rectangle(_r.Width / 2, _r.Height / 2 + _r.Y, 2, 2);
            pos_milieu = new Vector2(_r.Width / 2, _r.Height / 2 + _r.Y);
            color = Color.White;
            distance = _distance;
            InitializeBordure(screen);
            rotation = InitializeAngle();
        }

        private float InitializeAngle()
        {
            
            float rotation = 0;
            int blbl = 51;
            if (coin == new Vector2(ecran.Width, ecran.Y))
            {
                rotation = (float)DegreesToRadians(360 - blbl);
            }
            else if (coin == new Vector2(ecran.Width, ecran.Height + ecran.Y))
            {
                rotation = (float)DegreesToRadians( blbl);

            }
            else if (coin == new Vector2(ecran.X, ecran.Height + ecran.Y))
            {
                rotation = (float)DegreesToRadians(180 - blbl);
            }
            else
            {
                rotation = (float)DegreesToRadians(180+ blbl);
            }
            return rotation;
        }

        public void InitializeBordure(GameScreen screen)
        {
            texture = new Texture2D(screen.ScreenManager.GraphicsDevice, (int)r.Width, (int)r.Height, false, SurfaceFormat.Color);
            Color[] c = new Color[(int)r.Width * (int)r.Height];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = color;
            }
            texture.SetData(c);
        }

        public void UpdateDiagonale(float temps)
        {
            if (temps < TEMPS_AUGMENTATION)
            {
                r.Width = (int)(distance * (temps / TEMPS_AUGMENTATION)) + 1;
                InitializeBordure(_screen);
            }
            else
            {
                r.Width = distance;
            }
        }


        public static double AngleBetweenToLines(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            double angle;

            double dotprod = (p3.X - p1.X) * (p2.X - p1.X) + (p3.Y - p1.Y) * (p2.Y - p1.Y);
            double len1squared = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            double len2squared = (p3.X - p1.X) * (p3.X - p1.X) + (p3.Y - p1.Y) * (p3.Y - p1.Y);
            angle = Math.Acos(dotprod / Math.Sqrt(len1squared * len2squared));

            return DegreesToRadians(angle);
        }

        static double DegreesToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);
        }

        public void DrawDiagonale(GameScreen screen)
        {
            screen.ScreenManager.SpriteBatch.Draw(texture, _position,null, Color.White, rotation, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
    }
}
