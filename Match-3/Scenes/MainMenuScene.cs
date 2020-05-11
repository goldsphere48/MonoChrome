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

        private void OnCollision(Collision collision)
        {
            Console.WriteLine(collision.GameObject.Name);
        }
    }


    class MainMenuScene : Scene
    {

        public override void Setup()
        {
            var s = Content.Load<Texture2D>("1");
            var sprite1 = Entity.Create("Sprite1", new BoxCollider2D(), new SpriteRenderer(s), new TestComponent());
            sprite1.Transform.Position = new Vector2(100, 100);
            Entity.Synchronize();
            Add(sprite1);
        }
    }
}
