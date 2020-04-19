using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.SceneSystem;
using MonoChrome.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Scenes
{
    class MainMenuScene : Scene
    {
        public override void Setup()
        {
            var e = Entity.Create("1", typeof(Renderer2D));
        }
    }
}
