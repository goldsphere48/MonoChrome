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
using Microsoft.Xna.Framework.Audio;

namespace Match_3.Scenes
{
    class TestComponent : Component, IPointerClickHandler, IMouseOverHandler
    {

        [InsertComponent]
        private Transform _transform;
        [InsertGameObject("Sprite1")]
        private GameObject _sprite;

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
            _sprite.Transform.Parent = null;
            Scene.Remove(_sprite);
        }

        private void Update()
        {
            //_transform.Position += new Vector2(5, 0);
        }
    }


    class MainMenuScene : Scene
    {

        public override void Setup()
        {
            var s1 = Content.Load<Texture2D>("1");
            var s2 = Content.Load<Texture2D>("2");
            var f = Content.Load<SpriteFont>("font");
            var ss = new AudioPlayer
            {
                Source = Content.Load<SoundEffect>("3"),
                IsLooped = true
            };
            var sprite1 = Entity.Create("Sprite1",
                new BoxCollider2D(),
                new SpriteRenderer
                {
                    Texture = s1
                }, new DebugRenderer());
            sprite1.Transform.Position = new Vector2(100, 100);
            var compose = Entity.Create("Compose", new BoxCollider2D(100, 100), new DebugRenderer(), new TestComponent(), 
                ss
                );
            var sprite2 = Entity.Create("Sp2", new TextRenderer { SpriteFont = f, Text = "Hello" }, new BoxCollider2D(100, 100), new DebugRenderer());
            compose.Transform.Position = new Vector2(40, 40);
            sprite2.Transform.Position = new Vector2(10, 10);
            sprite2.Transform.Parent = compose.Transform;
            sprite1.Transform.Parent = compose.Transform;
            Entity.Synchronize();
            Add(compose);
            ss.IsLooped = false;
            ss.Play();
        }
    }
}
