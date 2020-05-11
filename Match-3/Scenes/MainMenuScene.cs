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
using Microsoft.Xna.Framework.Content;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.SceneSystem.Input;

namespace Match_3.Scenes
{
    class TestComponent : Component, IPointerClickHandler, IMouseOverHandler
    {

        [InsertComponent]
        private Transform _transform;

        public void OnMouseExit()
        {
            Console.WriteLine("Exit");
        }

        public void OnMouseOver()
        {
            Console.WriteLine("Over");
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            Console.WriteLine(pointerEventData.Position);
        }
    }


    class MainMenuScene : Scene
    {

        public override void Setup()
        {
            var s1 = Content.Load<Texture2D>("1");
            var s2 = Content.Load<Texture2D>("2");
            var f = Content.Load<SpriteFont>("font");
            var sprite1 = Entity.Create("Sprite1", 
                new BoxCollider2D(),
                new SpriteRenderer
                {
                    Texture = s1
                },
                new TextRenderer 
                { 
                    SpriteFont = f, 
                    Text = "Scor11111111111111111111111111111111111111111111111111111111111111111111111e",
                    Color = Color.Red
                });
            sprite1.Transform.Position = new Vector2(100, 100);
            var compose = Entity.Create("Compose", new BoxCollider2D(), new DebugRenderer(), new TestComponent());
            var sprite2 = Entity.Create("Sp2", new TextRenderer { SpriteFont = f, Text = "Hello" });
            sprite2.Transform.Position = new Vector2(10, 10);
            sprite2.Transform.Parent = compose.Transform;
            sprite1.Transform.Parent = compose.Transform;
            Entity.Synchronize();
            Add(compose);
        }
    }
}
