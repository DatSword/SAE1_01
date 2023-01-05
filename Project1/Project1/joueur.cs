using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace SAE101
{
    internal class joueur
    {
       public static void SpawnPersoCouloir()
       {
            if (chato_int_chambres_nord._posX == 0)
                chato_int_chambres_couloir._positionPerso = new Vector2(104, 112);

            if (chato_int_chambres_nord._posX >= 3 * 16 && chato_int_chambres_nord._posX < 5 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(104, 112);
            else if (chato_int_chambres_nord._posX >= 11 * 16 && chato_int_chambres_nord._posX < 13 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(14 * 16 + 8, 112);
            else if (chato_int_chambres_nord._posX >= 27 * 16 && chato_int_chambres_nord._posX < 29 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(30 * 16 + 8, 112);
            else if (chato_int_chambres_nord._posX >= 35 * 16 && chato_int_chambres_nord._posX < 37 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(38 * 16 + 8, 112);
            else if (chato_ext_cours_interieur._posX != 0)
                chato_int_chambres_couloir._positionPerso = new Vector2(22 * 16 + 8, 2 * 16 + 8);


       }

        /*public static void SpawnPersoChambres()
        {
            if (chato_int_chambres_nord._posX == 0)
                chato_int_chambres_nord._positionPerso = new Vector2(4 * 16 + 8, 3 * 16 + 8);

            if (chato_int_chambres_couloir._posX >= 5 * 16 && chato_int_chambres_couloir._posX < 7 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(72, 111);
            else if (chato_int_chambres_couloir._posX >= 13 * 16 && chato_int_chambres_couloir._posX < 15 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(12 * 16 + 8, 111);
            else if (chato_int_chambres_couloir._posX >= 29 * 16 && chato_int_chambres_couloir._posX < 31 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(28 * 16 + 8, 111);
            else if (chato_int_chambres_couloir._posX >= 37 * 16 && chato_int_chambres_couloir._posX < 39 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(36 * 16 + 8, 111);
        }*/


    }
}
