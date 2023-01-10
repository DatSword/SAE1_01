﻿using Microsoft.Xna.Framework;
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

    public class Event_et_dial : GameScreen
    {
        // défini dans Game1
        private new Game1 Game => (Game1)base.Game;
        private Game1 _myGame;

        public Event_et_dial(Game1 game) : base(game)
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

        //Boites de choix
        public static Texture2D _choiceBox;
        public static Vector2 _posChoiceBox;
        public static Texture2D _cursor;
        public static Vector2 _posCursor;
        public static String _yes;
        public static String _no;
        public static Vector2 _posYes;
        public static Vector2 _posNo;
        public static bool _choiceTrue;

        public static int u;
        public static int d;
        public static int l;
        public static int r;
        public static int ud;
        public static int dd;
        public Texture2D _choiceBox;
        public Vector2 _posChoiceBox;
        public Texture2D _cursor;
        public Vector2 _posCursor;
        public String _yes;
        public String _no;
        public Vector2 _posYes;
        public Vector2 _posNo;
        public bool _choiceTrue;


        public void toutDebut()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = "EH OH GAMIN, REVEIL - TOI! TU VAS M'FAIRE ATTENDRE\n" +
                          "ENCORE LONGTEMPS?!";
            _nom = "???";          
        }

        public void Fren1()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = ":)";
            _nom = "Fren";
            Game1._duck.Play();
        }
        public void Fren2()
        {
            _myGame.SetCoolDown();
            _dialTrue = true;
            _text = ":(";
            _nom = "Fren";
            Game1._duck.Play();
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
            Game1._firstvisit = false;
        }

        public void Jon3()
        {
            _myGame.SetCoolDown();
            _text = "Ah, rev'la des malfrats! J'croyais qu'on les avait fait tous\n" +
                    "déguerpir du Chato! On va devoir s'en débarrasser!";

            _nom = "Jon";
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
            _dialTrue = true;
        }

        public static void SetCollision()
        {
            Game1.mapLayer = Game1._tiledMap.GetLayer<TiledMapTileLayer>("collision");
            Game1.mapLayerDoor = Game1._tiledMap.GetLayer<TiledMapTileLayer>("element_interactif");
        }
        public static void BoiteDialogues()
        {
            //float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState _keyboardState = Keyboard.GetState();
            u = Game1.mapLayer.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight - 1)).GlobalIdentifier;
            d = Game1.mapLayer.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight + 1)).GlobalIdentifier;
            l = Game1.mapLayer.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth - 1), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight)).GlobalIdentifier;
            r = Game1.mapLayer.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth + 1), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight)).GlobalIdentifier;
            dd = Game1.mapLayerDoor.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight + 1)).GlobalIdentifier;
            ud = Game1.mapLayerDoor.GetTile((ushort)(Game1._positionPerso.X / Game1._tiledMap.TileWidth), (ushort)(Game1._positionPerso.Y / Game1._tiledMap.TileHeight - 1)).GlobalIdentifier;

            Console.WriteLine("r = " + r);
            Console.WriteLine("u = " + u);
            Console.WriteLine("d = " + d);
            Console.WriteLine("l = " + l);
            Console.WriteLine("ud = " + ud);
            Console.WriteLine("dd = " + dd);
        }

        public override void Update(GameTime gameTime) {   }

        public override void Draw(GameTime gameTime) {  }
    }
}
