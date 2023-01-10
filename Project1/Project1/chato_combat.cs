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
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.InteropServices;

namespace SAE101
{
    public class Chato_combat : GameScreen
    {
        // défini dans Game1
        private Game1 _myGame;
        private Event_et_dial _eventEtDial;

        //Constantes position pour animation;
        private const int POS_ALLIEX0 = 145;
        private const int POS_ALLIEX1 = 195;
        private const int POS_ALLIEX2 = 45;
        private const int POS_ALLIEX3 = 95;
        private const int POS_ALLIEY0 = 230;
        private const int POS_ALLIEY1 = 175;
        private int[] _posAllieBaseX;
        private const int POS_ENNX0 = 365;
        private const int POS_ENNX1 = 315;
        private const int POS_ENNX2 = 465;
        private const int POS_ENNX3 = 415;
        private const int POS_ENNY0 = 230;
        private const int POS_ENNY1 = 175;
        private int[] _posEnnBaseX;

        //ANIMATIONS
        private int _allyAnime;
        private int _enemyAnime;
        private bool _animationAttackA;
        private bool _animationAttackE;
        private bool _animationZeweurld;
        private bool _animationEnCours;
        private bool _animationP1;
        private bool _animationP2;
        private bool _animationP3;
        private bool _animationOver;
        private bool _coolDownAnimation;      
        

        //Ordre en fonction de la vitesse
        private int[] _ordretour;
        private int[] _ordretour2;
        private int kk;

        //Fond d'écran
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _chatoCombatDecor;

        //Menu
        private Texture2D _combatBox;
        private Vector2 _positionCombat;
        public SpriteFont _fontTest;
        private Texture2D _cursor;
        private Vector2 _positionCursor;
        private int _choixCursor;
        private Texture2D _cursorD;
        private Vector2 _positionCursorD;
        private int _choixCursorD;

        //Textes menu
        private String[] _choix;
        private String[] _choixBackup;
        private String[] _desc;
        private String[] _descBackup;
        private Vector2[] _posText;

        //Spécial
        private bool _premierCombat;

        //Tours
        private bool _tourPassé;
        private bool _sousMenuSpecial;
        private bool _sousMenuObjects;
        private bool _selectionEnn;
        private int _action;
        private bool _tourFini;
        public bool _victoire;
        public bool _gameOver;

        //Personazes
        private AnimatedSprite[] _allie;
        public static Vector2[] _posAllie;
        public static String[] _fileA;
        public static SpriteSheet[] _sheetA;
        public int[] _ordreA;
        String[] _animationA;

        //Stats Personazes
        public static int[] _vieAllie;
        public static int[] _attAllie;
        public static int[] _defAllie;
        public static int[] _vitAllie;

        //Attaque Personnages
        public static int[,] _attaquePerso;

        //Ennemies
        private AnimatedSprite[] _ennemy;      
        public static Vector2[] _posEnemy;
        public static String[] _fileE;
        public static SpriteSheet[] _sheetE;        
        public int[] _ordreE;
        String[] _animationE;

        //Stats Ennemies
        public static int[] _vieEnn;
        public static int[] _attEnn;
        public static int[] _defEnn;
        public static int[] _vitEnn;

        //Attaque Ennemies
        public static int[,] _attaqueEnnemy;
        private Random _random;

        //ATTAques spé
        private bool _attackZeuwerld;
        private int _nbTourZeuWerld;

        //The Legend came to life
        public static int _ordrefinal;

        //Camera
        public static Vector2 _centreCombat;

        public Chato_combat(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;

            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);

            _choixCursor = 0;
            _choixCursorD = 0;
            _sousMenuSpecial = false;
            _sousMenuObjects = false;
            _premierCombat = false;
            _selectionEnn = false;
            _action = 0;
            _victoire = false;
            _gameOver = false;
            _allyAnime = 0;
            _ordrefinal = 0;
            _animationAttackA = false;
            _animationAttackE = false;
            _animationZeweurld = false;
            _animationEnCours = false;
            _coolDownAnimation = false;
            _animationP1 = false;
            _animationP2 = false;
            _animationOver = false;
            _animationP3 = false;
            kk = 0;
            _attackZeuwerld = false;
            _nbTourZeuWerld = 0;

            _random = new Random();


            Chato_combat_contenu.Combat();

            //Menu
            _posText = new[] { new Vector2(40, 300), new Vector2(40, 336), new Vector2(40, 372), new Vector2(40, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };
            _attaquePerso = new int[Chato_combat_contenu._nbAlly, 3];
            _attaqueEnnemy = new int[Chato_combat_contenu._nbEnnemy, 3];

            //Camera j'crois
            _centreCombat = new Vector2(512 / 2, 448 / 2);
           
            //ordre allié
            _ordreA = new int[Chato_combat_contenu._nbAlly];
            int ordrejA = 0;

            for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
            {
                ordrejA = 0;
                for (int j = 0; j < Chato_combat_contenu._nbPersoJouable; j++)
                {
                    if (Chato_combat_contenu._ordreJoueur[i] == Chato_combat_contenu._nomPersoJouable[j])
                    {
                        _ordreA[i] = ordrejA;                        
                    }
                    ordrejA++;
                }               
            }

            //préparation génération
            _fileA = new String[Chato_combat_contenu._nbAlly];
            _sheetA = new SpriteSheet[Chato_combat_contenu._nbAlly];
            _allie = new AnimatedSprite[Chato_combat_contenu._nbAlly];
            _posAllie = new[] { new Vector2(POS_ALLIEX0, POS_ALLIEY0), new Vector2(POS_ALLIEX1, POS_ALLIEY1), new Vector2(POS_ALLIEX2, POS_ALLIEY0), new Vector2(POS_ALLIEX3, POS_ALLIEY1) };
            _posAllieBaseX = new int[] { POS_ALLIEX0, POS_ALLIEX1, POS_ALLIEX2, POS_ALLIEX3 };
            _vieAllie = new int[Chato_combat_contenu._nbAlly];
            _attAllie = new int[Chato_combat_contenu._nbAlly];
            _defAllie = new int[Chato_combat_contenu._nbAlly];
            _vitAllie = new int[Chato_combat_contenu._nbAlly];
            _animationA = new String[] { "idle_right", "idle_right", "idle_right", "idle_right" };

            //génération allié
            for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
            {
                if (_ordreA[i] == 0)
                {
                    Chato_combat_contenu.Hein();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 1)
                {
                    Chato_combat_contenu.Hero();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 2)
                {
                    Chato_combat_contenu.Jon();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 3)
                {
                    Chato_combat_contenu.Ben();
                    GenerationAllie();
                }
                _ordrefinal++;
            }

            //ordre ennemi
            _ordreE = new int[Chato_combat_contenu._nbEnnemy];
            int ordrejE = 0;

            for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
            {
                ordrejE = 0;
                for (int j = 0; j < Chato_combat_contenu._nbEnnJouable; j++)
                {
                    if (Chato_combat_contenu._ordreEnnemi[i] == Chato_combat_contenu._nomEnnJouable[j])
                    {
                        _ordreE[i] = ordrejE;
                    }
                    ordrejE++;
                }
            }

            //préparation génération ennemi
            _fileE = new String[Chato_combat_contenu._nbEnnemy];
            _sheetE = new SpriteSheet[Chato_combat_contenu._nbEnnemy];
            _ennemy = new AnimatedSprite[Chato_combat_contenu._nbEnnemy];
            _posEnemy = new[] { new Vector2(POS_ENNX0, POS_ENNY0), new Vector2(POS_ENNX1, POS_ENNY1), new Vector2(POS_ENNX2, POS_ENNY0), new Vector2(POS_ENNX3, POS_ENNY1) };
            _posEnnBaseX = new int[] { POS_ENNX0, POS_ENNX1, POS_ENNX2, POS_ENNX3 };                     
            _vieEnn = new int[Chato_combat_contenu._nbEnnemy];
            _attEnn = new int[Chato_combat_contenu._nbEnnemy];
            _defEnn = new int[Chato_combat_contenu._nbEnnemy];
            _vitEnn = new int[Chato_combat_contenu._nbEnnemy];
            _animationE = new String[] { "idle_left", "idle_left", "idle_left", "idle_left" };

            //génération ennemy
            _ordrefinal = 0;
            for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
            {
                if (_ordreE[i] == 0)
                {
                    Chato_combat_contenu.Grand();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 1)
                {
                    Chato_combat_contenu.Mechant();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 2)
                {
                    Chato_combat_contenu.Pabo();
                    GenerationEnnemi();
                }
                _ordrefinal++;
            }
           
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _chatoCombatDecor = Content.Load<Texture2D>("img/chato/combat_decor");
            _combatBox = Content.Load<Texture2D>("img/dialogue/combat_box");
            _cursor = Content.Load<Texture2D>("img/dialogue/cursor");
            _cursorD = Content.Load<Texture2D>("img/dialogue/cursord");
            _fontTest = Content.Load<SpriteFont>("font/font_test");

            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            Game1._camera.LookAt(_myGame._cameraPosition);

            //curseurs
            if (_animationEnCours == false && _gameOver == false && _victoire == false)
            {
                if (_selectionEnn == false)
                {
                    _positionCursorD = _posAllie[_action] - new Vector2(8, 55);

                    if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 0 && _myGame._cooldownVerif == false)
                    {
                        _positionCursor.Y = _positionCursor.Y - 36;
                        _choixCursor = _choixCursor - 1;
                        _myGame.SetCoolDown();
                    }
                    if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 3 && _myGame._cooldownVerif == false)
                    {
                        _positionCursor.Y = _positionCursor.Y + 36;
                        _choixCursor = _choixCursor + 1;
                        _myGame.SetCoolDown();
                    }

                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.Up) && _choixCursorD < Chato_combat_contenu._nbEnnemy - 1 && _myGame._cooldownVerif == false)
                    {
                        _choixCursorD = _choixCursorD + 1;
                        _myGame.SetCoolDown();
                    }
                    else if (keyboardState.IsKeyDown(Keys.Down) && _choixCursorD > 0 && _myGame._cooldownVerif == false)
                    {
                        _choixCursorD = _choixCursorD - 1;
                        _myGame.SetCoolDown();
                    }

                    if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _sousMenuSpecial == false)
                    {
                        _myGame.SetCoolDown();
                        Combat();
                        _selectionEnn = false;
                    }

                    _positionCursorD = _posEnemy[_choixCursorD] - new Vector2(8, 55);
                }

                //Selection dans le menu

                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && _myGame._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    _selectionEnn = true;
                    _myGame.SetCoolDown();

                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 1 && _myGame._cooldownVerif == false && _sousMenuSpecial == false && _premierCombat == false)
                {
                    _myGame.SetCoolDown();
                    _sousMenuSpecial = true;
                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 2 && _myGame._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    Objects();
                    _myGame.SetCoolDown();
                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 3 && _myGame._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    Fuite();
                    _sousMenuObjects = true;
                    _myGame.SetCoolDown();
                }

                if (keyboardState.IsKeyDown(Keys.X) && _myGame._cooldownVerif == false && (_sousMenuSpecial == true || _selectionEnn))
                {
                    _sousMenuSpecial = false;
                    _selectionEnn = false;
                    _myGame.SetCoolDown();
                }

                if (keyboardState.IsKeyDown(Keys.X) && _myGame._cooldownVerif == false && (_sousMenuSpecial == false || _selectionEnn == false) && _action > 0)
                {
                    _action = _action - 1;
                    _myGame.SetCoolDown();
                }

                //test (ou pas)
                if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _sousMenuSpecial == true && _premierCombat == false)
                {
                    _myGame.SetCoolDown();
                    Special();
                    _sousMenuSpecial = false;
                }
            }

            if (_attackZeuwerld == true && _action > 0 && _animationEnCours == false)
            {
                if (_nbTourZeuWerld == 2)
                {
                    MediaPlayer.Play(Game1._songCombat);
                    Game1._wend.Play();
                    _attackZeuwerld = false;
                    _nbTourZeuWerld = 0;
                    _action = 0;
                    kk = 0;
                    _animationOver = false;
                }
                else
                    BastonA(0);
            }

            //Fin du tour (à mettre juste avant choix action sinon plantage)
            if (Chato_combat_contenu._nbAlly == _action)
            {
                _tourFini = true;
                Vitesse();
                _action = 0;
                _tourFini = false;
            }

            //Perso choisissant son action
            if (_ordreA[_action] == 0)
                Chato_combat_contenu.Hein();
            else if (_ordreA[_action] == 1)
                Chato_combat_contenu.Hero();
            else if (_ordreA[_action] == 2)
                Chato_combat_contenu.Jon();
            else if (_ordreA[_action] == 3)
                Chato_combat_contenu.Ben();

            //Si tel Perso est mort, alors pas d'action
            if (_animationA[_action] == "ded")
                _action++;
            Console.WriteLine(_action);

            //Qui est suivi par le curseur
            if (_selectionEnn == false)
            {
                _positionCursorD = _posAllie[_action] - new Vector2(8, 55);

                if (_sousMenuSpecial == true)
                {
                    _choix = Chato_combat_contenu._specialP;
                    _desc = Chato_combat_contenu._descP;
                }
                else if (_sousMenuSpecial == false)
                {
                    _choix = _choixBackup;
                    _desc = _descBackup;
                    _choix[1] = Chato_combat_contenu._special;
                }

                //ANIMATIONS

                //Animation de sélection

                if (_gameOver == false && _victoire == false)
                {
                    for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
                    {
                        if (_positionCursorD == _posAllie[i] - new Vector2(8, 55) && _animationEnCours == false)
                            _animationA[i] = "selected_right";
                        else if (_animationEnCours == false && _animationA[i] != "ded")
                            _animationA[i] = "idle_right";
                    }
                }                

                //Animation de combat (l'attaque de base)
                if (_animationAttackA == true)
                {
                    if (_animationP1 == true)
                    {
                        _animationA[_allyAnime] = "attack_right1";

                        _coolDownAnimation = true;
                        _animationP1 = false;
                        _animationP3 = true;

                    }
                    else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                    {
                        _animationP1 = true;
                    }
                    else if (_posAllie[_allyAnime].X < _posAllieBaseX[_allyAnime])
                    {
                        _animationP2 = false;
                        _animationP3 = false;
                        _animationOver = true;
                        _posAllie[_allyAnime].X = _posAllieBaseX[_allyAnime];

                        _animationA[_allyAnime] = "idle_right";
                    }
                    else if (_animationP2 == true && _animationP3 == true)
                    {
                        Game1._hit.Play();
                        _animationA[_allyAnime] = "move_left";
                        _posAllie[_allyAnime].X -= 2;
                    }
                    else if (_animationP1 == false && _animationP2 == false && Game1._cooldownVerifC == false && _animationP3 == false)
                    {
                        _animationA[_allyAnime] = "move_right";
                        _posAllie[_allyAnime].X += 2;
                    }
                    else if (Game1._cooldownVerifC == false && _animationP3 == true)
                    {
                        _animationP1 = false;
                        _animationP2 = true;
                        _animationP3 = true;
                    }

                }
                Console.WriteLine(_animationOver);
                if (_animationAttackE == true)
                {
                    if (_animationP1 == true)
                    {
                        _animationE[_enemyAnime] = "attack_left1";

                        _coolDownAnimation = true;
                        _animationP1 = false;
                        _animationP3 = true;

                    }
                    else if (_posEnemy[_enemyAnime].X < _posEnnBaseX[_enemyAnime] - 80 && _animationP3 == false)
                    {
                        _animationP1 = true;
                    }
                    else if (_posEnemy[_enemyAnime].X > _posEnnBaseX[_enemyAnime])
                    {
                        _animationP2 = false;
                        _animationP3 = false;
                        _posEnemy[_enemyAnime].X = _posEnnBaseX[_enemyAnime];
                        _animationOver = true;
                        _animationE[_enemyAnime] = "idle_left";
                    }
                    else if (_animationP2 == true && _animationP3 == true)
                    {
                        Game1._hit.Play();
                        _animationE[_enemyAnime] = "move_right";
                        _posEnemy[_enemyAnime].X += 2;
                    }
                    else if (_animationP1 == false && _animationP2 == false && Game1._cooldownVerifC == false && _animationP3 == false)
                    {
                        _animationE[_enemyAnime] = "move_left";
                        _posEnemy[_enemyAnime].X -= 2;
                    }
                    else if (Game1._cooldownVerifC == false && _animationP3 == true)
                    {
                        _animationP1 = false;
                        _animationP2 = true;
                        _animationP3 = true;
                    }
                }

                if (_animationZeweurld == true)
                {
                    if (_animationP1 == true)
                    {
                        _animationA[_allyAnime] = "attack_right3";
                        Game1._wbeg.Play();
                        MediaPlayer.Stop();
                        _attackZeuwerld = true;
                        _coolDownAnimation = true;
                        _animationP1 = false;
                        _animationP3 = true;

                    }
                    else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                    {
                        _animationP1 = true;
                    }
                    else if (_posAllie[_allyAnime].X < _posAllieBaseX[_allyAnime])
                    {
                        _animationP2 = false;
                        _animationP3 = false;
                        _posAllie[_allyAnime].X = _posAllieBaseX[_allyAnime];
                        _animationOver = true;
                        _animationA[_allyAnime] = "idle_right";
                    }
                    else if (_animationP2 == true && _animationP3 == true)
                    {
                        _animationA[_allyAnime] = "move_left";
                        _posAllie[_allyAnime].X -= 2;
                        kk = Chato_combat_contenu._nbEnnemy + Chato_combat_contenu._nbAlly;

                    }
                    else if (_animationP1 == false && _animationP2 == false && Game1._cooldownVerifC == false && _animationP3 == false)
                    {
                        _animationA[_allyAnime] = "move_right";
                        _posAllie[_allyAnime].X += 2;
                    }
                    else if (Game1._cooldownVerifC == false && _animationP3 == true)
                    {
                        _animationP1 = false;
                        _animationP2 = true;
                        _animationP3 = true;
                    }
                }

                if (_animationOver == true)
                {

                    _animationEnCours = false;
                    _animationAttackA = false;
                    _animationAttackE = false;
                    _animationZeweurld = false;
                    EnnemiMort();
                    AllieMort();

                    if (_attackZeuwerld == true)
                    {
                        kk = Chato_combat_contenu._nbAlly + Chato_combat_contenu._nbEnnemy;
                        _nbTourZeuWerld++;
                    }
                    if (kk != Chato_combat_contenu._nbAlly + Chato_combat_contenu._nbEnnemy && _gameOver == false && _victoire == false)
                    {
                        Vitesse2();
                    }
                    else
                    {
                        _animationOver = false;
                        kk = 0;
                    }
                }



                if (_coolDownAnimation == true)
                    _myGame.SetCoolDownCombat();
                _coolDownAnimation = false;

                for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
                {
                    _allie[i].Play(_animationA[i]);
                    _allie[i].Update(deltaSeconds);
                }

                for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
                {
                    _ennemy[i].Play(_animationE[i]);
                    _ennemy[i].Update(deltaSeconds);
                }

                //Sortie du combat
                if (_myGame._cooldownVerifF == false && _gameOver == true)
                {
                    Game1._fin = 2;
                    _myGame.LoadScreenblack_jack();
                }
                else if (_myGame._cooldownVerifF == false && _victoire == true)
                {
                    RetourChato();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = Game1._camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_chatoCombatDecor, new Vector2(0, -75), Color.White);
            _spriteBatch.Draw(_combatBox, _positionCombat, Color.White);
            _spriteBatch.Draw(_cursor, _positionCursor, Color.White);
            _spriteBatch.Draw(_cursorD, _positionCursorD, Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[0], _posText[0], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[1], _posText[1], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[2], _posText[2], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[3], _posText[3], Color.White);
            _spriteBatch.DrawString(_fontTest, _desc[_choixCursor], _posText[4], Color.White);
            for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
            {
                _spriteBatch.Draw(_allie[i], _posAllie[i]);              
            }
            for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
            {
                _spriteBatch.Draw(_ennemy[i], _posEnemy[i]);
            }
            _spriteBatch.End();
        }

        //actions
        public void Combat()
        {
            _attaquePerso[_action, 0] = 0;
            _attaquePerso[_action, 1] = _choixCursorD;
            _attaquePerso[_action, 2] = 0;

            _action++;
        }

        public void Special()
        {
            //ZeuWerld
            if (_choixCursor == 0 && _ordreA[_action] == 1)
            {
                if (_attackZeuwerld == true)
                    Game1._non.Play();

                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 1;
                _action++;
            }
            //Baïtzedeust
            else if (_choixCursor == 1 && _ordreA[_action] == 1)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 1;
                _sousMenuSpecial = false;
                _action++;
            }
            //Boule de Feu
            else if (_choixCursor == 0 && _ordreA[_action] == 2)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 1;
                _sousMenuSpecial = false;
                _action++;
            }
            //Boule de Feu
            else if (_choixCursor == 1 && _ordreA[_action] == 2)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 1;
                _sousMenuSpecial = false;
                _action++;

            }
        }

        public void Objects()
        {
            _desc[2] = "Aucun objets!";
        }

        public void Fuite()
        {
            _desc[3] = "Fuite impossible!";
        }

        //Déroulement des attaques : Choisi l'ordre dans lequel tous les personnages vont jouer
        public void Vitesse()
        {
            _ordretour = new int[Chato_combat_contenu._nbAlly+ Chato_combat_contenu._nbEnnemy];
            _ordretour2 = new int[Chato_combat_contenu._nbAlly + Chato_combat_contenu._nbEnnemy];
            for (int i = 0; i < Chato_combat_contenu._nbAlly + Chato_combat_contenu._nbEnnemy; i++)
            {
                if (i < Chato_combat_contenu._nbAlly) 
                {
                    _ordretour[i] = _vitAllie[i];
                    _ordretour2[i] = _vitAllie[i];
                }
                    
                else if (i >= Chato_combat_contenu._nbAlly)
                {
                    _ordretour[i] = _vitEnn[i - Chato_combat_contenu._nbAlly];
                    _ordretour2[i] = _vitEnn[i - Chato_combat_contenu._nbAlly];
                }
                    
            }

            int temp = 0;

            for (int j = 0; j < _ordretour.Length; j++)
            {
                for (int i = 0; i < (_ordretour.Length - 1) - j; i++)
                {
                    if (_ordretour[i] < _ordretour[i + 1])
                    {
                        temp = _ordretour[i];
                        _ordretour[i] = _ordretour[i + 1];
                        _ordretour[i + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < _ordretour.Length; i++)
                Console.WriteLine(_ordretour[i]);

            Vitesse2();
        }

        //Invoque le personnage qui va jouer
        public void Vitesse2()
        {
            for (int i = 0; i < _ordretour.Length; i++)
            {
                if (_ordretour[kk] == _ordretour2[i])
                {
                    if (i >= Chato_combat_contenu._nbAlly)
                    {
                        BastonE(i - Chato_combat_contenu._nbAlly);
                    }
                    else
                    {
                        BastonA(i);
                    }                                        
                }               
            }
            kk++;
        }

        //Si un allié se bat
        public void BastonA(int i)
        {
            if (_animationA[i] != "ded")
            {
                _allyAnime = i;
                _animationEnCours = true;
                if (_attaquePerso[i, 0] == 0)
                {
                    _animationAttackA = true;
                    _animationP1 = false;
                    _animationP2 = false;
                    _animationOver = false;
                    _vieEnn[_attaquePerso[i, 1]] = _vieEnn[_attaquePerso[i, 1]] - _attAllie[i];
                    Console.WriteLine(i);
                }
                else if (_attaquePerso[i, 0] == 1 && _attaquePerso[i, 2] == 1)
                {
                    _animationZeweurld = true;
                    _animationP1 = false;
                    _animationP2 = false;
                    _animationOver = false;
                    Console.WriteLine(i);
                }
            }
            else
            {
                _animationOver = true;
            }
            
        }

        //SI un ennemi se bat
        public void BastonE(int i)
        {
            if (_animationE[i] != "ded")
            {
                _enemyAnime = i;
                int all = _random.Next(0, Chato_combat_contenu._nbAlly);

                _attaqueEnnemy[i, 0] = 0;
                _attaqueEnnemy[i, 1] = all;
                _attaqueEnnemy[i, 2] = 0;


                _animationAttackE = true;
                _animationEnCours = true;
                _animationP1 = false;
                _animationP2 = false;
                _animationOver = false;
                _vieAllie[_attaqueEnnemy[i, 1]] = _vieAllie[_attaqueEnnemy[i, 1]] - _attEnn[i];
                Console.WriteLine(i);
            }
            else
            {
                _animationOver = true;
            }
        }

        public void GenerationAllie()
        {
            _fileA[_ordrefinal] = Chato_combat_contenu._anim;
            _sheetA[_ordrefinal] = Content.Load<SpriteSheet>(_fileA[_ordrefinal], new JsonContentLoader());
            _allie[_ordrefinal] = new AnimatedSprite(_sheetA[_ordrefinal]);

            _vieAllie[_ordrefinal] = Chato_combat_contenu._stat[0];
            _attAllie[_ordrefinal] = Chato_combat_contenu._stat[1];
            _defAllie[_ordrefinal] = Chato_combat_contenu._stat[2];
            _vitAllie[_ordrefinal] = Chato_combat_contenu._stat[3];
        }

        public void GenerationEnnemi()
        {
            _fileE[_ordrefinal] = Chato_combat_contenu._anim;
            _sheetE[_ordrefinal] = Content.Load<SpriteSheet>(_fileE[_ordrefinal], new JsonContentLoader());
            _ennemy[_ordrefinal] = new AnimatedSprite(_sheetE[_ordrefinal]);

            _vieEnn[_ordrefinal] = Chato_combat_contenu._stat[0];
            _attEnn[_ordrefinal] = Chato_combat_contenu._stat[1];
            _defEnn[_ordrefinal] = Chato_combat_contenu._stat[2];
            _vitEnn[_ordrefinal] = Chato_combat_contenu._stat[3];
        }

        public void EnnemiMort()
        {
            for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                {
                    _animationE[i] = "ded";
                    Victoire();
                }

            }
        }

        public void AllieMort()
        {
            for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
            {
                if (_vieAllie[i] <= 0)
                {
                    _animationA[i] = "ded";
                    GameOver();
                }

            }
        }

        public void Victoire()
        {
            int verif = 0;

            for (int i = 0; i < Chato_combat_contenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                    verif++;
            }

            if (verif == Chato_combat_contenu._nbEnnemy)
            {
                kk = 0;
                _victoire = true;
                Game1._combatFini = true;
                _myGame.SetCoolDownFive();
                Game1._vic.Play();
                for (int j = 0; j < Chato_combat_contenu._nbAlly; j++)
                {
                    if (_animationA[j] != "ded")
                        _animationA[j] = "victory_right1";
                    kk = Chato_combat_contenu._nbAlly;
                    _desc[0] = "Victoire Totale!";
                    _desc[1] = "Victoire Totale!";
                }
            }           
        }

        public void GameOver()
        {
            int verif = 0;

            for (int i = 0; i < Chato_combat_contenu._nbAlly; i++)
            {
                if (_vieAllie[i] <= 0)
                    verif++;
            }

            if (verif == Chato_combat_contenu._nbAlly)
            {
                kk = 0;
                _gameOver = true;
                _myGame.SetCoolDownFive();
                Game1._death.Play();
                for (int j = 0; j < Chato_combat_contenu._nbAlly; j++)
                {
                    _desc[0] = "Anihilé";
                    _desc[1] = "Anihilé";
                }
            }
        }

        public void RetourChato()
        {
            if (Game1._numSalle == 1)
                _myGame.LoadScreenchato_int_chambres_couloir();
            else if (Game1._numSalle == 2)
                _myGame.LoadScreenchato_ext_cours_interieur();
            /*if (Game1._numSalle == 3)
                _myGame.LoadScreenchato_int_salle_courronnement();*/
        }
    }
}
