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
    public class BlackJack : GameScreen
    {
        //"map"
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Game1 _myGame;
        private EventEtDial _eventEtDial;

        //Début/Fin
        public Vector2 _textPos;
        public String _text;

        public BlackJack(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;

            _textPos = new Vector2(50, 150);
            _text = "";

            if (_myGame._fin == 1)
                MediaPlayer.Play(_myGame._songDodo);

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;



            if (Keyboard.GetState().IsKeyDown(Keys.Back))
                _myGame.Etat = Game1.Etats.Menu;

            //changements maps, tout premier dialogue
            if (_myGame._fin == 0)
                _text = "...et c'est ainsi qu'après cette grande aventure, nos héros ont\n" +
                        "bien mérité de précieuses heures de repos dans le Chato, juste\n" +
                        "avant le couronnement de leur ami Julius, pour enfin boucler\n" +
                        "cette hist-";
            else if (_myGame._fin == 10)
                _text = "";
            else if (_myGame._fin == 1)
            {
                _text = "Hum hum, malgré cette petite interruption, notre héros décida\n" +
                        "qu'il n'allait pas être présent lors du courronnement. Même si\n" +
                        "cet évènement est ce pourquoi lui et ses amis ont traversés tant\n" +
                        "d'épreuves, le sommeil reste son ennemi le plus puissant.";
            }
            else if (_myGame._fin == 2)
                _text = "Malheureusement, après avoir traversés tant d'obstacles, il\n" +
                        "fallut que deux de nos héros périssent juste avant le\n" +
                        "courronnement de leur ami. Vous ne voudriez pas d'une fin pareil,\n" +
                        "non?";
            else if (_myGame._fin == 3)
                _text = "Et non! Il semble donc que l'histoire n'est pas fini! On dirait\n" +
                        "même qu'elle vient tout juste de commencer! Que va t-il\n" +
                        "arriver à nos personnages? Qui est ce mystérieux jeune homme\n" +
                        "envoyé d'on-ne-sait-quand? Toutes ces réponses, vous les aurez...\n" +
                        "Peut-être un jour...?";


            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false && _eventEtDial._count <=1 && _myGame._fin == 0)
            {
                _myGame._fin = 10;
                _myGame._toink.Play();
                _eventEtDial.toutDebut();
            }

            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _eventEtDial.FermeBoite();
                Game.LoadScreenchato_int_chambres_nord();              
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _myGame._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            if (_eventEtDial._dialTrue == false)
                _spriteBatch.DrawString(_myGame._font, _text , _textPos, Color.White);

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
