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
        private String _spécial;
        private Vector2[] _posText;

        //Spécial
        private bool _premierCombat;

        //Tours
        private bool _tourPassé;
        private bool _sousMenuSpecial;
        private int _numPerso;
       
        private int _aAttaque;
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

        public static Vector2 _centreCombat;


        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);

            _choixCursor = 0;
            _sousMenuSpecial = false;
            _premierCombat = false;
            _aAttaque = 1;
            _numPerso = 1;

            //Combattest();

            //Menu
            _posText = new[] { new Vector2(48, 300), new Vector2(48, 336), new Vector2(48, 372), new Vector2(48, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };

            _centreCombat = new Vector2(512 / 2, 448 / 2);

            //Generation allie

            _fileA = new String[chato_combatcontenu._nbEquipe];
            _sheetA = new SpriteSheet[chato_combatcontenu._nbEquipe];
            _allie = new AnimatedSprite[chato_combatcontenu._nbEquipe];
            _posAllie = new[] { new Vector2(145,230), new Vector2(195, 175), new Vector2(45, 230), new Vector2(95, 175) };

            for (int i = 0; i < chato_combatcontenu._nbEquipe; i++)
            {
                _fileA[i] = "anim/char/base_model_m/base_model_movement.sf";
               _sheetA[i] = Content.Load<SpriteSheet>(_fileA[i], new JsonContentLoader());
               _allie[i] = new AnimatedSprite(_sheetA[i]);
            }

            //Generation ennemi

            _fileE = new String[chato_combatcontenu._nbEnnemy];
            _sheetE = new SpriteSheet[chato_combatcontenu._nbEnnemy];
            _ennemy = new AnimatedSprite[chato_combatcontenu._nbEnnemy];
            _posEnnemy = new[] { new Vector2(365, 230), new Vector2(315, 175), new Vector2(465, 230), new Vector2(415, 175)};

            SpriteSheet test = Content.Load<SpriteSheet>("anim/char/base_model_m/base_model_movement.sf", new JsonContentLoader());
            for (int i = 0; i < chato_combatcontenu._nbEnnemy; i++)
            {
                _fileE[i] = "anim/char/base_model_m/base_model_movement.sf";
                _sheetE[i] = Content.Load<SpriteSheet>(_fileA[i], new JsonContentLoader());
                _ennemy[i] = new AnimatedSprite(_sheetA[i]);
            }
           
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _chatoCombatDecor = Content.Load<Texture2D>("img/chato/combat_decor");
            _combatBox = Content.Load<Texture2D>("img/dialogue/combat_box");
            _cursor = Content.Load<Texture2D>("img/dialogue/cursor");
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

            //Perso choisissant son action
            if (_numPerso == 1)
            {
                Hero();
            }
            else if (_numPerso == 2)
            {
                Jon();
            }

            if (_sousMenuSpecial == true)
            {
                _choix = chato_combatcontenu._specialP;
                _desc = chato_combatcontenu._descP;
            }
            else if (_sousMenuSpecial == false)
            {
                _choix = _choixBackup;
                _desc = _descBackup;
                _choix[1] = _spécial;
            }

            //Selection dans le menu
            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && Game1._cooldownVerif == false && _sousMenuSpecial == false)
            {
                //ATTAQUE();
                Game1.SetCoolDown();
                _aAttaque = _aAttaque + 1;
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
            if (chato_combatcontenu._nbEquipe < _aAttaque)
            {
                _tourFini = true;
                _aAttaque = 1;
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

        public void Hero()
        {
            _spécial = "NomCool";
            String[] _specialH = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };          
            String[] _descH = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };
 
        }
        public void Jon()
        {
            _spécial = "Magie";
            String[] _specialJ = new String[] { "Boule de feu", "JSP", "_", "_" };
            String[] _descJ = new String[] { "BRÛLEZZZZ", "MOURREZZZZZ", "_", "_" };
        }

        public void Ben()
        {
            _spécial = "Cri";
            String[] _specialJ = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de Problèmes", "_" };
            String[] _descJ = new String[] { "_", "_", "Que des solutions!", "_" };
            if (_sousMenuSpecial == true)
            {
                _choix = _specialJ;
                _desc = _descJ;
            }
            else if (_sousMenuSpecial == false)
            {
                _choix = _choixBackup;
                _desc = _descBackup;
                _choix[1] = _spécial;
            }
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
    }
}
