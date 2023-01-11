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
    public class ChatoCombat : GameScreen
    {
        // défini dans Game1
        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private ChatoCombatContenu _chatoCombatContenu;

        //Constantes position pour animation;
        private const int POS_ALLIEX0 = 145;
        private const int POS_ALLIEX1 = 195;
        private const int POS_ALLIEX2 = 45;
        private const int POS_ALLIEX3 = 95;
        private const int POS_ALLIEY0 = 230;
        private const int POS_ALLIEY1 = 175;
        private const int POS_ENNX0 = 365;
        private const int POS_ENNX1 = 315;
        private const int POS_ENNX2 = 465;
        private const int POS_ENNX3 = 415;
        private const int POS_ENNY0 = 230;
        private const int POS_ENNY1 = 175;
             

        //Ordre en fonction de la vitesse
        private int[] _ordretour;
        private int[] _ordretour2;

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
        public String[] _fileA;
        public SpriteSheet[] _sheetA;
        public int[] _ordreA;       

        //Stats Personazes
        public int[] _vieAllie;
        public int[] _attAllie;
        public int[] _defAllie;
        public int[] _vitAllie;

        //Attaque Personnages
        public static int[,] _attaquePerso;

        //Ennemies
        private AnimatedSprite[] _ennemy;      
        public String[] _fileE;
        public SpriteSheet[] _sheetE;        
        public int[] _ordreE;

        //Stats Ennemies
        public int[] _vieEnn;
        public int[] _attEnn;
        public int[] _defEnn;
        public int[] _vitEnn;

        //Attaque Ennemies
        public int[,] _attaqueEnnemy;
        private Random _random;

        //ATTAques spé
        private int _nbTourZeuWerld;

        //Attaque projectiles
        private AnimatedSprite _proj;        
        public String _animationProj;
        private AnimatedSprite _explosion;       
        public String _animationExplosion;

        //The Legend came to life
        public int _ordrefinal;

        //Camera
        //public Vector2 _centreCombat;

        public ChatoCombat(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _chatoCombatContenu = _myGame._chatoCombatContenu;

            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);

            _choixCursor = 0;
            _choixCursorD = 0;

            _selectionEnn = false;
            _sousMenuSpecial = false;
            _sousMenuObjects = false;

            _premierCombat = false;
            
            _action = 0;
            _ordrefinal = 0;

            _victoire = false;
            _gameOver = false;
            

            _chatoCombatContenu._animationAttackA = false;
            _chatoCombatContenu._animationAttackE = false;
            _chatoCombatContenu._animationZeweurld = false;
            _chatoCombatContenu._animationBouleFeu = false;
            _chatoCombatContenu._animationEnCours = false;
            _chatoCombatContenu._animationSpe = false;
            _chatoCombatContenu._animationP1 = false;
            _chatoCombatContenu._animationP2 = false;
            _chatoCombatContenu._animationP3 = false;
            _chatoCombatContenu._animationOver = false;

            _chatoCombatContenu._fireBall = false;
            _chatoCombatContenu._attackZeuwerld = false;
            _nbTourZeuWerld = 0;

            _chatoCombatContenu._coolDownAnimation = false;
                        
            _chatoCombatContenu.kk = 0;                        

            _random = new Random();


            _chatoCombatContenu.Combat();

            //Menu
            _posText = new[] { new Vector2(40, 300), new Vector2(40, 336), new Vector2(40, 372), new Vector2(40, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };
            _attaquePerso = new int[_chatoCombatContenu._nbAlly, 3];
            _attaqueEnnemy = new int[_chatoCombatContenu._nbEnnemy, 3];
           
            //ordre allié
            _ordreA = new int[_chatoCombatContenu._nbAlly];
            int ordrejA = 0;

            for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
            {
                ordrejA = 0;
                for (int j = 0; j < _chatoCombatContenu._nbPersoJouable; j++)
                {
                    if (_chatoCombatContenu._ordreJoueur[i] == _chatoCombatContenu._nomPersoJouable[j])
                    {
                        _ordreA[i] = ordrejA;                        
                    }
                    ordrejA++;
                }               
            }

            //préparation génération
            _fileA = new String[_chatoCombatContenu._nbAlly];
            _sheetA = new SpriteSheet[_chatoCombatContenu._nbAlly];
            _allie = new AnimatedSprite[_chatoCombatContenu._nbAlly];
            _chatoCombatContenu._posAllie = new[] { new Vector2(POS_ALLIEX0, POS_ALLIEY0), new Vector2(POS_ALLIEX1, POS_ALLIEY1), new Vector2(POS_ALLIEX2, POS_ALLIEY0), new Vector2(POS_ALLIEX3, POS_ALLIEY1) };
            _chatoCombatContenu._posAllieBaseX = new int[] { POS_ALLIEX0, POS_ALLIEX1, POS_ALLIEX2, POS_ALLIEX3 };
            _vieAllie = new int[_chatoCombatContenu._nbAlly];
            _attAllie = new int[_chatoCombatContenu._nbAlly];
            _defAllie = new int[_chatoCombatContenu._nbAlly];
            _vitAllie = new int[_chatoCombatContenu._nbAlly];
            _chatoCombatContenu._animationA = new String[] { "idle_right", "idle_right", "idle_right", "idle_right" };

            //génération allié
            for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
            {
                if (_ordreA[i] == 0)
                {
                    _chatoCombatContenu.Hein();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 1)
                {
                    _chatoCombatContenu.Hero();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 2)
                {
                    _chatoCombatContenu.Jon();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 3)
                {
                    _chatoCombatContenu.Ben();
                    GenerationAllie();
                }
                _ordrefinal++;
            }

            //ordre ennemi
            _ordreE = new int[_chatoCombatContenu._nbEnnemy];
            int ordrejE = 0;

            for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
            {
                ordrejE = 0;
                for (int j = 0; j < _chatoCombatContenu._nbEnnJouable; j++)
                {
                    if (_chatoCombatContenu._ordreEnnemi[i] == _chatoCombatContenu._nomEnnJouable[j])
                    {
                        _ordreE[i] = ordrejE;
                    }
                    ordrejE++;
                }
            }

            //préparation génération ennemi
            _fileE = new String[_chatoCombatContenu._nbEnnemy];
            _sheetE = new SpriteSheet[_chatoCombatContenu._nbEnnemy];
            _ennemy = new AnimatedSprite[_chatoCombatContenu._nbEnnemy];
            _chatoCombatContenu._posEnemy = new[] { new Vector2(POS_ENNX0, POS_ENNY0), new Vector2(POS_ENNX1, POS_ENNY1), new Vector2(POS_ENNX2, POS_ENNY0), new Vector2(POS_ENNX3, POS_ENNY1) };
            _chatoCombatContenu._posEnnBaseX = new int[] { POS_ENNX0, POS_ENNX1, POS_ENNX2, POS_ENNX3 };                     
            _vieEnn = new int[_chatoCombatContenu._nbEnnemy];
            _attEnn = new int[_chatoCombatContenu._nbEnnemy];
            _defEnn = new int[_chatoCombatContenu._nbEnnemy];
            _vitEnn = new int[_chatoCombatContenu._nbEnnemy];
            _chatoCombatContenu._animationE = new String[] { "idle_left", "idle_left", "idle_left", "idle_left" };

            //génération ennemy
            _ordrefinal = 0;
            for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
            {
                if (_ordreE[i] == 0)
                {
                    _chatoCombatContenu.Grand();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 1)
                {
                    _chatoCombatContenu.Mechant();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 2)
                {
                    _chatoCombatContenu.Pabo();
                    GenerationEnnemi();
                }
                _ordrefinal++;
            }
            _chatoCombatContenu._posProj = new Vector2(-16, 0);
            _chatoCombatContenu._posExplosion = new Vector2(-32, 0);
            _animationProj = "fireball";
            _animationExplosion = "boom";


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
            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/objects/projectile1.sf", new JsonContentLoader());
            _proj = new AnimatedSprite(spriteSheetA);
            SpriteSheet spriteSheetB = Content.Load<SpriteSheet>("anim/objects/explosion.sf", new JsonContentLoader());
            _explosion = new AnimatedSprite(spriteSheetB);



            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            _myGame._camera.LookAt(_myGame._cameraPosition);

            //curseurs
            if (_chatoCombatContenu._animationEnCours == false && _gameOver == false && _victoire == false)
            {
                if (_selectionEnn == false)
                {
                    _positionCursorD = _chatoCombatContenu._posAllie[_action] - new Vector2(8, 55);

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
                    if (keyboardState.IsKeyDown(Keys.Up) && _choixCursorD < _chatoCombatContenu._nbEnnemy - 1 && _myGame._cooldownVerif == false)
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

                    _positionCursorD = _chatoCombatContenu._posEnemy[_choixCursorD] - new Vector2(8, 55);
                }

                //Selection dans le menu

                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && _myGame._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    _selectionEnn = true;
                    _myGame.SetCoolDown();

                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 1 && _myGame._cooldownVerif == false && _sousMenuSpecial == false && _premierCombat == false )
                {
                    _myGame.SetCoolDown();
                    _sousMenuSpecial = true;
                }
                if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _selectionEnn == false && _premierCombat == false && _sousMenuSpecial == true && _chatoCombatContenu._attackZeuwerld == false)
                {
                    _myGame.SetCoolDown();
                    _selectionEnn = true;
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
                if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _sousMenuSpecial == true && _premierCombat == false && _selectionEnn == true)
                {
                    _myGame.SetCoolDown();
                    Special();
                    _sousMenuSpecial = false;
                    _selectionEnn = false;

                }
            }

            if (_chatoCombatContenu._attackZeuwerld == true && _action > 0 && _chatoCombatContenu._animationEnCours == false)
            {
                if (_nbTourZeuWerld == 2)
                {
                    MediaPlayer.Play(_myGame._songCombat);
                    _myGame._wend.Play();
                    _chatoCombatContenu._attackZeuwerld = false;
                    _nbTourZeuWerld = 0;
                    _action = 0;
                    _chatoCombatContenu.kk = 0;
                    _chatoCombatContenu._animationOver = false;
                }
                else
                    BastonA(0);
            }

            //Fin du tour (à mettre juste avant choix action sinon plantage)
            if (_chatoCombatContenu._nbAlly == _action)
            {
                _tourFini = true;
                Vitesse();
                _action = 0;
                _tourFini = false;
            }

            //Perso choisissant son action
            if (_ordreA[_action] == 0)
                _chatoCombatContenu.Hein();
            else if (_ordreA[_action] == 1)
                _chatoCombatContenu.Hero();
            else if (_ordreA[_action] == 2)
                _chatoCombatContenu.Jon();
            else if (_ordreA[_action] == 3)
                _chatoCombatContenu.Ben();

            //Si tel Perso est mort, alors pas d'action
            if (_chatoCombatContenu._animationA[_action] == "ded")
                _action++;
            Console.WriteLine(_action);

            //Qui est suivi par le curseur
            if (_selectionEnn == false)
            {
                _positionCursorD = _chatoCombatContenu._posAllie[_action] - new Vector2(8, 55);

                if (_sousMenuSpecial == true)
                {
                    _choix = _chatoCombatContenu._specialP;
                    _desc = _chatoCombatContenu._descP;
                }
                else if (_sousMenuSpecial == false)
                {
                    _choix = _choixBackup;
                    _desc = _descBackup;
                    _choix[1] = _chatoCombatContenu._special;
                }

                //ANIMATIONS

                //Animation de sélection

                if (_gameOver == false && _victoire == false)
                {
                    for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
                    {
                        if (_positionCursorD == _chatoCombatContenu._posAllie[i] - new Vector2(8, 55) && _chatoCombatContenu._animationEnCours == false)
                            _chatoCombatContenu._animationA[i] = "selected_right";
                        else if (_chatoCombatContenu._animationEnCours == false && _chatoCombatContenu._animationA[i] != "ded")
                            _chatoCombatContenu._animationA[i] = "idle_right";
                    }
                }                

                
                _myGame._chatoCombatContenu.Animation();

                if (_chatoCombatContenu._animationOver == true)
                {

                    _chatoCombatContenu._animationEnCours = false;
                    _chatoCombatContenu._animationSpe = false;
                    _chatoCombatContenu._animationAttackA = false;
                    _chatoCombatContenu._animationAttackE = false;
                    _chatoCombatContenu._animationZeweurld = false;
                    _chatoCombatContenu._animationBouleFeu = false;
                    EnnemiMort();
                    AllieMort();

                    if (_chatoCombatContenu._attackZeuwerld == true)
                    {
                        _chatoCombatContenu.kk = _chatoCombatContenu._nbAlly + _chatoCombatContenu._nbEnnemy;
                        _nbTourZeuWerld++;
                    }
                    if (_chatoCombatContenu.kk != _chatoCombatContenu._nbAlly + _chatoCombatContenu._nbEnnemy && _gameOver == false && _victoire == false)
                    {
                        Vitesse2();
                    }
                    else
                    {
                        _chatoCombatContenu._animationOver = false;
                        _chatoCombatContenu.kk = 0;
                    }
                }

                if (_chatoCombatContenu._fireBall == true)
                {
                    _chatoCombatContenu._posProj.X += 2;
                    _animationProj = "fireball";
                    if (_chatoCombatContenu._posProj.X == _chatoCombatContenu._posEnemy[_attaquePerso[1, 1]].X)
                    {
                        _chatoCombatContenu._posProj = new Vector2(-16, 0);
                        _chatoCombatContenu._fireBall = false;
                    }
                }

                if (_chatoCombatContenu._badabim == true)
                {
                    _chatoCombatContenu._posProj.X += 2;
                    _animationProj = "badaboom";
                    if (_chatoCombatContenu._posProj.X == _chatoCombatContenu._posEnemy[_attaquePerso[1, 1]].X)
                    {
                        _myGame._pelo.Play();
                        _chatoCombatContenu._posProj = new Vector2(-16, 0);
                        _chatoCombatContenu._badabim = false;
                        _chatoCombatContenu._posExplosion = _chatoCombatContenu._posEnemy[_attaquePerso[1, 1]];
                    }
                }

                if (_chatoCombatContenu._coolDownAnimation == true)
                    _myGame.SetCoolDownCombat();
                _chatoCombatContenu._coolDownAnimation = false;

                //Animation update

                for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
                {
                    _allie[i].Play(_chatoCombatContenu._animationA[i]);
                    _allie[i].Update(deltaSeconds);
                }

                for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
                {
                    _ennemy[i].Play(_chatoCombatContenu._animationE[i]);
                    _ennemy[i].Update(deltaSeconds);
                }

                _proj.Play(_animationProj);
                _proj.Update(deltaSeconds);
                _explosion.Play(_animationExplosion);
                _explosion.Update(deltaSeconds);

                //Sortie du combat
                if (_myGame._cooldownVerifF == false && _gameOver == true)
                {
                    _myGame._fin = 2;
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

            var transformMatrix = _myGame._camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_chatoCombatDecor, new Vector2(0, -75), Color.White);
            _spriteBatch.Draw(_combatBox, _positionCombat, Color.White);
            _spriteBatch.Draw(_cursor, _positionCursor, Color.White);
            _spriteBatch.Draw(_cursorD, _positionCursorD, Color.White);
            _spriteBatch.Draw(_proj, _chatoCombatContenu._posProj);
            _spriteBatch.Draw(_explosion, _chatoCombatContenu._posExplosion);
            _spriteBatch.DrawString(_fontTest, _choix[0], _posText[0], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[1], _posText[1], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[2], _posText[2], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[3], _posText[3], Color.White);
            _spriteBatch.DrawString(_fontTest, _desc[_choixCursor], _posText[4], Color.White);
            for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
            {
                _spriteBatch.Draw(_allie[i], _chatoCombatContenu._posAllie[i]);              
            }
            for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
            {
                _spriteBatch.Draw(_ennemy[i], _chatoCombatContenu._posEnemy[i]);
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
                if (_chatoCombatContenu._attackZeuwerld == true)
                    _myGame._non.Play();

                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 0;
                _action++;
            }
            //Baïtzedeust
            else if (_choixCursor == 1 && _ordreA[_action] == 1)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 2;
                _sousMenuSpecial = false;
                _action++;
            }
            //Boule de Feu
            else if (_choixCursor == 0 && _ordreA[_action] == 2)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = _choixCursorD;
                _attaquePerso[_action, 2] = 3;
                _sousMenuSpecial = false;
                _action++;
            }
            //Intimidation
            else if (_choixCursor == 1 && _ordreA[_action] == 2)
            {
                _attaquePerso[_action, 0] = 1;
                _attaquePerso[_action, 1] = -1;
                _attaquePerso[_action, 2] = 4;
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
            _ordretour = new int[_chatoCombatContenu._nbAlly+ _chatoCombatContenu._nbEnnemy];
            _ordretour2 = new int[_chatoCombatContenu._nbAlly + _chatoCombatContenu._nbEnnemy];
            for (int i = 0; i < _chatoCombatContenu._nbAlly + _chatoCombatContenu._nbEnnemy; i++)
            {
                if (i < _chatoCombatContenu._nbAlly) 
                {
                    _ordretour[i] = _vitAllie[i];
                    _ordretour2[i] = _vitAllie[i];
                }
                    
                else if (i >= _chatoCombatContenu._nbAlly)
                {
                    _ordretour[i] = _vitEnn[i - _chatoCombatContenu._nbAlly];
                    _ordretour2[i] = _vitEnn[i - _chatoCombatContenu._nbAlly];
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
                if (_ordretour[_chatoCombatContenu.kk] == _ordretour2[i])
                {
                    if (i >= _chatoCombatContenu._nbAlly)
                    {
                        BastonE(i - _chatoCombatContenu._nbAlly);
                    }
                    else
                    {
                        BastonA(i);
                    }                                        
                }               
            }
            _chatoCombatContenu.kk++;
        }

        //Si un allié se bat
        public void BastonA(int i)
        {
            if (_chatoCombatContenu._animationA[i] != "ded")
            {
                _chatoCombatContenu._allyAnime = i;
                _chatoCombatContenu._animationEnCours = true;
                if (_attaquePerso[i, 0] == 0)
                {
                    _chatoCombatContenu._animationAttackA = true;
                    _chatoCombatContenu._animationP1 = false;
                    _chatoCombatContenu._animationP2 = false;
                    _chatoCombatContenu._animationOver = false;
                    _vieEnn[_attaquePerso[i, 1]] = _vieEnn[_attaquePerso[i, 1]] - _attAllie[i];
                    Console.WriteLine(i);
                }
                else if (_attaquePerso[i, 0] == 1 && _attaquePerso[i, 2] == 0)
                {
                    _chatoCombatContenu._animationSpe = true;
                    _chatoCombatContenu._animationZeweurld = true;
                    _chatoCombatContenu._animationP1 = false;
                    _chatoCombatContenu._animationP2 = false;
                    _chatoCombatContenu._animationOver = false;
                    Console.WriteLine(i);
                }
                else if (_attaquePerso[i, 0] == 1 && _attaquePerso[i, 2] == 3)
                {
                    _chatoCombatContenu._animationSpe = true;
                    _chatoCombatContenu._animationBouleFeu = true;
                    _chatoCombatContenu._animationP1 = false;
                    _chatoCombatContenu._animationP2 = false;
                    _chatoCombatContenu._animationOver = false;
                    Console.WriteLine(i);
                }
                else if (_attaquePerso[i, 0] == 1 && _attaquePerso[i, 2] == 4)
                {
                    _chatoCombatContenu._animationSpe = true;
                    _chatoCombatContenu._animationBouleFeu = true;
                    _chatoCombatContenu._animationP1 = false;
                    _chatoCombatContenu._animationP2 = false;
                    _chatoCombatContenu._animationOver = false;
                    Console.WriteLine(i);
                }
            }
            else
            {
                _chatoCombatContenu._animationOver = true;
            }
            
        }

        //SI un ennemi se bat
        public void BastonE(int i)
        {
            if (_chatoCombatContenu._animationE[i] != "ded")
            {
                _chatoCombatContenu._enemyAnime = i;
                int all = _random.Next(0, _chatoCombatContenu._nbAlly);

                _attaqueEnnemy[i, 0] = 0;
                _attaqueEnnemy[i, 1] = all;
                _attaqueEnnemy[i, 2] = 0;


                _chatoCombatContenu._animationAttackE = true;
                _chatoCombatContenu._animationEnCours = true;
                _chatoCombatContenu._animationP1 = false;
                _chatoCombatContenu._animationP2 = false;
                _chatoCombatContenu._animationOver = false;
                _vieAllie[_attaqueEnnemy[i, 1]] = _vieAllie[_attaqueEnnemy[i, 1]] - _attEnn[i];
                Console.WriteLine(i);
            }
            else
            {
                _chatoCombatContenu._animationOver = true;
            }
        }

        public void GenerationAllie()
        {
            _fileA[_ordrefinal] = _chatoCombatContenu._anim;
            _sheetA[_ordrefinal] = Content.Load<SpriteSheet>(_fileA[_ordrefinal], new JsonContentLoader());
            _allie[_ordrefinal] = new AnimatedSprite(_sheetA[_ordrefinal]);

            _vieAllie[_ordrefinal] = _chatoCombatContenu._stat[0];
            _attAllie[_ordrefinal] = _chatoCombatContenu._stat[1];
            _defAllie[_ordrefinal] = _chatoCombatContenu._stat[2];
            _vitAllie[_ordrefinal] = _chatoCombatContenu._stat[3];
        }

        public void GenerationEnnemi()
        {
            _fileE[_ordrefinal] = _chatoCombatContenu._anim;
            _sheetE[_ordrefinal] = Content.Load<SpriteSheet>(_fileE[_ordrefinal], new JsonContentLoader());
            _ennemy[_ordrefinal] = new AnimatedSprite(_sheetE[_ordrefinal]);

            _vieEnn[_ordrefinal] = _chatoCombatContenu._stat[0];
            _attEnn[_ordrefinal] = _chatoCombatContenu._stat[1];
            _defEnn[_ordrefinal] = _chatoCombatContenu._stat[2];
            _vitEnn[_ordrefinal] = _chatoCombatContenu._stat[3];
        }

        public void EnnemiMort()
        {
            for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                {
                    _chatoCombatContenu._animationE[i] = "ded";
                    Victoire();
                }

            }
        }

        public void AllieMort()
        {
            for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
            {
                if (_vieAllie[i] <= 0)
                {
                    _chatoCombatContenu._animationA[i] = "ded";
                    GameOver();
                }

            }
        }

        public void Victoire()
        {
            int verif = 0;

            for (int i = 0; i < _chatoCombatContenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                    verif++;
            }

            if (verif == _chatoCombatContenu._nbEnnemy)
            {
                _chatoCombatContenu.kk = 0;
                _victoire = true;
                _myGame._combatFini = true;
                _myGame.SetCoolDownFive();
                _myGame._vic.Play();
                for (int j = 0; j < _chatoCombatContenu._nbAlly; j++)
                {
                    if (_chatoCombatContenu._animationA[j] != "ded")
                        _chatoCombatContenu._animationA[j] = "victory_right1";
                    _chatoCombatContenu.kk = _chatoCombatContenu._nbAlly;
                    _desc[0] = "Victoire Totale!";
                    _desc[1] = "Victoire Totale!";
                }
            }           
        }

        public void GameOver()
        {
            int verif = 0;

            for (int i = 0; i < _chatoCombatContenu._nbAlly; i++)
            {
                if (_vieAllie[i] <= 0)
                    verif++;
            }

            if (verif == _chatoCombatContenu._nbAlly)
            {
                _chatoCombatContenu.kk = 0;
                _gameOver = true;
                _myGame.SetCoolDownFive();
                _myGame._death.Play();
                for (int j = 0; j < _chatoCombatContenu._nbAlly; j++)
                {
                    _desc[0] = "Anihilé";
                    _desc[1] = "Anihilé";
                }
            }
        }

        public void RetourChato()
        {
            if (_myGame._numSalle == 1)
                _myGame.LoadScreenchato_int_chambres_couloir();
            else if (_myGame._numEcran == 2)
                _myGame.LoadScreenchato_ext_cours_interieur();
            else if (_myGame._numSalle == 3)
                _myGame.LoadScreenchato_couronne();
        }
    }
}
