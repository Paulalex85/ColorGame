using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Couleur_test.DogeTools
{
    public class Evenement
    {
        public enum Type_Evenement
        {
            Carre,
            Element,
            Vitesse,
            Victoire_Carre,
            Victoire_Chrono,
            Victoire_Rebours,
            Fin_de_Partie
        };

        public enum Type_Ajout
        {
            Carre,
            Temps,
            Why_Not_Both
        }

        const float _temps_annimation = 10000f;

        public Type_Evenement _type_evenement;
        public Type_Ajout _type_ajout;
        public bool _evenement_actif;
        int _nombre_carre_but;
        int _nombre_carre_intermediaire;
        Timer _timer_annimation;
        Timer _timer_ajout;



        public Evenement(int nombre_carre_but, float timer_max_ajout, Type_Evenement type_eve)
        {
            _evenement_actif = false;
            _nombre_carre_but = nombre_carre_but;
            _timer_annimation = new Timer(_temps_annimation);
            _type_evenement = type_eve;
            _timer_ajout = new Timer(timer_max_ajout);
            _type_ajout = Determination_Type_Ajout();
        }

        public bool FinDePartie(float timer)
        {
            if (_timer_annimation.IncreaseTimer(timer))
            {
                return true;
            }
            return false;
        }

        public bool VerificationVictoire(float timer)
        {
            if (_type_evenement == Type_Evenement.Victoire_Carre && _nombre_carre_intermediaire >= _nombre_carre_but)
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            else if (_type_evenement == Type_Evenement.Victoire_Chrono && _timer_ajout.IncreaseTimer(timer))
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            else if (_type_evenement == Type_Evenement.Victoire_Rebours && !_timer_ajout.IncreaseTimer(timer) && _nombre_carre_intermediaire >= _nombre_carre_but)
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            return false;
        }

        public bool VerificationPerdu(float timer, bool limite_ligne_carre)
        {
            if (_type_evenement == Type_Evenement.Victoire_Carre && limite_ligne_carre)
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            else if (_type_evenement == Type_Evenement.Victoire_Chrono && _timer_ajout._timer_max < _timer_ajout._timer)
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            else if (_type_evenement == Type_Evenement.Victoire_Rebours && _timer_ajout._timer_max < _timer_ajout._timer || limite_ligne_carre)
            {
                _type_evenement = Type_Evenement.Fin_de_Partie;
                _evenement_actif = true;
                return true;
            }
            return false;
        }

        public bool VerificationEvenement(float timer)
        {
            if (_type_ajout == Type_Ajout.Why_Not_Both && (VerificationTimer(timer) || VerificationCarre()))
            {
                return true;
            }
            else if (_type_ajout == Type_Ajout.Carre && VerificationCarre())
            {
                return true;
            }
            else if (_type_ajout == Type_Ajout.Temps && VerificationTimer(timer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Type_Ajout Determination_Type_Ajout()
        {
            Type_Ajout add;
            if (_nombre_carre_but != 0 && _timer_ajout._timer_max != 0)
            {
                add = Type_Ajout.Why_Not_Both;
            }
            else if (_nombre_carre_but != 0)
            {
                add = Type_Ajout.Carre;
            }
            else
            {
                add = Type_Ajout.Temps;
            }

            return add;
        }

        public void ON_OFF_Evenement()
        {
            if (_evenement_actif)
            {
                _evenement_actif = false;
            }
            else
            {
                _evenement_actif = true;
            }
        }

        public void IncreaseCarre()
        {
            _nombre_carre_intermediaire++;
        }

        private bool VerificationTimer(float time)
        {
            if (_timer_ajout.IncreaseTimer(time))
            {
                _evenement_actif = true;
                return true;
            }
            return false;
        }

        private bool VerificationCarre()
        {
            if (_nombre_carre_but <= _nombre_carre_intermediaire)
            {
                _nombre_carre_intermediaire = 0;
                _evenement_actif = true;
                return true;
            }
            return false;
        }

        public void DureeEvenement(float time)
        {
            if (_evenement_actif && _timer_annimation.IncreaseTimer(time) && _type_evenement != Type_Evenement.Fin_de_Partie)
            {
                _evenement_actif = false;
            }
        }
    }
}
