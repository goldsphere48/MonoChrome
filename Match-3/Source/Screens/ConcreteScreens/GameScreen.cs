﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    class GameScreen : Screen
    {
        private static Screen _instance;
        public static Screen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameScreen();
                }
                return _instance;
            }
        }

        public override void Awake()
        {
            Console.WriteLine("Awake game");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {

        }

        public override void OnEnable()
        {
            Console.WriteLine("Enabale game");
        }

        public override void OnDisable()
        {
            Console.WriteLine("Disable game");
        }
    }
}
