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
    public class Chato_int_couloir : GameScreen
    {
        // défini dans Game1
        private Game1 _myGame;
        private Event_et_dial _eventEtDial;
        private Joueur _joueur;

        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;
        private TiledMapTileLayer mapLayerIntersect;

        private KeyboardState _keyboardState;

        //sprite
        private AnimatedSprite _perso;
        //public static Vector2 _positionPerso;       
        private int _sensPersoX;
        private int _sensPersoY;
        public static int _vitessePerso;
        public static int _posX;
        private int _stop;

        private AnimatedSprite _ennemi;
        private Vector2 _positionEnnemi;  

        public int _limiteChambreX1;
        public int _limiteChambreX2;
        public int _limiteCouloirY1;
        public int _limiteCouloirY2;

        public Chato_int_couloir(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;

            // Lieu Spawn
            _posX = 0;

            _joueur.Spawnchato_int_chambres_couloir();

            _stop = 1;

            _limiteChambreX1 = 19 * 16;
            _limiteChambreX2 = 25 * 16;
            _limiteCouloirY1 = 5 * 16;
            _limiteCouloirY1 = 5 * 16;

            _sensPersoX = 0;
            _sensPersoY = 0;
            _vitessePerso = 100;

            _positionEnnemi = new Vector2(26 * 16, 9 * 16);

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Game1._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_couloir");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, Game1._tiledMap);     
            
            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetA);

            SpriteSheet spriteSheetE = Content.Load<SpriteSheet>("anim/char/enemy/mechant/character_movement.sf", new JsonContentLoader());
            _ennemi = new AnimatedSprite(spriteSheetE);

            _eventEtDial.SetCollision();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            Game1._camera.LookAt(_myGame._cameraPosition);


            _tiledMapRenderer.Update(gameTime);
            
            _joueur.Mouvement(gameTime);
            _perso.Play(Game1._animationPlayer);
            _perso.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();

            //Enclenchement evenment

            //Changement de map          
            if (_keyboardState.IsKeyDown(Keys.Up) && (Event_et_dial.ud == 26))
            {
                _posX = (int)Game1._positionPerso.X;
                _myGame.LoadScreenchato_int_chambres_nord();
                
            }        
            if (_keyboardState.IsKeyDown(Keys.Up) && (Event_et_dial.ud == 30))
            {
                _posX = (int)Game1._positionPerso.X;
                _myGame.LoadScreenchato_ext_cours_interieur();
                Chato_int_chambres._posX = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_perso, Game1._positionPerso);
            _spriteBatch.Draw(_ennemi, _positionEnnemi);
            _spriteBatch.End();

            var transformMatrixDial = _myGame._cameraDial.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrixDial);
            if (_eventEtDial._dialTrue == true)
            {
                _spriteBatch.Draw(_eventEtDial._dialBox, _eventEtDial._posDialBox, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._text, _eventEtDial._posText, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._nom, _eventEtDial._posNom, Color.White);
            }
            _spriteBatch.End();
        }
    }
}