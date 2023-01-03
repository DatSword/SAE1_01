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
using System;


namespace SAE101
{
    public class chato_int_chambres_nord : GameScreen
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
        private Vector2 _positionPerso;        
        private KeyboardState _keyboardState;
        private int _sensPersoX;
        private int _sensPersoY;
        private int _vitessePerso;
        public static int _posX;

        private int test;  
        public chato_int_chambres_nord(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn
            _posX = 0;

            if (chato_int_chambres_couloir._posX  >= 5*16 && chato_int_chambres_couloir._posX < 7*16)
                _positionPerso = new Vector2(72, 111);
            else if (chato_int_chambres_couloir._posX >= 13 * 16 && chato_int_chambres_couloir._posX < 15 * 16)
                _positionPerso = new Vector2(12*16+8, 111);
            else if (chato_int_chambres_couloir._posX >= 29 * 16 && chato_int_chambres_couloir._posX < 31 * 16)
                _positionPerso = new Vector2(28*16+8, 111);
            else if (chato_int_chambres_couloir._posX >= 37 * 16 && chato_int_chambres_couloir._posX < 39 * 16)
                _positionPerso = new Vector2(36*16+8, 111);
            //x = casex * 16 + 8, y = casey * 16 + 8

            _sensPersoX = 0;
            _sensPersoY = 0;
            _vitessePerso = 100;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_nord");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("collision");
            mapLayerIntersect = _tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");

            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/base_model_m/base_model_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _sensPersoX = 0;
            _sensPersoY = 0;


            _keyboardState = Keyboard.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;
            String animation = "idle_down";

            // TODO: Add your update logic here
            _tiledMapRenderer.Update(gameTime);



            //Mouvement/animation
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1);
                animation = "move_up";
                if (!IsCollision(tx, ty))
                    _positionPerso.Y -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1);
                animation = "move_down";
                if (!IsCollision(tx, ty))
                    _positionPerso.Y += walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                animation = "move_left";
                if (!IsCollision(tx, ty))
                    _positionPerso.X -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                animation = "move_right";
                if (!IsCollision(tx, ty))
                    _positionPerso.X += walkSpeed;
            }
            _perso.Play(animation);
            _perso.Update(deltaSeconds);

            //debug changment de map
            int a = mapLayerIntersect.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1)).GlobalIdentifier;
            Console.WriteLine(a);
            //changement de map
            if (keyboardState.IsKeyDown(Keys.Down) && (a == 41))
            {
                _posX = (int)_positionPerso.X;
                Game.LoadScreenchato_int_chambres_couloir();
            }              
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _tiledMapRenderer.Draw();
            _spriteBatch.Draw(_perso, _positionPerso);
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