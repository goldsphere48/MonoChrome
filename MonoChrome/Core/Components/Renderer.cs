﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using MonoChrome.Core.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components
{
    public abstract class Renderer : Component
    {
        public Color Color { get; set; } = Color.White;
        public abstract Vector2 Size { get; }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
