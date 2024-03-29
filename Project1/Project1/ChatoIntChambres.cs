﻿using Microsoft.Xna.Framework;
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
        private Camera _camera;

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

        private AnimatedSprite[] _chest;
        private Vector2[] _positionChest;
        private String[] _animationChest;

        // camera
        public Vector2 _chambreCentre1;
        public Vector2 _chambreCentreUn;
        public Vector2 _chambreCentre2;
        public Vector2 _chambreCentreDeux;


        public int _limChambre_x1;
        public int _limChambre_x2;
        public int _limChambre_y1;
        public int _limChambre_y2;
        public int _limChambre_Gauche;
        public int _limChambre_Droite;

        public ChatoIntChambres(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _joueur = _myGame._joueur;
            _camera = _myGame._camera;

            // Lieu Spawn perso
            _posJon = new Vector2(-10 * 16 + 8, -10 * 16 + 8);
            _animJon = "idle_up";
            _posX = 0;

            _joueur.SpawnChatoIntChambres();

            _limChambre_x1 = 16 * 16;
            _limChambre_x2 = 24 * 16;
            _limChambre_y1 = 8 * 16;
            _limChambre_y2 = 8 * 16;
            _limChambre_Gauche = 8 * 16;
            _limChambre_Droite = 32 * 16;

            _chambreCentre1 = new Vector2((float)4.6 * 16, 7 * 16);
            _chambreCentreUn = new Vector2((float)12.6 * 16, 7 * 16);
            _chambreCentre2 = new Vector2((float)28.6 * 16, 7 * 16);
            _chambreCentreDeux = new Vector2((float)36.6 * 16, 7 * 16);


            // Lieu Spawn objects
            _positionFren = new Vector2(28 * 16 + 8, 4 * 16 + 8);
            _frenTrue = false;

            _chest = new AnimatedSprite[2];

            _animationChest = new string[2];
            _positionChest = new Vector2[2];
            _positionChest[0] = new Vector2(2 * 16 + 8, 4 * 16 + 8);
            _positionChest[1] = new Vector2(38 * 16 + 8, 4 * 16 + 8);

            
            _eventEtDial._choixCursor = 1;
            
            
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _myGame._tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_int_chambres_nord");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _myGame._tiledMap);

            _eventEtDial.SetCollision();

            //Load persos
            SpriteSheet spriteSheetPerso = Content.Load<SpriteSheet>("anim/char/ally/hero/character_movement.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheetPerso);
            SpriteSheet spriteSheetJon = Content.Load<SpriteSheet>("anim/char/ally/Jon/character_movement.sf", new JsonContentLoader());
            _jon = new AnimatedSprite(spriteSheetJon);


            //Load objects
            SpriteSheet spriteSheetFren = Content.Load<SpriteSheet>("anim/char/Fren/Fren.sf", new JsonContentLoader());
            _fren = new AnimatedSprite(spriteSheetFren);
            SpriteSheet spriteSheetChest = Content.Load<SpriteSheet>("anim/objects/chest1.sf", new JsonContentLoader());
            for (int i = 0; i < _chest.Length; i++)
                _chest[i] = new AnimatedSprite(spriteSheetChest);


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

            //Choix 

            if (_keyboardState.IsKeyDown(Keys.W) && (_eventEtDial.l == 72) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false)
            {
                _eventEtDial.Fin1();
                _myGame._fin = 1;
                //Game.LoadScreenblack_jack();
            }

            if (_eventEtDial._choiceTrue == true)
                _eventEtDial.Choix();

            if (_keyboardState.IsKeyDown(Keys.W) && _eventEtDial._choixCursor == 0 && _myGame._cooldownVerif == false && _eventEtDial._choiceTrue == true)
            {
                _myGame.SetCoolDown();
                _myGame._fin = 1;
                _myGame.LoadScreenBlackJack();
                _eventEtDial._choiceTrue = false;
                _eventEtDial._dialTrue = false;

            }
            else if ((_keyboardState.IsKeyDown(Keys.W) && _eventEtDial._choixCursor == 1 || _keyboardState.IsKeyDown(Keys.X)) && _myGame._cooldownVerif == false && _eventEtDial._choiceTrue == true)
            {
                _myGame.SetCoolDown();
                _eventEtDial._choiceTrue = false;
                _eventEtDial._dialTrue = false;
            }


            //MOUVEMENT/ANIMATION OBJETS

            //:)
            String animationFren = null;
            if (_frenTrue == false)
                animationFren = "idle";
            else
                animationFren = "hi";

            if (_keyboardState.IsKeyDown(Keys.W) && (_eventEtDial.u == 70) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _eventEtDial.FermeBoite();
            }
            else if (_keyboardState.IsKeyDown(Keys.W) && (_eventEtDial.u == 70) && animationFren == "idle" && _myGame._cooldownVerif == false
                && _myGame._positionPerso.X < _limChambre_Droite)
            {
                _eventEtDial.Fren1();
                _frenTrue = true;
            }        
            else if (_keyboardState.IsKeyDown(Keys.W) && (_eventEtDial.u == 70) && animationFren == "hi" && _myGame._cooldownVerif == false)
            {
                _eventEtDial.Fren2();
                _frenTrue = false;
            }
            _fren.Play(animationFren);
            _fren.Update(deltaSeconds);


            //Coffres

            if (_keyboardState.IsKeyDown(Keys.W) && _eventEtDial.u == 71 && _animationChest[1] == "close" && _myGame._positionPerso.X > 10 * 16 && _myGame._cooldownVerif == false && _eventEtDial._numDial == 2)
            {
                _myGame._chestTrue[1] = true;
                _eventEtDial.Chest1();
                _eventEtDial._numDial = 1;
            }

            if (_keyboardState.IsKeyDown(Keys.W) && _eventEtDial.u == 71 && _animationChest[0] == "close" && _myGame._positionPerso.X < 10 * 16 && _myGame._cooldownVerif == false && _eventEtDial._numDial == 2)
            {
                _myGame._chestTrue[0] = true;
                _eventEtDial._numDial = 1;
                if (_myGame.konami == true)
                    _eventEtDial.RPG();
                else
                    _eventEtDial.Chest0();

            }


            for (int i = 0; i < _chest.Length; i++)
            {

                if (_myGame._chestTrue[i] == false)
                    _animationChest[i] = "close";
                else
                    _animationChest[i] = "open";
            }

            for (int i = 0; i < _chest.Length; i++)
            {
                _chest[i].Play(_animationChest[i]);
                _chest[i].Update(deltaSeconds);
            }
            
            //Debut avec Jon

            _jon.Play(_animJon);
            _jon.Update(deltaSeconds);
            //Console.WriteLine(_eventEtDial._numDial);

            if (_myGame._firstVisitBedroom == true && _myGame._cooldownVerif == false && _eventEtDial._numDial == 3)
            {
                _eventEtDial.Jon1();
                _posJon = new Vector2(4*16+8,4*16+8);
                _myGame._firstVisitBedroom = false;
                _eventEtDial._numDial = 2;
            }
            else if (_myGame._cooldownVerif == false && _keyboardState.IsKeyDown(Keys.W) && _eventEtDial._numDial == 2)
            {
                _eventEtDial.Jon2();
                _eventEtDial._numDial = 1;
            }
            
            if (_eventEtDial._numDial == 0)
            {
                _posJon.Y += _myGame._walkSpeed;
                _animJon = "move_down";
            }             
            if (_posJon.Y > 8 * 16 + 8)
            {
                _eventEtDial._numDial = 2;
                _posJon = new Vector2(-100 * 16 + 8, -10 * 16 + 8);
            }
            

            if (_eventEtDial._numDial == 1 && _keyboardState.IsKeyDown(Keys.W) && _eventEtDial._dialTrue == true && _myGame._cooldownVerif == false)
            {
                _eventEtDial.FermeBoite();
                _eventEtDial._numDial = 0;
            }

            //changement de map
            if (_keyboardState.IsKeyDown(Keys.Down) && (_eventEtDial.dd == 41))
            {
                _posX = (int)_myGame._positionPerso.X;
                Game.LoadScreenchatoIntChambresCouloir();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tiledMapRenderer.Draw(transformMatrix);
            _spriteBatch.Draw(_fren, _positionFren);
            for (int i = 0; i < _chest.Length; i++)
            {
                _spriteBatch.Draw(_chest[i], _positionChest[i]);
                _spriteBatch.Draw(_perso, _myGame._positionPerso);
            }          
            _spriteBatch.Draw(_jon, _posJon);

            _spriteBatch.End();

            var transformMatrixDial = _camera._cameraDial.GetViewMatrix();
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