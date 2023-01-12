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
    public class ChatoIntCouloir : GameScreen
    {
        // défini dans Game1
        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private JoueurSpawn _joueur;
        private ChatoIntChambres _chatoIntChambres;
        private ChatoCombat _chatoCombat;
        private ChatoCombatContenu _chatoCombatContenu;
        private Camera _camera;

        //map
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _tiledMapRenderer;

        private KeyboardState _keyboardState;

        //sprite
        private AnimatedSprite _perso;
        public int _vitessePerso;
        public int _posX;

        private AnimatedSprite _ennemi;
        private Vector2 _positionEnnemi;
        private String _animationEnnemi;

        private bool _rencontre;

        private AnimatedSprite _Jon;
        private Vector2 _positionJon;
        private String _animationJon;

        public int _limChambre_x1;
        public int _limChambre_x2;
        public int _limCouloir;



        public ChatoIntCouloir(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;
            _chatoIntChambres = _myGame._chatoIntChambres;
            _chatoCombat = _myGame._chatoCombat;
            _camera = _myGame._camera;

            // Lieu Spawn
            _posX = 0;

            _joueur.SpawnChatoIntChambresCouloir();

            _limChambre_x1 = 19 * 16;
            _limChambre_x2 = 25 * 16;
            _limCouloir = 6 * 16;

            _vitessePerso = 100;
            _myGame._numSalle = 1;

            _positionEnnemi = new Vector2(26 * 16 + 8, 9 * 16 + 8);
            _animationEnnemi = "idle_down";

            _rencontre = false;
            _eventEtDial._numDial = 2;

            _positionJon = new Vector2(19 * 16 + 8, 7 * 16);
            _animationJon = "idle_down";

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_couloir");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);     
            
            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetA);

            SpriteSheet spriteSheetE = Content.Load<SpriteSheet>("anim/char/enemy/mechant/character_movement.sf", new JsonContentLoader());
            _ennemi = new AnimatedSprite(spriteSheetE);

            SpriteSheet spriteSheetJ = Content.Load<SpriteSheet>("anim/char/ally/Jon/character_movement.sf", new JsonContentLoader());
            _Jon = new AnimatedSprite(spriteSheetJ);

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
            
            _joueur.Mouvement(gameTime);
            _perso.Play(_myGame._animationPlayer);
            _perso.Update(deltaSeconds);
            _ennemi.Play(_animationEnnemi);
            _ennemi.Update(deltaSeconds);
            _Jon.Play(_animationJon);
            _Jon.Update(deltaSeconds);
            _eventEtDial.BoiteDialogues();

            //Enclenchement evenement

            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _rencontre = true;
                //_eventEtDial.FermeBoite();
                _myGame.LoadScreenChatoCombat();

                //contenu du combat
                _myGame._nbAlly = 2;
                _myGame._ordreJoueur = new String[] { "Hero", "Jon"};

                _myGame._nbEnemy = 3;
                _myGame._ordreEnnemi = new String[] {"Mechant","Mechant","Mechant"};
            }
            else if (_myGame._positionPerso.X >= 19 * 16 && _myGame._cooldownVerif == false && _rencontre == false && _eventEtDial._numDial == 2 && _myGame._firstVisitCorridor == true)
            {
                _animationEnnemi = "idle_left";
                _animationJon = "idle_right";
                _eventEtDial.Jon3();
                _eventEtDial._numDial = 1;
            }
            else if (_chatoCombat._victoire == true)
            {
                _myGame._firstVisitCorridor = false;
                _rencontre = true;
                _eventEtDial.FermeBoite();
                _chatoCombat._victoire = false;
                _eventEtDial._numDial = 3;
            }



            //Changement de map          
            if (_keyboardState.IsKeyDown(Keys.Up) && (_eventEtDial.ud == 26))
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchatoIntChambresNord();
            }        
            if (_keyboardState.IsKeyDown(Keys.Up) && (_eventEtDial.ud == 30))
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchatoExtCoursInterieur();
                _chatoIntChambres._posX = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            if (_rencontre == false && _myGame._firstVisitCorridor == true)
                _spriteBatch.Draw(_ennemi, _positionEnnemi);
            if (_myGame._firstVisitCorridor == true)
                _spriteBatch.Draw(_Jon, _positionJon);


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