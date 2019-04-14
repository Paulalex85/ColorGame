using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.DogeTools;

namespace Couleur_test
{
    class BackgroundMenuScreen : GameScreen
    {
        List<Carre> _liste_carre = new List<Carre>();
        List<Particule_Mini> _liste_particule = new List<Particule_Mini>();
        Timer _timer = new Timer(2000f);

        Random rand = new Random();

        bool add_particule = true;

        public BackgroundMenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            AjoutCarreRandom();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timer.IncreaseTimer(time) && _liste_carre.Count() < 20)
            {
                AjoutCarreRandom();
            }
            UpdateCarrePosition(time);
            VerificationCarreLimite();
            UpdateParticule();

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        private void UpdateParticule()
        {
            const int max_ajout_per_frame = 2;

            foreach (Particule_Mini caca in _liste_particule)
            {
                caca.UpdatePositionParticule();
                if (caca.VerificationLimite())
                {
                    add_particule = false;
                    caca.Initialization(this);
                }
            }

            if (add_particule)
            {
                int j = rand.Next(0, max_ajout_per_frame);
                for (int i = 0; i < j; i++)
                {
                    _liste_particule.Add(new Particule_Mini(this));
                }
            }
        }

        private void UpdateCarrePosition(float time)
        {
            foreach (Carre caca in _liste_carre)
            {
                caca.UpdatePositionCarre(time);
            }
        }

        private void VerificationCarreLimite()
        {
            foreach (Carre caca in _liste_carre)
            {
                if (caca._position.X > 600)
                {
                    caca._direction.X = -caca._direction.X;
                }
                if (caca._position.X < -150)
                {
                    caca._direction.X = -caca._direction.X;
                }
                if (caca._position.Y < -150)
                {
                    caca._direction.Y = -caca._direction.Y;
                }
                if (caca._position.Y > 1000)
                {
                    caca._direction.Y = -caca._direction.Y;
                }
            }
        }

        private void AjoutCarreRandom()
        {
            Vector2 _position, _direction;

            int cote = rand.Next(0,4);
            switch (cote)
            {
                case 0: _position = new Vector2(-150, -150); _direction = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()); break;
                case 1: _position = new Vector2(600, -150); _direction = new Vector2((float)-rand.NextDouble(), (float)rand.NextDouble()); break;
                case 2: _position = new Vector2(600, 1000); _direction = new Vector2((float)-rand.NextDouble(), (float)-rand.NextDouble()); break;
                case 3: _position = new Vector2(-150, 1000); _direction = new Vector2((float)rand.NextDouble(), (float)-rand.NextDouble()); break;
                default: _position = new Vector2(-150, -150); _direction = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()); break;
            }

            float _rotation = rand.Next(-30, 30);
            float _vitesse = rand.Next(1,4);

            _liste_carre.Add(new Carre(_position, _direction, RandomColor(), new Vector2(100, 100), _rotation, _vitesse, this));
        }

        private Color RandomColor()
        {
            Color caca;
            List<Color> liste_color = new List<Color>(){
                new Color(32, 211, 220), 
                new Color(204, 29, 29), 
                new Color(254, 217, 6),
                new Color(24, 171, 31),
                new Color(255, 78, 17),
                new Color(255, 255, 255),
                new Color(255, 89, 234),
                new Color(23, 68, 243),
                new Color(145, 0, 145)
            };
            caca = liste_color.ElementAt(rand.Next(0, liste_color.Count()));


            return caca;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            Rectangle r = new Rectangle(0, 0, 480, 800);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, Color.Black);

            foreach (Carre caca in _liste_carre)
            {
                caca.DrawCarre(this);
            }
            foreach (Particule_Mini caca in _liste_particule)
            {
                caca.DrawParticule(this);
            }
            spriteBatch.End();
        }
    }
}
