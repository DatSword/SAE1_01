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
    internal class chato_combatcontenu
    {
        public static int _nbEquipe;
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

        public static void CombatTest()
        {
            _nbEquipe = 4;
            _ordreJoueur = new String[] { "Hero", "Jon" , "Hein", "Ben"};

            _nbEnnemy = 3;
            _ordreEnnemi = new String[] {"Grand","Mechant","Pabo"};
        }

        //Personnages jouables
        public static void Hein()
        {
            _stat = new int[4] { 1, 1, 1, 1 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "???";
            _specialP = new String[] { "_", "_", "_", "_" };
            _descP = new String[] { "_", "_", "_", "_" };
        }
        public static void Hero()
        {
            _stat = new int[4] { 80, 50, 60, 70 };
            _anim = "anim/char/ally/hero/character_movement.sf";
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };

        }
        public static void Jon()
        {
            _stat = new int[4] { 100, 100, 40, 10 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "JSP", "_", "_" };
            _descP = new String[] { "BRÛLEZZZZ", "MOURREZZZZZ", "_", "_" };
        }
        public static void Ben()
        {
            _stat = new int[4] { 50, 40, 90, 60 };
            _anim = "anim/char/base_model_m/character_movement.sf";
            _special = "Cri";
            _specialP = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de 'blèmes", "_" };
            _descP = new String[] { "_", "_", "Que des solutions!", "_" };         
        }

        //personnages non joueurs

        public static void Grand()
        {
            _stat = new int[4] { 60, 20, 60, 100 };
            _anim = "anim/char/enemy/grand/character_movement.sf";
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };

        }
        public static void Mechant()
        {
            _stat = new int[4] { 70, 70, 50, 50 };
            _anim = "anim/char/enemy/mechant/character_movement.sf";
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "JSP", "_", "_" };
            _descP = new String[] { "BRÛLEZZZZ", "MOURREZZZZZ", "_", "_" };
        }
        public static void Pabo()
        {
            _stat = new int[4] { 70, 70, 50, 100 };
            _anim = "anim/char/enemy/pabo/character_movement.sf";
            _special = "Cri";
            String[] _specialJ = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de Problèmes", "_" };
            String[] _descJ = new String[] { "_", "_", "Que des solutions!", "_" };
        }
    }
}
