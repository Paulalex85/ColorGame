using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;
using Couleur_test.MenuScreenClass;
using System.IO.IsolatedStorage;
using Couleur_test.DogeTools;

namespace Couleur_test
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Song_Management song;

        Languages lang = new Languages();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            Content.RootDirectory = "Content";


            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("high_score"))
            {
                IsolatedStorageSettings.ApplicationSettings["high_score"] = "0";
            }
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("music"))
            {
				IsolatedStorageSettings.ApplicationSettings["music"] = "1";
            }

			AfterSplashScreen ();
        }

        private void AfterSplashScreen()
        {
            screenManager.AddScreen(new BackgroundMenuScreen());
            screenManager.AddScreen(new MainMenuScreen());
            song = new Song_Management(this.screenManager);
            song.Change_Intro();
        }

        /// <summary>
        /// LoadContent est appelé une fois par partie. Emplacement de chargement
        /// de tout votre contenu.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent est appelé une fois par partie. Emplacement de déchargement
        /// de tout votre contenu.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Déchargez le contenu non ContentManager ici
        }

        /// <summary>
        /// Permet au jeu d’exécuter la logique de mise à jour du monde,
        /// de vérifier les collisions, de gérer les entrées et de lire l’audio.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Appelé quand le jeu doit se dessiner.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
