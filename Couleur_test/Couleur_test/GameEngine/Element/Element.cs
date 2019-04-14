using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Couleur_test.GameEngine.Element
{
    public class Element
    {
        public int _difficulte;
        public bool _active;
        public int _id_carre;

        public Element(int difficulte, int id_carre)
        {
            _id_carre = id_carre;
            _difficulte = difficulte;
        }

        public virtual void InitializeElement()
        {
        }

        public virtual void SetId(int id)
        {
        }

        public virtual void AUGMENTER_DIFFICULTE()
        {
        }

        public virtual bool DIFFICULTE_IS_10()
        {
            return false;
        }

        public virtual int GetId()
        {
            return _id_carre;
        }

        public virtual void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif)
        {
        }

        public virtual void UpdateElement(float timer, List<Carre> liste_carre, Objectif_ligne ligne_objectif, List<Element> list_element,List<Color> list_couleur, GameScreen screen)
        {
        }
    }
}
