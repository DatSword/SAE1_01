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
        public static String[] _specialP;
        public static String[] _descP;
        public static String[] _nomPersoJouable = new String[4] {"???","Hero","Jon","Ben"};
        public static int _nbPersoJouable = 4;
        public static String[] _ordreJoueur;
        public static String[] _ordreEnnemi;

        public static String[] _fileA;
        public static String[] _fileE;
        public static String _anim1;
        public static String _anim2;
        public static String _anim3;
        public static String _anim4;
        public static String _anim5;
        public static String _anim6;
        public static String _anim7;
        public static String _anim8;

        public static void CombatTest()
        {
            _nbEquipe = 2;
            _fileA = new String[_nbEquipe];
            _ordreJoueur = new String[] { "Hero", "Jon" };

            _nbEnnemy = 2;
            _fileE = new String[_nbEnnemy];

        }

        //Personnages jouables
        public static void Hein()
        {
            _special = "???";
            _specialP = new String[] { "_", "_", "_", "_" };
            _descP = new String[] { "_", "_", "_", "_" };
        }

        public static void Hero()
        {
            
            _special = "NommCoul";
            _specialP = new String[] { "Zeuwerld", "Baïtzedeust", "_", "_" };
            _descP = new String[] { "Arrête le temps du tour en cours, et \ndu suivant. Affecte les ennemis comme les alliés.", "Remonte le temps jusqu'au dernier tour.\nUtile pour prévenir les actions ennemies.", "_", "_" };

        }
        public static void Jon()
        {
            _special = "Magie";
            _specialP = new String[] { "Boule de feu", "JSP", "_", "_" };
            _descP = new String[] { "BRÛLEZZZZ", "MOURREZZZZZ", "_", "_" };
        }
        public static void Ben()
        {
            _special = "Cri";
            String[] _specialJ = new String[] { "NON MAIS OH", "NOM DE DIOU", "Pas de Problèmes", "_" };
            String[] _descJ = new String[] { "_", "_", "Que des solutions!", "_" };         
        }
    }
}
