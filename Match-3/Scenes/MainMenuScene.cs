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
using MonoChrome.Core.Attributes;
using MonoChrome.SceneSystem.Layers;

namespace Match_3.Scenes
{
    class TestComponent2 : Component
    {
        public TestComponent testComponent;

        public int A;

        public TestComponent2()
        {

        }

        public TestComponent2(int a)
        {
            A = a;
        }

        private void OnDestroy()
        {
            Console.WriteLine("You11 " + A);
        }
    }

    class TestComponent3 : Component 
    {
        [InsertComponent(From = "Ho")]
        public TestComponent2 testetst;
    }

    class TestComponent : Component
    {
        [InsertComponent]
        public TestComponent2 comp;
        void Awake()
        {
            Console.WriteLine("Awake");
            Console.WriteLine(GameObject.Name);

        }

        void Update()
        {
            Console.WriteLine("Update");
        }

        void OnEnable()
        {
            Console.WriteLine("OnEnable");
        }

        void OnDisable()
        {
            Console.WriteLine("OnDisable");
        }

        void OnDestroy()
        {
            Console.WriteLine("OnDestroy");
        }

        void OnFinalise()
        {
            Console.WriteLine("OnFinalise");
        }
    }

    class MainMenuScene : Scene
    {
        public override void Setup()
        {
            var c = Entity.Create("Ho", new TestComponent2(1));
            var c1 = Entity.Create("Ho1", new TestComponent2(2));
            c.ZIndex = 1;
            c1.ZIndex = 0;
            Add(c, DefaultLayers.Background);
            Add(c1, DefaultLayers.Background);
            c1.ZIndex = 2;
            c.Enabled = false;
            c1.Enabled = false;
        }
    }
}
