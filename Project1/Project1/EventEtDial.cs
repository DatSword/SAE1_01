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

    public class EventEtDial
    {
        // défini dans Game1
        private Game1 _myGame;

        public EventEtDial(Game1 game)
        {
            _myGame = game;
        }

        public Texture2D _dialBox;
        public Vector2 _posDialBox;
        public String _text;
        public Vector2 _posText;
        public String _nom;
        public Vector2 _posNom;
        public bool _dialTrue;


        public static int u;
        public static int d;
        public static int l;
        public static int r;
        public static int ud;
        public static int dd;

        //Boites de choix
        public Texture2D _choiceBox;
        public Vector2 _posChoiceBox;
        public Texture2D _cursor;
        public Vector2 _posCursor;
        public String _yes;
        public String _no;
        public Vector2 _posYes;
        public Vector2 _posNo;
        public bool _choiceTrue;

        public int _count = 0;

        public void toutDebut()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "EH OH GAMIN, REVEIL - TOI! TU VAS M'FAIRE ATTENDRE\n" +
                          "ENCORE LONGTEMPS?!";
            _nom = "???";
            _count += 1;
        }

        public void Fren1()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = ":)";
            _nom = "Fren";
            _myGame._duck.Play();
        }
        public void Fren2()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = ":(";
            _nom = "Fren";
            _myGame._duck.Play();
        }

        public void Jon1()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Ah voilà, enfin réveillé, désolé d'avoir hurler mais\n" +
                    "tout le monde est déjà parti vers la salle du trône!\n" +
                    "Je comprend ta fatigue, mais ça serait dommage de ne pas\n" +
                    "assister au courronnement, on a un peu beaucoup galéré\n" +
                    "pour ce moment!";
            _nom = "Jon";
        }

        public void Jon2()
        {
            _myGame.SetCoolDown();
            _text = "J't'attend dans le couloir donc récupère vite tes affaires\n" +
                    ",ou j'vais croire que tu as décidé de prolonger ta nuit!";
            _nom = "Jon";
            _myGame._firstVisitBedroom = false;
        }

        public void Jon3()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Ah, rev'la des malfrats! J'croyais qu'on les avait fait tous\n" +
                    "déguerpir du Chato! On va devoir s'en débarrasser!";
            _nom = "Jon";
        }

        public void Ninja()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Tu ne passera pas cet cour!\n" +
                    "Du moins tant que on est là.";

            
            _nom = "Ninja";
        }

        public void OuVasTu()
        {
            //_myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Mais où vas-tu ?\n" +
                    "La salle du couronnement est au nord.";
            _nom = "Jon";
        } 

        public void Jon4()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Des ninjas ici? Je savais qu'on avait attiré l'intention de\n" +
                    "pas mal de monde, mais j'savais pas à ce point là!" +
                    "Allons-y, et rappelle toi que chacun de nous possède des actions\n" +
                    "spéciale";
            _nom = "Jon";
        }

        public void Chest0()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Vous retrouvez votre épée !";
            _nom = "";
            _myGame._epee = true;
        }

        public void RPG()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Vous retrouvez... un lance missile!?;";
            _nom = "";
            _myGame._boom = true;
        }

        public void Chest1()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "Vous obtenez de la Akdov!;";
            _nom = "";

        }

        public void FermeBoite()
        {
            _dialTrue = false;
            _myGame.SetCoolDown();
        }

        public void Fin1()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _choiceTrue = true;
            _text = "Un lit décidemment très confortable. Voulez-vous\nvous rendormir?";
            _nom = " ";
        }

    

        public void SetCollision()
        {
            _myGame.mapLayer = _myGame._tiledMap.GetLayer<TiledMapTileLayer>("collision");
            _myGame.mapLayerDoor = _myGame._tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");
        }
        public void BoiteDialogues()
        {
            //float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState _keyboardState = Keyboard.GetState();
            u = _myGame.mapLayer.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight - 1)).GlobalIdentifier;
            d = _myGame.mapLayer.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight + 1)).GlobalIdentifier;
            l = _myGame.mapLayer.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth - 1), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight)).GlobalIdentifier;
            r = _myGame.mapLayer.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth + 1), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight)).GlobalIdentifier;
            dd = _myGame.mapLayerDoor.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight + 1)).GlobalIdentifier;
            ud = _myGame.mapLayerDoor.GetTile((ushort)(_myGame._positionPerso.X / _myGame._tiledMap.TileWidth), (ushort)(_myGame._positionPerso.Y / _myGame._tiledMap.TileHeight - 1)).GlobalIdentifier;

            Console.WriteLine("r = " + r);
            Console.WriteLine("u = " + u);
            Console.WriteLine("d = " + d);
            Console.WriteLine("l = " + l);
            Console.WriteLine("ud = " + ud);
            Console.WriteLine("dd = " + dd);
        }
    }
}
