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

        //sprites
        private AnimatedSprite _perso;
        public static Vector2 _positionPerso;
        private KeyboardState _keyboardState;
        private int _sensPersoX;
        private int _sensPersoY;
        private int _vitessePerso;
        public static int _posX;
        private bool stopLeft;
        private bool stopRight;
        private bool stopUp;
        private bool stopDown;
        private int _stop;

        private AnimatedSprite _fren;
        private Vector2 _positionFren;
        private bool _frenTrue;

        private AnimatedSprite _chest1;
        private Vector2 _positionChest1;
        private bool _chestTrue;

        public static Vector2 _chambreCentre1;
        public static Vector2 _chambreCentre2;
        public static int _limiteChambreX1;
        public static int _limiteChambreX2;
        public static int _limiteChambreY1;
        public static int _limiteChambreY2;

        private float _cooldown;
        private bool _cooldownVerif;



        public chato_int_chambres_nord(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn perso
            _posX = 0;

            if (_posX == 0)
                _positionPerso = new Vector2(4 * 16 + 8, 3 * 16 + 8);
            if (chato_int_chambres_couloir._posX >= 5 * 16 && chato_int_chambres_couloir._posX < 7 * 16)
                _positionPerso = new Vector2(72, 111);
            else if (chato_int_chambres_couloir._posX >= 13 * 16 && chato_int_chambres_couloir._posX < 15 * 16)
                _positionPerso = new Vector2(12 * 16 + 8, 111);
            else if (chato_int_chambres_couloir._posX >= 29 * 16 && chato_int_chambres_couloir._posX < 31 * 16)
                _positionPerso = new Vector2(28 * 16 + 8, 111);
            else if (chato_int_chambres_couloir._posX >= 37 * 16 && chato_int_chambres_couloir._posX < 39 * 16)
                _positionPerso = new Vector2(36 * 16 + 8, 111);
            //x = casex * 16 + 8, y = casey * 16 + 8
            _stop = 1;

            // Lieu Spawn objects
            _positionFren = new Vector2(28 * 16 + 8, 4 * 16 + 8);
            _frenTrue = false;

            _positionChest1 = new Vector2(38 * 16 + 8, 4 * 16 + 8);
            _chestTrue = false;

            /*_positionDial = new Vector2(0, 348);
            _dialTrue = false;*/

            _chambreCentre1 = new Vector2((float)6.5 * 16, 6 * 16);
            _chambreCentre2 = new Vector2((float)30.5 * 16, 6 * 16);
            _limiteChambreX1 = 16 * 16;
            _limiteChambreX2 = 24 * 16;
            _limiteChambreY1 = 8 * 16;

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

            //Load persos
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/base_model_m/base_model_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);

            //Load objects
            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("anim/char/Fren/Fren.sf", new JsonContentLoader());
            _fren = new AnimatedSprite(spriteSheet2);

            SpriteSheet spriteSheet3 = Content.Load<SpriteSheet>("anim/objects/chest1.sf", new JsonContentLoader());
            _chest1 = new AnimatedSprite(spriteSheet3);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //debug map
            int a = mapLayerIntersect.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1)).GlobalIdentifier;
            Console.WriteLine(a);
            //debug autres collisions
            int b = mapLayer.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1)).GlobalIdentifier;
            Console.WriteLine(b);

            _sensPersoX = 0;
            _sensPersoY = 0;

            //Camera
            Game1._camera.LookAt(Game1._cameraPosition);

            _keyboardState = Keyboard.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            _tiledMapRenderer.Update(gameTime);

            //Mouvement/animation perso
            float walkSpeed = deltaSeconds * _vitessePerso;
            String animation = null;

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


            //MOUVEMENT/ANIMATION OBJETS

            //:)
            String animationFren = null;
            if (_frenTrue == false)
                animationFren = "idle";
            else
                animationFren = "hi";
            if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "idle" && _cooldownVerif == false)
            {
                eventsetdial.Event2();
                _frenTrue = true;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }
            else if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "hi" && _cooldownVerif == false)
            {
                _frenTrue = false;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }



            _fren.Play(animationFren);
            _fren.Update(deltaSeconds);


            //Coffre(s?)
            String animationChest = null;
            if (_chestTrue == false)
                animationChest = "close";
            else
                animationChest = "open";

            if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationChest == "close")
                _chestTrue = true;

            _chest1.Play(animationChest);
            _chest1.Update(deltaSeconds);

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }
            //EVENEMENTS

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
            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(Game1._camera.GetViewMatrix());
            _spriteBatch.Draw(_fren, _positionFren);
            _spriteBatch.Draw(_chest1, _positionChest1);
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
            TiledMapTile? tile;
            if (mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}