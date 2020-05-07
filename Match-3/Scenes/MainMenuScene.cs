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

        private void Awake()
        {
            Console.WriteLine("You");
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
            var c = Entity.Create("Ho");
            c.AddComponent(new TestComponent2());
            var d = new TestComponent();
            Add(c);
            c.Enabled = false;
        }
    }
}
