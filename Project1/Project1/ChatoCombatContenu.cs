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
    public class ChatoCombatContenu
    {

        // défini dans Game1
        private Game1 _myGame;
        private EventEtDial _eventEtDial;
        private ChatoCombatContenu _chatoCombatContenu;
        private ChatoExtCours _chatoExtCours;
        private ChatoIntChambres _chatoIntChambres;
        private ChatoIntCouloir _chatoIntCouloir;

        public ChatoCombatContenu(Game1 game)
        {
            _myGame = game;
            _eventEtDial = _myGame._eventEtDial;
            _chatoCombatContenu = _myGame._chatoCombatContenu;
            _chatoExtCours = _myGame._chatoExtCours;
            _chatoIntChambres = _myGame._chatoIntChambres;
            _chatoIntCouloir = _myGame._chatoIntCouloir;
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


        public static Vector2 _lastPosition;

        //animation
        public int _allyAnime;
        public int _enemyAnime;

        public bool _animationAttackA;
        public bool _animationAttackE;
        public bool _animationZeweurld;
        public bool _animationBouleFeu;
        public bool _animationSpe;
        public bool _animationEnCours;

        public bool _animationP1;
        public bool _animationP2;
        public bool _animationP3;
        public bool _animationOver;
        public bool _coolDownAnimation;

        public int[] _posEnnBaseX;
        public int[] _posAllieBaseX;

        public Vector2[] _posAllie;
        public String[] _animationA;
        public Vector2[] _posEnemy;
        public String[] _animationE;

        public bool _attackZeuwerld;
        public bool _fireBall;
        public bool _badabim;
        public Vector2 _posProj;
        public Vector2 _posExplosion;

        //El famoso variable qui permet de idre qui joue dans quel ordre
        public int kk;

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
            if (_myGame._epee == true)
                _stat[1] = _stat[1] + 20;
            if (_myGame._boom == true)
                _stat[1] = _stat[1] + 200000000;

        }
        public void Jon()
        {
            _stat = new int[4] { 100, 100, 40, 10 };//90
            _anim = "anim/char/ally/Jon/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "Sort d'intimidation", "_", "_" };
            _descP = new String[] { "Une Boule de feu puissante, ignore\nla défense ennemie.", "Un sort digne des plus grand\nmanupilateur. Baisse légèrement l'attaque\n de tous les ennemis", "_", "_" };
            if (_myGame._boom == true)
                _stat[1] = _stat[1] + 200000000;
        }
        public void Ben()
        {
            _stat = new int[4] { 50, 40, 90, 60 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "Cri";
            _specialP = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de 'blèmes", "_" };
            _descP = new String[] { "Mais c'est trivial ça!", "_", "Que des solutions!", "_" };         
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
            if (_animationAttackA == true)
            {
                if (_animationP1 == true)
                {
                    if (_myGame.konami != true)
                    {
                        _animationA[_allyAnime] = "attack_right1";
                        _myGame._hit.Play();
                        
                    }                   
                    else
                    {
                        _animationA[_allyAnime] = "attack_right2";
                        _badabim = true;
                        _myGame._fire.Play();
                        _posProj = new Vector2(_posEnemy[ChatoCombat._attaquePerso[1, 1]].X - 80, _posEnemy[ChatoCombat._attaquePerso[1, 1]].Y);
                        
                    }
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
                    _animationOver = true;
                    _posAllie[_allyAnime].X = _posAllieBaseX[_allyAnime];

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

            }

            //AttackEnnemi
            if (_animationAttackE == true)
            {
                if (_animationP1 == true)
                {
                        _animationE[_enemyAnime] = "attack_left1";
                        _myGame._hit.Play();
                        _coolDownAnimation = true;
                        _animationP1 = false;
                        _animationP3 = true;
                }
                else if (_posEnemy[_enemyAnime].X < _posEnnBaseX[_enemyAnime] - 80 && _animationP3 == false)
                {
                    _animationP1 = true;
                }
                else if (_posEnemy[_enemyAnime].X > _posEnnBaseX[_enemyAnime])
                {
                    _animationP2 = false;
                    _animationP3 = false;
                    _posEnemy[_enemyAnime].X = _posEnnBaseX[_enemyAnime];
                    _animationOver = true;
                    _animationE[_enemyAnime] = "idle_left";
                }
                else if (_animationP2 == true && _animationP3 == true)
                {

                    _animationE[_enemyAnime] = "move_right";
                    _posEnemy[_enemyAnime].X += 2;
                }
                else if (_animationP1 == false && _animationP2 == false && _myGame._cooldownVerifC == false && _animationP3 == false)
                {
                    _animationE[_enemyAnime] = "move_left";
                    _posEnemy[_enemyAnime].X -= 2;
                }
                else if (_myGame._cooldownVerifC == false && _animationP3 == true)
                {
                    _animationP1 = false;
                    _animationP2 = true;
                    _animationP3 = true;
                }
            }

            if (_animationSpe == true)
            {
                if (_animationP1 == true)
                {
                    if (_animationZeweurld == true)
                    {
                        _animationA[_allyAnime] = "attack_right3";
                        _myGame._wbeg.Play();
                        MediaPlayer.Stop();
                        _attackZeuwerld = true;
                    }
                    if (_animationBouleFeu == true)
                    {
                        _animationA[_allyAnime] = "attack_right2";
                        _fireBall = true;
                        _myGame._fire.Play();
                        _posProj = new Vector2(_posEnemy[ChatoCombat._attaquePerso[1, 1]].X - 80, _posEnemy[ChatoCombat._attaquePerso[1, 1]].Y);
                    }
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
                    if (_animationZeweurld == true)
                        kk = _nbEnnemy + _nbAlly;

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
        }
    }
}
