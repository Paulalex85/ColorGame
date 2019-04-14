using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Couleur_test.MenuScreenClass
{
    class MainMenuScreen : MenuScreen
    {
        Song_Management song;

        Texture2D son, son_on, son_off;
        string temps_best = (string)IsolatedStorageSettings.ApplicationSettings["high_score"];
        float[] scores;
		string music_int = (string)IsolatedStorageSettings.ApplicationSettings["music"];
		bool music;

        string jouer,quitter, high_score;

        Languages lang = new Languages();
        Langues langue = new Langues();
        ContentManager content;
        

        SpriteFont font;

        string music1 = "Music by";
        string music2 = "Skinwalker Audio - Dark Space";
        string music3 = "Squaremile - Orbital";
        string music4 = "CristMWalt - Electronic Space";

        private List<Color> _color_annimation = new List<Color>() { new Color(255, 45, 255), new Color(45, 255, 255), new Color(255, 255, 45) };
        int _frame_interface_annim;
        int _frame_interface_annim_max = 10;
        Color color;
        Random rand = new Random();

        public MainMenuScreen()
            : base()
        {
            EnabledGestures = GestureType.Tap;
        }

        private void InitilizeLanguages()
        {
			if (music_int == "1") {
				music = true;
			} else {
				music = false;
			}

            string[] k = temps_best.Split(new char[] { '-' });
            scores = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                scores[i] = float.Parse(k[i]);
            }

            jouer = lang.AffectationLANG("Jouer", langue);
            quitter = lang.AffectationLANG("Quitter", langue);
            high_score = lang.AffectationLANG("MeilleurTemps", langue);

            if (temps_best == "0")
            {
                high_score = high_score + " " + Minutes(0) + ":" + Secondes(0) + "'" + Millisecondes(0);
            }
            else
            {
                high_score = high_score + " " + Minutes(scores.Last()) + ":" + Secondes(scores.Last()) + "'" + Millisecondes(scores.Last()) + " - mode " + (scores.Length + 1).ToString();
            }

            MenuEntry JouerEntry = new MenuEntry(jouer.ToUpper());
            MenuEntry exitMenuEntry = new MenuEntry(quitter.ToUpper());

            JouerEntry.Selected += JouerEntrySelected;
            exitMenuEntry.Selected += Quitter;

            MenuEntries.Add(JouerEntry);
            MenuEntries.Add(exitMenuEntry);

        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();

            son = ScreenManager.Game.Content.Load<Texture2D>("Image/son_base");
            son_on = ScreenManager.Game.Content.Load<Texture2D>("Image/son_on");
            son_off = ScreenManager.Game.Content.Load<Texture2D>("Image/son_off");

            song = new Song_Management(this.ScreenManager);

            font = ScreenManager.Game.Content.Load<SpriteFont>("menu_credit");

            base.LoadContent();
        }

        void JouerEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new LevelScreen());
        }

        void Quitter(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        public override void HandleInput(InputState input)
        {
            Vector2 son_pos = new Vector2(10, 720);

            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X > son_pos.X &&
                        gesture.Position.X < son_pos.X + 150 &&
                        gesture.Position.Y > son_pos.Y &&
                        gesture.Position.Y < son_pos.Y + 100)
                    {
                        song.Change_Mute();
                        if (music)
                        {
                            music = false;
                        }
                        else
                        {
                            music = true;
                        }
                    }
                }
            }
            
            base.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.OnCancel(PlayerIndex.One);

            AnnimColor();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void AnnimColor()
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

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(font, high_score, new Vector2(480 / 2 - font.MeasureString(high_score).X / 2, 500), color * TransitionAlpha);

            ScreenManager.SpriteBatch.DrawString(font, music1, new Vector2(480 / 2 - font.MeasureString(music1).X / 2, 570), color * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(font, music2, new Vector2(480 / 2 - font.MeasureString(music2).X / 2, 610), Color.White * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(font, music3, new Vector2(480 / 2 - font.MeasureString(music3).X / 2, 650), Color.White * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(font, music4, new Vector2(480 / 2 - font.MeasureString(music4).X / 2, 690), Color.White * TransitionAlpha);

            ScreenManager.SpriteBatch.Draw(son, new Vector2(20, 720), Color.White * TransitionAlpha);
            if (music)
            {
                ScreenManager.SpriteBatch.Draw(son_on, new Vector2(70, 720), Color.White * TransitionAlpha);
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(son_off, new Vector2(80, 730), Color.White * TransitionAlpha);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
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
