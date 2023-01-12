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
        private Camera _camera;

        //sprite
        private AnimatedSprite _perso;
        private KeyboardState _keyboardState;
        public int _posX;

        // ennemi
        private AnimatedSprite _ennemiPabo;
        private Vector2 _positionEnnemiPabo;
        private String _animationPabo;

        private bool _rencontre;

        //NPC
        private SpriteSheet[] _NPC;
        private AnimatedSprite[] _spriteNPC;
        private Vector2[] _posNPC;
        private String[] _animNPC;


        public ChatoIntTrone(Game1 game) : base(game)
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
            _myGame._numSalle = 3;
            _eventEtDial._numDial = 2;

            _joueur.SpawnChatoIntCouronne();

            // ennemi
            _positionEnnemiPabo = new Vector2(10 * 16 + 8, 24 * 16);
            _animationPabo = "idle_up";

            // NPC
            _posNPC = new Vector2[12] { new Vector2(5 * 16 + 8, 7 * 16), new Vector2(9 * 16 + 8, 3 * 16), 
                                       new Vector2(5 * 16 + 8, 4 * 16), new Vector2(6 * 16 + 8, 3 * 16), 
                                       new Vector2(15 * 16 + 8, 4 * 16), new Vector2(15 * 16 + 8, 12 * 16),
                                       new Vector2(5 * 16 + 8, 9 * 16), new Vector2(11 * 16 + 8, 3 * 16),
                                       new Vector2(5 * 16 + 8, 12 * 16), new Vector2(14 * 16 + 8, 3 * 16),
                                       new Vector2(15 * 16 + 8, 9 * 16), new Vector2(15 * 16 + 8, 7 * 16)};

            _animNPC = new String[12] { "main_right", "baisse_down", "baisse_right", "baisse_down", "main_left", "baisse_left",
                                        "main_right", "baisse_down", "baisse_right", "baisse_down", "main_left", "baisse_left"};          
                
            _rencontre = false;

            if (_chatoCombat._victoire == true)
            {
                _myGame.Victoire();
            }

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_salle_courronnement");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            // héro
            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetA);

            // ennemi
            SpriteSheet spriteSheetE = Content.Load<SpriteSheet>("anim/char/enemy/pabo/character_movement.sf", new JsonContentLoader());
            _ennemiPabo = new AnimatedSprite(spriteSheetE);

            _NPC = new SpriteSheet[4] { Content.Load<SpriteSheet>("anim/char/NPC/bleuDemon/NPC.sf", new JsonContentLoader()), 
                                        Content.Load<SpriteSheet>("anim/char/NPC/verougeDemon/NPC.sf", new JsonContentLoader()),
                                        Content.Load<SpriteSheet>("anim/char/NPC/rougeF/NPC_rouge_F.sf", new JsonContentLoader()), 
                                        Content.Load<SpriteSheet>("anim/char/NPC/vertElf/NPC.sf", new JsonContentLoader())};

            _spriteNPC = new AnimatedSprite[12] { new AnimatedSprite(_NPC[0]), new AnimatedSprite(_NPC[0]),
                                                 new AnimatedSprite(_NPC[0]), new AnimatedSprite(_NPC[1]),
                                                 new AnimatedSprite(_NPC[1]), new AnimatedSprite(_NPC[1]),
                                                 new AnimatedSprite(_NPC[2]), new AnimatedSprite(_NPC[2]),
                                                 new AnimatedSprite(_NPC[2]), new AnimatedSprite(_NPC[3]),
                                                 new AnimatedSprite(_NPC[3]), new AnimatedSprite(_NPC[3])};

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

            /// ennemi
            _ennemiPabo.Play(_animationPabo);
            _ennemiPabo.Update(deltaSeconds);

            /// NPC
            for (int i = 0; i < _spriteNPC.Length; i++)
            {
                _spriteNPC[i].Play(_animNPC[i]);
                _spriteNPC[i].Update(deltaSeconds);
            }

            _eventEtDial.BoiteDialogues();


            //Evenements
            // Battle final (mettre la bonne replique à ennemi)

            if (_keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true && _eventEtDial._numDial == 1)
            {
                _rencontre = true;
                //_eventEtDial.FermeBoite();
                _myGame.LoadScreenChatoCombat();

                //contenu du combat
                _myGame._nbAlly = 2;
                _myGame._ordreJoueur = new String[] { "Hero", "Jon" };
                _chatoCombat._premierCombat = false;
                _myGame._nbEnemy = 3;
                _myGame._ordreEnnemi = new String[] { "Grand", "Mechant", "Pabo"};
            }
            else if ( _myGame._positionPerso.Y <= 31 * 16 && _myGame._cooldownVerif == false && _rencontre == false && _eventEtDial._numDial == 2)
            {
                _animationPabo = "idle_down";
                _eventEtDial.Jon5();
                _eventEtDial._numDial = 1;
            }
            else if (_chatoCombat._victoire == true)
            {
                _rencontre = true;
                _eventEtDial._dialTrue = false;
            }

            // fin 
            if (_myGame._positionPerso.Y <= 10 * 16)
            {
                _myGame._fin = 3;
                Game.LoadScreenBlackJack();
            }


            //changements maps
            if (_keyboardState.IsKeyDown(Keys.Down) && (_eventEtDial.dd == 1) && _myGame._positionPerso.Y > 38 * 16)
            {
                _posX = (int)_myGame._positionPerso.X;
                _myGame.LoadScreenchatoExtCoursInterieur();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(_camera._cameraMap.GetViewMatrix());
            _spriteBatch.Draw(_perso, _myGame._positionPerso);
            
            if (_rencontre == false)
                _spriteBatch.Draw(_ennemiPabo, _positionEnnemiPabo);

            for (int i = 0; i < _spriteNPC.Length; i++)
            {
                _spriteBatch.Draw(_spriteNPC[i], _posNPC[i]);
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