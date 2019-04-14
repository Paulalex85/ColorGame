using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couleur_test.DogeTools;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Couleur_test.GameEngine.Element
{
    class Alterne_Element : Element
    {
        Random rand = new Random();
        int _difficulte_element;
        public int _id_carre;

        Timer _timer_changement;
        public float _temps_changement;

        public Alterne_Element(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {
            InitializationVitesse(_difficulte_element);

            _timer_changement = new Timer(_temps_changement);
        }

        private void InitializationVitesse(int difficulte)
        {
            switch (difficulte)
            {
                case 1: _temps_changement = 12000f; break;
                case 2: _temps_changement = 11000f; break;
                case 3: _temps_changement = 10000f; break;
                case 4: _temps_changement = 9000f; break;
                case 5: _temps_changement = 8000f; break;
                case 6: _temps_changement = 7000f; break;
                case 7: _temps_changement = 6000f; break;
                case 8: _temps_changement = 5000f; break;
                case 9: _temps_changement = 4000f; break;
                case 10: _temps_changement = 3000f; break;
            }
        }

        public override void AUGMENTER_DIFFICULTE()
        {
            if (_difficulte_element < 10)
            {
                _difficulte_element++;
            }
        }

        public override bool DIFFICULTE_IS_10()
        {
            if (_difficulte_element == 10)
            {
                return true;
            }
            return false;
        }

        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif, List<Element> list_element, List<Color> list_couleur, GameScreen screen)
        {
            InitializationVitesse(_difficulte_element);
            if (_timer_changement.IncreaseTimer(timer))
            {
                int a = rand.Next(0, liste_carre.Count());
                int b = 0;
                while (a == b)
                {
                    b = rand.Next(0, liste_carre.Count());
                }

                int id_a = liste_carre[a]._identifiant;
                int id_b = liste_carre[b]._identifiant;

                foreach (var caca in list_element)
                {
                    if (caca.GetId()!= -1)
                    {
                        if (caca is Cercle)
                        {

                        }
                        else
                        {
                            if (caca.GetId() == id_a)
                            {
                                caca.SetId(id_b);
                            }
                            else if (caca.GetId() == id_b)
                            {
                                caca.SetId(id_a);
                            }
                        }
                    }
                }

            }
        }
    }
}
