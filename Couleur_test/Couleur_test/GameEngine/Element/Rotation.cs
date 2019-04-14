using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Rotation : Element
    {
        Vector2 _direction = new Vector2(0, 0);
        Random rand = new Random();
        int _id_carre;
        int _difficulte_element;

        float PiOver128 = MathHelper.Pi / 128;
        public float _rotation;

        public Rotation(int parametre, int id_carre)
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
                _rotation = -_rotation;
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

        private void InitializationVitesse(int difficulte)
        {
            switch (difficulte)
            {
                case 1: _rotation = 10f; break;
                case 2: _rotation = 20f; break;
                case 3: _rotation = 30f; break;
                case 4: _rotation = 40f; break;
                case 5: _rotation = 50f; break;
                case 6: _rotation = 60f; break;
                case 7: _rotation = 70f; break;
                case 8: _rotation = 80f; break;
                case 9: _rotation = 90f; break;
                case 10: _rotation = 100f; break;
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

        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            foreach (Carre caca in liste_carre)
            {
                if (caca._identifiant == _id_carre)
                {
                    caca._rotationAngle += PiOver128 * (_rotation / 100);
                    if (caca._rotationAngle > MathHelper.Pi * 2)
                    {
                        caca._rotationAngle = 0;
                    }
                    InitializationVitesse(_difficulte_element);
                }
            }
        }
    }
}
