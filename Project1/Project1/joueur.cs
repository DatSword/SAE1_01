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
       public static void Spawnchato_int_chambres_couloir()
       {
            if (chato_int_chambres_nord._posX == 0)
                chato_int_chambres_couloir._positionPerso = new Vector2(104, 112);

            if (chato_int_chambres_nord._posX >= 3 * 16 && chato_int_chambres_nord._posX < 5 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(6 * 16 + 8, 7 * 16);
            else if (chato_int_chambres_nord._posX >= 11 * 16 && chato_int_chambres_nord._posX < 13 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(14 * 16 + 8, 7 * 16);
            else if (chato_int_chambres_nord._posX >= 27 * 16 && chato_int_chambres_nord._posX < 29 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(30 * 16 + 8, 7 * 16);
            else if (chato_int_chambres_nord._posX >= 35 * 16 && chato_int_chambres_nord._posX < 37 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2(38 * 16 + 8, 7 * 16);

            else if (chato_ext_cours_interieur._posX >= 19 * 16 && chato_ext_cours_interieur._posX < 21 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2((float)20.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (chato_ext_cours_interieur._posX >= 20 * 16 && chato_ext_cours_interieur._posX < 22 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2((float)21.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (chato_ext_cours_interieur._posX >= 21 * 16 && chato_ext_cours_interieur._posX < 23 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2((float)22.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (chato_ext_cours_interieur._posX >= 22 * 16 && chato_ext_cours_interieur._posX < 24 * 16)
                chato_int_chambres_couloir._positionPerso = new Vector2((float)23.5 * 16 + 8, (float)1.5 * 16 + 8);

            //x = casex * 16 + 8, y = casey * 16 + 8
        }

        public static void Spawnchato_int_chambres_nord()
        {
            if (chato_int_chambres_nord._posX == 0)
                chato_int_chambres_nord._positionPerso = new Vector2(3 * 16 + 8, 2 * 16 + 8);

            if (chato_int_chambres_couloir._posX >= 5 * 16 && chato_int_chambres_couloir._posX < 7 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(72, 7 * 16);
            else if (chato_int_chambres_couloir._posX >= 13 * 16 && chato_int_chambres_couloir._posX < 15 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(12 * 16 + 8, 7 * 16);
            else if (chato_int_chambres_couloir._posX >= 29 * 16 && chato_int_chambres_couloir._posX < 31 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(28 * 16 + 8, 7 * 16);
            else if (chato_int_chambres_couloir._posX >= 37 * 16 && chato_int_chambres_couloir._posX < 39 * 16)
                chato_int_chambres_nord._positionPerso = new Vector2(36 * 16 + 8, 7 * 16);
        }

        public static void Spawnchato_ext_cours_interieur()
        {
            if (chato_int_chambres_couloir._posX == 0)
                chato_ext_cours_interieur._positionPerso = new Vector2(22 * 16, 49 * 16);

            if (chato_int_chambres_couloir._posX >= 19 * 16 && chato_int_chambres_couloir._posX < 21.5 * 16)
                chato_ext_cours_interieur._positionPerso = new Vector2(20 * 16 + 8, 49 * 16);
            else if (chato_int_chambres_couloir._posX >= 21.5 * 16 && chato_int_chambres_couloir._posX < 22.5 * 16)
                chato_ext_cours_interieur._positionPerso = new Vector2(21 * 16 + 8, 49 * 16);
            else if (chato_int_chambres_couloir._posX >= 22.5 * 16 && chato_int_chambres_couloir._posX < 23.5 * 16)
                chato_ext_cours_interieur._positionPerso = new Vector2(22 * 16 + 8, 49 * 16);
            else if (chato_int_chambres_couloir._posX >= 23.5 * 16 && chato_int_chambres_couloir._posX < 25 * 16)
                chato_ext_cours_interieur._positionPerso = new Vector2(23 * 16 + 8, 49 * 16);
        }

            public static TiledMapTileLayer MapLayer()
        {
            TiledMapTileLayer mapLayerCollision = chato_int_chambres_nord.mapLayer;

            if (Game1._numEcran == 1)
                mapLayerCollision = chato_int_chambres_nord.mapLayer;
            else if (Game1._numEcran == 2)
                mapLayerCollision = chato_int_chambres_couloir.mapLayer;
            else if (Game1._numEcran == 3)
                mapLayerCollision = chato_ext_cours_interieur.mapLayer;
            else
                mapLayerCollision = chato_int_chambres_nord.mapLayer;

            return mapLayerCollision;
        }

        public static bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (MapLayer().TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}
