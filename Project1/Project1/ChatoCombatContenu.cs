﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;

namespace SAE101
{
    public class ChatoCombatContenu
    {

        // défini dans Game1
        private Game1 _myGame;
        private ChatoCombat _chatoCombat;

        public ChatoCombatContenu(Game1 game)
        {
            _myGame = game;
            _chatoCombat = _myGame._chatoCombat;
        }

        public String _special;
        public String _anim;
        public String[] _specialP;
        public String[] _descP;
        public String[] _nomPersoJouable = new String[4] { "???", "Hero", "Jon", "Ben" };
        public int _nbPersoJouable = 4;
        public String[] _nomEnnJouable = new String[3] { "Grand", "Mechant", "Pabo" };
        public int _nbEnnJouable = 3;

        public int[] _stat; //PV, Attaque, Défense, Vitesse //PV : au pif, Attaque >= 50, Défense <= 50, Vitesse : entre 1 et 100


        public static Vector2 _lastPosition;

        //animation
        public int _allyAnime;
        public int _enemyAnime;

        public bool _animationAttackA;
        public bool _animationAttackE;
        public bool _animationZeuwerld;
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

        //El famoso variable qui permet de dire qui joue dans quel ordre
        public int whosPlaying;
        
        public void Combat()
        {
            _lastPosition = _myGame._positionPerso;
        }

        public static Personnage hein = new Personnage("Hein", 1, 1, 1, 1, "anim/char/base_model_m/character_movement.sf", "???",new string[] { "_", "_", "_", "_" }, new string[] { "_", "_", "_", "_" });
        public static Personnage hero = new Personnage("Hero", 160, 60, 30, 70, "anim/char/ally/hero/character_movement.sf","Nommcoul", new string[] { "Zeuwerld", "Baïtzedeust", "_", "_" }, new string[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.\nIdéal pour souffler et pour gagner un peu de temps", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" });
        public static Personnage jon = new Personnage("Jon", 200, 90, 20, 10, "anim/char/ally/Jon/character_movement.sf", "Magie", new string[] { "Boule de feu", "Sort d'intimidation", "_", "_" }, new string[] { "Une Boule de feu puissante, ignore\nla défense ennemie.", "Un sort digne des plus grand\nmanupilateur. Baisse légèrement l'attaque\n de tous les ennemis", "_", "_" });
        public static Personnage ben = new Personnage("Ben", 1000, 100, 50, 100, "anim/char/base_model_m/character_movement.sf","Cri",new string[] { "NON MAIS OH", "NOM DE DIOU", "Pas de 'blèmes", "_" }, new string[] { "Mais c'est trivial ça!", "_", "Que des solutions!", "_" });



        public void Hero()
        {
            _stat = new int[4] { 160, 60 ,30, 70 };
            _anim = "anim/char/ally/hero/character_movement.sf";
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", /*"Baïtzedeust"*/ "_", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.\nIdéal pour souffler et pour gagner un peu de temps", /*"Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies."*/ "_", "_", "_" };
            if (_myGame._epee == true)
                _stat[1] = 70;
            if (_myGame._boom == true)
                _stat[1] = _stat[1] + 200000000;
        }

        public void Jon()
        {
            _stat = new int[4] { 200, 90, 20, 10 };
            _anim = "anim/char/ally/Jon/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", /*"Sort d'intimidation"*/"_", "_", "_" };
            _descP = new String[] { "Une Boule de feu puissante, ignore\nla défense ennemie.", /*"Un sort digne des plus grand\nmanupilateur. Baisse légèrement l'attaque\n de tous les ennemis"*/"_", "_", "_" };
            if (_myGame._boom == true)
                _stat[1] = 200000000;
        }

        //personnages non joueurs

        public void Grand()
        {
            _stat = new int[4] { 70, 60, 30, 100 };
            _anim = "anim/char/enemy/grand/character_movement.sf";
            _special = "";
            _specialP = new String[] { "", "", "", "" };
            _descP = new String[] { "", "", "" , ""};

        }

        public void Mechant()
        {
            _stat = new int[4] { 100, 70, 20, 50 };
            _anim = "anim/char/enemy/mechant/character_movement.sf";
            _special = "";
            _specialP = new String[] { "", "", "", "" };
            _descP = new String[] { "", "", "", "" };
        }

        public void Pabo()
        {
            _stat = new int[4] { 140, 80, 40, 80 };
            _anim = "anim/char/enemy/pabo/character_movement.sf";
            _special = "";
            String[] _specialJ = new String[] { "", "", "", "" };
            String[] _descJ = new String[] { "", "", "", "" };
        }

        public void Animation()
        {
            if (_animationAttackA == true)
            {
                if (_animationP1 == true)
                {
                    if (_myGame._boom != true)
                    {
                        _animationA[_allyAnime] = "attack_right1";
                        _myGame._hit.Play(); 
                    }                   
                    else
                    {
                        _animationA[_allyAnime] = "attack_right2";
                        _badabim = true;
                        _myGame._fire.Play();
                        _posProj = new Vector2(_posEnemy[_chatoCombat._attaquePerso[_chatoCombat._playerAttacking, 1]].X - 80, _posEnemy[_chatoCombat._attaquePerso[_chatoCombat._playerAttacking, 1]].Y);
                        
                    }
                    _coolDownAnimation = true;
                    _animationP1 = false;
                    _animationP3 = true;

                }
                else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                    _animationP1 = true;
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
                    _animationP1 = true;
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
                    if (_animationZeuwerld == true)
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
                        _posProj = new Vector2(_posEnemy[_chatoCombat._attaquePerso[1, 1]].X - 80, _posEnemy[_chatoCombat._attaquePerso[1, 1]].Y);
                    }
                    _coolDownAnimation = true;
                    _animationP1 = false;
                    _animationP3 = true;

                }
                else if (_posAllie[_allyAnime].X > _posAllieBaseX[_allyAnime] + 80 && _animationP3 == false)
                    _animationP1 = true;
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
                    if (_animationZeuwerld == true)
                        whosPlaying = _myGame._nbEnemy + _myGame._nbAlly;
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

            if (_fireBall == true)
            {

                _posProj.X += 2;
                _chatoCombat._animationProj = "fireball";
                if (_posProj.X == _posEnemy[_chatoCombat._attaquePerso[1, 1]].X)
                {
                    _posProj = new Vector2(-32, -16);
                    _fireBall = false;
                }
            }

            if (_badabim == true)
            {

                _posProj.X += 2;
                _chatoCombat._animationProj = "badaboom";
                if (_posProj.X == _posEnemy[_chatoCombat._attaquePerso[_chatoCombat._playerAttacking, 1]].X)
                {
                    _myGame._pelo.Play();
                    _posProj = new Vector2(-32, -16);
                    _fireBall = false;
                    _posExplosion = _posEnemy[_chatoCombat._attaquePerso[_chatoCombat._playerAttacking, 1]];
                }
            }
        }
    }
}
