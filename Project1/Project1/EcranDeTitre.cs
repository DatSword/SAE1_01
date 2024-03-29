﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using System;

namespace SAE101
{
    public class EcranDeTitre : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;

        // pour récupérer une référence à l’objet game pour avoir accès à tout ce qui est 
        // défini dans Game1
        private Game1 _myGame;
        private Camera _camera;

        // texture menu
        private Texture2D _fondTantopie;
        private Texture2D _start;
        private Texture2D _option;
        public Texture2D _quit;

        // boutons 
        public Rectangle[] lesBoutons;

        //Titre
        public SpriteFont _fontTitle;

        public EcranDeTitre(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _camera = _myGame._camera;

            lesBoutons = new Rectangle[3];
            lesBoutons[0] = new Rectangle(_myGame.xE / 2 - 210 / 2, _myGame.yE / 3 + 63, 210, 63);
            lesBoutons[1] = new Rectangle(_myGame.xE / 2 - 210 / 2, (int)(_myGame.yE / 3 * 1.5 + 63), 210, 63);
            lesBoutons[2] = new Rectangle(_myGame.xE / 2 - 210 / 2, _myGame.yE / 3 * 2 + 63, 210, 63);
            

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _start = Content.Load<Texture2D>("menu/start");
            _option = Content.Load<Texture2D>("menu/options");
            _quit = Content.Load<Texture2D>("menu/quit");
            _fondTantopie = Content.Load<Texture2D>("menu/tantopie");
            _fontTitle = Content.Load<SpriteFont>("font/fonttitle");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lesBoutons[0] = new Rectangle( (int)(_myGame.xE / 2 - 210 * _myGame.chan / 2), (int)(_myGame.yE/3 * 1 + (63 * _myGame.chan) * _myGame.chan),
                 (int)(_myGame.chan* 210), (int)(_myGame.chan * 63));

            lesBoutons[1] = new Rectangle( (int)(_myGame.xE / 2 - 210 * _myGame.chan / 2), (int)(_myGame.yE/3 * 1.5 + (63 * _myGame.chan) * _myGame.chan),
                (int)(_myGame.chan * 210), (int)(_myGame.chan * 63));

            lesBoutons[2] = new Rectangle( (int)(_myGame.xE / 2 - 210 * _myGame.chan / 2), (int)(_myGame.yE/3 * 2 + (63 * _myGame.chan) * _myGame.chan),
                (int)(_myGame.chan * 210), (int)(_myGame.chan * 63));


            for (int i = 0; i < lesBoutons.Length; i++)
            {
                if (i == 0)
                    lesBoutons[i].Y = (int)(_myGame.yE / 3 * 1   + lesBoutons[i].Height);
                else if (i == 1)
                    lesBoutons[i].Y = (int)(_myGame.yE / 3 * 1.5 + lesBoutons[i].Height);
                else
                    lesBoutons[i].Y = (int)(_myGame.yE / 3 * 2   + lesBoutons[i].Height);
            }


            //changements maps

            MouseState _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < lesBoutons.Length; i++)
                {
                    // si le clic correspond à un des 3 boutons
                    if (lesBoutons[i].Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        // on change l'état défini dans Game1 en fonction du bouton cliqué
                        if (i == 0)
                            _myGame.Etat = Game1.Etats.Start;
                        else if (i == 1)
                            _myGame.Etat = Game1.Etats.Option;
                        else
                            _myGame.Etat = Game1.Etats.Quitter;
                        break;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            
            _spriteBatch.Draw(_fondTantopie, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_fontTitle, "Tantopie",new Vector2(15,0), Color.LightGray);
            _spriteBatch.Draw(_start, new Vector2(_myGame._xEcran / 2 - 210 / 2, _myGame._yEcran / 3 + 63), Color.White);
            _spriteBatch.Draw(_option, new Vector2(_myGame._xEcran / 2 - 210 / 2, (float)(_myGame._yEcran / 3 * 1.5 + 63)), Color.White);
            _spriteBatch.Draw(_quit, new Vector2(_myGame._xEcran / 2 - 210 / 2, _myGame._yEcran / 3 * 2 + 63), Color.White);
            
            _spriteBatch.End();
        }
    }
}
