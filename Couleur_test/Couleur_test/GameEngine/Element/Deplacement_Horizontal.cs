using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Deplacement_Honrizontal : Element
    {
        public float _vitesse;
        Vector2 _direction = new Vector2(0, 0);
        Random rand = new Random();
         public int _id_carre;
        int _difficulte_element;
        Detection_collision _collision = new Detection_collision();

        public Deplacement_Honrizontal(int parametre, int id_carre)
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
                _direction.X = 1;
            }
            else
            {
                _direction.X = -1;
            }

            InitializationVitesse(_difficulte_element);
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
                case 1: _vitesse = 0.5f; break;
                case 2: _vitesse = 1f; break;
                case 3: _vitesse = 1.5f; break;
                case 4: _vitesse = 2f; break;
                case 5: _vitesse = 2.5f; break;
                case 6: _vitesse = 3f; break;
                case 7: _vitesse = 3.5f; break;
                case 8: _vitesse = 4f; break;
                case 9: _vitesse = 4.5f; break;
                case 10: _vitesse = 5f; break;
            }
        }

        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            InitializationVitesse(_difficulte_element);
            foreach (Carre caca in liste_carre)
            {
                if (caca._identifiant == _id_carre)
                {
                    VerificationLimite(caca);
                    caca._position.X = caca._position.X += (_direction.X * _vitesse);

                }
            }
        }

        private void VerificationLimite(Carre caca)
        {
            if (_collision.Verification_Limite_X(caca))
            {
                _direction.X = -_direction.X;
            }
            if (_collision.Verification_Limite_Y(caca))
            {
                _direction.Y = -_direction.Y;
            }
        }
    }
}
