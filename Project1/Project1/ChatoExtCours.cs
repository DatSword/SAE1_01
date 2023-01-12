using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using AnimatedSprite = MonoGame.Extended.Sprites.AnimatedSprite;
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
        private ChatoCombat _chatoCombat;

        //sprite
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public int _posX;

        private AnimatedSprite _grand;
        private AnimatedSprite _grand2;
        private AnimatedSprite _grand3;

        private Vector2 _positionGrand;
        private Vector2 _positionGrand2;
        private Vector2 _positionGrand3;

        private String _animationGrand;
        private String _animationGrand2;
        private String _animationGrand3;

        private bool _rencontre;
        private bool _collisionPassage;


        public ChatoExtCours(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;
            _chatoCombat = _myGame._chatoCombat;

            // Lieu Spawn
            _posX = 0;

            _joueur.Spawnchato_ext_cours_interieur();

            _myGame._numSalle = 2;

            _positionGrand = new Vector2(21 * 16 + 8, 25 * 16 +8);
            _positionGrand2 = new Vector2(12 * 16 + 8, 21 * 16 + 8);
            _positionGrand3 = new Vector2(31 * 16 + 8, 23 * 16 + 8);
            _animationGrand = "idle_up";
            _animationGrand2 = "idle_left";
            _animationGrand3 = "idle_right";

            _rencontre = false;
            _collisionPassage = false;

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
            _grand2 = new AnimatedSprite(spriteSheet2);
            _grand3 = new AnimatedSprite(spriteSheet2);


            _eventEtDial.SetCollision();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            _myGame._cameraMap.LookAt(_myGame._cameraPosition);
     

            _tiledMapRenderer.Update(gameTime);
            _eventEtDial.BoiteDialogues();
            _joueur.Mouvement(gameTime);
            _perso.Play(_myGame._animationPlayer);
            _perso.Update(deltaSeconds);

            _grand.Play(_animationGrand);
            _grand2.Play(_animationGrand2);
            _grand3.Play(_animationGrand3);
            _grand.Update(deltaSeconds);
            _grand2.Update(deltaSeconds);
            _grand3.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();


            //Evenements

                // Ninja



            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 0)
            {
                _rencontre = true;
                _eventEtDial.FermeBoite();
                _myGame.LoadScreenchato_combat();
                _eventEtDial._numDial = 2;
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 1)
            {
                _eventEtDial.FermeBoite();         
                _eventEtDial._numDial = 0;
                _myGame.LoadScreenchato_combat();
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 2)
            {
                _eventEtDial.Jon4();
                _eventEtDial._numDial = 1;
            }
            else if (_myGame._positionPerso.Y <= 34 * 16 && _myGame._cooldownVerif == false && _rencontre == false && _eventEtDial._numDial == 3)
            {
                _animationGrand = "idle_down";
                _animationGrand2 = "idle_down";
                _animationGrand3 = "idle_down";
                _eventEtDial.Ninja();
                _eventEtDial._numDial = 2;
            }
            else if (_chatoCombat._victoire == true)
            {
                _rencontre = true;
                _eventEtDial.FermeBoite();
                _chatoCombat._victoire = false;
            }


            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == true)
            {
                _eventEtDial.FermeBoite();

            }
            else if (_myGame._positionPerso.Y < 31 * 16 && _myGame._positionPerso.Y > 28 * 16
                && (_myGame._positionPerso.X < 2 * 16 || _myGame._positionPerso.X > 41 * 16) && _myGame._cooldownVerif == false && _collisionPassage == false)
            {
                _collisionPassage = true;
                _eventEtDial.OuVasTu();
            }


            //changements maps

            if (_keyboardState.IsKeyDown(Keys.Down) && (EventEtDial.dd == 43) && _myGame._positionPerso.Y > 49 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchato_int_couloir();
            }
            if (_keyboardState.IsKeyDown(Keys.Up) && (EventEtDial.ud == 43) && _myGame._positionPerso.Y < 12 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchato_int_couronne();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _myGame._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            if (_rencontre == false)
            {
                _spriteBatch.Draw(_grand, _positionGrand);
                _spriteBatch.Draw(_grand2, _positionGrand2);
                _spriteBatch.Draw(_grand3, _positionGrand3);
            }

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