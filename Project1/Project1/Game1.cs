using Microsoft.Xna.Framework;
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
        public BlackJack _blackJack;
        public Camera _camera;
        public ChatoCombat _chatoCombat;
        public ChatoCombatContenu _chatoCombatContenu;
        public ChatoExtCours _chatoExtCours;
        public ChatoIntChambres _chatoIntChambres;
        public ChatoIntCouloir _chatoIntCouloir;
        public ChatoIntTrone _chatoIntTrone;
        public EcranDeTitre _ecranDeTitre;
        public EventEtDial _eventEtDial;
        public JoueurSpawn _joueur;
        public Option _option;

        //Ecran interactif
        // états du jeu
        public enum Etats { Menu, Start, Play, Quitter, Option };
        private Etats etat;

        //Ecran
        public int _xEcran = 514;
        public int _yEcran = 448;  
        public int xE;
        public int yE;

        public double chan;

        //Camera
        /*public OrthographicCamera _cameraMap;
        public OrthographicCamera _cameraDial;
        public Vector2 _cameraPosition;*/
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
        public SoundEffect _explo;
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
        //cooldown silencieux 0.2s
        public float _cooldownS;
        public bool _cooldownVerifS;
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
        public bool _firstVisitCorridor;
        public int _fin;

        //pour évènements et déplacementss
        public float _walkSpeed;
        public const float SPEED = 100;
        public TiledMap _tiledMap;
        public Vector2 _positionPerso;
        public TiledMapTileLayer mapLayer;
        public int _stop;
        public String _animationPlayer;
        public TiledMapTileLayer mapLayerDoor;
        public int _numSalle;
        public bool[] _chestTrue;
        public bool konami;
        public int konamiCount;
        public bool _epee;
        public bool _boom;

        //Pour les combats
        public int _nbAlly;
        public int _nbEnemy;
        public String[] _ordreJoueur;
        public String[] _ordreEnnemi;


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
            xE = _xEcran;
            yE = _yEcran;

            chan = 1;

            _graphics.PreferredBackBufferWidth = _xEcran;
            _graphics.PreferredBackBufferHeight = _yEcran;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            // variables écran
            _blackJack = new BlackJack(this);
            _chatoCombat = new ChatoCombat(this);
            _chatoCombatContenu = new ChatoCombatContenu(this);
            _chatoExtCours = new ChatoExtCours(this);
            _chatoIntChambres = new ChatoIntChambres(this);
            _chatoIntCouloir = new ChatoIntCouloir(this);
            _chatoIntTrone = new ChatoIntTrone(this);
            _ecranDeTitre = new EcranDeTitre(this);
            _eventEtDial = new EventEtDial(this);
            _joueur = new JoueurSpawn(this);
            _option = new Option(this);
            _camera = new Camera(this);

            Etat = Etats.Menu;

            //Zone jeu
            _chato = false;
            _ecranTitre = false;
            _combatChato = false;

            _camera.InitialisationCamera();

            _numEcran = 0;

            _chestTrue = new bool[2];
            for (int i = 0; i < _numEcran; i++)
                _chestTrue[i] = false;

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
            _epee = false;
            _boom = false;

            //event
            _firstVisitBedroom = true;
            _firstVisitCorridor = true;
            _fin = 0;
            _animationPlayer = "idle_down";

            _nbAlly = 0;
            _nbEnemy = 0;
            _ordreJoueur = new String[1];
            _ordreEnnemi = new String[1];



            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/chato");
            _titleTheme = Content.Load<Song>("music/title/titre");
            _songCombat = Content.Load<Song>("music/chato/combat");
            _songDodo = Content.Load<Song>("music/fins/sleep");

            //SFX
            _menu = Content.Load<SoundEffect>("sfx/menu");
            _non = Content.Load<SoundEffect>("sfx/non");
            _hit = Content.Load<SoundEffect>("sfx/hit");
            _hit2 = Content.Load<SoundEffect>("sfx/hit2");
            _explo = Content.Load<SoundEffect>("sfx/explo");
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
            LoadScreenecranDeTtitre();

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //Console.WriteLine(_chestTrue[0] + " " + _chestTrue[1]);
            // Test clic de souris + Etat 
            MouseState _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (this.Etat == Etats.Quitter)
                    Exit();

                else if (this.Etat == Etats.Start)
                    LoadScreenBlackJack();

                else if (this.Etat == Etats.Option)
                    LoadScreenOption();

                else
                    Console.WriteLine("");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                if (this.Etat == Etats.Start || this.Etat == Etats.Option || this.Etat == Etats.Menu)
                    LoadScreenecranDeTtitre();

            //Console.WriteLine(_cooldownVerif);

            if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == false && _cooldownVerif == false)
            {
                LoadScreenChatoCombat();
                _combatTest = true;
                SetCoolDown();
            }
            else if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == true && _cooldownVerif == false)
            {
                LoadScreenchatoIntChambresNord();
                _combatTest = false;
                SetCoolDown();
            }

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }
            if (_cooldownVerifS == true)
            {
                _cooldownS = _cooldownS - deltaSeconds;
                if (_cooldownS <= 0)
                    _cooldownVerifS = false;
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
            _camera.PositionCamera();
 

            _walkSpeed = SPEED * deltaSeconds;
            //Console.WriteLine(ChatoCombatContenu._lastPosition);



            if(_numEcran == 0)
            {
                //Console.WriteLine(konamiCount);
                if (_keyboardState.IsKeyDown(Keys.Up) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount < 2)
                        konamiCount++;
                    else
                        konamiCount = 0;
                }

                if (_keyboardState.IsKeyDown(Keys.Down) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount >= 2 && konamiCount < 4)
                        konamiCount++;
                    else
                        konamiCount = 0;
                }

                if (_keyboardState.IsKeyDown(Keys.Left) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount == 4 || konamiCount == 6)
                        konamiCount++;
                    else
                        konamiCount = 0;
                }

                if (_keyboardState.IsKeyDown(Keys.Right) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount == 5 || konamiCount == 7)
                        konamiCount++;
                    else
                        konamiCount = 0;
                } 

                if (_keyboardState.IsKeyDown(Keys.X) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount == 8)
                        konamiCount++;
                    else
                        konamiCount = 0;
                }  

                if (_keyboardState.IsKeyDown(Keys.W) && _cooldownVerifS == false)
                {
                    SetCoolDownS();
                    if (konamiCount == 9)
                        konamiCount++;
                    else
                        konamiCount = 0;
                }   
            }

            if (konamiCount == 10)
            {
                konami = true;
                konamiCount = 0;
            }

            base.Update(gameTime);
        }
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }


        //Chargements maps
        public void LoadScreenecranDeTtitre()
        {
            _screenManager.LoadScreen(_ecranDeTitre, new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            _numEcran = 0;
            this.Etat = Etats.Menu;
        }

        public void LoadScreenOption()
        {
            _screenManager.LoadScreen(_option, new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            this.Etat = Etats.Option;
            _numEcran = 11;
        }

        public void LoadScreenBlackJack()
        {
            MediaPlayer.Stop();
            _ecranTitre = false;
            _screenManager.LoadScreen(_blackJack, new FadeTransition(GraphicsDevice, Color.Black));
            _numEcran = 4;
            this.Etat = Etats.Play;
        }

        public void LoadScreenchatoIntChambresNord()
        {
            _screenManager.LoadScreen(_chatoIntChambres, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _ecranTitre =false;
            _numEcran = 1;
            _combatChato = false;
        }

        public void LoadScreenchatoIntChambresCouloir()
        {
            _screenManager.LoadScreen(_chatoIntCouloir, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _numEcran = 2;
            _combatChato = false;
        }

        public void LoadScreenchatoExtCoursInterieur()
        {
            _screenManager.LoadScreen(_chatoExtCours, new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            this.Etat = Etats.Play;
            _numEcran = 3;
            _combatChato = false;
        }

        public void LoadScreenChatoCombat()
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

        public void LoadScreenChatoCouronne()
        {
            _screenManager.LoadScreen(_chatoIntTrone, new FadeTransition(GraphicsDevice, Color.Black));
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

        public void SetCoolDownS()
        {
            _cooldownVerifS = true;
            _cooldownS = 0.2f;

        }

        public void SetCoolDownC()
        {
            _cooldownVerifC = true;
            _cooldownC = 0.5f;
        }

        public void SetCoolDownF()
        {
            _cooldownVerifF = true;
            _cooldownF = 4.0f;
        }


        // Musique
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
            _graphics.PreferredBackBufferWidth = (int)(_xEcran * changement);
            _graphics.PreferredBackBufferHeight = (int)(_yEcran * changement);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            xE = (int)(_xEcran * changement);
            yE = (int)(_yEcran * changement);
            chan = changement;
            _graphics.ApplyChanges();
        }
    }
}