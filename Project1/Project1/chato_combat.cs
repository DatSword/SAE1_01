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
        private int _choix;


        public chato_combat(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _positionCombat = new Vector2(0, 248);
            _positionCursor = new Vector2(16,300);
            _choix = 1;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _chatoCombatDecor = Content.Load<Texture2D>("img/chato/combat_decor");
            _combatBox = Content.Load<Texture2D>("img/dialogue/combat_box");
            _cursor = Content.Load<Texture2D>("img/dialogue/cursor");


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //curseur controls

            if (keyboardState.IsKeyDown(Keys.Down) && _choix < 4)
            {
                _positionCursor.Y = _positionCursor.Y + 36;
                _choix = _choix + 1;
            }
            if (keyboardState.IsKeyDown(Keys.Up) && _choix > 1)
            {
                _positionCursor.Y = _positionCursor.Y - 36;
                _choix = _choix - 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_chatoCombatDecor, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_combatBox, _positionCombat , Color.White);
            _spriteBatch.Draw(_cursor, _positionCursor, Color.White);
            _spriteBatch.End();
        }
    }
}
