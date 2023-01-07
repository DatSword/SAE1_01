﻿using Microsoft.Xna.Framework;
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
using static System.Formats.Asn1.AsnWriter;

namespace SAE101
{
    internal class ecran_de_titre : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // pour récupérer une référence à l’objet game pour avoir accès à tout ce qui est 
        // défini dans Game1
        private Game1 _myGame;

        // texture du menu avec 3 boutons
        private Texture2D _titleS;
        private Texture2D _start;
        private Texture2D _option;
        private Texture2D _quit;
     
        // contient les rectangles : position et taille des 3 boutons présents dans la texture 
        private Rectangle[] lesBoutons;

        //Titre
        public SpriteFont _fontTitle;

        public ecran_de_titre(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            
            lesBoutons = new Rectangle[3];
            lesBoutons[0] = new Rectangle(144, 75, 192, 144);
            lesBoutons[1] = new Rectangle(144, 150, 192, 144);
            lesBoutons[2] = new Rectangle(144, 200, 192, 144);

            

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _start = Content.Load<Texture2D>("menu/start");
            _option = Content.Load<Texture2D>("menu/options");
            _quit = Content.Load<Texture2D>("menu/quit");
            _titleS = Content.Load<Texture2D>("menu/tantopie");
            _fontTitle = Content.Load<SpriteFont>("font/fonttitle");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                            _myGame.Etat = Game1.Etats.Play;
                        else if (i == 1)
                            _myGame.Etat = Game1.Etats.Option;
                        else
                            _myGame.Etat = Game1.Etats.Quitter;
                        break;
                    }

                }
            }

            /*if (keyboardState.IsKeyDown(Keys.Enter))
            {
                Game.LoadScreenblack_jack();
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_titleS, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_fontTitle, "Tantopie",new Vector2(0,0), Color.White);
            _spriteBatch.Draw(_start, new Vector2(144, 150), Color.White); //75
            _spriteBatch.Draw(_option, new Vector2(144, 225), Color.White); //150
            _spriteBatch.Draw(_quit, new Vector2(144, 300), Color.White); //225
            _spriteBatch.End();
        }
    }
}
