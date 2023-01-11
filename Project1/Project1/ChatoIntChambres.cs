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
    public class ChatoIntChambres : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _tiledMapRenderer;

        // défini dans Game1
        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private JoueurSpawn _joueur;

        //sprites
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public int _posX;

        private AnimatedSprite _jon;
        private Vector2 _posJon;
        private String _animJon;

        private AnimatedSprite _fren;
        private Vector2 _positionFren;
        private bool _frenTrue;

        private AnimatedSprite _chest1;
        private Vector2 _positionChest1;
        private bool _chestTrue;

        public Vector2 _chambreCentre1;
        public Vector2 _chambreCentreUn;
        public Vector2 _chambreCentre2;
        public Vector2 _chambreCentreDeux;

        public int _limiteChambreX1;
        public int _limiteChambreX2;
        public int _limiteChambreY1;
        public int _limiteChambreY2;
        public int _limiteChambreGauche;
        public int _limiteChambreDroite;

        private int _choixCursor;

        int numDial;

        public ChatoIntChambres(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;

            // Lieu Spawn perso
            _posJon = new Vector2(-10 * 16 + 8, -10 * 16 + 8);
            _animJon = "idle_up";
            _posX = 0;
            numDial = 0;

            _joueur.Spawnchato_int_chambres_nord();

            // Lieu Spawn objects
            _positionFren = new Vector2(28 * 16 + 8, 4 * 16 + 8);
            _frenTrue = false;

            _positionChest1 = new Vector2(38 * 16 + 8, 4 * 16 + 8);
            _chestTrue = false;
            
            // Emplacements pour camera
            _chambreCentre1 = new Vector2((float)4.6 * 16, 7 * 16);
            _chambreCentreUn = new Vector2((float)12.6 * 16, 7 * 16);

            _chambreCentre2 = new Vector2((float)28.6 * 16, 7 * 16);
            _chambreCentreDeux = new Vector2((float)36.6 * 16, 7 * 16);

            _limiteChambreX1 = 16 * 16;
            _limiteChambreX2 = 24 * 16;
            _limiteChambreY1 = 7 * 16;

            _limiteChambreGauche = 8 * 16;
            _limiteChambreDroite = 32 * 16;
            _choixCursor = 1;
            
            
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_nord");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            _eventEtDial.SetCollision();

            //Load persos
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
            SpriteSheet spriteSheet4 = Content.Load<SpriteSheet>("anim/char/ally/Jon/character_movement.sf", new JsonContentLoader());
            _jon = new AnimatedSprite(spriteSheet4);


            //Load objects
            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("anim/char/Fren/Fren.sf", new JsonContentLoader());
            _fren = new AnimatedSprite(spriteSheet2);

            SpriteSheet spriteSheet3 = Content.Load<SpriteSheet>("anim/objects/chest1.sf", new JsonContentLoader());
            _chest1 = new AnimatedSprite(spriteSheet3);


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
            

            if (_eventEtDial._choiceTrue == true)
            {
                if (_keyboardState.IsKeyDown(Keys.Up) && _choixCursor == 1 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    _eventEtDial._posCursor = new Vector2(430, 301);
                }
                else if (_keyboardState.IsKeyDown(Keys.Down) && _choixCursor == 0 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    _eventEtDial._posCursor = new Vector2(430, 301);
                }
                Console.WriteLine(_myGame._cooldownVerif);

                if (_keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    Game1._fin = 1;
                    _myGame.LoadScreenblack_jack();
                    
                }
                if ((_keyboardState.IsKeyDown(Keys.W) && _choixCursor ==1) || _keyboardState.IsKeyDown(Keys.X) && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    _eventEtDial._choiceTrue = false;
                    _eventEtDial._dialTrue = false;
                }
            }


            //MOUVEMENT/ANIMATION OBJETS

            //:)
            String animationFren = null;
            if (_frenTrue == false)
                animationFren = "idle";
            else
                animationFren = "hi";

            if (_keyboardState.IsKeyDown(Keys.W) && (EventEtDial.u == 70) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _eventEtDial.FermeBoite();
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && (EventEtDial.u == 70) && animationFren == "idle" && _myGame._cooldownVerif == false
                && _myGame._positionPerso.X < _limiteChambreDroite)
            {
                _eventEtDial.Fren1();
                _frenTrue = true;
            }        
            else if (_keyboardState.IsKeyDown(Keys.W) && (EventEtDial.u == 70) && animationFren == "hi" && _myGame._cooldownVerif == false)
            {
                _eventEtDial.Fren2();
                _frenTrue = false;
            }
            _fren.Play(animationFren);
            _fren.Update(deltaSeconds);

            //Coffre(s?)
            String animationChest = null;
            if (_chestTrue == false)
                animationChest = "close";
            else
                animationChest = "open";

            if (_keyboardState.IsKeyDown(Keys.W) && (EventEtDial.u == 70) && animationChest == "close" 
                && _myGame._positionPerso.X > _limiteChambreDroite)
                _chestTrue = true;
            

            _chest1.Play(animationChest);
            _chest1.Update(deltaSeconds);

            _jon.Play(_animJon);
            _jon.Update(deltaSeconds);


            //EVENEMENTS

            if (Game1._firstVisitBedroom == true && _myGame._cooldownVerif == false && numDial == 0)
            {
                _eventEtDial.Jon1();
                _posJon = new Vector2(4*16+8,4*16+8);
                Game1._firstVisitBedroom = false;
                numDial = 1;
            }
            if (_myGame._cooldownVerif == false && _keyboardState.IsKeyDown(Keys.W) && numDial == 1)
            {
                _eventEtDial.Jon2();
                numDial = 2;
            }
            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false &&  numDial == 2)
            {
                _eventEtDial.FermeBoite();
                _animJon = "move_down";
                numDial = 3;
            }

            //DODO
            if (_keyboardState.IsKeyDown(Keys.W) && (EventEtDial.l == 72) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false && _myGame._cooldownVerif == false)
            {
                //Event_et_dial.Fin1();
                Game1._fin = 1;
                Game.LoadScreenblack_jack();
            }

            if (numDial == 3)
                _posJon.Y += _myGame._walkSpeed;
            if (_posJon.Y > 8 * 16 + 8)
            {
                _posJon = new Vector2(-100 * 16 + 8, -10 * 16 + 8);
            }
                

            //changement de map
            if (_keyboardState.IsKeyDown(Keys.Down) && (EventEtDial.dd == 41))
            {
                _posX = (int)_myGame._positionPerso.X;
                Game.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = _myGame._camera.GetViewMatrix();
            
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_fren, _positionFren);
            _spriteBatch.Draw(_chest1, _positionChest1);
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            _spriteBatch.Draw(_jon, _posJon);

            _spriteBatch.End();

            var transformMatrixDial = _myGame._cameraDial.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrixDial);
            if (_eventEtDial._dialTrue == true)
            {
                _spriteBatch.Draw(_eventEtDial._dialBox, _eventEtDial._posDialBox, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._text, _eventEtDial._posText, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._nom, _eventEtDial._posNom, Color.White);
                
            }
            if (_eventEtDial._choiceTrue == true)
            {
                _spriteBatch.Draw(_eventEtDial._choiceBox, _eventEtDial._posChoiceBox, Color.White);
                _spriteBatch.Draw(_eventEtDial._cursor, _eventEtDial._posCursor, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._yes, _eventEtDial._posYes, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._no, _eventEtDial._posNo, Color.White);
            }
            _spriteBatch.End();
        }
    }
}