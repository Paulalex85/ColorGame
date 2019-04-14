using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace XMLObject
{
    public class Sauvegarde
    {
        public Parties ListeSave { get; set; }

        public Sauvegarde() { }
    }

    public class Parties
    {
        public int TypeMatch { get; set; }
        public int RoundMatch { get; set; }
        public int NombreDeManches { get; set; }
        public int NombreManchesJouer { get; set; }
        public int Difficulte { get; set; }
        public string ScoreJoueur { get; set; }
        public string ScoreAdversaire { get; set; }
        public string NomAdversaire { get; set; }

        public Parties() { }
    }

    public class Liste_Level
    {
        public Levels ListeLevel { get; set; }

        public Liste_Level() { }
    }

    public class Levels
    {
        //CARRE
        //public int N_Level { get; set; }
        public int Nombre_carre_debut { get; set; }
        /*public int Ajout_carre_nombre { get; set; }
        public float Ajout_carre_timer { get; set; }
        public int Ajout_carre_nbr_carre { get; set; }*/
        //ELEMENT
        //public int Element_Debut { get; set; }
        public string Detail_Element { get; set; }
        /*public float Ajout_Element_timer { get; set; }
        public int Ajout_Element_carre { get; set; }*/
        //LIGNE OBJECTIF
        public int Objectif_Position_X_Debut { get; set; }
        public float Vitesse_objectif { get; set; }
        /*public float Vitesse_objectif_incrementation { get; set; }
        public float Vitesse_objectif_incrementation_timer { get; set; }
        public int Vitesse_objectif_incrementation_carre { get; set; }*/
        //VICTOIRE
        public int Condition_Victoire { get; set; } 
        public int Carre_a_eliminer { get; set; }
        public float Chrono_a_tenir { get; set; }
        public string Compte_a_rebours { get; set; } //(temps nombre_carre_a_faire)

        public Levels() { }
    }

    public class Langues
    {
        public Valeurs[] Values;

        public Langues() { }
    }

    public class Valeurs
    {
        public string ID;
        public string caca;

        public Valeurs() { }
    }
}
