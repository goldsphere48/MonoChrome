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
        public int a = 2;

        public TestComponent2()
        {

        }

        public TestComponent2(int aa)
        {
            a = aa;
        }

        private void Awake()
        {
            Console.WriteLine("You");
        }
    }

    class TestComponent : Component
    {
        [InsertComponent]
        TestComponent2 comp;
        void Awake()
        {
            Console.WriteLine("Awake");
            Console.WriteLine(comp.a);
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
            Entity.Define("A", typeof(TestComponent2), typeof(TestComponent));
            var a = Entity.CreateFromDefinition("A", "Ho");
            var b = Entity.CreateFromDefinition("A", "Ho");
            var c = Entity.CreateFromDefinition("A", "Ho");
            c.Transform.Parent = b.Transform;
            b.Transform.Parent = a.Transform;
            Add(a);
            a.Enabled = false;
        }
    }
}
