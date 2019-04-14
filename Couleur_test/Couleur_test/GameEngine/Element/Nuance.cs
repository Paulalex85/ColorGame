using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Couleur_test.GameEngine.Element
{
    class Nuance : Element
    {
        int _id_carre;
        int _difficulte_element;

        Random rand = new Random();
        Timer _timer_melanger;
        float _float_timer_melanger;
        Timer _timer_avant_evenement;
        float _float_timer_av_eve;
        bool _no_timer = false;
        bool _nuance_en_cours = false;
        bool _initialize_no_timer = false;

        List<Color> liste_couleur_nuance = new List<Color>();
        List<Color> save_color;

        public Nuance(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {

            InitializationVitesse(_difficulte_element);

            if (!_no_timer)
            {
                _timer_melanger = new Timer(_float_timer_melanger);
                _float_timer_av_eve = 9000f - _float_timer_melanger;
                _timer_avant_evenement = new Timer(_float_timer_av_eve);
            }
        }

        public override void SetId(int id)
        {
            _id_carre = id;
        }

        public override int GetId()
        {
            return _id_carre;
        }

        private void InitializationVitesse(int difficulte)
        {
            switch (difficulte)
            {
                case 1: _float_timer_melanger = 2000f; break;
                case 2: _float_timer_melanger = 2500f; break;
                case 3: _float_timer_melanger = 3000f; break;
                case 4: _float_timer_melanger = 3500f; break;
                case 5: _float_timer_melanger = 4000f; break;
                case 6: _float_timer_melanger = 4500f; break;
                case 7: _float_timer_melanger = 5000f; break;
                case 8: _float_timer_melanger = 5500f; break;
                case 9: _float_timer_melanger = 6000f; break;
                case 10: _no_timer = true; break;
            }
        }


        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif, List<Element> list_element, List<Color> list_couleur, GameScreen screen)
        {
            if (save_color == null)
            {
                save_color = new List<Color>();
                save_color.AddRange(list_couleur);
            }
            // NO TIMER
            if (_no_timer && !_initialize_no_timer)
            {
                int k = rand.Next(0, liste_carre.Count());
                SetCouleurNuance(liste_carre[k]._couleur);
                for (int i = 0; i < list_couleur.Count(); i++)
                {
                    foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                    {
                        if (pd._couleur == liste_carre[i]._couleur)
                        {
                            pd._couleur = liste_couleur_nuance[i];
                        }
                        liste_carre[i]._couleur = liste_couleur_nuance[i];
                        liste_carre[i].Initialize_Texture(liste_couleur_nuance[i], screen);
                    }
                }
                _initialize_no_timer = true;
            }

            // MODE NORMAL WOLOLO
            if(!_no_timer)
            {
                if (_nuance_en_cours)
                {
                    if (_timer_melanger.IncreaseTimer(timer))
                    {
                        _nuance_en_cours = false;
                        for (int i = 0; i < list_couleur.Count(); i++)
                        {
                            foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                            {
                                if (pd._couleur == liste_carre[i]._couleur)
                                {
                                    pd._couleur = save_color[i];
                                }

                            }
                            liste_carre[i]._couleur = save_color[i];
                        }
                    }
                }
                else
                {
                    if (_timer_avant_evenement.IncreaseTimer(timer))
                    {
                        _nuance_en_cours = true;
                        int k = rand.Next(0, liste_carre.Count());
                        SetCouleurNuance(liste_carre[k]._couleur);
                        for (int i = 0; i < list_couleur.Count(); i++)
                        {
                            foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                            {
                                if (pd._couleur == liste_carre[i]._couleur)
                                {
                                    pd._couleur = liste_couleur_nuance[i];
                                }
                            }
                            liste_carre[i]._couleur = liste_couleur_nuance[i];
                        }
                    }
                }
            }
        }

        private void SetCouleurNuance(Color couleur)
        {
            if (couleur == new Color(32, 211, 220))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(13, 89, 92));
                liste_couleur_nuance.Add(new Color(32, 211, 220));
                liste_couleur_nuance.Add(new Color(101, 220, 226));
                liste_couleur_nuance.Add(new Color(41, 90, 92));
                liste_couleur_nuance.Add(new Color(25, 162, 169));
            }
            else if (couleur == new Color(204, 29, 29))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(76, 11, 11));
                liste_couleur_nuance.Add(new Color(204, 29, 29));
                liste_couleur_nuance.Add(new Color(212, 94, 94));
                liste_couleur_nuance.Add(new Color(76, 34, 34));
                liste_couleur_nuance.Add(new Color(153, 22, 22));
            }
            else if (couleur == new Color(254, 217, 6))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(126, 108, 3));
                liste_couleur_nuance.Add(new Color(254, 217, 6));
                liste_couleur_nuance.Add(new Color(254, 229, 82));
                liste_couleur_nuance.Add(new Color(126, 114, 41));
                liste_couleur_nuance.Add(new Color(203, 173, 5));
            }
            else if (couleur == new Color(24, 171, 31))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(35, 247, 45));
                liste_couleur_nuance.Add(new Color(81, 185, 86));
                liste_couleur_nuance.Add(new Color(212, 94, 94));
                liste_couleur_nuance.Add(new Color(109, 247, 116));
                liste_couleur_nuance.Add(new Color(17, 120, 22));
            }
            else if (couleur == new Color(255, 78, 17))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(127, 39, 8));
                liste_couleur_nuance.Add(new Color(255, 135, 93));
                liste_couleur_nuance.Add(new Color(212, 94, 94));
                liste_couleur_nuance.Add(new Color(127, 67, 47));
                liste_couleur_nuance.Add(new Color(204, 62, 13));
            }
            else if (couleur == new Color(145, 0, 145))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(145, 0, 145));
                liste_couleur_nuance.Add(new Color(222, 0, 222));
                liste_couleur_nuance.Add(new Color(164, 49, 164));
                liste_couleur_nuance.Add(new Color(222, 67, 222));
                liste_couleur_nuance.Add(new Color(94, 0, 94));
            }
            else if (couleur == new Color(255, 89, 234))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(255, 89, 234));
                liste_couleur_nuance.Add(new Color(191, 67, 175));
                liste_couleur_nuance.Add(new Color(133, 46, 122));
                liste_couleur_nuance.Add(new Color(94, 33, 87));
                liste_couleur_nuance.Add(new Color(224, 78, 206));
            }
            else if (couleur == new Color(23, 68, 243))
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(23, 68, 243));
                liste_couleur_nuance.Add(new Color(17, 50, 179));
                liste_couleur_nuance.Add(new Color(11, 34, 121));
                liste_couleur_nuance.Add(new Color(8, 23, 82));
                liste_couleur_nuance.Add(new Color(20, 59, 212));
            }
            else
            {
                liste_couleur_nuance.Clear();
                liste_couleur_nuance.Add(new Color(13, 89, 92));
                liste_couleur_nuance.Add(new Color(32, 211, 220));
                liste_couleur_nuance.Add(new Color(101, 220, 226));
                liste_couleur_nuance.Add(new Color(41, 90, 92));
                liste_couleur_nuance.Add(new Color(25, 162, 169));
            }
        }
    }
}
