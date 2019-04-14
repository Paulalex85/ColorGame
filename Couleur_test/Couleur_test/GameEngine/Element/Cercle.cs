using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Couleur_test.DogeTools;

namespace Couleur_test.GameEngine.Element
{
    class Cercle : Element
    {
        Vector2 _direction = new Vector2(0, 0);
        Random rand = new Random();
        public int _id_carre;
        int _difficulte_element;
        public float _vitesse;
        Vector2 _distance;
        Vector2 _position_carre_pour_cercle;
        Vector2 _position_carre_avant_positionnement;
        Detection_collision _collision = new Detection_collision();
        Timer _timer_positionnement = new Timer(1000f);
        bool _sens = true;
        bool _positionne = false;
        Vector2 _distance_a_realiser_pour_positionner = new Vector2();

        float RotationAngle = 0f;
        float PiOver128 = MathHelper.Pi / 128;

        public Cercle(int parametre, int id_carre)
            : base(parametre, id_carre)
        {
            _id_carre = id_carre;
            _difficulte_element = parametre;
            InitializeElement();
        }

        public override void InitializeElement()
        {
            _distance.X = 190;
            _distance.Y = rand.Next(200,400);

            if (_distance.Y - 80 > 240 && 680 - _distance.Y > 240)
            {
                _position_carre_pour_cercle.X = 240;
                _position_carre_pour_cercle.Y = rand.Next((int)_distance.Y - 240, (int)_distance.Y + 240)+20;
            }
            else
            {
                int max_possible = (int)_distance.Y -80;
                if(max_possible > 680 - _distance.Y)
                {
                    max_possible = 680 - (int)_distance.Y;
                }
                _position_carre_pour_cercle.X = 240;
                _position_carre_pour_cercle.Y = _distance.Y - max_possible+20; 
            }
            int k = rand.Next(0, 2);
            if (k == 0)
            {
                _sens = true;
            }
            else
            {
                _sens = false;
            }
            InitializationVitesse(_difficulte_element);
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

        public override void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
            foreach (Carre caca in liste_carre)
            {
                if (caca._identifiant == _id_carre)
                {
                    InitializationVitesse(_difficulte_element);
                    if (caca._point_rotation_cercle == new Vector2(0, 0))
                    {
                        caca._point_rotation_cercle = _distance;
                    }
                    //VerificationLimite(caca);
                    if (_positionne)
                    {
                        if (_sens)
                        {
                            RotationAngle += timer;
                        }
                        else
                        {
                            RotationAngle -= timer;
                        }
                        float circle = MathHelper.Pi * 2;
                        RotationAngle = RotationAngle % circle * (_vitesse / 1000);

                        if (RotationAngle > 360)
                        {
                            RotationAngle = 0;
                        }
                        else
                        {
                            caca._position = RotatePoint(caca._position, caca._point_rotation_cercle, RotationAngle);
                        }
                    }
                    else
                    {
                        if (_timer_positionnement.IncreaseTimer(timer))
                        {
                            _positionne = true;
                            caca._position = _position_carre_pour_cercle;
                        }
                        else
                        {
                            if(_position_carre_avant_positionnement == Vector2.Zero)
                            {
                                _position_carre_avant_positionnement = caca._position;
                                _distance_a_realiser_pour_positionner = _position_carre_pour_cercle - _position_carre_avant_positionnement;
                            }

                            caca._position = _position_carre_avant_positionnement + (_distance_a_realiser_pour_positionner * _timer_positionnement._timer / _timer_positionnement._timer_max);
                        }
                    }
                }
            }
        }

        private void VerificationLimite(Carre caca)
        {
            if (_collision.Verification_Limite_X(caca) || _collision.Verification_Limite_Y(caca))
            {
                RotationAngle = 0;
                if(_sens)
                {
                    _sens = false;
                }
                else
                {
                    _sens = true;
                }
            }
        }

        static double DegreesToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);

        }

        private Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, double _angleInRadians)
        {
            double angleInRadians = _angleInRadians;
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta *(pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
    }
}
