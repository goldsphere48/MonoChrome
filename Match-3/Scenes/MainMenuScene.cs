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
            var compose = Entity.Compose("Root",
                    Entity.Create(),
                    Entity.Create(),
                    Entity.Create(),
                    Entity.Compose(
                        Entity.Create(),
                        Entity.Create(),
                        Entity.Create()
                    )
            );
            Add(compose);
        }
    }
}
