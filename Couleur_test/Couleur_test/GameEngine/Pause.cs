using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Couleur_test.MenuScreenClass;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;

namespace Couleur_test.GameEngine
{
    class Pause
    {
        Song_Management song;

        Texture2D son, son_on, son_off;
		string music_int = (string)IsolatedStorageSettings.ApplicationSettings["music"];
		bool music;
        SpriteFont font;

        Languages lang = new Languages();
        Langues langue = new Langues();

        string reprendre, quitter;
        Vector2 position1, position2;

        GameScreen _screen;

        public Pause(GameScreen screen)
        {
            _screen = screen;
            Initialize();
        }

        public void Initialize()
        {
			if (music_int == "1") {
				music = true;
			} else {
				music = false;
			}

            song = new Song_Management(_screen.ScreenManager);
            langue = _screen.ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();
            font = _screen.ScreenManager.Game.Content.Load<SpriteFont>("menufont");
            son = _screen.ScreenManager.Game.Content.Load<Texture2D>("Image/son_base");
            son_on = _screen.ScreenManager.Game.Content.Load<Texture2D>("Image/son_on");
            son_off = _screen.ScreenManager.Game.Content.Load<Texture2D>("Image/son_off");

            position1 = new Vector2(480 / 2 - font.MeasureString(reprendre.ToUpper()).X / 2, 300);
            position2 = new Vector2(480 / 2 - font.MeasureString(quitter.ToUpper()).X / 2, 450);
        }

        private void InitilizeLanguages()
        {
            reprendre = lang.AffectationLANG("Reprendre", langue);
            quitter = lang.AffectationLANG("Quitter", langue);
        }

        public void HandleInput(InputState input)
        {
            Vector2 son_pos = new Vector2(10, 720);

            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X > position1.X &&
                        gesture.Position.X < position1.X + font.MeasureString(reprendre.ToUpper()).X &&
                        gesture.Position.Y > position1.Y &&
                        gesture.Position.Y < position1.Y + font.MeasureString(reprendre.ToUpper()).Y)
                    {
                        Reprendre();
                    }
                    if (gesture.Position.X > position2.X &&
                        gesture.Position.X < position2.X + font.MeasureString(quitter.ToUpper()).X &&
                        gesture.Position.Y > position2.Y &&
                        gesture.Position.Y < position2.Y + font.MeasureString(quitter.ToUpper()).Y)
                    {
                        foreach (GameScreen screen in _screen.ScreenManager.GetScreens())
                        {
                            screen.ExitScreen();
                        }
                        _screen.ScreenManager.AddScreen(new BackgroundMenuScreen());
                        _screen.ScreenManager.AddScreen(new MainMenuScreen());
                        song = new Song_Management(_screen.ScreenManager);
                        song.Change_Intro();
                    }
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
        }

        private void Reprendre()
        {
            _screen.ScreenManager.Pause = false;
            song.ToResume();
        }

        public void UpdatePause()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Reprendre();
        }

        public void DrawPause()
        {
            Rectangle r = new Rectangle(0, 0, 480, 800);

            _screen.ScreenManager.SpriteBatch.Draw(_screen.ScreenManager.BlankTexture, r, Color.Black * 0.5f);
            _screen.ScreenManager.SpriteBatch.DrawString(font, reprendre.ToUpper(), position1, Color.White);
            _screen.ScreenManager.SpriteBatch.DrawString(font, quitter.ToUpper(), position2, Color.White);

            _screen.ScreenManager.SpriteBatch.Draw(son, new Vector2(20, 720), Color.White);
            if (music)
            {
                _screen.ScreenManager.SpriteBatch.Draw(son_on, new Vector2(70, 720), Color.White);
            }
            else
            {
                _screen.ScreenManager.SpriteBatch.Draw(son_off, new Vector2(80, 730), Color.White);
            }
        }
    }
}
