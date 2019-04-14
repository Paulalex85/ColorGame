using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Opacity : Element
    {
        float _vitesse;
        Random rand = new Random();
        public int _id_carre;
        int _difficulte_element;
        int _frequence_element;

        bool _float_duree_opacity_rand = true;
        float _float_changement_opacity;
        Timer _timer_change_opacity;
        float _float_duree_opacity = 5000f;
        Timer _timer_duree_opacity;
        bool _annim_opacity = false;

        public Opacity(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {

            InitializationVitesse(_difficulte_element);
            InitializeDureeOpacity(_frequence_element);
                _timer_change_opacity = new Timer(_float_changement_opacity);
            
            _timer_duree_opacity = new Timer(_float_duree_opacity);
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
                case 1: _float_changement_opacity = 3000f; break;
                case 2: _float_changement_opacity = 2750f; break;
                case 3: _float_changement_opacity = 2500f; break;
                case 4: _float_changement_opacity = 2250f; break;
                case 5: _float_changement_opacity = 2000f; break;
                case 6: _float_changement_opacity = 1700f; break;
                case 7: _float_changement_opacity = 1400f; break;
                case 8: _float_changement_opacity = 1100f; break;
                case 9: _float_changement_opacity = 800f; break;
                case 10: _float_changement_opacity = 500f; break;
            }
        }

        private void InitializeDureeOpacity(int variable2)
        {
            switch (variable2)
            {
                case 1: _float_duree_opacity = 1000f; break;
                case 2: _float_duree_opacity = 1500f; break;
                case 3: _float_duree_opacity = 2000f; break;
                case 4: _float_duree_opacity = 2500f; break;
                case 5: _float_duree_opacity = 3000f; break;
                case 6: _float_duree_opacity = 3500f; break;
                case 7: _float_duree_opacity = 4000f; break;
                case 8: _float_duree_opacity = 4500f; break;
                case 9: _float_duree_opacity = 5000f; break;
                case 10: _float_duree_opacity = 5500f; break;
            }
        }

        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            InitializationVitesse(_difficulte_element);
            InitializeDureeOpacity(_difficulte_element);
            foreach (Carre caca in liste_carre)
            {
                if (caca._identifiant == _id_carre)
                {
                    if (_float_duree_opacity_rand && _float_duree_opacity == 5000f)
                    {
                        _frequence_element = rand.Next(1, 11);
                        InitializeDureeOpacity(_frequence_element);
                    }

                    if (_timer_change_opacity.IncreaseTimer(timer) && !_annim_opacity)
                    {
                        _annim_opacity = true;
                    }

                    if (_annim_opacity)
                    {
                        if (!_timer_duree_opacity.IncreaseTimer(timer))
                        {
                            if (_timer_duree_opacity._timer < (_float_duree_opacity / 2))
                            {
                                caca._opacity -= (timer / (_float_duree_opacity / 2));
                            }
                            else
                            {
                                caca._opacity += (timer / (_float_duree_opacity / 2));
                            }
                        }
                        else
                        {
                            if (_float_duree_opacity_rand)
                            {
                                _frequence_element = rand.Next(1, 11);
                                InitializeDureeOpacity(_frequence_element);
                            }
                            _annim_opacity = false;
                        }
                    }
                }
            }
        }
    }
}
