using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Scenes
{
    class MainMenuScene : Scene
    {
        public override void Awake()
        {
            Console.WriteLine("Awake main");
        }

        public override void Update()
        {

        }

        public override void OnEnable()
        {
            Console.WriteLine("Enabale main");
        }

        public override void OnDisable()
        {
            Console.WriteLine("Disable main");
        }

    }
}
