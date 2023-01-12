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
        private Camera _camera;

        //sprite
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public int _posX;

        private AnimatedSprite _ninja;
        private AnimatedSprite _ninja2;
        private AnimatedSprite _ninja3;

        private Vector2 _positionNinja;
        private Vector2 _positionNinja2;
        private Vector2 _positionNinja3;

        private String _animationNinja;
        private String _animationNinja2;
        private String _animationNinja3;

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
            _camera = _myGame._camera;

            // Lieu Spawn
            _posX = 0;
            _joueur.SpawnChatoExtCours();
            _myGame._numSalle = 2;

            //Scénario
            _positionNinja = new Vector2(21 * 16 + 8, 25 * 16 +8);
            _positionNinja2 = new Vector2(12 * 16 + 8, 21 * 16 + 8);
            _positionNinja3 = new Vector2(31 * 16 + 8, 23 * 16 + 8);
            _animationNinja = "idle_up";
            _animationNinja2 = "idle_left";
            _animationNinja3 = "idle_right";
            _collisionPassage = false;
            _eventEtDial._numDial = 3;

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
            _ninja = new AnimatedSprite(spriteSheet2);
            _ninja2 = new AnimatedSprite(spriteSheet2);
            _ninja3 = new AnimatedSprite(spriteSheet2);


            _eventEtDial.SetCollision();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Camera
            _camera._cameraMap.LookAt(_camera._cameraPosition);
     

            _tiledMapRenderer.Update(gameTime);
            _eventEtDial.BoiteDialogues();
            _joueur.Mouvement(gameTime);
            _perso.Play(_myGame._animationPlayer);
            _perso.Update(deltaSeconds);

            _ninja.Play(_animationNinja);
            _ninja2.Play(_animationNinja2);
            _ninja3.Play(_animationNinja3);
            _ninja.Update(deltaSeconds);
            _ninja2.Update(deltaSeconds);
            _ninja3.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();


            //Evenements

                // Ninja

            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 0)
            {
                _eventEtDial.FermeBoite();
                //_myGame.LoadScreenChatoCombat();
                _eventEtDial._numDial = 2;
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 1)
            {
                _eventEtDial.FermeBoite();         
                _eventEtDial._numDial = 0;
                _myGame.LoadScreenChatoCombat();

                //contenu du combat
                _myGame._nbAlly = 2;
                _myGame._ordreJoueur = new String[] { "Hero", "Jon" };
                _chatoCombat._premierCombat = false;
                _myGame._nbEnemy = 3;
                _myGame._ordreEnnemi = new String[] { "Grand", "Grand", "Grand" };
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == false && _eventEtDial._numDial == 2)
            {
                _eventEtDial.Jon4();
                _eventEtDial._numDial = 1;
            }
            else if (_myGame._positionPerso.Y <= 34 * 16 && _myGame._cooldownVerif == false && _eventEtDial._numDial == 3)
            {
                _animationNinja = "idle_down";
                _animationNinja2 = "idle_down";
                _animationNinja3 = "idle_down";
                _eventEtDial.Ninja();
                _eventEtDial._numDial = 2;
            }
            else if (_chatoCombat._victoire == true)
            {
                _eventEtDial.FermeBoite();
                _chatoCombat._victoire = false;
                _myGame._firstVisitCorridor = false;
            }


            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _collisionPassage == true)
            {
                _eventEtDial.FermeBoite();

            }
            else if (_myGame._positionPerso.Y < 31 * 16 && _myGame._positionPerso.Y > 28 * 16
                && (_myGame._positionPerso.X < 3 * 16 || _myGame._positionPerso.X > 41 * 16) && _myGame._cooldownVerif == false && _collisionPassage == false)
            {
                _collisionPassage = true;
                _eventEtDial.OuVasTu();
            }


            //changements maps

            if (_keyboardState.IsKeyDown(Keys.Down) && (_eventEtDial.dd == 43) && _myGame._positionPerso.Y > 49 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchatoIntChambresCouloir();
            }
            if (_keyboardState.IsKeyDown(Keys.Up) && (_eventEtDial.ud == 43) && _myGame._positionPerso.Y < 12 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenChatoCouronne();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            if ( _myGame._firstVisitCorridor == true)
            {
                _spriteBatch.Draw(_ninja, _positionNinja);
                _spriteBatch.Draw(_ninja2, _positionNinja2);
                _spriteBatch.Draw(_ninja3, _positionNinja3);
            }

            _spriteBatch.End();

            var transformMatrixDial = _camera._cameraDial.GetViewMatrix();
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