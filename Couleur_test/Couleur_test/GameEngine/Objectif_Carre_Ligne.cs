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
    public class Objectif_Carre_Ligne
    {
        public Vector2 _position;
        public Color _couleur;
        public Vector2 _taille;
        public int _identifiant;
        private int BorderThickness = 1;
        Random rand = new Random();

        public Objectif_Carre_Ligne(int identifiant, Vector2 position, Color couleur, Vector2 taille)
        {
            _identifiant = identifiant;
            _position = position;
            _couleur = couleur;
            _taille = taille;
        }

        public void DrawPetitCarre(GameScreen screen)
        {
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
            Texture2D blank = screen.ScreenManager.BlankTexture;

            Rectangle r = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_taille.X,
                (int)_taille.Y);

            Rectangle r1 = new Rectangle(r.Left, r.Top, r.Width, BorderThickness);
            Rectangle r2 = new Rectangle(r.Left, r.Top, BorderThickness, r.Height);
            Rectangle r3 = new Rectangle(r.Right - BorderThickness, r.Top, BorderThickness, r.Height);
            Rectangle r4 = new Rectangle(r.Left, r.Bottom - BorderThickness, r.Width, BorderThickness);

            spriteBatch.Draw(blank, r, _couleur);

        }
    }
}
