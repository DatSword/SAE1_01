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
    internal class eventsetdial
    {
        
        public static void Event1()
        {
            Game1._dialTrue = true;
            Game1._text = "EH OH GAMIN, REVEIL - TOI! TU VAS M'FAIRE ATTENDRE\nENCORE LONGTEMPS?!";
            Game1._nom = "???";
        }

        public static void Event2()
        {
            Game1._dialTrue = true;
            Game1._text = ":)";
            Game1._nom = "Fren";
        }
    }
}
