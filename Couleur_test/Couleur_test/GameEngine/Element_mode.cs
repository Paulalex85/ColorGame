using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows;
using Couleur_test.DogeTools;
using Couleur_test.GameEngine.Element;
using Microsoft.Xna.Framework.Graphics;
using Couleur_test.MenuScreenClass;
using Couleur_test.GameEngine;
using System.IO.IsolatedStorage;

namespace Couleur_test
{
    public class Element_mode : GameScreen
    {
        #region VARIABLES
        //PARAMETRE GENERAUX
        int height = 800, width = 480;
        float _time_montre = 0f;
        string temps_best = (string)IsolatedStorageSettings.ApplicationSettings["high_score"];
        float[] scores;
        bool mode_high_score;

        Song_Management song;

        Languages lang = new Languages();
        Langues langue = new Langues();
        Pause pause = null;
        Fin_Partie fin_partie = null;
        Background_Ingame background;
        Tuto _tuto;
        public enum Statut_Partie
        {
            En_cours,
            Perdu,
            Initialization,
            Pause,
            Tuto
        }

        public Statut_Partie _statut_partie;
        bool _initialization_done = false;

        //ELEMENT
        ElementManage _element_manage = new ElementManage();
        List<Element> _element_a_ajouter = new List<Element>();

        // PARAMETRE COULEUR
        List<Color> liste_color;
        public List<Carre> liste_carre { get; set; }
        private List<Color> _color_argument = new List<Color>();
        private List<Color> _color_annimation = new List<Color>() { new Color(255, 45, 255), new Color(45, 255, 255), new Color(255, 255, 45) };

        //PARAMETRE LEVEL
        int _nombre_carre, _objectif_position_x_debut, _numero_level;
        float _vitesse_objectif, _vitesse_objectif_incrementation;

        //TIMER INITIALIZATION
        Timer _timer_initialization_avant_go = new Timer(1000f);

        //OBJECTIF LIGNE CARRE
        Objectif_ligne objectif_ligne_carre;
        int _nombre_de_objectif_effectue;

        //DIVERS
        Random rand = new Random();

        //BOUTON PAUSE
        Vector2 _pause_taille = new Vector2(50, 50);
        Vector2 _pause_position = new Vector2(20, 700);

        //EVENEMENT
        bool _eve_level;
        bool _eve_3;
        bool _eve_2;
        bool _eve_1;
        bool _eve_go;
        bool _annim_interface;
        Timer _increase_level = new Timer(5000f);
        Timer _temps_annim = new Timer(1500f);
        Vector2 _position_texte_evenement = new Vector2(0, 0);
        Vector2 _position_texte_evenement_high_score = new Vector2(0, 0);
        Color _color_annimation_texte;
        Color _color_high_score;
        Color _color_texte;
        

        //DRAW_VARIABLES
        SpriteFont font_evenement;
        string string_game_over, string_points, string_level, string_plus_vite, string_plus_difficile, string_carre_restant, string_chrono, string_highcore1, string_highscore2, string_objectif;

        //INTERFACE
        Rectangle _black_haut = new Rectangle(0, 0, 480, 80);
        Rectangle _black_bas = new Rectangle(0, 680, 480, 120);
        Color _interface_color = Color.Black;
        Timer _timer_evenement_interface = new Timer(500f);
        int _frame_interface_annim_max = 4;
        int _frame_interface_annim = 0;

        //POSITION CARRE
        int p_X1 = 95, p_X2 = 290;
        int p_Y1 = 120, p_Y2 = 260, p_Y3 = 400, p_Y4 = 540;
        #endregion

        public Element_mode(int nombre_carre)
        {
            _nombre_carre = nombre_carre;
            liste_carre = new List<Carre>();
            _numero_level = 1;
            _statut_partie = Statut_Partie.Initialization;
            EnabledGestures = GestureType.Tap;
        }

        #region LOAD

        public override void LoadContent()
        {
            string[] k = temps_best.Split(new char[] { '-' });
            scores = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                scores[i] = float.Parse(k[i]);
            }

            song = new Song_Management(this.ScreenManager);
            background = new Background_Ingame(this, _nombre_carre);
            if (temps_best == "0")
            {
                _tuto = new Tuto(this);
                _statut_partie = Statut_Partie.Tuto;
            }

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path);
            InitilizeLanguages();

            font_evenement = ScreenManager.Game.Content.Load<SpriteFont>("menufont");

            _color_texte = Color.White;

            _nombre_de_objectif_effectue = 0;

            liste_color = new List<Color>(){
                new Color(32, 211, 220), 
                new Color(204, 29, 29), 
                new Color(254, 217, 6),
                new Color(24, 171, 31),
                new Color(255, 78, 17),
                new Color(255, 255, 255),
                new Color(255, 89, 234),
                new Color(23, 68, 243),
                new Color(145, 0, 145)
            };

            Initialize_Carre();

            Initialize_Objectif();

            _eve_3 = true;
            EvenementTexteInitializationTEMPS();

            _initialization_done = true;

            base.LoadContent();
        }

        private void InitilizeLanguages()
        {
            string_objectif = lang.AffectationLANG("Objectif", langue);
            string_highcore1 = lang.AffectationLANG("NewHighScore", langue);
            string_highscore2 = lang.AffectationLANG("MeilleurTemps", langue);
            string_points = lang.AffectationLANG("Points", langue);
            string_plus_difficile = lang.AffectationLANG("Plus_Difficile", langue);
            string_plus_vite = lang.AffectationLANG("Plus_Vite", langue);
            string_chrono = lang.AffectationLANG("Temps", langue);
            string_carre_restant = lang.AffectationLANG("Carre_restant", langue);
            string_game_over = lang.AffectationLANG("GameOver", langue);
            string_level = lang.AffectationLANG("Level", langue);
        }

        private void Initialize_Objectif()
        {
            objectif_ligne_carre = new Objectif_ligne(_color_argument, _vitesse_objectif, _objectif_position_x_debut);
            objectif_ligne_carre.InitializeCarre();
        }

        private void AssignationDesParametres()
        {
            _objectif_position_x_debut = width / 2;
            _vitesse_objectif = 0.5f;
            _vitesse_objectif_incrementation = 0.00f;
        }

        private void Initialize_Carre()
        {
            for (int i = 0; i < _nombre_carre; i++)
            {
                liste_carre.Add(new Carre(i, Position_Carre_Debut(i), new Vector2(0, 0), RandomColor(), new Vector2(125, 125), true, this));
            }
        }

        private Vector2 Position_Carre_Debut(int i_carre)
        {
            switch (i_carre)
            {
                case 0: return new Vector2(p_X1, p_Y3);
                case 1: return new Vector2(p_X2, p_Y3);
                case 2: return new Vector2(p_X1, p_Y2);
                case 3: return new Vector2(p_X2, p_Y2);
                case 4: return new Vector2(p_X1, p_Y1);
                case 5: return new Vector2(p_X2, p_Y1);
                case 6: return new Vector2(p_X1, p_Y4);
                case 7: return new Vector2(p_X2, p_Y4);
                default: return new Vector2(p_X1, p_Y3);
            }
        }

        private Color RandomColor()
        {
            Color caca = liste_color.ElementAt(rand.Next(0, liste_color.Count()));
            _color_argument.Add(caca);
            liste_color.Remove(caca);
            return caca;
        }

        #endregion

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #region INPUT

        public override void HandleInput(InputState input)
        {
            if (_statut_partie == Statut_Partie.En_cours)
            {
                foreach (GestureSample gesture in input.Gestures)
                {
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        if (gesture.Position.X > _pause_position.X &&
                            gesture.Position.X < _pause_position.X + _pause_taille.X &&
                            gesture.Position.Y > _pause_position.Y &&
                            gesture.Position.Y < _pause_position.Y + _pause_taille.Y)
                        {
                            Pause();
                            
                        }
                        else
                        {
                            Carre carre_taped = null;
                            foreach (Carre caca in liste_carre)
                            {
                                if (caca.HandleTap(gesture.Position))
                                {
                                    carre_taped = caca;
                                }
                            }
                            if (carre_taped != null)
                            {
                                foreach (Carre caca in liste_carre)
                                {
                                    if (caca == carre_taped)
                                    {
                                        caca._bool_ajout_particule = true;
                                        caca._taped_annim_scale = true;
                                    }
                                }

                                VerificationBonCarreTaped(carre_taped._identifiant);
                            }
                        }
                    }
                }
            }
            else if (_statut_partie == Statut_Partie.Pause)
            {
                pause.HandleInput(input);
            }
            else if (_statut_partie == Statut_Partie.Perdu)
            {
                fin_partie.HandleInput(input);
            }
            else if (_statut_partie == Statut_Partie.Tuto)
            {
                _tuto.HandleInput(input);
            }

            base.HandleInput(input);
        }

        private void Pause()
        {
            this.ScreenManager.Pause = true;
            _statut_partie = Statut_Partie.Pause;
            pause = new Pause(this);
        }
        #endregion

        #region UPDATE
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new BackgroundMenuScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
                song = new Song_Management(this.ScreenManager);
                song.Change_Intro();
            }

            song.UpdateSong();

            float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_initialization_done && _statut_partie == Statut_Partie.En_cours)
            {
                GestionDesEvenementsDuree(timer);

                Update_Liste_Carre_Par_Element(timer);

                objectif_ligne_carre.UpdatePosition();

                Gagner_Perdu(timer);

                _time_montre += timer;

                GestionAnnimationColorInterface(timer);

                CarreUpdate(timer);
                UpdateParticule();
                UpdateBackground(timer);
                AnnimColor();
                GestionHighScore();
                REPOSIOTNEMENTMESCOUILLES();
                
            }
            else if (_initialization_done && _statut_partie == Statut_Partie.Initialization)
            {
                Update_Liste_Carre_Par_Element(timer);
                Statut_partie_initialization(timer);
                UpdateParticule();
            }
            else if (_statut_partie == Statut_Partie.Pause)
            {
                pause.UpdatePause();
                Statut_sortie_de_pause();
            }
            else if (_statut_partie == Statut_Partie.Perdu)
            {
                fin_partie.Update(timer);
            }
            else if (_statut_partie == Statut_Partie.Tuto)
            {
                _tuto.UpdatePause();
                Statut_sortie_de_pause();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void REPOSIOTNEMENTMESCOUILLES()
        {
            foreach (Carre caca in liste_carre)
            {
                if (caca._position.Y < 70)
                {
                    caca._position.Y = 200;
                }
            }
        }

        private void GestionHighScore()
        {
            if (scores.Length >= _nombre_carre - 1 && scores[_nombre_carre - 2] > 1 && _time_montre > scores[_nombre_carre - 2])
            {
                mode_high_score = true;
                _position_texte_evenement_high_score = new Vector2(480 / 2 - font_evenement.MeasureString(string_highcore1 + " " + string_highscore2).X / 2, 200);
                _color_texte = Color.Black;
            }
        }

        private void UpdateBackground(float timer)
        {
            background.Update_Background(timer);
        }

        private void UpdateParticule()
        {
            objectif_ligne_carre.UpdateParticule();
        }

        private void CarreUpdate(float timer)
        {
            foreach (Carre caca in liste_carre)
            {
                caca.UpdateCarre(timer, this);
            }
        }

        private void Statut_sortie_de_pause()
        {
            if (this.ScreenManager.Pause == false && _statut_partie == Statut_Partie.Pause)
            {
                pause = null;
                _statut_partie = Statut_Partie.Initialization;
                _eve_3 = true;
                EvenementTexteInitializationTEMPS();
            }
            else if (this.ScreenManager.Pause == false && _statut_partie == Statut_Partie.Tuto)
            {
                _statut_partie = Statut_Partie.Initialization;
            }
        }

        private void Statut_partie_initialization(float timer)
        {
            GestionPosition321GO(timer);
            if (_timer_initialization_avant_go.IncreaseTimer(timer))
            {
                if (_eve_3)
                {
                    _eve_3 = false;
                    _eve_2 = true;
                    EvenementTexteInitializationTEMPS();
                }
                else if (_eve_2)
                {
                    _eve_2 = false;
                    _eve_1 = true;
                    EvenementTexteInitializationTEMPS();
                }
                else if (_eve_1)
                {
                    _eve_1 = false;
                    _eve_go = true;
                    EvenementTexteInitializationTEMPS();
                }
                else
                {
                    _eve_go = false;
                    _statut_partie = Statut_Partie.En_cours;
                }
            }
        }

        private void Gagner_Perdu(float timer)
        {
            if (objectif_ligne_carre.LimitePerdu())
            {
                _statut_partie = Statut_Partie.Perdu;
                fin_partie = new Fin_Partie(_time_montre,_numero_level, this,_nombre_carre);
            }
        }

        private void GestionDesEvenementsDuree(float timer)
        {
            DureeEve(timer);
            if (_numero_level < 100 && _increase_level.IncreaseTimer(timer))
            {
                _numero_level++;
                _eve_level = true;
                EvenementTexteInitialization();
                _annim_interface = true;
                background.Level_Increase();
                AugmentationVitesseEnCoursDeLevel(timer);
                if (_numero_level == 2)
                {
                    for (int i = 0; i < liste_carre.Count(); i++)
                    {
                        AjoutElement(true, i);
                    }
                }
                else if(_numero_level < 10)
                {
                    foreach (Element caca in _element_manage.ListeElement)
                    {
                        if (caca is Alterne_Deplacement_H_et_V || caca is Deplacement_Aleatoire || caca is Deplacement_Diagonale || caca is Deplacement_Honrizontal || caca is Deplacement_Vertical || caca is Cercle || caca is Rotation)
                        {
                            caca.AUGMENTER_DIFFICULTE();
                        }
                    }
                }


                if (_numero_level >= 8)
                {
                    int k = rand.Next(0, 2);
                    if (k == 1)
                    {
                        AjoutElement(false, -1);
                    }
                    else
                    {
                        UpgradeElement();
                    }
                }
            }
        }

        private void EvenementTexteInitialization()
        {
            _position_texte_evenement = new Vector2(0 - font_evenement.MeasureString(string_level + " " + _numero_level.ToString()).X, 250);
            int k = rand.Next(0, _color_annimation.Count);
            _color_annimation_texte = _color_annimation[k];
        }

        private void EvenementTexteInitializationHighScore()
        {
            _position_texte_evenement = new Vector2(0 - font_evenement.MeasureString(string_level + " " + _numero_level.ToString()).X, 200);
            int k = rand.Next(0, _color_annimation.Count);
            _color_annimation_texte = _color_annimation[k];
        }

        private void EvenementTexteInitializationTEMPS()
        {
            _position_texte_evenement = new Vector2(0 - font_evenement.MeasureString("GO").X, 250);
            int k = rand.Next(0, _color_annimation.Count);
            _color_annimation_texte = _color_annimation[k];
        }

        private void AnnimColor()
        {
            _frame_interface_annim++;
            if (_frame_interface_annim >= _frame_interface_annim_max)
            {
                Color caca = _color_high_score;
                while (caca == _color_high_score)
                {
                    int r = rand.Next(0, _color_annimation.Count);
                    _color_high_score = _color_annimation[r];
                }
                _frame_interface_annim = 0;
            }

        }

        private void DureeEve(float timer)
        {
            if (_eve_level)
            {
                if (_temps_annim.IncreaseTimer(timer))
                {
                    _eve_level = false;
                }
                else
                {
                    int pos_debut_slow = width / 2 - 150;
                    int pos_fin_slow = pos_debut_slow + 100;
                    int distance = (int)font_evenement.MeasureString(string_level + " " + _numero_level.ToString()).X + pos_debut_slow;
                    
                    if (_position_texte_evenement.X < pos_debut_slow || _position_texte_evenement.X > pos_fin_slow )
                    {
                        _position_texte_evenement.X += distance * timer / (_temps_annim._timer_max / 5);
                    }
                    else
                    {
                        _position_texte_evenement.X += font_evenement.MeasureString(string_level + " " + _numero_level.ToString()).X * timer / (_temps_annim._timer_max * 3 / 5);
                    }
                }
            }
        }

        private void GestionPosition321GO(float timer )
        {
            if (_eve_go || _eve_3 || _eve_2 || _eve_1)
            {
                int pos_debut_slow = width / 2 - 50;
                int pos_fin_slow = pos_debut_slow + 100;
                int distance = (int)font_evenement.MeasureString("GO").X + pos_debut_slow;

                if (_position_texte_evenement.X < pos_debut_slow || _position_texte_evenement.X > pos_fin_slow)
                {
                    _position_texte_evenement.X += distance * timer / (_temps_annim._timer_max / 7);
                }
                else
                {
                    _position_texte_evenement.X += 100 * timer / (_temps_annim._timer_max * 3 / 5);
                }
            }
        }

        private void GestionAnnimationColorInterface(float timer)
        {
            if (_annim_interface)
            {
                if (_timer_evenement_interface.IncreaseTimer(timer))
                {
                    _annim_interface = false;
                    _interface_color = Color.Black;
                    background.Change_Couleur_Annimation(Color.White, true);
                }
                else
                {
                    _frame_interface_annim++;
                    if (_frame_interface_annim >= _frame_interface_annim_max)
                    {
                        Color caca = _interface_color;
                        while (caca == _interface_color)
                        {
                            int r = rand.Next(0, _color_annimation.Count);
                            _interface_color = _color_annimation[r];
                            background.Change_Couleur_Annimation(_color_annimation[r], false);
                        }
                        _frame_interface_annim = 0;
                    }
                }
            }
        }
    
        private void AjoutElement(bool deplacement_only, int N_carre)
        {
            bool add = false;
            int n_element = 0;
            int aleatoire = rand.Next(0, 100);
            if (deplacement_only)
            {
                n_element = rand.Next(1, 7);
            }
            else
            {
                if (_numero_level < 5)
                {
                    if (aleatoire < 50)
                    {
                        n_element = rand.Next(1, 7);
                    }
                    else
                    {
                        n_element = 7;
                    }
                }
                else
                {
                    n_element = rand.Next(1, 12);
                }
            }
            int n_carre;
            if (N_carre == -1)
            {
                n_carre = rand.Next(0, liste_carre.Count());
            }
            else
            {
                n_carre = N_carre;
            }

            if (n_element == 1 || n_element == 2 || n_element == 4 || n_element == 5 || n_element == 6 || n_element == 3)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Deplacement_Honrizontal)
                        {
                            caca.AUGMENTER_DIFFICULTE();
                            add = true;
                            break;
                        }
                        else if (caca is Deplacement_Vertical)
                        {
                            caca.AUGMENTER_DIFFICULTE();
                            add = true;
                            break;
                        }
                        else if (caca is Cercle)
                        {
                            caca.AUGMENTER_DIFFICULTE();

                            add = true;
                            break;
                        }
                        else if (caca is Alterne_Deplacement_H_et_V)
                        {
                            caca.AUGMENTER_DIFFICULTE();
                            add = true;
                            break;
                        }
                        else if (caca is Deplacement_Diagonale)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                        else if (caca is Deplacement_Aleatoire)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            else if (n_element == 7)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Rotation)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            else if (n_element == 8)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Opacity)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            else if (n_element == 9)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Alterne_Couleur)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }
            else if (n_element == 10)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Alterne_Element)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            else if (n_element == 11)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Objectif_Barre_Remelanger)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            else if (n_element == 12)
            {
                foreach (Element caca in _element_manage.ListeElement)
                {
                    if (caca._id_carre == n_carre)
                    {
                        if (caca is Objectif_Barre_Inverser)
                        {
                            add = true;
                            caca.AUGMENTER_DIFFICULTE();
                            break;
                        }
                    }
                }
            }

            if (!add)
            {
                _element_manage.AjoutElement(n_element, 1, n_carre);
            }
        }

        private void UpgradeElement()
        {
            bool k = false;
            List<int> _index_a_add = new List<int>();
            for (int i = 0; i < _element_manage.ListeElement.Count(); i++)
            {
                _index_a_add.Add(i);
            }
            while (!k)
            {
                int z = rand.Next(0, _index_a_add.Count());
                if (!_element_manage.ListeElement[_index_a_add[z]].DIFFICULTE_IS_10())
                {
                    _element_manage.ListeElement[_index_a_add[z]].AUGMENTER_DIFFICULTE();
                    k = true;
                }
                else
                {
                    _index_a_add.RemoveAt(z);
                }
                if (_index_a_add.Count() == 0)
                {
                    k = true;
                    AjoutElement(false, -1);
                }
            }
        }


        private void Update_Liste_Carre_Par_Element(float timer)
        {
            foreach (Element caca in _element_manage.ListeElement)
            {
                if (caca is Alterne_Element || caca is Nuance || caca is Alterne_Couleur_Barre_Carre || caca is Alterne_Couleur)
                {
                    caca.UpdateElement(timer, liste_carre, objectif_ligne_carre, _element_manage.ListeElement, _color_argument, this);
                }
                else
                {
                    caca.UpdateElement(timer, liste_carre, objectif_ligne_carre);
                }
            }
        }

        private void AugmentationVitesseEnCoursDeLevel(float timer)
        {
            objectif_ligne_carre.AugmenterVitesse(_vitesse_objectif_incrementation);
        }

        /*private void AjoutCarreEnCoursDeLevel(float timer)
        {
            //Variables modifié généralements
            _nombre_carre++;

            //position carre
            int position_y = rand.Next(0, 500) + 80;
            int position_x = rand.Next(0, 380);
            Vector2 _position_carre = new Vector2(position_x, position_y);

            Color _couleur_a_add = RandomColor();
            int id = _nombre_carre - 1;
            liste_carre.Add(new Carre(id, _position_carre, new Vector2(0, 0), _couleur_a_add, new Vector2(100, 100), true, this));
            objectif_ligne_carre._couleur.Add(_couleur_a_add);

            //update_objectif
            objectif_ligne_carre.RefaireObjectifNonAffiche();

            //objectif_ligne_carre.AugmenterVitesse(-(_vitesse_objectif_incrementation * 8));
        }*/

        private bool EstLevel5(int level)
        {
            string l = level.ToString();
            int nombre = int.Parse(l.Substring(l.Length - 1, 1));
            if (nombre == 5)
            {
                return true;
            }
            return false;
        }

        private void AjoutElementEnCoursDeLevel(float timer)
        {
            //ECRIRE LE CODE D AJOUT
        }

        private void VerificationBonCarreTaped(int id)
        {
            if (objectif_ligne_carre.objectif_liste_ligne[objectif_ligne_carre.pointeur_liste]._identifiant == id)
            {
                objectif_ligne_carre.BonneCouleurTaped(this);

                _nombre_de_objectif_effectue++;
            }
            else
            {
                if (!objectif_ligne_carre._inverser)
                {
                    foreach (Objectif_Carre_Ligne x in objectif_ligne_carre.objectif_liste_ligne)
                    {
                        x._position.X -= 30;
                    }
                }
                else
                {
                    foreach (Objectif_Carre_Ligne x in objectif_ligne_carre.objectif_liste_ligne)
                    {
                        x._position.X += 30;
                    }
                }
            }
        }

        #endregion
        #region DRAW
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            DrawBackground();
            foreach (Carre caca in liste_carre)
            {
                caca.DrawCarre(this);
                caca.DrawParticule(this);
            }

            Draw_Messages();

            DrawInterface();

            objectif_ligne_carre.Draw_Objectif_Carre(this);

            objectif_ligne_carre.Draw_Particule(this);

            if (_statut_partie == Statut_Partie.Initialization)
            {
                Draw_Compte_a_rebours_Initialization();
                DrawObjectif();
            }
            if (_statut_partie == Statut_Partie.Tuto)
            {
                _tuto.DrawPause();
            }

            if (pause != null)
            {
                pause.DrawPause();
            }
            if (fin_partie != null)
            {
                fin_partie.Draw();
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawObjectif()
        {
            ScreenManager.SpriteBatch.DrawString(font_evenement, string_objectif + " " + "1:'00\"000", new Vector2(480 / 2 - font_evenement.MeasureString(string_objectif + " " + "1:'00\"000").X / 2, 650), _color_texte);
        }

        private void DrawBackground()
        {
            background.Draw_Background();
        }

        private void DrawInterface()
        {
            //Draw BLACK MERDE
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, _black_haut, _interface_color);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, _black_bas, _interface_color);

            //DRAW PAUSE A FAIRE
            Draw_Pause();

            //DRAW POINTS
            ScreenManager.SpriteBatch.DrawString(font_evenement, Minutes(_time_montre) + ":" + Secondes(_time_montre) + "'" + Millisecondes(_time_montre), new Vector2(130, 710), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_evenement, _numero_level.ToString(), new Vector2(400, 710), Color.White);
        }

        private void Draw_Compte_a_rebours_Initialization()
        {
            if (_eve_3)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, "3", _position_texte_evenement, _color_texte);
            }
            else if (_eve_2)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, "2", _position_texte_evenement, _color_texte);
            }
            else if (_eve_1)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, "1", _position_texte_evenement, _color_texte);
            }
            else if (_eve_go)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, "GO", _position_texte_evenement, _color_texte);
            }
        }

        private string Minutes(float temps)
        {
            int blbl = (int)temps / 60000;
            if (blbl > 0)
            {
                return blbl.ToString();
            }
            else
            {
                return "0";
            }
        }

        private string Secondes(float temps)
        {
            double caca = TimeSpan.FromMilliseconds(temps).TotalSeconds;
            int blbl = (int)caca;
            if (caca < 60)
            {
                if (caca < 10)
                {
                    return "0" + blbl;
                }
                else if (blbl == 0)
                {
                    return "00";
                }
                else
                {
                    return blbl.ToString();
                }
            }
            else
            {
                int k = (int)caca / 60;
                blbl = (int)caca - (60 * k);
                if (caca < 10)
                {
                    return "0" + blbl;
                }
                else if (blbl == 0)
                {
                    return "00";
                }
                else
                {
                    return blbl.ToString();
                }
            }
        }

        private string Millisecondes(float temps)
        {
            double no_virg = TimeSpan.FromMilliseconds(temps).TotalMilliseconds;
            if (no_virg > 999)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(c - 3, 3);
            }
            else if (no_virg > 99)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 3);
            }
            else if (no_virg > 9)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 2);
            }
            else if (no_virg > 0)
            {
                string blbl = no_virg.ToString();
                int c = blbl.Length;
                return temps.ToString().Substring(0, 1);
            }
            else
            {
                return "000";
            }
        }

        private void Draw_Messages()
        {
            if (_eve_level && _statut_partie != Statut_Partie.Initialization)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, string_level + " " + _numero_level.ToString(), _position_texte_evenement, _color_annimation_texte);
            }
            if (_statut_partie == Statut_Partie.Perdu)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, string_game_over, _position_texte_evenement, _color_annimation_texte);
            }
            if (mode_high_score)
            {
                ScreenManager.SpriteBatch.DrawString(font_evenement, string_highcore1 + " " + string_highscore2, _position_texte_evenement_high_score, _color_high_score);
            }
        }

        private void Draw_Pause()
        {
            Vector2 dimensionRectangle = new Vector2(_pause_taille.X / 3, _pause_taille.Y);
            Rectangle r1 = new Rectangle((int)_pause_position.X, (int)_pause_position.Y, (int)dimensionRectangle.X, (int)dimensionRectangle.Y);
            Rectangle r2 = new Rectangle((int)_pause_position.X + (int)dimensionRectangle.X, (int)_pause_position.Y, (int)dimensionRectangle.X, (int)dimensionRectangle.Y);
            Rectangle r3 = new Rectangle((int)_pause_position.X + (int)dimensionRectangle.X * 2, (int)_pause_position.Y, (int)dimensionRectangle.X, (int)dimensionRectangle.Y);

            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r1, Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r2, Color.Black*0);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r3, Color.White);
        }

        #endregion
    }
}
