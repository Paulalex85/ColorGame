using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test
{
    public class Objectif_ligne
    {
        public bool _inverser = false;
        private const int _nombre_de_carre = 20;
        private const int _taille_carre = 30;
        public float _vitesse = 0.1f;
        public int pointeur_liste = 0;
        public int _debut_x;
        Random rand = new Random();
        public List<Color> _couleur = new List<Color>();
        public List<Particule_Mini> liste_particule = new List<Particule_Mini>();

        public Objectif_Carre_Ligne[] objectif_liste_ligne = new Objectif_Carre_Ligne[_nombre_de_carre];

        public Objectif_ligne(List<Color> couleur, float vitesse, int debut_X)
        {
            _vitesse = vitesse;
            _debut_x = debut_X;
            _couleur.AddRange(couleur);
        }

        public void InitializeCarre()
        {
            for (int i = 0; i < _nombre_de_carre; i++)
            {
                int id = rand.Next(0,_couleur.Count());
                objectif_liste_ligne[i] = new Objectif_Carre_Ligne(id, new Vector2(_debut_x + i * _taille_carre, 800 - _taille_carre), _couleur[id], new Vector2(_taille_carre, _taille_carre));
            }
        }

        public void UpdatePosition()
        {
            if (!_inverser)
            {
                foreach (Objectif_Carre_Ligne x in objectif_liste_ligne)
                {
                    x._position.X -= _vitesse;
                }
            }
            else
            {
                foreach (Objectif_Carre_Ligne x in objectif_liste_ligne)
                {
                    x._position.X += _vitesse;
                }
            }
        }

        public void AugmenterVitesse(float incrementation)
        {
            _vitesse += incrementation;
        }

        public void BonneCouleurTaped(GameScreen screen)
        {
            //InitializationParticule(screen, objectif_liste_ligne[pointeur_liste]._position.X);

            int _blblbl = 0;
            if (_inverser)
            {
                _blblbl = -_taille_carre;
            }
            else
            {
                _blblbl = _taille_carre;
            }
            if (pointeur_liste == 0)
            {
                objectif_liste_ligne[pointeur_liste]._position.X = objectif_liste_ligne[_nombre_de_carre - 1]._position.X + _blblbl;
            }
            else
            {
                objectif_liste_ligne[pointeur_liste]._position.X = objectif_liste_ligne[pointeur_liste - 1]._position.X + _blblbl;
            }
            int id = rand.Next(0, _couleur.Count());
            objectif_liste_ligne[pointeur_liste]._identifiant = id;
            objectif_liste_ligne[pointeur_liste]._couleur = _couleur[id];

            if (pointeur_liste == _nombre_de_carre - 1)
            {
                pointeur_liste = 0;
            }
            else
            {
                pointeur_liste++;
            }
        }

        private void InitializationParticule(GameScreen screen, float position_X)
        {
            int k = rand.Next(4, 10);
            for (int i = 0; i < k; i++)
            {
                liste_particule.Add(new Particule_Mini(screen, new Vector2(position_X, 800 - 30)));
            }

        }

        public bool LimitePerdu()
        {
            if (!_inverser)
            {
                if (objectif_liste_ligne[pointeur_liste]._position.X < 0)
                {
                    return true;
                }
            }
            else
            {
                if (objectif_liste_ligne[pointeur_liste]._position.X + objectif_liste_ligne[pointeur_liste]._taille.X > 480)
                {
                    return true;
                }
            }
            return false;
        }

        public void RefaireObjectifNonAffiche()
        {
            foreach (Objectif_Carre_Ligne caca in objectif_liste_ligne)
            {
                if (!_inverser)
                {
                    if (caca._position.X > 480)
                    {
                        int id = rand.Next(0, _couleur.Count());
                        caca._identifiant = id;
                        caca._couleur = _couleur[id];
                    }
                }
                else
                {
                    if (caca._position.X < 0)
                    {
                        int id = rand.Next(0, _couleur.Count());
                        caca._identifiant = id;
                        caca._couleur = _couleur[id];
                    }
                }
            }
        }

        public void UpdateParticule()
        {
            for (int i = 0; i < liste_particule.Count; i++)
            {
                if (liste_particule[i] != null)
                {
                    liste_particule[i].UpdatePositionParticule();
                    if (liste_particule[i].VerificationLimite())
                    {
                        liste_particule.RemoveAt(i);
                    }
                }
            }
        }

        public void Draw_Particule(GameScreen screen)
        {
            foreach (Particule_Mini caca in liste_particule)
            {
                caca.DrawParticule(screen);
            }
        }

        public void Draw_Objectif_Carre(GameScreen screen)
        {
            foreach (Objectif_Carre_Ligne x in objectif_liste_ligne)
            {
                x.DrawPetitCarre(screen);
            }
        }
    }
}
