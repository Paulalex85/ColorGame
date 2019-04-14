using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Objectif_Barre_Remelanger : Element
    {
        Random rand = new Random();
        int _id_carre;
        int _difficulte_element;

        Timer _timer_melanger;
        public float _float_timer_melanger;

        public Objectif_Barre_Remelanger(int parametre, int id_carre)
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
                case 1: _float_timer_melanger = 20000f; break;
                case 2: _float_timer_melanger = 19000f; break;
                case 3: _float_timer_melanger = 18000f; break;
                case 4: _float_timer_melanger = 17000f; break;
                case 5: _float_timer_melanger = 16000f; break;
                case 6: _float_timer_melanger = 15000f; break;
                case 7: _float_timer_melanger = 14000f; break;
                case 8: _float_timer_melanger = 13000f; break;
                case 9: _float_timer_melanger = 11500f; break;
                case 10: _float_timer_melanger = 10000f; break;
            }
        }


        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            InitializationVitesse(_difficulte_element);
            if (_timer_melanger.IncreaseTimer(timer))
            {
                foreach (Carre caca in liste_carre)
                {
                    if (caca._identifiant == _id_carre)
                    {
                        foreach (Objectif_Carre_Ligne pd in ligne_objectif.objectif_liste_ligne)
                        {
                            int id = rand.Next(0, liste_carre.Count());
                            pd._identifiant = liste_carre[id]._identifiant;
                            pd._couleur = liste_carre[id]._couleur;

                        }
                    }
                }
            }
        }
    }
}
