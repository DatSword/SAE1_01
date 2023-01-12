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
        private ChatoCombatContenu _chatoCombatContenu;
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
        private AnimatedSprite _demonBleuDroite;
        private AnimatedSprite _demonBleuBas;
        private AnimatedSprite _femmeRouge;
        private AnimatedSprite _elfBas;
        private AnimatedSprite _elfGauche;
        private AnimatedSprite _demonRV;

        private Vector2 _positionDemonBleuDroite;
        private Vector2 _positionDemonBleuBas;
        private Vector2 _positionFemmeRouge;
        private Vector2 _positionElfBas;
        private Vector2 _positionElfGauche;
        private Vector2 _positionDemonRV;

        private String _animationDemonBleuDroite;
        private String _animationBas;
        private String _animationFemmeRouge;
        private String _animationElfGauche;
        private String _animationDemonRV;


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

            _joueur.SpawnchatoIntCouronne();

            _myGame._numSalle = 5;

            ///ennemi
            _positionEnnemiPabo = new Vector2(10 * 16 + 8, 24 * 16);
            _animationPabo = "idle_up";

            ///NPC
            _positionDemonBleuDroite = new Vector2(3 * 16 + 8, 6 * 16);
            _animationDemonBleuDroite = "idle_right";
            _positionDemonBleuBas = new Vector2(15 * 16 + 8, 4 * 16);
            _animationBas = "idle_down";
            _positionFemmeRouge = new Vector2(6 * 16 + 8, 3 * 16);
            _animationFemmeRouge = "idle_right";
            _positionElfBas = new Vector2(4 * 16 + 8, 9 * 16);
            _positionElfGauche = new Vector2(18 * 16 + 8, 6 * 16);
            _animationElfGauche = "idle_left";
            _positionDemonRV = new Vector2(17 * 16 + 8, 10 * 16);
            _animationDemonRV = "idle_left";

            _rencontre = false;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_salle_courronnement");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            /// héro
            SpriteSheet spriteSheetA = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetA);

            /// ennemi
            SpriteSheet spriteSheetE = Content.Load<SpriteSheet>("anim/char/enemy/pabo/character_movement.sf", new JsonContentLoader());
            _ennemiPabo = new AnimatedSprite(spriteSheetE);

            /// NPC
            SpriteSheet spriteSheetDB = Content.Load<SpriteSheet>("anim/char/NPC/bleuDemon/NPC.sf", new JsonContentLoader());
            _demonBleuDroite = new AnimatedSprite(spriteSheetDB);
            _demonBleuBas = new AnimatedSprite(spriteSheetDB);

            SpriteSheet spriteSheetDVR = Content.Load<SpriteSheet>("anim/char/NPC/verougeDemon/NPC.sf", new JsonContentLoader());
            _demonRV = new AnimatedSprite(spriteSheetDVR);

            SpriteSheet spriteSheetFR = Content.Load<SpriteSheet>("anim/char/NPC/rougeF/NPC_rouge_F.sf", new JsonContentLoader());
            _femmeRouge = new AnimatedSprite(spriteSheetFR);

            SpriteSheet spriteSheetELF = Content.Load<SpriteSheet>("anim/char/NPC/vertElf/NPC.sf", new JsonContentLoader());
            _elfBas = new AnimatedSprite(spriteSheetELF);
            _elfGauche = new AnimatedSprite(spriteSheetELF);



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
            if (_animationDemonBleuDroite == "idle_right")
                _animationDemonBleuDroite = "main_right";
            else
                _animationDemonBleuDroite = "idle_right";


            if (_animationBas == "idle_down")
                _animationBas = "baisse_down";
            else
                _animationBas = "idle_down";


            if (_animationFemmeRouge == "idle_right")
                _animationFemmeRouge = "baisse_right";
            else
                _animationFemmeRouge = "idle_right";


            if (_animationElfGauche == "idle_left")
                _animationElfGauche = "main_left";
            else
                _animationElfGauche = "idle_left";


            if (_animationDemonRV == "idle_left")
                _animationDemonRV = "mains_left";
            else
                _animationDemonRV = "idle_left";

            _demonBleuDroite.Play(_animationDemonBleuDroite);
            _demonBleuDroite.Update(deltaSeconds);
            _demonBleuBas.Play(_animationBas);
            _demonBleuBas.Update(deltaSeconds);
            _femmeRouge.Play(_animationFemmeRouge);
            _femmeRouge.Update(deltaSeconds);
            _elfBas.Play(_animationBas);
            _elfBas.Update(deltaSeconds);
            _elfGauche.Play(_animationElfGauche);
            _elfGauche.Update(deltaSeconds);
            _demonRV.Play(_animationDemonRV);
            _demonRV.Update(deltaSeconds);




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

            _spriteBatch.Draw(_demonBleuDroite, _positionDemonBleuDroite);
            _spriteBatch.Draw(_demonBleuBas, _positionDemonBleuBas);
            _spriteBatch.Draw(_femmeRouge, _positionFemmeRouge);
            _spriteBatch.Draw(_elfBas, _positionElfBas);
            _spriteBatch.Draw(_elfGauche, _positionElfGauche);
            _spriteBatch.Draw(_demonRV, _positionDemonRV);


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