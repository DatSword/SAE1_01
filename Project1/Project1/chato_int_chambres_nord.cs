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
    public class Chato_int_chambres_nord : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;
        private TiledMapTileLayer mapLayerIntersect;

        // défini dans Game1
        private Game1 _myGame;
        private Event_et_dial _eventEtDial;

        //sprites
        private AnimatedSprite _perso;
        //public static Vector2 _positionPerso;
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
        public static Vector2 _chambreCentreUn;
        public static Vector2 _chambreCentre2;
        public static Vector2 _chambreCentreDeux;
        public static int _limiteChambreX1;
        public static int _limiteChambreX2;
        public static int _limiteChambreY1;
        public static int _limiteChambreY2;
        public static int _limiteChambreGauche;
        public static int _limiteChambreDroite;

        private int _choixCursor;

        int numDial;

        public Chato_int_chambres_nord(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;

            // Lieu Spawn perso
            _posX = 0;
            numDial = 0;

            Joueur.Spawnchato_int_chambres_nord();

            _stop = 1;

            // Lieu Spawn objects
            _positionFren = new Vector2(28 * 16 + 8, 4 * 16 + 8);
            _frenTrue = false;

            _positionChest1 = new Vector2(38 * 16 + 8, 4 * 16 + 8);
            _chestTrue = false;
            
            // Emplacements pour camera
            _chambreCentre1 = new Vector2((float)6.5 * 16, 6 * 16);
            //_chambreCentre1 = new Vector2((float)8 * 16, 6 * 16);
            _chambreCentreUn = new Vector2((float)14.5 * 16, 6 * 16);
            _chambreCentre2 = new Vector2((float)30.5 * 16, 6 * 16);
            //_chambreCentre2 = new Vector2((float)32.5 * 16, 6 * 16);
            _chambreCentreDeux = new Vector2((float)38.5 * 16, 6 * 16);

            _limiteChambreX1 = 16 * 16;
            _limiteChambreX2 = 24 * 16;
            _limiteChambreY1 = 8 * 16;

            _limiteChambreGauche = 8 * 16;
            _limiteChambreDroite = 32 * 16;

            _sensPersoX = 0;
            _sensPersoY = 0;
            _vitessePerso = 100;
            _choixCursor = 1;
            
            
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Game1._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_nord");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, Game1._tiledMap);
            //mapLayer = Game1._tiledMap.GetLayer<TiledMapTileLayer>("collision");
            mapLayerIntersect = Game1._tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");
            Event_et_dial.SetCollision();
            //Load persos
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
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
            int a = mapLayerIntersect.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight + 1)).GlobalIdentifier;
            //Console.WriteLine(a);
            //debug autres collisions
            int b = Game1.mapLayer.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth-1), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight)).GlobalIdentifier;
            Console.WriteLine(b);

            Event_et_dial.BoiteDialogues();
            //Camera
            _myGame._camera.LookAt(Game1._cameraPosition);

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

            if (_eventEtDial._dialTrue == false)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight - 1);
                    animation = "move_up";
                    _stop = 2;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight + 1);
                    animation = "move_down";
                    _stop = 1;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y += walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight);
                    animation = "move_left";
                    _stop = 3;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight);
                    animation = "move_right";
                    _stop = 4;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X += walkSpeed;
                }
            }
            else if (animation == null)
                animation = "idle_down";
            else
                animation = animation;

            _perso.Play(animation);
            _perso.Update(deltaSeconds);

            if(_eventEtDial._choiceTrue == true)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && _choixCursor == 1 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    _eventEtDial._posCursor = new Vector2(430, 301);
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && _choixCursor == 0 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    _eventEtDial._posCursor = new Vector2(430, 301);
                }
                Console.WriteLine(_myGame._cooldownVerif);
                if (keyboardState.IsKeyDown(Keys.W) && _choixCursor == 0 && _myGame._cooldownVerif == false)
                {
                    _myGame.SetCoolDown();
                    Game1._fin = 1;
                    Game.LoadScreenblack_jack();
                    
                }
                if ((keyboardState.IsKeyDown(Keys.W) && _choixCursor ==1) || keyboardState.IsKeyDown(Keys.X) && _myGame._cooldownVerif == false)
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

            if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _eventEtDial.FermeBoite();
            }
            else if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "idle" && _myGame._cooldownVerif == false
                && Game1._positionPerso.X < _limiteChambreDroite)
            {
                _eventEtDial.Fren1();
                _frenTrue = true;
            }        
            else if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "hi" && _myGame._cooldownVerif == false)
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

            if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationChest == "close" 
                && Game1._positionPerso.X > _limiteChambreDroite)
                _chestTrue = true;
            

            _chest1.Play(animationChest);
            _chest1.Update(deltaSeconds);


            //EVENEMENTS
            
            if(Game1._firstvisit == true && _myGame._cooldownVerif == false && numDial == 0)
            {
                _eventEtDial.Jon1();
                Game1._firstvisit = false;
                numDial = 1;
                _stop = 4;
            }
            if (_myGame._cooldownVerif == false && keyboardState.IsKeyDown(Keys.W) && numDial == 1)
            {
                _eventEtDial.Jon2();
                numDial = 2;
            }
            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false &&  numDial == 2)
            {
                _eventEtDial.FermeBoite();
                numDial = 3;
            }

            //DODO

            if (keyboardState.IsKeyDown(Keys.W) && (b == 72) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false && _myGame._cooldownVerif == false)
            {
                //Event_et_dial.Fin1();
                Game1._fin = 1;
                Game.LoadScreenblack_jack();
            }

            //changement de map
            if (keyboardState.IsKeyDown(Keys.Down) && (a == 41))
            {
                _posX = (int)Game1._positionPerso.X;
                Game.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = _myGame._camera.GetViewMatrix();
            
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(_myGame._camera.GetViewMatrix());
            _spriteBatch.Draw(_fren, _positionFren);
            _spriteBatch.Draw(_chest1, _positionChest1);
            _spriteBatch.Draw(_perso, Game1._positionPerso);
            _spriteBatch.End();

            var transformMatrixDial = Game1._cameraDial.GetViewMatrix();
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

        private bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (Game1.mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}