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

namespace SAE101
{
    public class Option : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // défini dans Game1
        private Game1 _myGame;

        //Titre
        private Texture2D _titleS;
        private SpriteFont _fontTitle;

        // dialbox
        private Texture2D _optBox;
        private Vector2 _posOptBox;
        private String[] _textOpt;
        private Vector2[] _posTextOpt;
        private Texture2D[] _touchesOpt;
        private Vector2[] _posTouches;

        // taille écran
        private Texture2D _cursor;
        private Vector2 _positionCursor;
        private int _espaceText;
        private String[] _textResEcran;
        private Vector2[] _posTextResEcran;


        public Option(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _posOptBox = new Vector2(0, 224);

            _textOpt = new String[3] { "pour valider", "pour revenir en arrière", "pour les déplacements"};
            _posTextOpt = new Vector2[3] { new Vector2(125, 285), new Vector2(125, 320), new Vector2(125, 355) };

            _posTouches = new Vector2[6] { new Vector2(50, 280), new Vector2(50, 310), new Vector2(20, 370), new Vector2(50, 340), new Vector2(50, 370), new Vector2(80, 370) };

            _espaceText = 130;

            _positionCursor = new Vector2(40, 250);
            _textResEcran = new String[3] { "514 x 448", "771 x 672", "1028 x 996" };
            _posTextResEcran = new Vector2[3] { new Vector2(70, 250), new Vector2(70 + _espaceText, 250), new Vector2(70 + _espaceText * 2, 250) };

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _titleS = Content.Load<Texture2D>("menu/tantopie");
            _fontTitle = Content.Load<SpriteFont>("font/fonttitle");

            _optBox = Content.Load<Texture2D>("menu/options_box");

            _touchesOpt = new Texture2D[6] { Content.Load<Texture2D>("menu/toucheW"), Content.Load<Texture2D>("menu/toucheX"), Content.Load<Texture2D>("menu/toucheG"), Content.Load<Texture2D>("menu/toucheH"), Content.Load<Texture2D>("menu/toucheB"), Content.Load<Texture2D>("menu/toucheD")};

            _cursor = Content.Load<Texture2D>("img/dialogue/cursor");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.Right) && _myGame._cooldownVerif == false && _positionCursor.X < 300)
            {
                _positionCursor.X += _espaceText;
                _myGame.SetCoolDown();
            }
            if (_keyboardState.IsKeyDown(Keys.Left) && _myGame._cooldownVerif == false && _positionCursor.X > 40)
            {
                _positionCursor.X -= _espaceText;
                _myGame.SetCoolDown();
            }

            if (_keyboardState.IsKeyDown(Keys.W) && _positionCursor.X == 40)
                _myGame.ChangementEcran(1);
            else if (_keyboardState.IsKeyDown(Keys.W) && _positionCursor.X == 40 + _espaceText)
                _myGame.ChangementEcran(1.5);
            else if (_keyboardState.IsKeyDown(Keys.W) && _positionCursor.X == 40 + _espaceText * 2)
                _myGame.ChangementEcran(2);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            var transformMatrix = _myGame._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(_titleS, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_fontTitle, "Tantopie", new Vector2(10, 0), Color.Gray);
            _spriteBatch.Draw(_optBox, _posOptBox, Color.White);
            _spriteBatch.Draw(_cursor, _positionCursor, Color.White);

            for (int i = 0; i < _textOpt.Length; i++)
                _spriteBatch.DrawString(_myGame._font, _textOpt[i], _posTextOpt[i], Color.White);

            for (int j = 0; j < _touchesOpt.Length; j++)
                _spriteBatch.Draw(_touchesOpt[j], _posTouches[j], Color.White);

            for (int k = 0; k < _textResEcran.Length; k++)
                _spriteBatch.DrawString(_myGame._font, _textResEcran[k], _posTextResEcran[k], Color.White);

            _spriteBatch.End();
        }
    }
}
