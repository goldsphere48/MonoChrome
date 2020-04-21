using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.SceneSystem;
using MonoChrome.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoChrome.Core;
using MonoChrome.Core.EntityManager;

namespace Match_3.Scenes
{
    class TestComponent : Component
    {
        void Awake()
        {
            Console.WriteLine("Awake");
        }

        void Update()
        {
            Console.WriteLine("Update");
        }
    }
    class MainMenuScene : Scene
    {
        public override void Setup()
        {
            var e = Entity.Create("1", new TestComponent());
        }
    }
}
