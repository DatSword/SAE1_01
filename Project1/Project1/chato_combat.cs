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

namespace SAE101
{
    internal class chato_combat : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _chatoCombatDecor;

        private Texture2D _combatBox;
        private Vector2 _positionCombat;

        private Texture2D _cursor;
        private Vector2 _positionCursor;
        public SpriteFont _fontTest;

        private bool _premierCombat;
        private bool _tourPassé;
        private int _choixCursor;
        private bool _sousMenu;
        private int _numPerso;
        private int _nbEquipe;
        private int _aAttaque;
        private bool _tourFini;

        private String[] _choix;
        private String[] _choixBackup;
        private String[] _desc;
        private String[] _descBackup;
        private String _spécial;

        private Vector2[] _posText;

        private float _cooldown;
        private bool _cooldownVerif;


        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);

            _choixCursor = 0;
            _sousMenu = false;
            _premierCombat = false;
            _nbEquipe = 2;
            _aAttaque = 1;
            _numPerso = 1;

            _posText = new[] { new Vector2(48, 300), new Vector2(48, 336), new Vector2(48, 372), new Vector2(48, 408), new Vector2(180, 265) };
            _choix = new String[] { "Combat", "???", "Objets","Fuite"};
            _choixBackup = new String[] { "Combat", "???", "Objets", "Fuite" };
            _desc = new String[] { "_", "_", "_", "_" };
            _descBackup = new String[] { "_", "_", "_", "_" };

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

            //curseur controls
            if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 3 && _cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y + 36;
                _choixCursor = _choixCursor + 1;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }            
            if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 0 && _cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y - 36;
                _choixCursor = _choixCursor - 1;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }
            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
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
            
            //Selection dans le menu
            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && _cooldownVerif == false && _sousMenu == false)
            {
                //ATTAQUE();
                _cooldownVerif = true;
                _cooldown = 0.2f;
                _aAttaque = _aAttaque + 1;
            }

            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 1 && _cooldownVerif == false && _sousMenu == false && _premierCombat == false)
            {

                _sousMenu = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 2 && _cooldownVerif == false && _sousMenu == false)
            {
                Objects();
                _sousMenu = true;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }

            if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 3 && _cooldownVerif == false && _sousMenu == false)
            {
                Fuite();
                _sousMenu = false;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }

            if (keyboardState.IsKeyDown(Keys.X) && _cooldownVerif == false && _sousMenu == true)
            {
                _sousMenu = false;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }

            //Fin du tour
            if (_nbEquipe < _aAttaque)
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

            // TODO: Add your drawing code here
            //var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(/*transformMatrix: transformMatrix*/);
            _spriteBatch.Draw(_chatoCombatDecor, new Vector2(0, -75), Color.White);
            _spriteBatch.Draw(_combatBox, _positionCombat , Color.White);
            _spriteBatch.Draw(_cursor, _positionCursor, Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[0], _posText[0], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[1], _posText[1], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[2], _posText[2], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix[3], _posText[3], Color.White);
            _spriteBatch.DrawString(_fontTest, _desc[_choixCursor], _posText[4], Color.White);
            _spriteBatch.End();
        }

        public void Hero()
        {
            _spécial = "NomCool";
            String[] _specialH = new String[] { "Zeweurld", "Baïtezedeuste", "_", "_" };          
            String[] _descH = new String[] { "Arrête le temps", "Reviens au dernier tour", "_", "_" };
            if (_sousMenu == true)
            {
                _choix = _specialH;
                _desc = _descH;
            }
            else if (_sousMenu == false)
            {
                _choix = _choixBackup;
                _desc = _descBackup;
                _choix[1] = _spécial;
            }

            if (_aAttaque == 2)
            {
                _numPerso = 2;
            }

        }
        public void Jon()
        {
            _spécial = "Magie";
            String[] _specialJ = new String[] { "_", "_", "_", "_" };
            String[] _descJ = new String[] { "BRÛLEZZZZ", "MOURREZZZZZ", "_", "_" };
            if (_sousMenu == true)
            {
                _choix = _specialJ;
                _desc = _descJ;
            }
            else if (_sousMenu == false)
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
    }
}
