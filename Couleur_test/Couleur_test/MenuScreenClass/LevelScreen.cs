using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couleur_test.DogeTools;
using System.IO.IsolatedStorage;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Couleur_test.MenuScreenClass
{
    class LevelScreen : MenuScreen
    {
        Song_Management song;
        string temps_best = (string)IsolatedStorageSettings.ApplicationSettings["high_score"];
        float[] scores;

		string jouer, quitter, locked, difficulte;

        Languages lang = new Languages();
        Langues langue = new Langues();
        ContentManager content;

        SpriteFont font;

        private List<Color> _color_annimation = new List<Color>() { new Color(255, 45, 255), new Color(45, 255, 255), new Color(255, 255, 45) };
        int _frame_interface_annim;
        int _frame_interface_annim_max = 10;
        Color color;
        Random rand = new Random();

        public LevelScreen()
            : base()
        {
        }

        private void InitilizeLanguages()
        {
            string[] k = temps_best.Split(new char[] { '-' });
            scores = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                scores[i] = float.Parse(k[i]);
            }

            difficulte = lang.AffectationLANG("Difficulte", langue);
            locked = lang.AffectationLANG("Locked", langue);
            jouer = lang.AffectationLANG("Difficulte", langue);
            quitter = lang.AffectationLANG("Retour", langue);

            if (scores.Length > 5 && scores[5] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry JouerEntry4 = new MenuEntry(jouer + " 4");
                MenuEntry JouerEntry5 = new MenuEntry(jouer + " 5");
                MenuEntry JouerEntry6 = new MenuEntry(jouer + " 6");
                MenuEntry JouerEntry7 = new MenuEntry(jouer + " 7");
                MenuEntry JouerEntry8 = new MenuEntry(jouer + " 8");

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                JouerEntry4.Selected += Jouer4;
                JouerEntry5.Selected += Jouer5;
                JouerEntry6.Selected += Jouer6;
                JouerEntry7.Selected += Jouer7;
                JouerEntry8.Selected += Jouer8;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(JouerEntry4);
                MenuEntries.Add(JouerEntry5);
                MenuEntries.Add(JouerEntry6);
                MenuEntries.Add(JouerEntry7);
                MenuEntries.Add(JouerEntry8);
            }
            else if (scores.Length > 4 && scores[4] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry JouerEntry4 = new MenuEntry(jouer + " 4");
                MenuEntry JouerEntry5 = new MenuEntry(jouer + " 5");
                MenuEntry JouerEntry6 = new MenuEntry(jouer + " 6");
                MenuEntry JouerEntry7 = new MenuEntry(jouer + " 7");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                JouerEntry4.Selected += Jouer4;
                JouerEntry5.Selected += Jouer5;
                JouerEntry6.Selected += Jouer6;
                JouerEntry7.Selected += Jouer7;
                ADEBLOQUER.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(JouerEntry4);
                MenuEntries.Add(JouerEntry5);
                MenuEntries.Add(JouerEntry6);
                MenuEntries.Add(JouerEntry7);
                MenuEntries.Add(ADEBLOQUER);
            }
            else if (scores.Length >3 && scores[3] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry JouerEntry4 = new MenuEntry(jouer + " 4");
                MenuEntry JouerEntry5 = new MenuEntry(jouer + " 5");
                MenuEntry JouerEntry6 = new MenuEntry(jouer + " 6");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);
                MenuEntry ADEBLOQUER1 = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                JouerEntry4.Selected += Jouer4;
                JouerEntry5.Selected += Jouer5;
                JouerEntry6.Selected += Jouer6;
                ADEBLOQUER.Selected += Locked;
                ADEBLOQUER1.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(JouerEntry4);
                MenuEntries.Add(JouerEntry5);
                MenuEntries.Add(JouerEntry6);
                MenuEntries.Add(ADEBLOQUER);
                MenuEntries.Add(ADEBLOQUER1);
            }
            else if (scores.Length >2 && scores[2] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry JouerEntry4 = new MenuEntry(jouer + " 4");
                MenuEntry JouerEntry5 = new MenuEntry(jouer + " 5");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);
                MenuEntry ADEBLOQUER1 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER2 = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                JouerEntry4.Selected += Jouer4;
                JouerEntry5.Selected += Jouer5;
                ADEBLOQUER.Selected += Locked;
                ADEBLOQUER1.Selected += Locked;
                ADEBLOQUER2.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(JouerEntry4);
                MenuEntries.Add(JouerEntry5);
                MenuEntries.Add(ADEBLOQUER);
                MenuEntries.Add(ADEBLOQUER1);
                MenuEntries.Add(ADEBLOQUER2);
            }
            else if (scores.Length >1 && scores[1] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry JouerEntry4 = new MenuEntry(jouer + " 4");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);
                MenuEntry ADEBLOQUER1 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER2 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER3 = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                JouerEntry4.Selected += Jouer4;
                ADEBLOQUER.Selected += Locked;
                ADEBLOQUER1.Selected += Locked;
                ADEBLOQUER2.Selected += Locked;
                ADEBLOQUER3.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(JouerEntry4);
                MenuEntries.Add(ADEBLOQUER);
                MenuEntries.Add(ADEBLOQUER1);
                MenuEntries.Add(ADEBLOQUER2);
                MenuEntries.Add(ADEBLOQUER3);
            }
            else if (scores[0] >= 60000f)
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry JouerEntry3 = new MenuEntry(jouer + " 3");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);
                MenuEntry ADEBLOQUER1 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER2 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER3 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER4 = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                JouerEntry3.Selected += Jouer3;
                ADEBLOQUER.Selected += Locked;
                ADEBLOQUER1.Selected += Locked;
                ADEBLOQUER2.Selected += Locked;
                ADEBLOQUER3.Selected += Locked;
                ADEBLOQUER4.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(JouerEntry3);
                MenuEntries.Add(ADEBLOQUER);
                MenuEntries.Add(ADEBLOQUER1);
                MenuEntries.Add(ADEBLOQUER2);
                MenuEntries.Add(ADEBLOQUER3);
                MenuEntries.Add(ADEBLOQUER4);
            }
            else
            {
                MenuEntry JouerEntry2 = new MenuEntry(jouer + " 2");
                MenuEntry ADEBLOQUER = new MenuEntry(locked);
                MenuEntry ADEBLOQUER1 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER2 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER3 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER4 = new MenuEntry(locked);
                MenuEntry ADEBLOQUER5 = new MenuEntry(locked);

                JouerEntry2.Selected += Jouer2;
                ADEBLOQUER.Selected += Locked;
                ADEBLOQUER1.Selected += Locked;
                ADEBLOQUER2.Selected += Locked;
                ADEBLOQUER3.Selected += Locked;
                ADEBLOQUER4.Selected += Locked;
                ADEBLOQUER5.Selected += Locked;

                MenuEntries.Add(JouerEntry2);
                MenuEntries.Add(ADEBLOQUER);
                MenuEntries.Add(ADEBLOQUER1);
                MenuEntries.Add(ADEBLOQUER2);
                MenuEntries.Add(ADEBLOQUER3);
                MenuEntries.Add(ADEBLOQUER4);
                MenuEntries.Add(ADEBLOQUER5);
            }
            MenuEntry exitMenuEntry = new MenuEntry(quitter);
            exitMenuEntry.Selected += Quitter;
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();

            song = new Song_Management(this.ScreenManager);

            font = ScreenManager.Game.Content.Load<SpriteFont>("menu_credit");

            base.LoadContent();
        }

        void Locked(object sender, EventArgs e)
        {
        }

        void Jouer2(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(2));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer3(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(3));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer4(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(4));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer5(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(5));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer6(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(6));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer7(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(7));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }
        void Jouer8(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new Element_mode(8));
            song = new Song_Management(this.ScreenManager);
            song.Change_Ingame();
        }

        void Quitter(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen());
            }

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

            for (int i = 0; i < scores.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, Minutes(scores[i]) + ":" + Secondes(scores[i]) + "'" + Millisecondes(scores[i]), new Vector2(330, 190 + (70 * i)), color * TransitionAlpha);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
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
