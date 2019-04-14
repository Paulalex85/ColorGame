using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Couleur_test.GameEngine.Element
{
    public class ElementManage
    {
        public List<Element> ListeElement;

        public ElementManage()
        {
            ListeElement = new List<Element>();
        }

        public void InitializationElement(string stringElement)
        {
            string[] liste_element_string = stringElement.Split(new char[] { ' ' });
            foreach (string caca in liste_element_string)
            {
                string[] id_element = caca.Split(new char[] { '.' });

                ListeElement.Add(Correspondance_ID_Element(int.Parse(id_element[0]), int.Parse(id_element[2]), int.Parse(id_element[1])));
            }
        }

        public void AjoutElement(int id, int difficulte, int id_carre)
        {
            ListeElement.Add(Correspondance_ID_Element(id,difficulte,id_carre));
        }

        private Element Correspondance_ID_Element(int ID, int parametre, int id_carre)
        {
            switch (ID)
            {
                case 1: return new Deplacement_Honrizontal(parametre, id_carre);
                case 2: return new Deplacement_Vertical(parametre, id_carre);
                case 3: return new Deplacement_Aleatoire(parametre, id_carre);
                case 4: return new Cercle(parametre, id_carre);
                case 5: return new Alterne_Deplacement_H_et_V(parametre, id_carre);
                case 6: return new Deplacement_Diagonale(parametre, id_carre);
                case 7: return new Rotation(parametre, id_carre);
                case 8: return new Opacity(parametre, id_carre);
                case 9: return new Alterne_Couleur(parametre, id_carre);
                case 10: return new Alterne_Element(parametre, id_carre);
                case 11: return new Objectif_Barre_Remelanger(parametre, id_carre);
                case 12: return new Objectif_Barre_Inverser(parametre, id_carre);
                default: return new Deplacement_Honrizontal(parametre, id_carre);
            }
        }
    }
}
