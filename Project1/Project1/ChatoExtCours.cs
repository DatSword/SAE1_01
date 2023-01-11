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
    public class ChatoExtCours : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _tiledMapRenderer;
        public TiledMapTileLayer mapLayer;

        // défini dans Game1
        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private JoueurSpawn _joueur;

        //sprite
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public int _posX;

        private AnimatedSprite _grand;
        private Vector2 _positionGrand;
        private String _animationGrand;


        public ChatoExtCours(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;

            // Lieu Spawn
            _posX = 0;

            _joueur.Spawnchato_ext_cours_interieur();

            _myGame._numSalle = 2;

            _positionGrand = new Vector2(21 * 16 +8, 25 * 16 +8);
            _animationGrand = "idle_up";

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_ext_cours_interieur");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);

            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("anim/char/enemy/grand/character_movement.sf", new JsonContentLoader());
            _grand = new AnimatedSprite(spriteSheet2);

            _eventEtDial.SetCollision();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            _myGame._camera.LookAt(_myGame._cameraPosition);

     

            _tiledMapRenderer.Update(gameTime);
            _eventEtDial.BoiteDialogues();
            _joueur.Mouvement(gameTime);
            _perso.Play(_myGame._animationPlayer);
            _perso.Update(deltaSeconds);

            _grand.Play(_animationGrand);
            _grand.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();


            if ( _myGame._positionPerso.Y <= 34 * 16)
            {
                _animationGrand = "idle_down";
                _eventEtDial.Ninja();
                if (_keyboardState.IsKeyDown(Keys.W))
                    _myGame.LoadScreenchato_combat();
            }

            if (_myGame._positionPerso.Y > 34 * 16)
                _animationGrand = "idle_up";

            //changements maps

            if (_keyboardState.IsKeyDown(Keys.Down) && (EventEtDial.dd == 43) && _myGame._positionPerso.Y > 49 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _myGame._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            _spriteBatch.Draw(_grand, _positionGrand);

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