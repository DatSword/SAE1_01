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
    internal class chato_combat : GameScreen
    {
        //Constantes position pour animation;
        private const int POS_ALLIEX0 = 145;
        private const int POS_ALLIEX1 = 195;
        private const int POS_ALLIEX2 = 45;
        private const int POS_ALLIEX3 = 95;
        private const int POS_ALLIEY0 = 230;
        private const int POS_ALLIEY1 = 175;
        private int[] _posAllieBaseX;
        private int _persoAnime;
        private bool _droite;
        private bool _gauche;
        private bool _animationAttack;
        private bool un;
        private bool deux;
        private bool fini;
        private bool _cool;
        private bool _merde;
        private bool _animationZeweurld;
        private int[] _ordretour;
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
        private int _numPerso;
        private bool _selectionEnn;
        private int _action;
        private bool _tourFini;
        public bool _victoire;

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
        public static Vector2[] _posEnnemy;
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

        //The Legend came to life
        public static int _ordrefinal;

        //Camera
        public static Vector2 _centreCombat;

        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);

            _choixCursor = 0;
            _choixCursorD = 0;
            _sousMenuSpecial = false;
            _sousMenuObjects = false;
            _premierCombat = false;
            _selectionEnn = false;
            _action = 0;
            _numPerso = 1;
            _victoire = false;
            _persoAnime = 0;
            _ordrefinal = 0;
            _droite = false;
            _gauche = false;
            _animationAttack = false;
            _animationZeweurld = false;
            _cool = false;
            un = false;
            deux = false;
            fini = false;
            _merde = false;
            kk = 0;

            chato_combatcontenu.CombatTest();

            //Menu
            _posText = new[] { new Vector2(40, 300), new Vector2(40, 336), new Vector2(40, 372), new Vector2(40, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };
            _attaquePerso = new int[chato_combatcontenu._nbEquipe, 3];
            _attaqueEnnemy = new int[chato_combatcontenu._nbEnnemy, 3];

            //Camera j'crois
            _centreCombat = new Vector2(512 / 2, 448 / 2);
           
            //ordre allié
            _ordreA = new int[chato_combatcontenu._nbEquipe];
            int ordrejA = 0;

            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                ordrejA = 0;
                for (int j = 0; j < chato_combatcontenu._nbPersoJouable; j++)
                {
                    if (chato_combatcontenu._ordreJoueur[i] == chato_combatcontenu._nomPersoJouable[j])
                    {
                        _ordreA[i] = ordrejA;                        
                    }
                    ordrejA++;
                }               
            }

            //préparation génération
            _fileA = new String[chato_combatcontenu._nbEquipe];
            _sheetA = new SpriteSheet[chato_combatcontenu._nbEquipe];
            _allie = new AnimatedSprite[chato_combatcontenu._nbEquipe];
            _posAllie = new[] { new Vector2(POS_ALLIEX0, POS_ALLIEY0), new Vector2(POS_ALLIEX1, POS_ALLIEY1), new Vector2(POS_ALLIEX2, POS_ALLIEY0), new Vector2(POS_ALLIEX3, POS_ALLIEY1) };
            _posAllieBaseX = new int [] {POS_ALLIEX0, POS_ALLIEX1, POS_ALLIEX2, POS_ALLIEX3 };
            _vieAllie = new int[chato_combatcontenu._nbEquipe];
            _attAllie = new int[chato_combatcontenu._nbEquipe];
            _defAllie = new int[chato_combatcontenu._nbEquipe];
            _vitAllie = new int[chato_combatcontenu._nbEquipe];
            _animationA = new String[] { "idle_right", "idle_right", "idle_right", "idle_right" };

            //génération allié
            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                if (_ordreA[i] == 0)
                {
                    chato_combatcontenu.Hein();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 1)
                {
                    chato_combatcontenu.Hero();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 2)
                {
                    chato_combatcontenu.Jon();
                    GenerationAllie();
                }
                else if (_ordreA[i] == 3)
                {
                    chato_combatcontenu.Ben();
                    GenerationAllie();
                }
                _ordrefinal++;
            }

            //ordre ennemi
            _ordreE = new int[chato_combatcontenu._nbEnnemy];
            int ordrejE = 0;

            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                ordrejE = 0;
                for (int j = 0; j < chato_combatcontenu._nbEnnJouable; j++)
                {
                    if (chato_combatcontenu._ordreEnnemi[i] == chato_combatcontenu._nomEnnJouable[j])
                    {
                        _ordreE[i] = ordrejE;
                    }
                    ordrejE++;
                }
            }

            //préparation génération ennemi
            _fileE = new String[chato_combatcontenu._nbEnnemy];
            _sheetE = new SpriteSheet[chato_combatcontenu._nbEnnemy];
            _ennemy = new AnimatedSprite[chato_combatcontenu._nbEnnemy];
            _posEnnemy = new[] { new Vector2(365, 230), new Vector2(315, 175), new Vector2(465, 230), new Vector2(415, 175) };
            _vieEnn = new int[chato_combatcontenu._nbEnnemy];
            _attEnn = new int[chato_combatcontenu._nbEnnemy];
            _defEnn = new int[chato_combatcontenu._nbEnnemy];
            _vitEnn = new int[chato_combatcontenu._nbEnnemy];
            _animationE = new String[] { "idle_left", "idle_left", "idle_left", "idle_left" };

            //génération ennemy
            _ordrefinal = 0;
            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                if (_ordreE[i] == 0)
                {
                    chato_combatcontenu.Grand();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 1)
                {
                    chato_combatcontenu.Mechant();
                    GenerationEnnemi();
                }
                else if (_ordreE[i] == 2)
                {
                    chato_combatcontenu.Pabo();
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
            Game1._camera.LookAt(Game1._cameraPosition);

            //curseurs
            if (_animationAttack != true && _animationZeweurld != true)
            {
                if (_selectionEnn == false)
                {
                    _positionCursorD = _posAllie[_action] - new Vector2(8, 55);

                    if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 0 && Game1._cooldownVerif == false)
                    {
                        _positionCursor.Y = _positionCursor.Y - 36;
                        _choixCursor = _choixCursor - 1;
                        Game1.SetCoolDown();
                    }
                    if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 3 && Game1._cooldownVerif == false)
                    {
                        _positionCursor.Y = _positionCursor.Y + 36;
                        _choixCursor = _choixCursor + 1;
                        Game1.SetCoolDown();

                    }

                }
                else
                {

                    if (keyboardState.IsKeyDown(Keys.Up) && _choixCursorD < chato_combatcontenu._nbEnnemy - 1 && Game1._cooldownVerif == false)
                    {
                        _choixCursorD = _choixCursorD + 1;
                        Game1.SetCoolDown();
                    }
                    else if (keyboardState.IsKeyDown(Keys.Down) && _choixCursorD > 0 && Game1._cooldownVerif == false)
                    {
                        _choixCursorD = _choixCursorD - 1;
                        Game1.SetCoolDown();
                    }

                    if (keyboardState.IsKeyDown(Keys.W) && Game1._cooldownVerif == false && _sousMenuSpecial == false)
                    {
                        Game1.SetCoolDown();
                        Combat();
                        _selectionEnn = false;
                    }

                    _positionCursorD = _posEnnemy[_choixCursorD] - new Vector2(8, 55);                    
                }

                //Selection dans le menu

                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && Game1._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    _selectionEnn = true;
                    Game1.SetCoolDown();

                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 1 && Game1._cooldownVerif == false && _sousMenuSpecial == false && _premierCombat == false)
                {
                    Game1.SetCoolDown();
                    _sousMenuSpecial = true;
                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 2 && Game1._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    Objects();
                    Game1.SetCoolDown();
                }
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 3 && Game1._cooldownVerif == false && _sousMenuSpecial == false)
                {
                    Fuite();
                    _sousMenuObjects = true;
                    Game1.SetCoolDown();
                }

                if (keyboardState.IsKeyDown(Keys.X) && Game1._cooldownVerif == false && (_sousMenuSpecial == true || _selectionEnn))
                {
                    _sousMenuSpecial = false;
                    _selectionEnn = false;
                    Game1.SetCoolDown();
                }

                //test
                if (keyboardState.IsKeyDown(Keys.W) && Game1._cooldownVerif == false && _sousMenuSpecial == true && _premierCombat == false)
                {
                    Game1.SetCoolDown();
                    Special();
                    _sousMenuSpecial = true;
                }
            }

            //Fin du tour (à mettre juste avant choix action sinon plantage)

            if (chato_combatcontenu._nbEquipe == _action)
            {
                _tourFini = true;
                Vitesse();
                _action = 0;
                _numPerso = 1;
                _tourFini = false;
            }

            //Perso choisissant son action
            if (_ordreA[_action] == 0)
                chato_combatcontenu.Hein();
            else if (_ordreA[_action] == 1)
                chato_combatcontenu.Hero();
            else if (_ordreA[_action] == 2)
                chato_combatcontenu.Jon();
            else if (_ordreA[_action] == 3)
                chato_combatcontenu.Ben();

            if (_selectionEnn == false)
                _positionCursorD = _posAllie[_action] - new Vector2(8, 55);

            //Pas revoir
            if (_sousMenuSpecial == true)
            {
                _choix = chato_combatcontenu._specialP;
                _desc = chato_combatcontenu._descP;
            }
            else if (_sousMenuSpecial == false)
            {
                _choix = _choixBackup;
                _desc = _descBackup;
                _choix[1] = chato_combatcontenu._special;
            }

            //ANIMATIONS

            //Animation de sélection



            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                if (_positionCursorD == _posAllie[i] - new Vector2(8, 55) && _animationA)
                    _animationA[i] = "selected_right";
                else
                    _animationA[i] = "idle_right";
            }
            



            //Animation de combat (l'attaque de base)
            if (_animationAttack == true)
            {               
                if (un == true)
                {
                    _animationA[_persoAnime] = "attack_right1";

                    _cool = true;
                    un = false;
                    _merde = true;
                                     
                }              
                else if (_posAllie[_persoAnime].X > _posAllieBaseX[_persoAnime] + 80 && _merde == false)
                {
                    un = true;
                }
                else if (_posAllie[_persoAnime].X < _posAllieBaseX[_persoAnime])
                {
                    deux = false;
                    _merde = false;
                    _posAllie[_persoAnime].X = _posAllieBaseX[_persoAnime];
                    fini = true;
                    _animationA[_persoAnime] = "idle_right";
                }
                else if (deux == true && _merde == true)
                {
                    Game1._hit.Play();
                    _animationA[_persoAnime] = "move_left";
                    _posAllie[_persoAnime].X -= 2;
                }
                else if (un == false && deux == false && Game1._cooldownVerifC  == false && _merde == false)
                {
                    _animationA[_persoAnime] = "move_right";
                    _posAllie[_persoAnime].X += 2;
                }
                else if (Game1._cooldownVerifC == false && _merde == true)
                {
                    un = false;
                    deux = true;
                    _merde = true;
                }

            }

            

            if (_animationZeweurld == true)
            {
                if (un == true)
                {
                    _animationA[_persoAnime] = "attack_right3";
                    Game1._wbeg.Play();
                    MediaPlayer.Stop();
                    _cool = true;
                    un = false;
                    _merde = true;

                }
                else if (_posAllie[_persoAnime].X > _posAllieBaseX[_persoAnime] + 80 && _merde == false)
                {
                    un = true;
                }
                else if (_posAllie[_persoAnime].X < _posAllieBaseX[_persoAnime])
                {
                    deux = false;
                    _merde = false;
                    _posAllie[_persoAnime].X = _posAllieBaseX[_persoAnime];
                    fini = true;
                    _animationA[_persoAnime] = "idle_right";
                }
                else if (deux == true && _merde == true)
                {
                    _animationA[_persoAnime] = "move_left";
                    _posAllie[_persoAnime].X -= 2;
                }
                else if (un == false && deux == false && Game1._cooldownVerifC == false && _merde == false)
                {
                    _animationA[_persoAnime] = "move_right";
                    _posAllie[_persoAnime].X += 2;
                }
                else if (Game1._cooldownVerifC == false && _merde == true)
                {
                    un = false;
                    deux = true;
                    _merde = true;
                }
            }

            if (fini == true)
            {
                _animationAttack = false;
                _animationZeweurld = false;
                EnnemiMort();
                if (kk != chato_combatcontenu._nbEquipe)
                {
                    Vitesse2();
                }
                else
                    kk = 0;
                fini = false;
            }

            if (_cool == true)
                Game1.SetCoolDownCombat();
            _cool = false;

            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                _allie[i].Play(_animationA[i]);
                _allie[i].Update(deltaSeconds);
            }

            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                _ennemy[i].Play(_animationE[i]);
                _ennemy[i].Update(deltaSeconds);
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
            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                _spriteBatch.Draw(_allie[i], _posAllie[i]);              
            }
            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                _spriteBatch.Draw(_ennemy[i], _posEnnemy[i]);
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
            if (_choixCursor == 0 && _ordreA[_action] == 1)
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

        //Déroulement des attaques;
        public void Vitesse()
        {
            _ordretour = new int[chato_combatcontenu._nbEquipe];
            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                _ordretour[i] = _vitAllie[i];
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
            Console.WriteLine(_ordretour[0]+ " " +_ordretour[1] + " " + _ordretour[2] + " " + _ordretour[3]);
            Console.WriteLine(_vitAllie[0] + " " + _vitAllie[1] + " " + _vitAllie[2] + " " + _vitAllie[3]);

            Vitesse2();
        }

        public void Vitesse2()
        {
            for (int i = 0; i < _ordretour.Length; i++)
            {
                if (_ordretour[kk] == _vitAllie[i])
                {
                    Baston(i);
                    
                }

            }
            kk++;
        }

        public void Baston(int i)
        {
            _persoAnime = i;
            if (_attaquePerso[i, 0] == 0)
            {
                _animationAttack = true;
                un = false;
                deux = false;
                fini = false;
                _vieEnn[_attaquePerso[i, 1]] = _vieEnn[_attaquePerso[i, 1]] - _attAllie[i];
                Console.WriteLine(i);
            }
            else if (_attaquePerso[i, 0] == 1 && _attaquePerso[i, 2] == 1)
            {
                _animationZeweurld = true;
                un = false;
                deux = false;
                fini = false;
                Console.WriteLine(i);
            }
        }

        public void GenerationAllie()
        {
            _fileA[_ordrefinal] = chato_combatcontenu._anim;
            _sheetA[_ordrefinal] = Content.Load<SpriteSheet>(_fileA[_ordrefinal], new JsonContentLoader());
            _allie[_ordrefinal] = new AnimatedSprite(_sheetA[_ordrefinal]);

            _vieAllie[_ordrefinal] = chato_combatcontenu._stat[0];
            _attAllie[_ordrefinal] = chato_combatcontenu._stat[1];
            _defAllie[_ordrefinal] = chato_combatcontenu._stat[2];
            _vitAllie[_ordrefinal] = chato_combatcontenu._stat[3];
        }

        public void GenerationEnnemi()
        {
            _fileE[_ordrefinal] = chato_combatcontenu._anim;
            _sheetE[_ordrefinal] = Content.Load<SpriteSheet>(_fileE[_ordrefinal], new JsonContentLoader());
            _ennemy[_ordrefinal] = new AnimatedSprite(_sheetE[_ordrefinal]);

            _vieEnn[_ordrefinal] = chato_combatcontenu._stat[0];
            _attEnn[_ordrefinal] = chato_combatcontenu._stat[1];
            _defEnn[_ordrefinal] = chato_combatcontenu._stat[2];
            _vitEnn[_ordrefinal] = chato_combatcontenu._stat[3];
        }

        public void EnnemiMort()
        {
            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                {
                    _animationE[i] = "ded";
                    Victoire();
                }

            }
        }

        public void Victoire()
        {
            int verif = 0;

            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                if (_vieEnn[i] <= 0)
                    verif++;
            }

            if (verif == chato_combatcontenu._nbEnnemy)
            {
                _victoire = true;
                Game1._pelo.Play();
                for (int j = 0; j < chato_combatcontenu._nbEquipe; j++)
                {
                    if (_animationA[j] != "ded")
                        _animationA[j] = "victory_right1";
                    kk = chato_combatcontenu._nbEquipe;
                    _desc[0] = "Victoire Totale!";
                    _desc[1] = "Victoire Totale!";
                }

            }
            

        }
    }
}
