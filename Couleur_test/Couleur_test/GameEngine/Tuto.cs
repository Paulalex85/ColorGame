using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace Couleur_test.GameEngine
{
    class Tuto
    {
        SpriteFont font;
        int mode = 0;

        Languages lang = new Languages();
        Langues langue = new Langues();

        GameScreen _screen;
        string string_1,string_1_bis,string_1_ter, string_2,string_2_bis, next;

        public Tuto(GameScreen screen)
        {
            _screen = screen;
            _screen.ScreenManager.Pause = true;
            Initialize();
        }

        public void Initialize()
        {
            langue = _screen.ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();
            font = _screen.ScreenManager.Game.Content.Load<SpriteFont>("Fin_partie_font");
        }

        private void InitilizeLanguages()
        {
            string_1 = lang.AffectationLANG("String1", langue);
            string_1_bis = lang.AffectationLANG("String1_bis", langue);
            string_1_ter = lang.AffectationLANG("String1_ter", langue);
            string_2 = lang.AffectationLANG("String2", langue);
            string_2_bis = lang.AffectationLANG("String2_bis", langue);
            next = lang.AffectationLANG("Suivant", langue);
        }

        public void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X > 0 &&
                        gesture.Position.X < 480 &&
                        gesture.Position.Y > 0 &&
                        gesture.Position.Y < 800)
                    {
                        Next();
                    }
                }
            }
        }

        private void Next()
        {
            mode++;
            if (mode == 2)
            {
                _screen.ScreenManager.Pause = false;
            }
        }

        public void UpdatePause()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Next();
            }
        }

        public void DrawPause()
        {
            if (mode == 0)
            {
                Rectangle r = new Rectangle(0, 0, 480, 750);

                _screen.ScreenManager.SpriteBatch.Draw(_screen.ScreenManager.BlankTexture, r, Color.Black * 0.7f);
                _screen.ScreenManager.SpriteBatch.DrawString(font,string_1, new Vector2(240 - font.MeasureString(string_1).X/2, 300), Color.White);
                _screen.ScreenManager.SpriteBatch.DrawString(font, string_1_bis, new Vector2(240 - font.MeasureString(string_1_bis).X / 2, 350), Color.White);
                _screen.ScreenManager.SpriteBatch.DrawString(font, string_1_ter, new Vector2(240 - font.MeasureString(string_1_ter).X / 2, 400), Color.White);
                _screen.ScreenManager.SpriteBatch.DrawString(font, next, new Vector2(240 - font.MeasureString(next).X / 2, 500), Color.White);

            }
            else if (mode == 1)
            {
                Rectangle r = new Rectangle(0, 0, 480, 350);
                Rectangle r1 = new Rectangle(0, 550, 480, 250);

                _screen.ScreenManager.SpriteBatch.Draw(_screen.ScreenManager.BlankTexture, r, Color.Black * 0.7f);
                _screen.ScreenManager.SpriteBatch.Draw(_screen.ScreenManager.BlankTexture, r1, Color.Black * 0.7f);

                _screen.ScreenManager.SpriteBatch.DrawString(font, string_2, new Vector2(240 - font.MeasureString(string_2).X / 2, 200), Color.White);
                _screen.ScreenManager.SpriteBatch.DrawString(font, string_2_bis, new Vector2(240 - font.MeasureString(string_2_bis).X / 2, 250), Color.White);

                _screen.ScreenManager.SpriteBatch.DrawString(font, next, new Vector2(240 - font.MeasureString(next).X / 2, 350), Color.White);
            
            }
        }
    }
}
