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

    internal class Event_et_dial
    {
        public static Texture2D _dialBox;
        public static Vector2 _posDialBox;
        public static String _text;
        public static Vector2 _posText;
        public static String _nom;
        public static Vector2 _posNom;
        public static bool _dialTrue;

        public static void toutDebut()
        {
            Game1.SetCoolDown();
            _dialTrue = true;
            _text = "EH OH GAMIN, REVEIL - TOI! TU VAS M'FAIRE ATTENDRE\n" +
                          "ENCORE LONGTEMPS?!";
            _nom = "???";          
        }

        public static void Fren1()
        {
            Game1.SetCoolDown();
            _dialTrue = true;
            _text = ":)";
            _nom = "Fren";
            _dialTrue = true;
            Game1._duck.Play();
        }
        public static void Fren2()
        {
            Game1.SetCoolDown();
            _dialTrue = true;
            _text = ":(";
            _nom = "Fren";
            _dialTrue = true;
            Game1._duck.Play();
        }

        public static void Jon1()
        {
            Game1.SetCoolDown();
            _dialTrue = true;
            _text = "Ah voilà, enfin réveillé, désolé d'avoir hurler mais\n" +
                    "tout le monde est déjà parti vers la salle du trône!\n" +
                    "Je comprend ta fatigue, mais ça serait dommage de ne pas\n" +
                    "assister au courronnement, on a un peu beaucoup galéré\n" +
                    "pour ce moment!";
            _nom = "Jon";
        }

        public static void Jon2()
        {
            Game1.SetCoolDown();
            _text = "J't'attend dans le couloir donc récupère vite tes affaires\n" +
                    ",ou j'vais croire que tu as décidé de prolonger ta nuit!";
            _nom = "Jon";
            Game1._firstvisit = false;
        }

        public static void Jon3()
        {
            Game1.SetCoolDown();
            _text = "Ah, rev'la des malfrats! J'croyais qu'on les avait fait tous\n" +
                    "déguerpir du Chato! On va devoir s'en débarrasser!";

            _nom = "Jon";
        }
        public static void FermeBoite()
        {
            _dialTrue = false;
            Game1.SetCoolDown();
        }

    }
}