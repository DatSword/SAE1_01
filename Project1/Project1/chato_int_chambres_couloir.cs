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
using Oscetch.MonoGame.Camera;


namespace SAE101
{
    public class chato_int_chambres_couloir : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer mapLayer;
        private TiledMapTileLayer mapLayerIntersect;

        //sprite
        private AnimatedSprite _perso;
        public static Vector2 _positionPerso;       
        private KeyboardState _keyboardState;
        private int _sensPersoX;
        private int _sensPersoY;
        public static int _vitessePerso;
        public static int _posX;
        private int _stop;

        public static int _limiteChambreX1;
        public static int _limiteChambreX2;
        public static int _limiteCouloirY1;

        public chato_int_chambres_couloir(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn
            _posX = 0;

            if (chato_int_chambres_nord._posX == 0)
                _positionPerso = new Vector2(104, 112);
            if (chato_int_chambres_nord._posX >= 3 * 16 && chato_int_chambres_nord._posX < 5 * 16)
                _positionPerso = new Vector2(104, 112);
            else if (chato_int_chambres_nord._posX >= 11 * 16 && chato_int_chambres_nord._posX < 13 * 16)
                _positionPerso = new Vector2(14 * 16 + 8, 112);
            else if (chato_int_chambres_nord._posX >= 27 * 16 && chato_int_chambres_nord._posX < 29 * 16)
                _positionPerso = new Vector2(30 * 16 + 8, 112);
            else if (chato_int_chambres_nord._posX >= 35 * 16 && chato_int_chambres_nord._posX < 37 * 16)
                _positionPerso = new Vector2(38 * 16 + 8, 112);
            else if (chato_ext_cours_interieur._posX != 0)
                _positionPerso = new Vector2(22 * 16 + 8, 2 * 16 + 8);

            _stop = 1;

            _limiteChambreX1 = 19 * 16;
            _limiteChambreX2 = 25 * 16;
            _limiteCouloirY1 = 5 * 16;

            _sensPersoX = 0;
            _sensPersoY = 0;
            _vitessePerso = 100;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_couloir");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("collision");
            mapLayerIntersect = _tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");

            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/base_model_m/base_model_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Debug changement de map
            int a = mapLayerIntersect.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1)).GlobalIdentifier;
            Console.WriteLine(a);

            _keyboardState = Keyboard.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;
            String animation = "idle_down";

            //Camera
            Game1._camera.LookAt(Game1._cameraPosition);

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
            _perso.Play(animation);
            _perso.Update(deltaSeconds);


            //Changement de map          
            if (keyboardState.IsKeyDown(Keys.Up) && (a == 26))
            {
                _posX = (int)_positionPerso.X;
                Game.LoadScreenchato_int_chambres_nord();
            }        
            if (keyboardState.IsKeyDown(Keys.Up) && (a == 29 || a == 30))
            {
                Game.LoadScreenchato_ext_cours_interieur();
                chato_int_chambres_nord._posX = 0;
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

            _spriteBatch.Begin();
            if (Game1._dialTrue == true)
            {
                _spriteBatch.Draw(Game1._dialBox, Game1._posDialBox, Color.White);
                _spriteBatch.DrawString(Game1._font, Game1._text, Game1._posText, Color.White);
                _spriteBatch.DrawString(Game1._font, Game1._nom, Game1._posNom, Color.White);
            }
            _spriteBatch.End();
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