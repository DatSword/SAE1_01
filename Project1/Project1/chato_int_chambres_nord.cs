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
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer mapLayer;
        private TiledMapTileLayer mapLayerIntersect;

        //sprites
        private AnimatedSprite _perso;
        public static Vector2 _positionPerso;
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

        int numDial;

        public Chato_int_chambres_nord(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // Lieu Spawn perso
            _posX = 0;
            numDial = 0;

            joueur.Spawnchato_int_chambres_nord();

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

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_nord");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("collision");
            mapLayerIntersect = _tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");

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
            int a = mapLayerIntersect.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1)).GlobalIdentifier;
            Console.WriteLine(a);
            //debug autres collisions
            int b = mapLayer.GetTile((ushort)(_positionPerso.X / _tiledMap.TileWidth), (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1)).GlobalIdentifier;
            Console.WriteLine(b);


            //Camera
            Game1._camera.LookAt(Game1._cameraPosition);

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

            if (Eventsetdial._dialTrue == false)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1);
                    animation = "move_up";
                    _stop = 2;
                    if (!IsCollision(tx, ty))
                        _positionPerso.Y -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 1);
                    animation = "move_down";
                    _stop = 1;
                    if (!IsCollision(tx, ty))
                        _positionPerso.Y += walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_left";
                    _stop = 3;
                    if (!IsCollision(tx, ty))
                        _positionPerso.X -= walkSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);
                    animation = "move_right";
                    _stop = 4;
                    if (!IsCollision(tx, ty))
                        _positionPerso.X += walkSpeed;
                }
            }
            else if (animation == null)
                animation = "idle_down";
            else
                animation = animation;

            _perso.Play(animation);
            _perso.Update(deltaSeconds);


            //MOUVEMENT/ANIMATION OBJETS

            //:)
            String animationFren = null;
            if (_frenTrue == false)
                animationFren = "idle";
            else
                animationFren = "hi";

            if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && Game1._cooldownVerif == false && Eventsetdial._dialTrue == true)
            {
                Eventsetdial.FermeBoite();
            }
            else if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "idle" && Game1._cooldownVerif == false
                && _positionPerso.X < _limiteChambreDroite)
            {
                Eventsetdial.Fren1();
                _frenTrue = true;
            }        
            else if (keyboardState.IsKeyDown(Keys.W) && (b == 70) && animationFren == "hi" && Game1._cooldownVerif == false)
            {
                Eventsetdial.Fren2();
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
                && _positionPerso.X > _limiteChambreDroite)
                _chestTrue = true;
            

            _chest1.Play(animationChest);
            _chest1.Update(deltaSeconds);


            //EVENEMENTS
            
            if(Game1._firstvisit == true && Game1._cooldownVerif == false && numDial == 0)
            {
                Eventsetdial.Jon1();
                Game1._firstvisit = false;
                numDial = 1;
                _stop = 4;
            }
            if (Game1._cooldownVerif == false && keyboardState.IsKeyDown(Keys.W) && numDial == 1)
            {
                Eventsetdial.Jon2();
                numDial = 2;
            }
            if (keyboardState.IsKeyDown(Keys.W) && Game1._cooldownVerif == false &&  numDial == 2)
            {
                Eventsetdial.FermeBoite();
                numDial = 3;
            }


            //changement de map
            if (keyboardState.IsKeyDown(Keys.Down) && (a == 41))
            {
                _posX = (int)_positionPerso.X;
                Game.LoadScreenchato_int_chambres_couloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = Game1._camera.GetViewMatrix();
            
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tiledMapRenderer.Draw(Game1._camera.GetViewMatrix());
            _spriteBatch.Draw(_fren, _positionFren);
            _spriteBatch.Draw(_chest1, _positionChest1);
            _spriteBatch.Draw(_perso, _positionPerso);
            _spriteBatch.End();

            var transformMatrixDial = Game1._cameraDial.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrixDial);
            if (Eventsetdial._dialTrue == true)
            {
                _spriteBatch.Draw(Eventsetdial._dialBox, Eventsetdial._posDialBox, Color.White);
                _spriteBatch.DrawString(Game1._font, Eventsetdial._text, Eventsetdial._posText, Color.White);
                _spriteBatch.DrawString(Game1._font, Eventsetdial._nom, Eventsetdial._posNom, Color.White);
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