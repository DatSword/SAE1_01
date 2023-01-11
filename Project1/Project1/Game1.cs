﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using AnimatedSprite = MonoGame.Extended.Sprites.AnimatedSprite;
using System;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Audio;

namespace SAE101
{
    public class Game1 : Game
    {
        //Graphique
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        public KeyboardState _keyboardState;

        // on définit écrans
        public EcranDeTitre _ecranDeTitre;
        public BlackJack _blackJack;
        public EventEtDial _eventEtDial;
        public JoueurSpawn _joueur;
        public ChatoIntCouloir _chatoIntCouloir;
        public ChatoIntChambres _chatoIntChambres;
        public ChatoExtCours _chatoExtCours;
        public ChatoIntTrone _couronne;
        public ChatoCombatContenu _chatoCombatContenu;
        public ChatoCombat _chatoCombat;
        public Option _option;

        //Ecran interactif
        // états du jeu
        public enum Etats { Menu, Start, Play, Quitter, Option };
        private Etats etat;

        //Ecran
        public int xEcran;
        public int yEcran;
        public int xE;
        public int yE;

        public double chan;

        //Camera
        public OrthographicCamera _camera;
        public OrthographicCamera _cameraDial;
        public Vector2 _cameraPosition;
        public int _numEcran;

        //Musiques
        private Song _songChato;
        private Song _titleTheme;
        public Song _songCombat;
        public Song _songDodo;

        //ZoneMusique
        private bool _chato;
        private bool _combatChato;
        private bool _ecranTitre;

        //SFX
        public SoundEffect _hit;
        public SoundEffect _hit2;
        public SoundEffect _lose;
        public SoundEffect _macron_1;
        public SoundEffect _menu;
        public SoundEffect _non;
        public SoundEffect _pelo;
        public SoundEffect _vic;
        public SoundEffect _duck;
        public SoundEffect _wbeg;
        public SoundEffect _wend;
        public SoundEffect _toink;
        public SoundEffect _death;
        public SoundEffect _fire;

        //Combat?
        private bool _combatTest;
        public bool _combatFini;

        //Control cooldown 0.2s
        public float _cooldown;
        public bool _cooldownVerif;
        public float deltaSeconds;
        //Combat cooldown 0.5s
        public float _cooldownC;
        public bool _cooldownVerifC;
        //Combat cooldown 5.0s
        public float _cooldownF;
        public bool _cooldownVerifF;

        //font
        public SpriteFont _font;

        //event
        public bool _firstVisitBedroom;
        public int _fin;

        //pour évènements et déplacementss
        public float _walkSpeed;
        public float _speed;
        public TiledMap _tiledMap;
        public Vector2 _positionPerso;
        public TiledMapTileLayer mapLayer;
        public int _stop;
        public String _animationPlayer;
        public TiledMapTileLayer mapLayerDoor;
        public int _numSalle;
        public bool[] _chestTrue;

        public Etats Etat
        {
            get
            {   return this.etat;   }
            set
            {   this.etat = value;  }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }


        protected override void Initialize()
        {
            // Definition écran
            xEcran = 514;
            yEcran = 448;
            xE = xEcran;
            yE = yEcran;

            chan = 1;

            _graphics.PreferredBackBufferWidth = xEcran;
            _graphics.PreferredBackBufferHeight = yEcran;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            // variables écran
            _ecranDeTitre = new EcranDeTitre(this);
            _eventEtDial = new EventEtDial(this);
            _blackJack = new BlackJack(this);
            _chatoIntCouloir = new ChatoIntCouloir(this);
            _chatoIntChambres = new ChatoIntChambres(this);
            _chatoExtCours = new ChatoExtCours(this);
            _couronne = new ChatoIntTrone(this);
            _joueur = new JoueurSpawn(this);
            _chatoCombatContenu = new ChatoCombatContenu(this);
            _chatoCombat = new ChatoCombat(this);
            _option = new Option(this);

            Etat = Etats.Menu;

            //Zone jeu
            _chato = false;
            _ecranTitre = false;
            _combatChato = false;

            //Camera

            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _camera = new OrthographicCamera(viewportadapter);

            var viewportadapterDial = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _cameraDial = new OrthographicCamera(viewportadapterDial);

            _cameraPosition = _chatoIntChambres._chambreCentre1;
            _numEcran = 0;


            //Dialogue
            _eventEtDial._posText = new Vector2(105, 360);
            _eventEtDial._posNom = new Vector2(25, 360);
            _eventEtDial._posDialBox = new Vector2(0, 348);
            _eventEtDial._dialTrue = false;
            _eventEtDial._posChoiceBox = new Vector2(450, 284);
            _eventEtDial._posCursor = new Vector2(430, 316);
            _eventEtDial._posYes = new Vector2(470, 300);
            _eventEtDial._posNo = new Vector2(470, 315);
            _eventEtDial._yes = "oui";
            _eventEtDial._no = "non";


            //Combat?
            _combatTest = false;

            //event
            _firstVisitBedroom = true;
            _fin = 0;
            _speed = 100;
            _animationPlayer = "idle_down";
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/chato");
            _titleTheme = Content.Load<Song>("music/title/title");
            _songCombat = Content.Load<Song>("music/chato/combat");
            _songDodo = Content.Load<Song>("music/fins/sleep");

            //SFX
            _menu = Content.Load<SoundEffect>("sfx/menu");
            _non = Content.Load<SoundEffect>("sfx/non");
            _hit = Content.Load<SoundEffect>("sfx/hit");
            _hit2 = Content.Load<SoundEffect>("sfx/hit2");
            _macron_1 = Content.Load<SoundEffect>("sfx/macron_1");
            _pelo = Content.Load<SoundEffect>("sfx/pelo");
            _vic = Content.Load<SoundEffect>("sfx/vic");
            _non = Content.Load<SoundEffect>("sfx/non");
            _duck = Content.Load<SoundEffect>("sfx/duck");
            _wbeg = Content.Load<SoundEffect>("sfx/wbeg");
            _wend = Content.Load<SoundEffect>("sfx/wend");
            _toink = Content.Load<SoundEffect>("sfx/toink");
            _death = Content.Load<SoundEffect>("sfx/death");
            _fire = Content.Load<SoundEffect>("sfx/fire");

            //Boite de dialogue
            _eventEtDial._dialBox = Content.Load<Texture2D>("img/dialogue/dialogue_box");
            _eventEtDial._choiceBox = Content.Load<Texture2D>("img/dialogue/choice_box");
            _eventEtDial._cursor = Content.Load<Texture2D>("img/dialogue/cursor");

            //font
            _font = Content.Load<SpriteFont>("font/font_test");


            // on charge l'écran de menu par défaut 
            LoadScreenecran_de_titre();
            //LoadScreenchato_ext_cours_interieur();
            //LoadScreenchato_couronne();

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_keyboardState.IsKeyDown(Keys.Escape))
                Exit();


            // Test clic de souris + Etat 
            MouseState _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (this.Etat == Etats.Quitter)
                    Exit();

                else if (this.Etat == Etats.Start)
                    LoadScreenblack_jack();

                else if (this.Etat == Etats.Option)
                    LoadScreenoption();

                else
                    Console.WriteLine("");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                if (this.Etat == Etats.Start || this.Etat == Etats.Option || this.Etat == Etats.Menu)
                    LoadScreenecran_de_titre();

            //Console.WriteLine(_cooldownVerif);

            if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == false && _cooldownVerif == false)
            {
                LoadScreenchato_combat();
                _combatTest = true;
                SetCoolDown();
            }
            else if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == true && _cooldownVerif == false)
            {
                LoadScreenchato_int_chambres_nord();
                _combatTest = false;
                SetCoolDown();
            }

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }

            if (_cooldownVerifC == true)
            {
                _cooldownC = _cooldownC - deltaSeconds;
                if (_cooldownC <= 0)
                    _cooldownVerifC = false;
            }

            if (_cooldownVerifF == true)
            {
                _cooldownF = _cooldownF - deltaSeconds;
                if (_cooldownF <= 0)
                    _cooldownVerifF = false;
            }
            //Console.WriteLine(_cooldownVerifC);
            //Console.WriteLine(_cooldownC);


            //Camera

            // chambres nord
            if (_numEcran == 1 && _positionPerso.X < _chatoIntChambres._limiteChambreX1
                                && _positionPerso.Y < _chatoIntChambres._limiteChambreY1
                                && _positionPerso.X < _chatoIntChambres._limiteChambreGauche)
                _cameraPosition = _chatoIntChambres._chambreCentre1;

            else if (_numEcran == 1 && _positionPerso.X < _chatoIntChambres._limiteChambreX1
                                && _positionPerso.Y < _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentreUn;

            else if (_numEcran == 1 && _positionPerso.X > _chatoIntChambres._limiteChambreX2
                                && _positionPerso.Y < _chatoIntChambres._limiteChambreY1
                                && _positionPerso.X > _chatoIntChambres._limiteChambreDroite)
                _cameraPosition = _chatoIntChambres._chambreCentreDeux;

            else if (_numEcran == 1 && _positionPerso.X > _chatoIntChambres._limiteChambreX2 
                                && _positionPerso.Y < _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentre2;

            else if (_numEcran == 1 && _positionPerso.Y >= _chatoIntCouloir._limiteCouloirY1)
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);


            // couloir
            if (_numEcran == 2 && (_positionPerso.Y > 0
                                && (_positionPerso.X > _chatoIntCouloir._limiteChambreX1 ||
                                _positionPerso.X < _chatoIntCouloir._limiteChambreX2)))
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);

            else if (_numEcran == 2 && _positionPerso.X < _chatoIntChambres._limiteChambreX1
                                && _positionPerso.X < _chatoIntChambres._limiteChambreGauche
                                && _positionPerso.Y >= _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentre1;

            else if (_numEcran == 2 && _positionPerso.X < _chatoIntChambres._limiteChambreX1
                                && _positionPerso.X > _chatoIntChambres._limiteChambreGauche
                                && _positionPerso.Y >= _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentreUn;

            else if (_numEcran == 2 && _positionPerso.X > _chatoIntChambres._limiteChambreX2
                                && _positionPerso.X < _chatoIntChambres._limiteChambreDroite
                                && _positionPerso.Y >= _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentre2;

            else if (_numEcran == 2 && _positionPerso.X > _chatoIntChambres._limiteChambreX2
                                && _positionPerso.X > _chatoIntChambres._limiteChambreDroite
                                && _positionPerso.Y >= _chatoIntChambres._limiteChambreY1)
                _cameraPosition = _chatoIntChambres._chambreCentreDeux;

            else if (_numEcran == 2 && _positionPerso.Y > 49 * 16)
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);

            // cours
            else if (_numEcran == 3 && _positionPerso.Y >= 1 * 16)
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);

            else if (_numEcran == 3 && _positionPerso.Y < 1 * 16)
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);

            // combat
            else if (_numEcran == 4)
                _cameraPosition = new Vector2(xEcran / 2, yEcran / 2);

            // couronne
            else if (_numEcran == 5)
                _cameraPosition = new Vector2(_positionPerso.X, _positionPerso.Y);

            Console.WriteLine(_numEcran);

            _walkSpeed = _speed * deltaSeconds;
            //Console.WriteLine(_chatoIntChambres._posX);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }


        //Chargements maps
        public void LoadScreenecran_de_titre()
        {
            _screenManager.LoadScreen(_ecranDeTitre, new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            this.Etat = Etats.Menu;
        }

        public void LoadScreenoption()
        {
            _screenManager.LoadScreen(_option, new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            this.Etat = Etats.Option;
        }

        public void LoadScreenblack_jack()
        {
            MediaPlayer.Stop();
            _ecranTitre = false;
            _screenManager.LoadScreen(_blackJack, new FadeTransition(GraphicsDevice, Color.Black));
            _numEcran = 4;
            this.Etat = Etats.Play;
        }

        public void LoadScreenchato_int_chambres_nord()
        {
            _screenManager.LoadScreen(_chatoIntChambres, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _ecranTitre =false;
            _numEcran = 1;
            _combatChato = false;
        }

        public void LoadScreenchato_int_chambres_couloir()
        {
            _screenManager.LoadScreen(_chatoIntCouloir, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _numEcran = 2;
            _combatChato = false;
        }

        public void LoadScreenchato_ext_cours_interieur()
        {
            _screenManager.LoadScreen(_chatoExtCours, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _numEcran = 3;
            _combatChato = false;
        }

        public void LoadScreenchato_combat()
        {
            MediaPlayer.Stop();
            _screenManager.LoadScreen(_chatoCombat, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChatoCombat();
            MediaPlayer.Play(_songCombat);
            this.Etat = Etats.Play;
            _numEcran = 4;
            _chato = false;
            _combatChato = true;
        }

        public void LoadScreenchato_couronne()
        {
            _screenManager.LoadScreen(_couronne, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _numEcran = 5;
            _combatChato = false;
        }

        //autre
        public void SetCoolDown()
        {
            _cooldownVerif = true;
            _cooldown = 0.2f;
            _menu.Play();
        }

        public void SetCoolDownCombat()
        {
            _cooldownVerifC = true;
            _cooldownC = 0.5f;
        }

        public void SetCoolDownFive()
        {
            _cooldownVerifF = true;
            _cooldownF = 5.0f;
        }

        public void MusiqueChatoCombat()
        {
            if (_combatChato == false)
            {
                MediaPlayer.Stop();
                _combatChato = true;
                MediaPlayer.Play(_songCombat);
            }
        }

        public void MusiqueChato()
        {
            if (_chato == false)
            {
                MediaPlayer.Stop();
                _chato = true;
                MediaPlayer.Play(_songChato);
            }
        }

        public void MusiqueTitre()
        {
            if (_ecranTitre == false)
            {
                MediaPlayer.Stop();
                _ecranTitre = true;
                MediaPlayer.Play(_titleTheme);
            }
        }

        public void ChangementEcran(double changement)
        {
            _graphics.PreferredBackBufferWidth = (int)(xEcran * changement);
            _graphics.PreferredBackBufferHeight = (int)(yEcran * changement);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            xE = (int)(xEcran * changement);
            yE = (int)(yEcran * changement);
            chan = changement;
            _graphics.ApplyChanges();
        }
    }
}