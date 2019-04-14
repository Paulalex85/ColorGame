using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Couleur_test.MenuScreenClass;
using System.IO.IsolatedStorage;

namespace Couleur_test.GameEngine
{
    class Fin_Partie
    {
        Song_Management song;

        string temps_best = (string)IsolatedStorageSettings.ApplicationSettings["high_score"];
        float[] scores;
        float[] blblbl;

        int _nombre_carre;
        bool _acces_niveau_suivant = false;

        bool _transition_debut = true;
        bool _transition_fin = false;
        Timer _timer_transition = new Timer(200f);
        Vector2 _position;
        Vector2 _taille;
        Vector2 _position_final;
        int value_blblb = 0;
        bool recommencer = false;
        bool bool_next_level = false;
        Vector2 position1, position2, position3;
        float Alpha = 0;
        float _temps_score;
        int _level;
        bool meilleur_temps = false;
        private List<Color> _color_annimation = new List<Color>() { new Color(255, 45, 255), new Color(45, 255, 255), new Color(255, 255, 45) };
        int _frame_interface_annim;
        int _frame_interface_annim_max = 10;
        Color color;
        Random rand = new Random();


        string temps, high_score, best_score, string_level, retry, menu, game_over, score, next_level, debloque;

        SpriteFont font;
        SpriteFont font_over;

        Languages lang = new Languages();
        Langues langue = new Langues();
        GameScreen _screen;

        public Fin_Partie(float millisecondes_score, int level, GameScreen screen, int nombre_carre)
        {
            _nombre_carre = nombre_carre;
            _level = level;
            _screen = screen;
            _temps_score = millisecondes_score;
            _taille = new Vector2(400, 450);
            _position = new Vector2(-_taille.X, 200);
            _position_final = new Vector2(480 / 2 - _taille.X / 2, _position.Y);

            position1 = new Vector2(_position.X + 20, _position.Y + _taille.Y - 50);
            position2 = new Vector2(_position.X +230, _position.Y + _taille.Y - 50);
            position3 = new Vector2(_position.X + 100, _position.Y + _taille.Y - 150);
            Initialize();
            CheckMeilleurTemps();
            CheckNiveauSuivant();
        }

        private void CheckNiveauSuivant()
        {
            if (_nombre_carre < 8 && blblbl.Length == _nombre_carre - 1 && blblbl[_nombre_carre - 2] > 60000f)
            {
                _acces_niveau_suivant = true;
            }
        }

        private void CheckMeilleurTemps()
        {
            if (scores.Length > _nombre_carre - 1)
            {
                blblbl = new float[scores.Length];
            }
            else
            {
                blblbl = new float[_nombre_carre - 1];
            }
            if (scores.Length >= _nombre_carre - 1 )
            {
                if(_temps_score > scores[_nombre_carre - 2])
                {
                    meilleur_temps = true;

                    for (int i = 0; i < blblbl.Length; i++)
                    {
                        if (i == (_nombre_carre - 2))
                        {
                            blblbl[i] = _temps_score;
                        }
                        else
                        {
                            blblbl[i] = scores[i];
                        }
                    }
                    string caca = "0";
                    for (int i = 0; i < blblbl.Length; i++)
                    {
                        if (i == 0)
                        {
                            caca = blblbl[i].ToString();
                        }
                        else
                        {
                            caca = caca + "-" + blblbl[i].ToString();
                        }
                    }
                    IsolatedStorageSettings.ApplicationSettings["high_score"] = caca;
                }
            }
            else
            {
                meilleur_temps = true;

                for (int i = 0; i < blblbl.Length; i++)
                {
                    if (i == (_nombre_carre - 2))
                    {
                        blblbl[i] = _temps_score;
                    }
                    else
                    {
                        blblbl[i] = scores[i];
                    }
                }
                string caca = "0";
                for (int i = 0; i < blblbl.Length; i++)
                {
                    if (i == 0)
                    {
                        caca = blblbl[i].ToString();
                    }
                    else
                    {
                        caca = caca + "-" + blblbl[i].ToString();
                    }
                }
                IsolatedStorageSettings.ApplicationSettings["high_score"] = caca;
            }
        }

        public void Initialize()
        {
            string[] k = temps_best.Split(new char[] { '-' });
            scores = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                scores[i] = float.Parse(k[i]);
            }

            font = _screen.ScreenManager.Game.Content.Load<SpriteFont>("Fin_partie_font");
            font_over = _screen.ScreenManager.Game.Content.Load<SpriteFont>("Game_Over");

            langue = _screen.ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();
        }

        private void InitilizeLanguages()
        {
            debloque = lang.AffectationLANG("NextLevelDebloque", langue);
            next_level = lang.AffectationLANG("NextLevel", langue);
            score = lang.AffectationLANG("Score", langue);
            temps = lang.AffectationLANG("Temps", langue);
            high_score = lang.AffectationLANG("NewHighScore", langue);
            best_score = lang.AffectationLANG("MeilleurTemps", langue);
            string_level = lang.AffectationLANG("Level", langue);
            retry = lang.AffectationLANG("Retry", langue);
            menu = lang.AffectationLANG("Menu", langue);
            game_over = lang.AffectationLANG("GameOver", langue);
        }

        public void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X > position1.X &&
                        gesture.Position.X < position1.X + font.MeasureString(retry).X &&
                        gesture.Position.Y > position1.Y &&
                        gesture.Position.Y < position1.Y + font.MeasureString(retry).Y)
                    {
                        recommencer = true;
                        _transition_fin = true;
                    }
                    if (gesture.Position.X > position2.X &&
                        gesture.Position.X < position2.X + font.MeasureString(menu).X &&
                        gesture.Position.Y > position2.Y &&
                        gesture.Position.Y < position2.Y + font.MeasureString(menu).Y)
                    {
                        Quitter();
                    }
                    if (_acces_niveau_suivant && gesture.Position.X > position3.X &&
                        gesture.Position.X < position3.X + font.MeasureString(next_level).X &&
                        gesture.Position.Y > position3.Y &&
                        gesture.Position.Y < position3.Y + font.MeasureString(next_level).Y)
                    {
                        bool_next_level = true;
                        _transition_fin = true;
                    }
                }
            }
        }

        public void Update(float timer)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Quitter();
            }

            position1 = new Vector2(_position.X + 20, _position.Y + _taille.Y - 50);
            position2 = new Vector2(_position.X + 260, _position.Y + _taille.Y - 50);
            position3 = new Vector2(_position.X + 50, _position.Y + _taille.Y - 120);

            AnnimColor();

            if (_transition_debut)
            {
                if (_timer_transition.IncreaseTimer(timer))
                {
                    _transition_debut = false;
                    _position = _position_final;
                    Alpha = 1;
                }
                else
                {
                    if (value_blblb == 0)
                    {
                        value_blblb = (int)_position_final.X - (int)_position.X;
                    }
                    _position.X = -350 +(_timer_transition._timer * value_blblb / _timer_transition._timer_max);
                    Alpha = _timer_transition._timer / _timer_transition._timer_max;
                }
            }
            else if (_transition_fin)
            {
                if (_timer_transition.IncreaseTimer(timer))
                {
                    _transition_fin = false;
                    if (recommencer)
                    {
                        Recommencer();
                    }
                    else if (bool_next_level)
                    {
                        Next_Level();
                    }
                    else
                    {
                        Quitter();
                    }
                }
                else
                {
                    if (value_blblb == 0)
                    {
                        value_blblb = (int)_position_final.X - (int)_position.X;
                    }
                    _position.X += _timer_transition._timer * value_blblb / _timer_transition._timer_max;
                    Alpha -= timer / _timer_transition._timer_max;
                }
            }
        }

        private void AnnimColor()
        {
            if (meilleur_temps)
            {
                _frame_interface_annim++;
                if (_frame_interface_annim >= _frame_interface_annim_max)
                {
                    Color caca = color;
                    while (caca == color)
                    {
                        int r = rand.Next(0, _color_annimation.Count);
                        color = _color_annimation[r];
                    }
                    _frame_interface_annim = 0;
                }
            }
        }

        private void Quitter()
        {
            _screen.ExitScreen();
            _screen.ScreenManager.AddScreen(new BackgroundMenuScreen());
            _screen.ScreenManager.AddScreen(new MainMenuScreen());
            song = new Song_Management(_screen.ScreenManager);
            song.Change_Intro();
        }

        private void Next_Level()
        {
            foreach (GameScreen screen in _screen.ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            _screen.ScreenManager.AddScreen(new Element_mode(_nombre_carre +1));
        }

        private void Recommencer()
        {
            foreach (GameScreen screen in _screen.ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            _screen.ScreenManager.AddScreen(new Element_mode(_nombre_carre));
        }

        public void Draw()
        {
            Color FillColor = Color.Gray;
            Color BorderColor = Color.Black;
            int BorderThickness = 5;
            Texture2D blank = _screen.ScreenManager.BlankTexture;

            Rectangle back = new Rectangle(0, 0, 480, 800);

            _screen.ScreenManager.SpriteBatch.Draw(_screen.ScreenManager.BlankTexture, back, Color.Black * 0.5f);

            Rectangle r = new Rectangle((int)_position.X, (int)_position.Y, (int)_taille.X, (int)_taille.Y);
            Rectangle r1 = new Rectangle(r.Left, r.Top, r.Width, BorderThickness);
            Rectangle r2 = new Rectangle(r.Left, r.Top, BorderThickness, r.Height);
            Rectangle r3 = new Rectangle(r.Right - BorderThickness, r.Top, BorderThickness, r.Height);
            Rectangle r4 = new Rectangle(r.Left, r.Bottom - BorderThickness, r.Width, BorderThickness);
            // Fill the button
            _screen.ScreenManager.SpriteBatch.Draw(blank, r, Color.Black * Alpha);

            // Draw the border
            _screen.ScreenManager.SpriteBatch.Draw(
               blank,
               new Rectangle(r.Left, r.Top + BorderThickness, r.Width, BorderThickness),
               Color.White * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(
                blank,
                new Rectangle(r.Left + BorderThickness, r.Top, BorderThickness, r.Height),
                Color.White * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(
                blank,
                new Rectangle(r.Right - (BorderThickness * 2), r.Top, BorderThickness, r.Height),
                Color.White * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Bottom - (BorderThickness * 2), r.Width, BorderThickness),
                Color.White * Alpha);

            _screen.ScreenManager.SpriteBatch.Draw(blank, r1, _color_annimation[0] * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(blank, r2, _color_annimation[0] * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(blank, r3, _color_annimation[1] * Alpha);
            _screen.ScreenManager.SpriteBatch.Draw(blank, r4, _color_annimation[1] * Alpha);

            //DRAW texte
            //GAMEOVER
            _screen.ScreenManager.SpriteBatch.DrawString(font_over, game_over, new Vector2(480 / 2 - font_over.MeasureString(game_over).X / 2, _position.Y - 80), Color.White);
            //SCORE TITRE
            _screen.ScreenManager.SpriteBatch.DrawString(font, score, new Vector2(_position.X + _taille.X / 2 - font.MeasureString(score).X / 2, _position.Y + 20), Color.White);
            //LEVEL
            _screen.ScreenManager.SpriteBatch.DrawString(font, string_level, new Vector2(_position.X + 20, _position.Y + 80), Color.White);
            _screen.ScreenManager.SpriteBatch.DrawString(font, _level.ToString(), new Vector2(_position.X + 300, _position.Y + 80), Color.White);
            //TEMPS
            _screen.ScreenManager.SpriteBatch.DrawString(font, temps, new Vector2(_position.X + 20, _position.Y + 130), Color.White);
            _screen.ScreenManager.SpriteBatch.DrawString(font, Minutes(_temps_score) + ":" + Secondes(_temps_score) + "'" + Millisecondes(_temps_score), new Vector2(_position.X + 220, _position.Y + 130), Color.White);

            _screen.ScreenManager.SpriteBatch.DrawString(font, best_score, new Vector2(_position.X + 20, _position.Y + 180), Color.White);
            if (scores.Length >= _nombre_carre - 1)
            {
                _screen.ScreenManager.SpriteBatch.DrawString(font, Minutes(scores[_nombre_carre - 2]) + ":" + Secondes(scores[_nombre_carre - 2]) + "'" + Millisecondes(scores[_nombre_carre - 2]), new Vector2(_position.X + 220, _position.Y + 180), Color.White);
            }
            else
            {
                _screen.ScreenManager.SpriteBatch.DrawString(font, Minutes(0) + ":" + Secondes(0) + "'" + Millisecondes(0), new Vector2(_position.X + 220, _position.Y + 180), Color.White);
            }
            if (meilleur_temps)
            {
                _screen.ScreenManager.SpriteBatch.DrawString(font, high_score, new Vector2(_position.X +20, _position.Y + 230), color);
                _screen.ScreenManager.SpriteBatch.DrawString(font, best_score, new Vector2(_position.X +220  , _position.Y + 230), color);
            }
            if (_acces_niveau_suivant)
            {
                _screen.ScreenManager.SpriteBatch.DrawString(font, debloque, new Vector2(_position.X + _taille.X/2 - font.MeasureString(debloque).X/2 , _position.Y + 260), color);
                _screen.ScreenManager.SpriteBatch.DrawString(font, next_level, position3, Color.White);
            }

            _screen.ScreenManager.SpriteBatch.DrawString(font, retry, position1, Color.White);
            _screen.ScreenManager.SpriteBatch.DrawString(font, menu, position2, Color.White);

        }

        private string Minutes(float temps)
        {
            int blbl = (int)temps / 60000;
            if (blbl > 0)
            {
                return blbl.ToString();
            }
            else
            {
                return "0";
            }
        }

        private string Secondes(float temps)
        {
            double caca = TimeSpan.FromMilliseconds(temps).TotalSeconds;
            int blbl = (int)caca;
            if (caca < 60)
            {
                if (caca < 10)
                {
                    return "0" + blbl;
                }
                else if (blbl == 0)
                {
                    return "00";
                }
                else
                {
                    return blbl.ToString();
                }
            }
            else
            {
                int k = (int)caca / 60;
                blbl = (int)caca - (60 * k);
                if (caca < 10)
                {
                    return "0" + blbl;
                }
                else if (blbl == 0)
                {
                    return "00";
                }
                else
                {
                    return blbl.ToString();
                }
            }
        }

        private string Millisecondes(float temps)
        {
            double no_virg = TimeSpan.FromMilliseconds(temps).TotalMilliseconds;
            if (no_virg > 999)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(c - 3, 3);
            }
            else if (no_virg > 99)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 3);
            }
            else if (no_virg > 9)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 2);
            }
            else if (no_virg > 0)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 1);
            }
            else
            {
                return "000";
            }
        }
    }
}
