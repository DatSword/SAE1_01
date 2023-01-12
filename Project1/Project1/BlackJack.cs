using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using System;
using System.Threading;

namespace SAE101
{
    public class BlackJack : GameScreen
    {
        // "map"
        private new Game1 Game => (Game1)base.Game;
        private SpriteBatch _spriteBatch;

        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private Camera _camera;

        //Début/Fin
        public Vector2 _posTextFin;
        public String _text;

        public bool _fin;

        private String _credit;
        public Vector2 _posCr;

        public BlackJack(Game1 game) : base(game) 
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _eventEtDial = _myGame._eventEtDial;
            _camera = _myGame._camera;

            _posTextFin = new Vector2(50, 160);
            _text = "";

            _fin = false;

            _credit = "----TANTOPIE STAFF----\n\n" +
                      "Scénario : Quentin BASTARD\n" +
                      "Cartes   : Quentin BASTARD\n" +
                      "           Anna KOMPANIETS\n" +
                      "\n " +
                      "Programmation : Quentin BASTARD,\n " +
                      "                Marine GIMENEZ,\n " +
                      "                Anna KOMPANIETS\n" +
                      "\n " +
                      "Sprites et images : Anna KOMPANIETS,\n" +
                      "                    Marine GIMENEZ,\n" +
                      "                    Quentin BASTARD\n" +
                      "\n " +
                      "Musiques \n"+
                      "Ecran de Titre : Bit By Bit Sound \n" +
                      "fin \"canon\"  : Bit By Bit Sound\n" +
                      "Combat         : SilvaGunner\n" +
                      "Chato          : SilvaGunner\n" +
                      "fin \"dodo\"   : SilvaGunner\n" +
                      "\n " +                     
                      "Compte rendu : \n" +
                      "Vidéo trailer : \n" +
                      "\n" +
                      "remerciments spéciaux : Dylan MIFTARI\n";

            _posCr = new Vector2(_myGame._xEcran / (float)3.5, _myGame._yEcran * (float)1.9);

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
                        "cet évènement était ce pourquoi lui et ses amis avaient traversés\n" +
                        "tant d'épreuves, le sommeil reste son ennemi le plus puissant.";
                _fin = true;
            }
            else if (_myGame._fin == 2)
            {
                _text = "Malheureusement, après avoir traversés tant d'obstacles, il\n" +
                        "fallut que deux de nos héros périssent juste avant le\n" +
                        "couronnement de leur ami. Vous ne voudriez pas d'une fin pareil,\n" +
                        "n'est-ce pas?";
            }
            else if (_myGame._fin == 3)
            {
                _text = "Et voilà, le couronnement débuta, ce qui marqua la fin de\n" +
                        "notre histoire. On pourra dire que nos héros auront bien\n" +
                        "mérité tout ce repos.";
                _fin = true;
            }

            /* Gros Cliffanger???
             * "Et non! Il semble donc que l'histoire n'est pas fini! On dirait\n" +
                        "même qu'elle vient tout juste de commencer! Que va t-il\n" +
                        "arriver à nos personnages? Qui est ce mystérieux jeune homme\n" +
                        "envoyé d'on-ne-sait-quand? Toutes ces réponses, vous les aurez...\n" +
                        "Peut-être un jour...?";*/

            //Début
            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false && _eventEtDial._count <=1 && _myGame._fin == 0)
            {
                _myGame._fin = 10;
                _myGame._toink.Play();
                _eventEtDial.toutDebut();
            }
            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == true)
            {
                _eventEtDial.FermeBoite();
                Game.LoadScreenchatoIntChambresNord();              
            }

            //GameOver
            if (keyboardState.IsKeyDown(Keys.W) && _myGame._cooldownVerif == false && _eventEtDial._dialTrue == false && _eventEtDial._count <= 1 && _myGame._fin == 2)
            {
                _myGame._fin = 1;
                _myGame.SetCoolDownS();
                _myGame.LoadScreenecranDeTtitre();
            }

            if (_fin == true)
            {
                _posTextFin.Y -= (float)0.25;
                _posCr.Y -= (float)0.5;
            }


        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _camera._cameraMap.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            if (_eventEtDial._dialTrue == false)
                _spriteBatch.DrawString(_myGame._font, _text , _posTextFin, Color.White);

            if (_eventEtDial._dialTrue == true)
            {
                _spriteBatch.Draw(_eventEtDial._dialBox, _eventEtDial._posDialBox, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._text, _eventEtDial._posText, Color.White);
                _spriteBatch.DrawString(_myGame._font, _eventEtDial._nom, _eventEtDial._posNom, Color.White);
            }
            else if (_fin == true)
            {
                Thread.Sleep(40);
                _spriteBatch.DrawString(_myGame._font, _credit, _posCr, Color.White);
            }

            _spriteBatch.End();
        }

      

    }
}
