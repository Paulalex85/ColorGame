using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.DogeTools;

namespace Couleur_test
{
    public class Carre
    {
        //Vector2 origin_position;
        Texture2D texture;
        public Vector2 _position;
        public Vector2 _direction;
        public Color _couleur;
        public Vector2 _taille;
        public int _identifiant = -1;
        public float _rotation;
        public float _vitesse = 0f;
        public bool _rotation_bool = false;
        public Vector2 _point_rotation_cercle;
        public float _opacity = 1;
        public float _scale = 1;

        private int[] _trace_blanc;

        public Vector2 _origin = new Vector2();
        public  float _rotationAngle = 0f;
        private float PiOver128 = MathHelper.Pi / 128;

        private Timer _timer_particule_ajout = new Timer(300f);
        List<Particule_Carre> liste_particule = new List<Particule_Carre>();
        public  bool _bool_ajout_particule = false;
        bool _bool_add1 = false;
        bool _bool_add2 = false;

        Timer _timer_tap = new Timer(100f);
        public bool _taped_annim_scale;

        public Carre(int identifiant, Vector2 position, Vector2 direction, Color couleur, Vector2 taille, bool rotation, GameScreen screen)
        {
            _identifiant = identifiant;
            _position = position;
            _direction = direction;
            _couleur = couleur;
            _taille = taille;
            _rotation_bool = rotation;

            if (_rotation_bool)
            {
                _origin = (_taille / 2);
            }

            Initialize_Texture(_couleur, screen);
        }

        public Carre(Vector2 position, Vector2 direction, Color couleur, Vector2 taille, float rotation, float vitesse, GameScreen screen)
        {
            _position = position;
            _direction = direction;
            _couleur = couleur;
            _taille = taille;
            _rotation = rotation;
            _vitesse = vitesse;
            _rotation_bool = true;

            _origin =  (_taille / 2);
            
            Initialize_Texture(_couleur, screen);
        }

        public bool HandleTap(Vector2 tap)
        {
            if (_rotationAngle == 0)
            {
                if (tap.X >= _position.X &&
                    tap.Y >= _position.Y &&
                    tap.X <= _position.X + _taille.X &&
                    tap.Y <= _position.Y + _taille.Y)
                {
                    return true;
                }
            }
            else
            {
                if (IsClicked(tap))
                {
                    return true;
                }
            }
            return false;
        }

        #region ForRotate

        private bool IsClicked(Vector2 point)
        {
            List<Vector2> caca = List_point_rectangle(_rotationAngle, _taille, _position);
            Vector2 A = caca[0];
            Vector2 B = caca[1];
            Vector2 C = caca[2];
            Vector2 D = caca[3];

            if (triangleArea(A, B, point) > 0 || triangleArea(B, C, point) > 0 || triangleArea(C, D, point) > 0 || triangleArea(D, A, point) > 0)
            {
                return false;
            }
            return true;
        }

        private float triangleArea(Vector2 A, Vector2 B, Vector2 C)
        {
            return (C.X * B.Y - B.X * C.Y) - (C.X * A.Y - A.X * C.Y) + (B.X * A.Y - A.X * B.Y);
        }

        public List<Vector2> List_point_rectangle(float rotation, Vector2 _taille, Vector2 position)
        {
            Vector2 gorge = position + (_taille / 2);
            List<Vector2> caca = new List<Vector2>();
            float _angle_en_degre = Angle_En_Degree(rotation);
            double theta = _angle_en_degre * (Math.PI / 180);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            Vector2 A = getTopLeft(_taille.X, _taille.Y, cos, sin);
            Vector2 B = getTopRight(_taille.X, _taille.Y, cos, sin);
            Vector2 C = getBottomRight(_taille.X, _taille.Y, cos, sin);
            Vector2 D = getBottomLeft(_taille.X, _taille.Y, cos, sin);

            caca.Add(A + gorge);
            caca.Add(B + gorge);
            caca.Add(C + gorge);
            caca.Add(D + gorge);

            return caca;
        }

        

        public float Angle_En_Degree(float rotation)
        {
            return 360 * rotation / (MathHelper.Pi * 2);
        }

        public static Vector2 getTopLeft(float width, float height,double cos, double sin)
        {
            float hw = -width/2;
            float hh = -height / 2;
            return new Vector2((float)(hw * cos - hh * sin), (float)(hw * sin + hh * cos));
        }
        public static Vector2 getTopRight(float width, float height, double cos, double sin)
        {
            float hw = width / 2;
            float hh = -height / 2;
            return new Vector2((float)(hw * cos - hh * sin), (float)(hw * sin + hh * cos));
        }
        public static Vector2 getBottomLeft(float width, float height, double cos, double sin)
        {
            float hw = -width / 2;
            float hh = height / 2;
            return new Vector2((float)(hw * cos - hh * sin), (float)(hw * sin + hh * cos));
        }
        public static Vector2 getBottomRight(float width, float height, double cos, double sin)
        {
            float hw = width / 2;
            float hh = height / 2;
            return new Vector2((float)(hw * cos - hh * sin), (float)(hw * sin + hh * cos));
        }

        #endregion

        public void Initialize_Texture(Color color, GameScreen screen)
        {
            if (_trace_blanc == null)
            {
                _trace_blanc = new int[(int)_taille.X * (int)_taille.Y];
                for (int i = 0; i < _trace_blanc.Length; i++)
                {
                    _trace_blanc[i] = 0;
                }
                InitializeTraceBlanc();
            }

            // SET CARRE
            int taille_bordure = 5;
            Color color_bordure = BordureColor(color);
            Color color_barre = ColorBarre(color);

            texture = new Texture2D(screen.ScreenManager.GraphicsDevice, (int)_taille.X, (int)_taille.Y, false, SurfaceFormat.Color);
            Color[] c = new Color[(int)_taille.X * (int)_taille.Y];
            for (int i = 0; i < c.Length; ++i)
            {
                if (i < (taille_bordure * _taille.X) || i > (_taille.X * _taille.Y) - (taille_bordure * _taille.X) || taille_bordure > (i % _taille.Y) || _taille.X - taille_bordure <= (i % _taille.Y))
                {
                    c[i] = color_bordure;
                }
                else if (_trace_blanc[i] == 1)
                {
                    c[i] = color_barre;
                }
                else
                {
                    c[i] = color;
                }
            }
            texture.SetData(c);

        }

        private void InitializeTraceBlanc()
        {
            int _taille_grande_barre = 3;
            Random rand = new Random();
            //BARRE MILIEU
            int r = rand.Next(0, 3);
            if (r == 0)
            {
                Horizontal_Barre(_taille_grande_barre, -1, 0);
            }
            else if (r == 1)
            {
                Vertical_Barre(_taille_grande_barre, -1, 0);
            }
            else
            {
                Horizontal_Barre(_taille_grande_barre, -1, 0);
                Vertical_Barre(_taille_grande_barre, -1, 0);
            }

            for (int i = 0; i < 4; i++)
            {
                int h = rand.Next(0, 2);
                if (h == 0)
                {
                    Horizontal_Barre(1, i, 0);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int h = rand.Next(0, 2);
                if (h == 0)
                {
                    Horizontal_Barre(1, i, 1);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int h = rand.Next(0, 2);
                if (h == 0)
                {
                    Vertical_Barre(1, 0, i);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int h = rand.Next(0, 2);
                if (h == 0)
                {
                    Vertical_Barre(1, 1, i);
                }
            }
            
        }

        private void Horizontal_Barre(int epaisseur, int quartier_X, int quartier_Y)
        {
            int start = 0;
            int nbr_pixel = 0;
            if (quartier_X == -1)
            {
                if (Pair(epaisseur))
                {
                    start = (int)(_taille.X * _taille.X) / 2 - ((int)_taille.X * (epaisseur / 2));
                }
                else
                {
                    start = (int)(_taille.X * _taille.X) / 2 - ((int)_taille.X * ((epaisseur - 1) / 2));
                }
                nbr_pixel = epaisseur * (int)_taille.X;
                
            }
            else
            {
                if (quartier_Y == 0)
                {
                    start = (int)(_taille.X * _taille.X) / 4;
                }
                else
                {
                    start = (int)(_taille.X * _taille.X) / 4;
                    start = start * 3;
                }
                start += (int)_taille.X / 4 * quartier_Y;
                nbr_pixel = (int)_taille.X / 4 + 1;

            }
            for (int i = start; i < (start + nbr_pixel); i++)
            {
                _trace_blanc[i] = 1;
            }

        }

        private void Vertical_Barre(int epaisseur, int quartier_X, int quartier_Y)
        {
            int colonne_depart = 0;
            int nbr_pixel = 0;
            int hauteur = 0;
            if (quartier_X == -1)
            {
                if (Pair(epaisseur))
                {
                    colonne_depart = (int)_taille.X / 2 - ((epaisseur / 2));
                    hauteur = (int)_taille.Y - 1;
                }
                else
                {
                    colonne_depart = (int)_taille.X / 2 - ((epaisseur - 1) / 2);
                    hauteur = (int)_taille.Y - 1;
                }
            }
            else
            {
                if (quartier_X == 0)
                {
                    colonne_depart = (int)_taille.X / 4;
                }
                else
                {
                    colonne_depart = (int)_taille.X / 4;
                    colonne_depart = colonne_depart * 3;
                }
                hauteur = (int)_taille.Y / 4;
            }

            for (int i = 0; i < hauteur; i++)
            {
                for (int l = 0; l < epaisseur; l++)
                {
                    int pos = colonne_depart + ((int)_taille.X * i) + l;
                    _trace_blanc[pos] = 1;
                }
            }
        }

        private bool Pair(int nombre)
        {
            if (nombre % 2 == 0)
            {
                return true;
            }
            else
                return false;
        }

        private Color ColorBarre(Color color)
        {
            if (color == new Color(255, 255, 255))
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Color BordureColor(Color color)
        {
            Color caca = color;

            if (caca == new Color(32, 211, 220))
            {
                return new Color(26, 149, 176);
            }
            else if (caca == new Color(204, 29, 29))
            {
                return new Color(143, 20, 20);
            }
            else if (caca == new Color(254, 217, 6))
            {
                return new Color(207, 175, 1);
            }
            else if (caca == new Color(24, 171, 31))
            {
                return new Color(19, 133, 27);
            }
            else if (caca == new Color(255, 78, 17))
            {
                return new Color(195, 44, 0);
            }
            else if (caca == new Color(145, 0, 145))
            {
                return new Color(96, 0, 107);
            }
            else if (caca == new Color(255, 89, 234))
            {
                return new Color(220, 0, 179);
            }
            else if (caca == new Color(23, 68, 243))
            {
                return new Color(8, 35, 162);
            }
            else
            {
                return Color.Black;
            }
        }

        public void UpdatePositionCarre(float elapsed)
        {
            _position.X = _position.X += (_direction.X * _vitesse);
            _position.Y = _position.Y += (_direction.Y * _vitesse);

            if (_identifiant == -1)
            {
                _rotationAngle += PiOver128 * (_rotation / 100);
                if (_rotationAngle > MathHelper.Pi * 2)
                {
                    _rotationAngle = 0;
                }
            }
        }

        public void UpdateCarre(float timer, GameScreen screen)
        {
            GestionParticule(timer, screen);
            TapedAnnimation(timer);
        }

        private void GestionParticule(float timer, GameScreen screen)
        {
            if (_bool_ajout_particule)
            {
                if (_timer_particule_ajout.IncreaseTimer(timer))
                {
                    liste_particule.Add(new Particule_Carre(3, _position, _couleur, _rotationAngle, screen));
                    _bool_add1 = false;
                    _bool_add2 = false;
                    _bool_ajout_particule = false;
                }
                else
                {
                    if (_timer_particule_ajout._timer > 100f && !_bool_add1)
                    {
                        _bool_add1 = true;
                        liste_particule.Add(new Particule_Carre(1, _position, _couleur, _rotationAngle, screen));
                    }
                    else if (_timer_particule_ajout._timer > 200f && !_bool_add2)
                    {
                        _bool_add2 = true;
                        liste_particule.Add(new Particule_Carre(2, _position, _couleur, _rotationAngle, screen));
                    }
                }
            }

            foreach (Particule_Carre caca in liste_particule)
            {
                if (caca.VerificationTempsParticule(timer))
                {
                    liste_particule.Remove(caca); break;
                }
            }

        }

        private void TapedAnnimation(float timer)
        {
            if (_taped_annim_scale)
            {
                if (_timer_tap._timer == 0.0f)
                {
                    _scale = 0.95f;
                }
                if (_timer_tap.IncreaseTimer(timer))
                {
                    _taped_annim_scale = false;
                    _scale = 1;
                }
            }
        }

        public void DrawCarre(GameScreen screen)
        {

            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
            Texture2D blank = screen.ScreenManager.BlankTexture;

            Rectangle r = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_taille.X,
                (int)_taille.Y);
            if (_rotation_bool)
            {
                spriteBatch.Draw(texture, _position + _origin, null, Color.White * _opacity, _rotationAngle, _origin, _scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(blank, r, _couleur * _opacity);
            }
        }

        public void DrawParticule(GameScreen screen)
        {
            foreach (Particule_Carre caca in liste_particule)
            {
                caca.DrawCarre(screen);
            }
        }
    }
}
