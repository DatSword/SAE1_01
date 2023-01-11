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
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;

namespace SAE101
{
    public class ChatoCombatContenu : GameScreen
    {

        // défini dans Game1
        private new Game1 Game => (Game1)base.Game;
        private Game1 _myGame;

        public ChatoCombatContenu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public int _nbAlly;
        public int _nbEnnemy;

        public String _special;
        public String _anim;
        public String[] _specialP;
        public String[] _descP;
        public String[] _nomPersoJouable = new String[4] {"???","Hero","Jon","Ben"};
        public int _nbPersoJouable = 4;
        public String[] _nomEnnJouable = new String[3] { "Grand", "Mechant", "Pabo" };
        public int _nbEnnJouable = 3;       
        public String[] _ordreJoueur;
        public String[] _ordreEnnemi;
        public int[] _stat; //PV, Attaque, Défense, Vitesse


        public Vector2 _lastPosition;

        public void Combat()
        {
            _lastPosition = _myGame._positionPerso;

            _nbAlly = 2;
            _ordreJoueur = new String[] { "Hero", "Jon" , "Hein", "Ben"};

            _nbEnnemy = 3;
            _ordreEnnemi = new String[] {"Grand","Mechant","Pabo"};
        }

        //Personnages jouables
        public void Hein()
        {
            _stat = new int[4] { 1, 1, 1, 1 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "???";
            _specialP = new String[] { "_", "_", "_", "_" };
            _descP = new String[] { "_", "_", "_", "_" };
        }
        public void Hero()
        {
            _stat = new int[4] { 80, 100 ,60, 70 };//50
            _anim = "anim/char/ally/hero/character_movement.sf";
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.\nIdéal pour souffler et pour ", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };

        }
        public void Jon()
        {
            _stat = new int[4] { 100, 100, 40, 10 };//90
            _anim = "anim/char/ally/Jon/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "Sort d'intimidation", "_", "_" };
            _descP = new String[] { "Une Boule de feu puissante, ignore\nla défense ennemie.", "Un sort digne des plus grand\nmanupilateur. Baisse légèrement l'attaque\n de tous les ennemis", "_", "_" };
        }
        public void Ben()
        {
            _stat = new int[4] { 50, 40, 90, 60 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "Cri";
            _specialP = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de 'blèmes", "_" };
            _descP = new String[] { "_", "_", "Que des solutions!", "_" };         
        }

        //personnages non joueurs

        public void Grand()
        {
            _stat = new int[4] { 60, 0, 60, 100 };//20
            _anim = "anim/char/enemy/grand/character_movement.sf";
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };

        }
        public void Mechant()
        {
            _stat = new int[4] { 70, 0, 50, 50 };//70
            _anim = "anim/char/enemy/mechant/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "Sort d'intimidation", "_", "_" };
            _descP = new String[] { "Une Boule de feu puissante, ignore\nla défense ennemie", "Un sort digne des plus grand\nmanupilateur. Baisse légèrement l'attaque\n de tous les ennemis", "_", "_" };
        }
        public void Pabo()
        {
            _stat = new int[4] { 70, 0, 50, 90 };//70
            _anim = "anim/char/enemy/pabo/character_movement.sf";
            _special = "Cri";
            String[] _specialJ = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de Problèmes", "_" };
            String[] _descJ = new String[] { "_", "_", "Que des solutions!", "_" };
        }

        public void Animation()
        {
            if (ChatoCombat._animationAttackA == true)
            {
                if (ChatoCombat._animationP1 == true)
                {
                    ChatoCombat._animationA[ChatoCombat._allyAnime] = "attack_right1";

                    ChatoCombat._coolDownAnimation = true;
                    ChatoCombat._animationP1 = false;
                    ChatoCombat._animationP3 = true;

                }
                else if (ChatoCombat._posAllie[ChatoCombat._allyAnime].X > ChatoCombat._posAllieBaseX[ChatoCombat._allyAnime] + 80 && ChatoCombat._animationP3 == false)
                {
                    ChatoCombat._animationP1 = true;
                }
                else if (ChatoCombat._posAllie[ChatoCombat._allyAnime].X < ChatoCombat._posAllieBaseX[ChatoCombat._allyAnime])
                {
                    ChatoCombat._animationP2 = false;
                    ChatoCombat._animationP3 = false;
                    ChatoCombat._animationOver = true;
                    ChatoCombat._posAllie[ChatoCombat._allyAnime].X = ChatoCombat._posAllieBaseX[ChatoCombat._allyAnime];

                    ChatoCombat._animationA[ChatoCombat._allyAnime] = "idle_right";
                }
                else if (ChatoCombat._animationP2 == true && ChatoCombat._animationP3 == true)
                {
                    _myGame._hit.Play();
                    ChatoCombat._animationA[ChatoCombat._allyAnime] = "move_left";
                    ChatoCombat._posAllie[ChatoCombat._allyAnime].X -= 2;
                }
                else if (ChatoCombat._animationP1 == false && ChatoCombat._animationP2 == false && _myGame._cooldownVerifC == false && ChatoCombat._animationP3 == false)
                {
                    ChatoCombat._animationA[ChatoCombat._allyAnime] = "move_right";
                    ChatoCombat._posAllie[ChatoCombat._allyAnime].X += 2;
                }
                else if (_myGame._cooldownVerifC == false && ChatoCombat._animationP3 == true)
                {
                    ChatoCombat._animationP1 = false;
                    ChatoCombat._animationP2 = true;
                    ChatoCombat._animationP3 = true;
                }

            }
            Console.WriteLine(ChatoCombat._animationOver);
            if (ChatoCombat._animationAttackE == true)
            {
                if (ChatoCombat._animationP1 == true)
                {
                    ChatoCombat._animationE[ChatoCombat._enemyAnime] = "attack_left1";

                    ChatoCombat._coolDownAnimation = true;
                    ChatoCombat._animationP1 = false;
                    ChatoCombat._animationP3 = true;
                }
                else if (ChatoCombat._posEnemy[ChatoCombat._enemyAnime].X < ChatoCombat._posEnnBaseX[ChatoCombat._enemyAnime] - 80 && ChatoCombat._animationP3 == false)
                {
                    ChatoCombat._animationP1 = true;
                }
                else if (ChatoCombat._posEnemy[ChatoCombat._enemyAnime].X > ChatoCombat._posEnnBaseX[ChatoCombat._enemyAnime])
                {
                    ChatoCombat._animationP2 = false;
                    ChatoCombat._animationP3 = false;
                    ChatoCombat._posEnemy[ChatoCombat._enemyAnime].X = ChatoCombat._posEnnBaseX[ChatoCombat._enemyAnime];
                    ChatoCombat._animationOver = true;
                    ChatoCombat._animationE[ChatoCombat._enemyAnime] = "idle_left";
                }
                else if (ChatoCombat._animationP2 == true && ChatoCombat._animationP3 == true)
                {
                    _myGame._hit.Play();
                    ChatoCombat._animationE[ChatoCombat._enemyAnime] = "move_right";
                    ChatoCombat._posEnemy[ChatoCombat._enemyAnime].X += 2;
                }
                else if (ChatoCombat._animationP1 == false && ChatoCombat._animationP2 == false && _myGame._cooldownVerifC == false && ChatoCombat._animationP3 == false)
                {
                    ChatoCombat._animationE[ChatoCombat._enemyAnime] = "move_left";
                    ChatoCombat._posEnemy[ChatoCombat._enemyAnime].X -= 2;
                }
                else if (_myGame._cooldownVerifC == false && ChatoCombat._animationP3 == true)
                {
                    ChatoCombat._animationP1 = false;
                    ChatoCombat._animationP2 = true;
                    ChatoCombat._animationP3 = true;
                }
            }

            /*if (_animationZeweurld == true)
            {
                if (_animationP1 == true)
                {
                    _animationA[_allyAnime] = "attack_right3";
                    _myGame._wbeg.Play();
                    MediaPlayer.Stop();
                    _attackZeuwerld = true;
                    _coolDownAnimation = true;
                    _animationP1 = false;
                    _animationP3 = true;

                }
                else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                {
                    _animationP1 = true;
                }
                else if (_posAllie[_allyAnime].X < _posAllieBaseX[_allyAnime])
                {
                    _animationP2 = false;
                    _animationP3 = false;
                    _posAllie[_allyAnime].X = _posAllieBaseX[_allyAnime];
                    _animationOver = true;
                    _animationA[_allyAnime] = "idle_right";
                }
                else if (_animationP2 == true && _animationP3 == true)
                {
                    _animationA[_allyAnime] = "move_left";
                    _posAllie[_allyAnime].X -= 2;
                    kk = _chatoCombatContenu._nbEnnemy + _chatoCombatContenu._nbAlly;

                }
                else if (_animationP1 == false && _animationP2 == false && _myGame._cooldownVerifC == false && _animationP3 == false)
                {
                    _animationA[_allyAnime] = "move_right";
                    _posAllie[_allyAnime].X += 2;
                }
                else if (_myGame._cooldownVerifC == false && _animationP3 == true)
                {
                    _animationP1 = false;
                    _animationP2 = true;
                    _animationP3 = true;
                }
            }

            //boule de feu
            if (_animationBouleFeu == true)
            {
                if (_animationP1 == true)
                {
                    _animationA[_allyAnime] = "attack_right2";
                    _fireBall = true;
                    _myGame._fire.Play();
                    _posProj = new Vector2(_posEnemy[_attaquePerso[1, 1]].X - 80, _posEnemy[_attaquePerso[1, 1]].Y);
                    _coolDownAnimation = true;
                    _animationP1 = false;
                    _animationP3 = true;

                }
                else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                {
                    _animationP1 = true;
                }
                else if (_posAllie[_allyAnime].X < _posAllieBaseX[_allyAnime])
                {
                    _animationP2 = false;
                    _animationP3 = false;
                    _posAllie[_allyAnime].X = _posAllieBaseX[_allyAnime];
                    _animationOver = true;
                    _animationA[_allyAnime] = "idle_right";
                }
                else if (_animationP2 == true && _animationP3 == true)
                {
                    _animationA[_allyAnime] = "move_left";
                    _posAllie[_allyAnime].X -= 2;

                }
                else if (_animationP1 == false && _animationP2 == false && _myGame._cooldownVerifC == false && _animationP3 == false)
                {
                    _animationA[_allyAnime] = "move_right";
                    _posAllie[_allyAnime].X += 2;
                }
                else if (_myGame._cooldownVerifC == false && _animationP3 == true)
                {
                    _animationP1 = false;
                    _animationP2 = true;
                    _animationP3 = true;
                }
            }*/
        }

        public override void Update(GameTime gameTime)
        {        }

        public override void Draw(GameTime gameTime)
        {        }
    }
}
