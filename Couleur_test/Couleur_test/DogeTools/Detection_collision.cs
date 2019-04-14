using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Couleur_test.DogeTools
{
    class Detection_collision
    {
        int height = 800, width = 480, debut_height = 80, height_jeu = 600;

        public Detection_collision()
        {
        }

        public bool Verification_Limite_X(Carre caca)
        {
            List<Vector2> prout = caca.List_point_rectangle(caca._rotationAngle, caca._taille, caca._position);
            foreach (Vector2 jambon in prout)
            {
                if ((jambon.X > width) || (jambon.X < 0))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Verification_Limite_Y(Carre caca)
        {
            List<Vector2> prout = caca.List_point_rectangle(caca._rotationAngle, caca._taille, caca._position);
            foreach (Vector2 jambon in prout)
            {
                if ((jambon.Y > height_jeu + debut_height) || (jambon.Y < debut_height))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
