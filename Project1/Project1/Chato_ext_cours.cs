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
    public class Chato_ext_cours : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;

        // défini dans Game1
        private Game1 _myGame;
        private Event_et_dial _eventEtDial;
        private Joueur _joueur;

        //sprite
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public static int _posX;

        private AnimatedSprite _grand;
        private Vector2 _positionGrand;


        public Chato_ext_cours(Game1 game) : base(game)
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

            Game1._numSalle = 2;

            _positionGrand = new Vector2(21 * 16 +8, 25 * 16 +8);


            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Game1._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_ext_cours_interieur");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, Game1._tiledMap);

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
            Game1._camera.LookAt(_myGame._cameraPosition);

            String animationGrand = null;

            _tiledMapRenderer.Update(gameTime);
            _eventEtDial.BoiteDialogues();
            _joueur.Mouvement(gameTime);
            _perso.Play(Game1._animationPlayer);
            _perso.Update(deltaSeconds);

            animationGrand = "idle_up"; 


            if (Game1._positionPerso.X >= 20 && Game1._positionPerso.X <= 23 && Game1._positionPerso.Y == 34)
            {
                animationGrand = "idle_down";
                _eventEtDial.Ninja();
            }

            _grand.Play(animationGrand);
            _grand.Update(deltaSeconds);

            //changements maps

            if (_keyboardState.IsKeyDown(Keys.Down) && (Event_et_dial.dd == 43) && Game1._positionPerso.Y > 49 * 16)
            {
                _posX = (int)Game1._positionPerso.X;
                _myGame.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(Game1._camera.GetViewMatrix());
            _spriteBatch.Draw(_perso, Game1._positionPerso);

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