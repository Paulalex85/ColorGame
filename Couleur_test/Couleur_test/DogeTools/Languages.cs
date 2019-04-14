using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using GameStateManagement;
using Microsoft.Xna.Framework.Content;

namespace Couleur_test.DogeTools
{
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

	public class Languages
    {
        CultureInfo Culture;
        public string lang;
        public string path;

        public Languages()
        {
            Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            lang = Culture.ToString().Substring(0, 2);
            path = PathDictionnaire();
        }

        public string AffectationLANG(string id, Langues langue)
        {
            string facial = "";

            foreach (var caca in langue.Values)
            {
                if (caca.ID == id)
                {
                    facial = caca.caca;
                    break;
                }
            }
            return facial;
        }


        public string PathDictionnaire()
        {
			switch (lang) {
			case "fr":
				return "fr";
			//case "nl": return "Languages/Dutch/";
			case "en":
				return "en";
			//case "de": return "Languages/German/";
			//case "it": return "Languages/Italian/";
			//case "pt": return "Languages/Portuguese/";
			//case "es": return "Languages/Spanish/";
			default:
				return "en";
			}
        }
    }
}
