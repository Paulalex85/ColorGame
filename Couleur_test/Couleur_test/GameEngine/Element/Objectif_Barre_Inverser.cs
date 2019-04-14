using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Objectif_Barre_Inverser :Element
    {
        bool sens_inverse = false;
        Random rand = new Random();
        int _id_carre;
        int _difficulte_element;

        Timer _timer_melanger;
        public float _float_timer_melanger;

        public Objectif_Barre_Inverser(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {
            InitializationVitesse(_difficulte_element);

            _timer_melanger = new Timer(_float_timer_melanger);
        }

        public override void SetId(int id)
        {
            _id_carre = id;
        }

        public override int GetId()
        {
            return _id_carre;
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

        private void InitializationVitesse(int difficulte)
        {
            switch (difficulte)
            {
                case 1: _float_timer_melanger = 6000f; break;
                case 2: _float_timer_melanger = 5500f; break;
                case 3: _float_timer_melanger = 5000f; break;
                case 4: _float_timer_melanger = 4500f; break;
                case 5: _float_timer_melanger = 4000f; break;
                case 6: _float_timer_melanger = 3500f; break;
                case 7: _float_timer_melanger = 3000f; break;
                case 8: _float_timer_melanger = 2500f; break;
                case 9: _float_timer_melanger = 2000f; break;
                case 10: _float_timer_melanger = 1500f; break;
            }
        }


        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            InitializationVitesse(_difficulte_element);
            if (_timer_melanger.IncreaseTimer(timer))
            {
                if (sens_inverse)
                {
                    foreach (Carre caca in liste_carre)
                    {
                        if (caca._identifiant == _id_carre)
                        {
                            foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                            {
                                pd._position.X = InversePosition(pd._position.X, pd._taille.X);
                            }
                        }
                    }

                    sens_inverse = false;
                    ligne_objectif._inverser = false;
                }
                else
                {
                    foreach (Carre caca in liste_carre)
                    {
                        if (caca._identifiant == _id_carre)
                        {
                            foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                            {
                                pd._position.X = InversePosition(pd._position.X, pd._taille.X);
                            }
                        }
                    }

                    ligne_objectif._inverser = true;
                    sens_inverse = true;
                }
            }
        }

        private float InversePosition(float position, float taille)
        {
            float distance;
            float pos;
            if (position <= 240)
            {
                if (position <= 0)
                {
                    distance = -position + 240;
                }
                else
                {
                    distance = 240 - position;
                }
                pos = position + (distance * 2) - taille;
            }
            else
            {
                distance = position - 240;
                pos = position - (distance * 2) - taille;
            }
            /*if (sens)
            {
                distance = position - 240;
                pos = position - (distance * 2) - taille;
            }
            else
            {
                if (position <= 0)
                {
                    distance = -position + 240;
                }
                else
                {
                    distance = 240 - position;
                }
                pos = position + (distance * 2) - taille;
            }*/
            return pos;   
        }
    }
}
