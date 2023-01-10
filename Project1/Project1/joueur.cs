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
    internal class Joueur
    {
       public static void Spawnchato_int_chambres_couloir()
       {
            if (Chato_int_chambres_nord._posX == 0)
                Game1._positionPerso = new Vector2(104, 112);

            if (Chato_int_chambres_nord._posX >= 3 * 16 && Chato_int_chambres_nord._posX < 5 * 16)
                Game1._positionPerso = new Vector2(6 * 16 + 8, 7 * 16);
            else if (Chato_int_chambres_nord._posX >= 11 * 16 && Chato_int_chambres_nord._posX < 13 * 16)
                Game1._positionPerso = new Vector2(14 * 16 + 8, 7 * 16);
            else if (Chato_int_chambres_nord._posX >= 27 * 16 && Chato_int_chambres_nord._posX < 29 * 16)
                Game1._positionPerso = new Vector2(30 * 16 + 8, 7 * 16);
            else if (Chato_int_chambres_nord._posX >= 35 * 16 && Chato_int_chambres_nord._posX < 37 * 16)
                Game1._positionPerso = new Vector2(38 * 16 + 8, 7 * 16);

            else if (Chato_ext_cours_interieur._posX >= 19 * 16 && Chato_ext_cours_interieur._posX < 21 * 16)
                Game1._positionPerso = new Vector2((float)20.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (Chato_ext_cours_interieur._posX >= 20 * 16 && Chato_ext_cours_interieur._posX < 22 * 16)
                Game1._positionPerso = new Vector2((float)21.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (Chato_ext_cours_interieur._posX >= 21 * 16 && Chato_ext_cours_interieur._posX < 23 * 16)
                Game1._positionPerso = new Vector2((float)22.5 * 16 + 8, (float)1.5 * 16 + 8);
            else if (Chato_ext_cours_interieur._posX >= 22 * 16 && Chato_ext_cours_interieur._posX < 24 * 16)
                Game1._positionPerso = new Vector2((float)23.5 * 16 + 8, (float)1.5 * 16 + 8);

            //x = casex * 16 + 8, y = casey * 16 + 8
        }

        public static void Spawnchato_int_chambres_nord()
        {
            if (Chato_int_chambres_nord._posX == 0)
                Game1._positionPerso = new Vector2(3 * 16 + 8, 2 * 16 + 8);

            if (Chato_int_chambres_couloir._posX >= 5 * 16 && Chato_int_chambres_couloir._posX < 7 * 16)
                Game1._positionPerso = new Vector2(72, 7 * 16);
            else if (Chato_int_chambres_couloir._posX >= 13 * 16 && Chato_int_chambres_couloir._posX < 15 * 16)
                Game1._positionPerso = new Vector2(12 * 16 + 8, 7 * 16);
            else if (Chato_int_chambres_couloir._posX >= 29 * 16 && Chato_int_chambres_couloir._posX < 31 * 16)
                Game1._positionPerso = new Vector2(28 * 16 + 8, 7 * 16);
            else if (Chato_int_chambres_couloir._posX >= 37 * 16 && Chato_int_chambres_couloir._posX < 39 * 16)
                Game1._positionPerso = new Vector2(36 * 16 + 8, 7 * 16);
        }

        public static void Spawnchato_ext_cours_interieur()
        {
            if (Chato_int_chambres_couloir._posX == 0)
                Game1._positionPerso = new Vector2(22 * 16, 49 * 16);

            if (Chato_int_chambres_couloir._posX >= 19 * 16 && Chato_int_chambres_couloir._posX < 21.5 * 16)
                Game1._positionPerso = new Vector2(20 * 16 + 8, 49 * 16);
            else if (Chato_int_chambres_couloir._posX >= 21.5 * 16 && Chato_int_chambres_couloir._posX < 22.5 * 16)
                Game1._positionPerso = new Vector2(21 * 16 + 8, 49 * 16);
            else if (Chato_int_chambres_couloir._posX >= 22.5 * 16 && Chato_int_chambres_couloir._posX < 23.5 * 16)
                Game1._positionPerso = new Vector2(22 * 16 + 8, 49 * 16);
            else if (Chato_int_chambres_couloir._posX >= 23.5 * 16 && Chato_int_chambres_couloir._posX < 25 * 16)
                Game1._positionPerso = new Vector2(23 * 16 + 8, 49 * 16);
        }

            public static TiledMapTileLayer MapLayer()
        {
            TiledMapTileLayer mapLayerCollision = Chato_int_chambres_nord.mapLayer;

            if (Game1._numEcran == 1)
                mapLayerCollision = Chato_int_chambres_nord.mapLayer;
            else if (Game1._numEcran == 2)
                mapLayerCollision = Chato_int_chambres_couloir.mapLayer;
            else if (Game1._numEcran == 3)
                mapLayerCollision = Chato_ext_cours_interieur.mapLayer;
            else
                mapLayerCollision = Chato_int_chambres_nord.mapLayer;

            return mapLayerCollision;
        }
        public static void Mouvement(GameTime gameTime)
        {
            if (Game1._stop == 1 && Game1._keyboardState.IsKeyUp(Keys.Down))
                Game1._animationPlayer = "idle_down";
            else if (Game1._stop == 2 && Game1._keyboardState.IsKeyUp(Keys.Up))
                Game1._animationPlayer = "idle_up";
            else if (Game1._stop == 3 && Game1._keyboardState.IsKeyUp(Keys.Left))
                Game1._animationPlayer = "idle_left";
            else if (Game1._stop == 4 && Game1._keyboardState.IsKeyUp(Keys.Right))
                Game1._animationPlayer = "idle_right";

            if (Event_et_dial._dialTrue == false)
            {
                if (Game1._keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight - 1);
                    Game1._animationPlayer = "move_up";
                    Game1._stop = 2;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y -= Game1._walkSpeed;
                }
                if (Game1._keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight + 1);
                    Game1._animationPlayer = "move_down";
                    Game1._stop = 1;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.Y += Game1._walkSpeed;
                }
                if (Game1._keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight);
                    Game1._animationPlayer = "move_left";
                    Game1._stop = 3;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X -= Game1._walkSpeed;
                }
                if (Game1._keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight);
                    Game1._animationPlayer = "move_right";
                    Game1._stop = 4;
                    if (!IsCollision(tx, ty))
                        Game1._positionPerso.X += Game1._walkSpeed;
                }
                else if (Game1._animationPlayer == null)
                    Game1._animationPlayer = "idle_down";
                else
                    Game1._animationPlayer = Game1._animationPlayer;


            }
        }

        public static bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (Game1.mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}
