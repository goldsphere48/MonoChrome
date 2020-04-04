using Match_3.Source.Core.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Scenes
{
    class GameScene : Scene
    {
        public override void Awake()
        {
            Console.WriteLine("Awake game");
        }

        public override void Update()
        {

        }

        public override void OnEnable()
        {
            Console.WriteLine("Enabale game");
        }

        public override void OnDisable()
        {
            Console.WriteLine("Disable game");
        }
    }
}
