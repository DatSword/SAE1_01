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
    public class ChatoIntTrone : GameScreen
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

        // ennemi
        private AnimatedSprite _ennemiPabo;
        private Vector2 _positionEnnemiPabo;
        private String _animationPabo;

        private bool _rencontre;


        public ChatoIntTrone(Game1 game) : base(game)
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

            _joueur.Spawnchato_int_couronne();

            _myGame._numSalle = 5;

            _positionEnnemiPabo = new Vector2(10 * 16 + 8, 24 * 16);
            _animationPabo = "idle_up";

            _rencontre = false;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_salle_courronnement");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetA);

            SpriteSheet spriteSheetE = Content.Load<SpriteSheet>("anim/char/enemy/pabo/character_movement.sf", new JsonContentLoader());
            _ennemiPabo = new AnimatedSprite(spriteSheetE);

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

            _ennemiPabo.Play(_animationPabo);
            _ennemiPabo.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();

            //Evenements

            // Battle final (mettre la bonne replique à ennemi)

            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _rencontre = true;
                _eventEtDial.FermeBoite();
                //_myGame.LoadScreenchato_combat();
            }
            else if ( _myGame._positionPerso.Y <= 31 * 16 && _myGame._cooldownVerif == false && _rencontre == false)
            {
                _animationPabo = "idle_down";
                _eventEtDial.Ninja();
            }
            else if (_chatoCombat._victoire == true)
            {
                _rencontre = true;
                _eventEtDial.FermeBoite();
            }

            // fin 
            if (_myGame._positionPerso.Y <= 10 * 16)
            {
                _myGame._fin = 3;
                Game.LoadScreenblack_jack();
            }


            //changements maps
            if (_keyboardState.IsKeyDown(Keys.Down) && (EventEtDial.dd == 1) && _myGame._positionPerso.Y > 38 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchato_ext_cours_interieur();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _myGame._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(_myGame._camera.GetViewMatrix());
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            
            if (_rencontre == false)
                _spriteBatch.Draw(_ennemiPabo, _positionEnnemiPabo);

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