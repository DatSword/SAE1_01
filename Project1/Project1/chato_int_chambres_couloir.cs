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
    public class Chato_int_chambres_couloir : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;
        private TiledMapTileLayer mapLayerIntersect;

        //sprite
        private AnimatedSprite _perso;
        //public static Vector2 _positionPerso;       
        private KeyboardState _keyboardState;
        private int _sensPersoX;
        private int _sensPersoY;
        public static int _vitessePerso;
        public static int _posX;
        private int _stop;

        public static int _limiteChambreX1;
        public static int _limiteChambreX2;
        public static int _limiteCouloirY1;
        public static int _limiteCouloirY2;

        public Chato_int_chambres_couloir(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn
            _posX = 0;

            Joueur.Spawnchato_int_chambres_couloir();

            _stop = 1;

            _limiteChambreX1 = 19 * 16;
            _limiteChambreX2 = 25 * 16;
            _limiteCouloirY1 = 5 * 16;
            _limiteCouloirY1 = 5 * 16;

            _sensPersoX = 0;
            _sensPersoY = 0;
            _vitessePerso = 100;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Game1._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_couloir");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            mapLayer = Game1._tiledMap.GetLayer<TiledMapTileLayer>("collision");
            mapLayerIntersect = Game1._tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");

            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            _keyboardState = Keyboard.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;
            String animation = "idle_down";

            //Camera
            Game1._camera.LookAt(Game1._cameraPosition);

            _tiledMapRenderer.Update(gameTime);

            //Debug changement de map
            int a = mapLayerIntersect.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight - 1)).GlobalIdentifier;
            //Console.WriteLine(a);

            //Mouvement/animation
            if (_stop == 1 && keyboardState.IsKeyUp(Keys.Down))
                animation = "idle_down";
            else if (_stop == 2 && keyboardState.IsKeyUp(Keys.Up))
                animation = "idle_up";
            else if (_stop == 3 && keyboardState.IsKeyUp(Keys.Left))
                animation = "idle_left";
            else if (_stop == 4 && keyboardState.IsKeyUp(Keys.Right))
                animation = "idle_right";

            if (Event_et_dial._dialTrue == false)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / _tiledMap.TileHeight - 1);
                    animation = "move_up";
                    _stop = 2;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / _tiledMap.TileHeight + 1);
                    animation = "move_down";
                    _stop = 1;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y += walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / _tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_left";
                    _stop = 3;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / _tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_right";
                    _stop = 4;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X += walkSpeed;
                }
            }
            _perso.Play(animation);
            _perso.Update(deltaSeconds);

            //Enclenchement evenment

            //Changement de map          
            if (keyboardState.IsKeyDown(Keys.Up) && (a == 26))
            {
                _posX = (int)Game1._positionPerso.X;
                Game.LoadScreenchato_int_chambres_nord();
                
            }        
            if (keyboardState.IsKeyDown(Keys.Up) && (a == 30))
            {
                _posX = (int)Game1._positionPerso.X;
                Game.LoadScreenchato_ext_cours_interieur();
                Chato_int_chambres_nord._posX = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(Game1._camera.GetViewMatrix());
            _spriteBatch.Draw(_perso, Game1._positionPerso);
            _spriteBatch.End();

            var transformMatrixDial = Game1._cameraDial.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrixDial);
            if (Event_et_dial._dialTrue == true)
            {
                _spriteBatch.Draw(Event_et_dial._dialBox, Event_et_dial._posDialBox, Color.White);
                _spriteBatch.DrawString(Game1._font, Event_et_dial._text, Event_et_dial._posText, Color.White);
                _spriteBatch.DrawString(Game1._font, Event_et_dial._nom, Event_et_dial._posNom, Color.White);
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