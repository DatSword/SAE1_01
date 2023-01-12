using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;

namespace SAE101
{
    public class JoueurSpawn
    {
        // défini dans Game1
        public Game1 _myGame;
        private EventEtDial _eventEtDial;
        private ChatoExtCours _chatoExtCours;
        private ChatoIntChambres _chatoIntChambres;
        private ChatoIntCouloir _chatoIntCouloir;
        private ChatoIntTrone _chatoIntTrone;

        public JoueurSpawn(Game1 game)
        {
            _myGame = game;
            _eventEtDial = _myGame._eventEtDial;
            _chatoExtCours = _myGame._chatoExtCours;
            _chatoIntChambres = _myGame._chatoIntChambres;
            _chatoIntCouloir = _myGame._chatoIntCouloir;
            _chatoIntTrone = _myGame._chatoIntTrone;
        }

        public void Spawnchato_int_chambres_couloir()
        {
            if (_myGame._combatFini == true)
            {
                _myGame._positionPerso = ChatoCombatContenu._lastPosition;
                _myGame._combatFini = false;
            }
            else
            {
                if (_chatoIntChambres._posX == 0)
                    _myGame._positionPerso = new Vector2(104, 112);

                if (_chatoIntChambres._posX >= 3 * 16 && _chatoIntChambres._posX < 5 * 16)
                    _myGame._positionPerso = new Vector2(6 * 16 + 8, 7 * 16);
                else if (_chatoIntChambres._posX >= 11 * 16 && _chatoIntChambres._posX < 13 * 16)
                    _myGame._positionPerso = new Vector2(14 * 16 + 8, 7 * 16);
                else if (_chatoIntChambres._posX >= 27 * 16 && _chatoIntChambres._posX < 29 * 16)
                    _myGame._positionPerso = new Vector2(30 * 16 + 8, 7 * 16);
                else if (_chatoIntChambres._posX >= 35 * 16 && _chatoIntChambres._posX < 37 * 16)
                    _myGame._positionPerso = new Vector2(38 * 16 + 8, 7 * 16);

                else if (_chatoExtCours._posX >= 19 * 16 && _chatoExtCours._posX < 21 * 16)
                    _myGame._positionPerso = new Vector2((float)20.5 * 16 + 8, (float)1.5 * 16 + 8);
                else if (_chatoExtCours._posX >= 20 * 16 && _chatoExtCours._posX < 22 * 16)
                    _myGame._positionPerso = new Vector2((float)21.5 * 16 + 8, (float)1.5 * 16 + 8);
                else if (_chatoExtCours._posX >= 21 * 16 && _chatoExtCours._posX < 23 * 16)
                    _myGame._positionPerso = new Vector2((float)22.5 * 16 + 8, (float)1.5 * 16 + 8);
                else if (_chatoExtCours._posX >= 22 * 16 && _chatoExtCours._posX < 24 * 16)
                    _myGame._positionPerso = new Vector2((float)23.5 * 16 + 8, (float)1.5 * 16 + 8);
            }
            

            //x = casex * 16 + 8, y = casey * 16 + 8
        }

        public void Spawnchato_int_chambres_nord()
        {
            if (_myGame._combatFini == true)
            {
                _myGame._positionPerso = ChatoCombatContenu._lastPosition;
                _myGame._combatFini = false;
            }
            else
            {
                if (_chatoIntChambres._posX == 0)
                    _myGame._positionPerso = new Vector2(3 * 16 + 8, 2 * 16 + 8);

                if (_chatoIntCouloir._posX >= 5 * 16 && _chatoIntCouloir._posX < 7 * 16)
                    _myGame._positionPerso = new Vector2(72, 7 * 16);
                else if (_chatoIntCouloir._posX >= 13 * 16 && _chatoIntCouloir._posX < 15 * 16)
                    _myGame._positionPerso = new Vector2(12 * 16 + 8, 7 * 16);
                else if (_chatoIntCouloir._posX >= 29 * 16 && _chatoIntCouloir._posX < 31 * 16)
                    _myGame._positionPerso = new Vector2(28 * 16 + 8, 7 * 16);
                else if (_chatoIntCouloir._posX >= 37 * 16 && _chatoIntCouloir._posX < 39 * 16)
                    _myGame._positionPerso = new Vector2(36 * 16 + 8, 7 * 16);
            }
        }

        public void Spawnchato_ext_cours_interieur()
        {
            if (_myGame._combatFini == true)
            {
                _myGame._positionPerso = ChatoCombatContenu._lastPosition;
                _myGame._combatFini = false;
            }
            else
            {
                /*if (_chatoIntCouloir._posX == 0)
                    _myGame._positionPerso = new Vector2(22 * 16, 49 * 16); */

                if (_chatoIntCouloir._posX >= 19 * 16 && _chatoIntCouloir._posX < 21.5 * 16)
                    _myGame._positionPerso = new Vector2(20 * 16 + 8, 49 * 16);
                else if (_chatoIntCouloir._posX >= 21.5 * 16 && _chatoIntCouloir._posX < 22.5 * 16)
                    _myGame._positionPerso = new Vector2(21 * 16 + 8, 49 * 16);
                else if (_chatoIntCouloir._posX >= 22.5 * 16 && _chatoIntCouloir._posX < 23.5 * 16)
                    _myGame._positionPerso = new Vector2(22 * 16 + 8, 49 * 16);
                else if (_chatoIntCouloir._posX >= 23.5 * 16 && _chatoIntCouloir._posX < 25 * 16)
                    _myGame._positionPerso = new Vector2(23 * 16 + 8, 49 * 16);

                if (_chatoIntTrone._posX >= 8 * 16 && _chatoIntTrone._posX < 9 * 16)
                    _myGame._positionPerso = new Vector2(20 * 16, 10 * 16);
                else if (_chatoIntTrone._posX >= 9 * 16 && _chatoIntTrone._posX < 10 * 16)
                    _myGame._positionPerso = new Vector2(21 * 16, 10 * 16);
                else if (_chatoIntTrone._posX >= 10 * 16 && _chatoIntTrone._posX < 11 * 16)
                    _myGame._positionPerso = new Vector2(22 * 16, 10 * 16);
                else if (_chatoIntTrone._posX >= 11 * 16 && _chatoIntTrone._posX < 12 * 16)
                    _myGame._positionPerso = new Vector2(23 * 16, 10 * 16);
                else if (_chatoIntTrone._posX >= 12 * 16)
                    _myGame._positionPerso = new Vector2(23 * 16, 10 * 16);
            }
        }

        public void Spawnchato_int_couronne()
        {
            if (_myGame._combatFini == true)
            {
                _myGame._positionPerso = ChatoCombatContenu._lastPosition;
                _myGame._combatFini = false;
            }
            else
                _myGame._positionPerso = new Vector2(11 * 16, 38 * 16);
        }

        public void Mouvement(GameTime gameTime)
        {
            if (_myGame._stop == 1 && _myGame._keyboardState.IsKeyUp(Keys.Down))
                _myGame._animationPlayer = "idle_down";
            else if (_myGame._stop == 2 && _myGame._keyboardState.IsKeyUp(Keys.Up))
                _myGame._animationPlayer = "idle_up";
            else if (_myGame._stop == 3 && _myGame._keyboardState.IsKeyUp(Keys.Left))
                _myGame._animationPlayer = "idle_left";
            else if (_myGame._stop == 4 && _myGame._keyboardState.IsKeyUp(Keys.Right))
                _myGame._animationPlayer = "idle_right";

            if (_eventEtDial._dialTrue == false)
            {
                if (_myGame._keyboardState.IsKeyDown(Keys.Up))
                {
                    ushort tx = (ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth);
                    ushort ty = (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight - 1);
                    _myGame._animationPlayer = "move_up";
                    _myGame._stop = 2;
                    if (!IsCollision(tx, ty))
                        _myGame._positionPerso.Y -= _myGame._walkSpeed;
                }
                if (_myGame._keyboardState.IsKeyDown(Keys.Down))
                {
                    ushort tx = (ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth);
                    ushort ty = (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight + 1);
                    _myGame._animationPlayer = "move_down";
                    _myGame._stop = 1;
                    if (!IsCollision(tx, ty))
                        _myGame._positionPerso.Y += _myGame._walkSpeed;
                }
                if (_myGame._keyboardState.IsKeyDown(Keys.Left))
                {
                    ushort tx = (ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight);
                    _myGame._animationPlayer = "move_left";
                    _myGame._stop = 3;
                    if (!IsCollision(tx, ty))
                        _myGame._positionPerso.X -= _myGame._walkSpeed;
                }
                if (_myGame._keyboardState.IsKeyDown(Keys.Right))
                {
                    ushort tx = (ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight);
                    _myGame._animationPlayer = "move_right";
                    _myGame._stop = 4;
                    if (!IsCollision(tx, ty))
                        _myGame._positionPerso.X += _myGame._walkSpeed;
                }
                else if (_myGame._animationPlayer == null)
                    _myGame._animationPlayer = "idle_down";
                else
                    _myGame._animationPlayer = _myGame._animationPlayer;
            }
        }

        public bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (_myGame.mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}
