using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;
using System;

namespace SAE101
{
    public class Camera
    {
        // défini dans Game1
        public Game1 _myGame;
        private ChatoIntChambres _chatoIntChambres;
        private ChatoIntCouloir _chatoIntCouloir;

        public Camera(Game1 game)
        {
            _myGame = game;
            _chatoIntChambres = _myGame._chatoIntChambres;
            _chatoIntCouloir = _myGame._chatoIntCouloir;
        }

        public void PositionCamera()
        {
            // chambres nord
            if (_myGame._numEcran == 1 && _myGame._positionPerso.X < _chatoIntChambres._limChambre_x1
                                && _myGame._positionPerso.Y < _chatoIntChambres._limChambre_y1
                                && _myGame._positionPerso.X < _chatoIntChambres._limChambre_Gauche)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentre1;

            else if (_myGame._numEcran == 1 && _myGame._positionPerso.X < _chatoIntChambres._limChambre_x1
                                && _myGame._positionPerso.Y < _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentreUn;

            else if (_myGame._numEcran == 1 && _myGame._positionPerso.X > _chatoIntChambres._limChambre_x2
                                && _myGame._positionPerso.Y < _chatoIntChambres._limChambre_y1
                                && _myGame._positionPerso.X > _chatoIntChambres._limChambre_Droite)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentreDeux;

            else if (_myGame._numEcran == 1 && _myGame._positionPerso.X > _chatoIntChambres._limChambre_x2
                                && _myGame._positionPerso.Y < _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentre2;

            else if (_myGame._numEcran == 1 && _myGame._positionPerso.Y >= _chatoIntCouloir._limCouloir)
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);


            // couloir
            if (_myGame._numEcran == 2 && (_myGame._positionPerso.Y > 0
                                && (_myGame._positionPerso.X > _chatoIntCouloir._limChambre_x1 ||
                                _myGame._positionPerso.X < _chatoIntCouloir._limChambre_x2)))
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);

            else if (_myGame._numEcran == 2 && _myGame._positionPerso.X < _chatoIntChambres._limChambre_x1
                                && _myGame._positionPerso.X < _chatoIntChambres._limChambre_Gauche
                                && _myGame._positionPerso.Y >= _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentre1;

            else if (_myGame._numEcran == 2 && _myGame._positionPerso.X < _chatoIntChambres._limChambre_x1
                                && _myGame._positionPerso.X > _chatoIntChambres._limChambre_Gauche
                                && _myGame._positionPerso.Y >= _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentreUn;

            else if (_myGame._numEcran == 2 && _myGame._positionPerso.X > _chatoIntChambres._limChambre_x2
                                && _myGame._positionPerso.X < _chatoIntChambres._limChambre_Droite
                                && _myGame._positionPerso.Y >= _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentre2;

            else if (_myGame._numEcran == 2 && _myGame._positionPerso.X > _chatoIntChambres._limChambre_x2
                                && _myGame._positionPerso.X > _chatoIntChambres._limChambre_Droite
                                && _myGame._positionPerso.Y >= _chatoIntChambres._limChambre_y1)
                _myGame._cameraPosition = _chatoIntChambres._chambreCentreDeux;

            else if (_myGame._numEcran == 2 && _myGame._positionPerso.Y > 49 * 16)
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);


            // cours
            else if (_myGame._numEcran == 3 && _myGame._positionPerso.Y >= 1 * 16)
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);

            else if (_myGame._numEcran == 3 && _myGame._positionPerso.Y < 1 * 16)
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);


            // combat
            else if (_myGame._numEcran == 4)
                _myGame._cameraPosition = new Vector2(_myGame._xEcran / 2, _myGame._yEcran / 2);


            // couronne
            else if (_myGame._numEcran == 5)
                _myGame._cameraPosition = new Vector2(_myGame._positionPerso.X, _myGame._positionPerso.Y);


            Console.WriteLine(_myGame._numEcran);
        }
    }
}
