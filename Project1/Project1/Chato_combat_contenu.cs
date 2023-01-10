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
    public class Chato_combat_contenu : GameScreen
    {

        // défini dans Game1
        private new Game1 Game => (Game1)base.Game;
        private Game1 _myGame;

        public Chato_combat_contenu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public static int _nbAlly;
        public static int _nbEnnemy;

        public static String _special;
        public static String _anim;
        public static String[] _specialP;
        public static String[] _descP;
        public static String[] _nomPersoJouable = new String[4] {"???","Hero","Jon","Ben"};
        public static int _nbPersoJouable = 4;
        public static String[] _nomEnnJouable = new String[3] { "Grand", "Mechant", "Pabo" };
        public static int _nbEnnJouable = 3;       
        public static String[] _ordreJoueur;
        public static String[] _ordreEnnemi;
        public static int[] _stat; //PV, Attaque, Défense, Vitesse

        public static Vector2 _lastPosition;

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
            _anim = "anim/char/base_model_m/character_movement.sf";
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

        }

        public override void Update(GameTime gameTime)
        {        }

        public override void Draw(GameTime gameTime)
        {        }
    }
}
