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


namespace SAE101
{
    public class chato_ext_cours_interieur : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;

        //sprite
        private AnimatedSprite _perso;
        public static Vector2 _positionPerso;
        private KeyboardState _keyboardState;
        private int _sensPersoX;
        private int _sensPersoY;
        private int _vitessePerso;
        public static int _posX;
        private int _stop;

        public chato_ext_cours_interieur(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn
            _posX = 0;
            _stop = 1;

            _positionPerso = new Vector2(40, 480);
            _positionPerso = new Vector2(22*16, 49*16);
            _sensPersoX = 0;
            _sensPersoY = 0;

            _vitessePerso = 100;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_ext_cours_interieur");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("collision");

            //Load Perso
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/base_model_m/base_model_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //debug map
            int a = mapLayer.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1)).GlobalIdentifier;
            //Console.WriteLine(a);

            _sensPersoX = 0;
            _sensPersoY = 0;

            //Camera
            Game1._camera.LookAt(Game1._cameraPosition);

            _keyboardState = Keyboard.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;
            String animation = "idle_down";

            // TODO: Add your update logic here
            _tiledMapRenderer.Update(gameTime);

            //Mouvement/animation
            if (_stop == 1 && keyboardState.IsKeyUp(Keys.Down))
                animation = "idle_down";
            else if (_stop == 2 && keyboardState.IsKeyUp(Keys.Up))
                animation = "idle_up";
            else if (_stop == 3 && keyboardState.IsKeyUp(Keys.Left))
                animation = "idle_left";
            else if (_stop == 4 && keyboardState.IsKeyUp(Keys.Right))
                animation = "idle_right";

            if (eventsetdial._dialTrue == false)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1);
                    animation = "move_up";
                    _stop = 2;
                    if (!IsCollision(tx, ty))
                        _positionPerso.Y -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1);
                    animation = "move_down";
                    _stop = 1;
                    if (!IsCollision(tx, ty))
                        _positionPerso.Y += walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_left";
                    _stop = 3;
                    if (!IsCollision(tx, ty))
                        _positionPerso.X -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_right";
                    _stop = 4;
                    if (!IsCollision(tx, ty))
                        _positionPerso.X += walkSpeed;
                }
            }
            _perso.Play(animation);
            _perso.Update(deltaSeconds);


            //changements maps

            if (keyboardState.IsKeyDown(Keys.Down) && (a == 101) && _positionPerso.Y >= 48 * 16)
            {
                _posX = (int)_positionPerso.X;
                Game.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(Game1._camera.GetViewMatrix());
            _spriteBatch.Draw(_perso, _positionPerso);
            _spriteBatch.End();

            var transformMatrixDial = Game1._cameraDial.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrixDial);
            if (eventsetdial._dialTrue == true)
            {
                _spriteBatch.Draw(eventsetdial._dialBox, eventsetdial._posDialBox, Color.White);
                _spriteBatch.DrawString(Game1._font, eventsetdial._text, eventsetdial._posText, Color.White);
                _spriteBatch.DrawString(Game1._font, eventsetdial._nom, eventsetdial._posNom, Color.White);
            }
        }

        private bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}