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
        private int _choixCursor;
        public SpriteFont _fontTest;

        private String[] _choix1;
        private String[] _choix2;
        private String[] _choix3;
        private String[] _choix4;
        private String[] _desc;
        private Vector2[] _posText;

        private float _cooldown;
        private bool _cooldownVerif;


        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);
            _choixCursor = 1;

            _posText = new[] { new Vector2(64, 300), new Vector2(64, 336), new Vector2(64, 372), new Vector2(64, 408), new Vector2(180, 265) };
            _choix1 = new String[] { "Combat", "Combat", "Combat" };
            _choix2 = new String[] { "???", "Nomcool", "Magie" };
            _choix3 = new String[] { "Objets", "Objets", "Objets" };
            _choix4 = new String[] { "Objets", "Objets", "Objets" };
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
            if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor < 4 && _cooldownVerif == false)
            {
                _positionCursor.Y = _positionCursor.Y + 36;
                _choixCursor = _choixCursor + 1;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }
            
            if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor > 1 && _cooldownVerif == false)
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

            if (_choixCursor == 1)
                _desc = "Attaque un ennemi conventionnellement";
            else if (_choixCursor == 2)
                _desc = "???";
            else if (_choixCursor == 3)
                _desc = "Permet d'utiliser un objet";
            else if (_choixCursor == 4)
                _desc = "Fuir un le combat";
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
            _spriteBatch.DrawString(_fontTest, _choix1, _posText[0], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix2, _posText[1], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix3, _posText[2], Color.White);
            _spriteBatch.DrawString(_fontTest, _choix4, _posText[3], Color.White);
            _spriteBatch.DrawString(_fontTest, _desc, _posText[4], Color.White);
            _spriteBatch.End();
        }
    }
}
