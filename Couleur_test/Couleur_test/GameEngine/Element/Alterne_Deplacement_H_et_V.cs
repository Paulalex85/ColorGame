using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Alterne_Deplacement_H_et_V : Element
    {
        Random rand = new Random();
        public int _id_carre;
        int _difficulte_element;
        float _timer_changement_deplacement;
        Timer _timer_change_deplacement;
        Element _element;

        public Alterne_Deplacement_H_et_V(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {

            int k = rand.Next(0, 2);
            if (k == 0)
            {
                _element = new Deplacement_Honrizontal(_difficulte_element, _id_carre);
            }
            else
            {
                _element = new Deplacement_Vertical(_difficulte_element, _id_carre);
            }

            InitializationVitesse(_difficulte_element);
            _timer_change_deplacement = new Timer(_timer_changement_deplacement);
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
                case 1: _timer_changement_deplacement = 3000f; break;
                case 2: _timer_changement_deplacement = 2750f; break;
                case 3: _timer_changement_deplacement = 2500f; break;
                case 4: _timer_changement_deplacement = 2250f; break;
                case 5: _timer_changement_deplacement = 2000f; break;
                case 6: _timer_changement_deplacement = 1700f; break;
                case 7: _timer_changement_deplacement = 1400f; break;
                case 8: _timer_changement_deplacement = 1100f; break;
                case 9: _timer_changement_deplacement = 800f; break;
                case 10: _timer_changement_deplacement = 500f; break;
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

        public override void UpdateElement(float timer, List<Carre> liste_carre,  Objectif_ligne ligne_objectif)
        {
            InitializationVitesse(_difficulte_element);
            if (_timer_change_deplacement.IncreaseTimer(timer))
            {
                if (_element is Deplacement_Honrizontal)
                {
                    _element = new Deplacement_Vertical(_difficulte_element, _id_carre);
                }
                else
                {
                    _element = new Deplacement_Honrizontal(_difficulte_element, _id_carre);
                }
            }
            else
            {
                _element.UpdateElement(timer, liste_carre, ligne_objectif);
            }
        }
    }
}
