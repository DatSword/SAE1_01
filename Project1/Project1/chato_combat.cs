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

namespace SAE101
{
    internal class chato_combat : GameScreen
    {
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
       
        private int _action;
        private bool _tourFini;

        //Perso nazes
        private AnimatedSprite[] _allie;
        private AnimatedSprite[] _ennemy;
        public static Vector2[] _posAllie;
        public static Vector2[] _posEnnemy;
        public static SpriteSheet[] _sheetA;
        public static SpriteSheet[] _sheetE;
        public static String[] _fileA;
        public static String[] _fileE;
        public int[] _ordreA;
        public int[] _ordreE;
        public static int _ordrefinal;

        public static Vector2 _centreCombat;


        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);
            _positionCursorD = new Vector2(145, 200);

            _choixCursor = 0;
            _sousMenuSpecial = false;
            _sousMenuObjects = false;
            _premierCombat = false;
            _action = 0;
            _numPerso = 1;
            _ordrefinal = 0;

            //Menu
            _posText = new[] { new Vector2(48, 300), new Vector2(48, 336), new Vector2(48, 372), new Vector2(48, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };

            //Camera j'crois
            _centreCombat = new Vector2(512 / 2, 448 / 2);

            chato_combatcontenu.CombatTest();

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
            _posAllie = new[] { new Vector2(145, 230), new Vector2(195, 175), new Vector2(45, 230), new Vector2(95, 175) };

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

            //curseur controls
            if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 3 && Game1._cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y + 36;
                _choixCursor = _choixCursor + 1;
                Game1.SetCoolDown();
                
            }            
            if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 0 && Game1._cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y - 36;
                _choixCursor = _choixCursor - 1;
                Game1.SetCoolDown();
            }

            //curseurd controls
            if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 3 && Game1._cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y + 36;
                _choixCursor = _choixCursor + 1;
                Game1.SetCoolDown();

            }
            if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 0 && Game1._cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y - 36;
                _choixCursor = _choixCursor - 1;
                Game1.SetCoolDown();
            }

            //Perso choisissant son action
            //A revoir

            if (_ordreA[_action] == 0)
                chato_combatcontenu.Hein();
            else if (_ordreA[_action] == 1)
                chato_combatcontenu.Hero();
            else if (_ordreA[_action] == 2)
                chato_combatcontenu.Jon();
            else if (_ordreA[_action] == 3)
                chato_combatcontenu.Ben();

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

            //Selection dans le menu
            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && Game1._cooldownVerif == false && _sousMenuSpecial == false)
            {
                //ATTAQUE();
                Game1.SetCoolDown();
                _action = _action + 1;
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
                _sousMenuSpecial = false;
                Game1.SetCoolDown();
            }

            if (keyboardState.IsKeyDown(Keys.X) && Game1._cooldownVerif == false && _sousMenuSpecial == true)
            {
                _sousMenuSpecial = false;
                Game1.SetCoolDown();
            }

            

            //Fin du tour
            if (chato_combatcontenu._nbEquipe == _action)
            {
                _tourFini = true;
                _action = 0;
                _numPerso = 1;
                _tourFini = false;
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
        public void Baston()
        {
            
        }

        public void Objects()
        {
            _desc[2] = "Aucun objets!";
        }

        public void Fuite()
        {
            _desc[3] = "Hm? On dirait qu'un mur en scénarium vous\nempêche d'appuyer sur ce bouton!";
        }

        public void CombatSet()
        {
            //nombre d'ennemi
            chato_combatcontenu._nbEquipe = 2;
            chato_combatcontenu._nbEnnemy = 1;
        }

        public void GenerationAllie()
        {
            _fileA[_ordrefinal] = chato_combatcontenu._anim;
            _sheetA[_ordrefinal] = Content.Load<SpriteSheet>(_fileA[_ordrefinal], new JsonContentLoader());
            _allie[_ordrefinal] = new AnimatedSprite(_sheetA[_ordrefinal]);
        }

        public void GenerationEnnemi()
        {
            _fileE[_ordrefinal] = chato_combatcontenu._anim;
            _sheetE[_ordrefinal] = Content.Load<SpriteSheet>(_fileE[_ordrefinal], new JsonContentLoader());
            _ennemy[_ordrefinal] = new AnimatedSprite(_sheetE[_ordrefinal]);
        }
    }
}
