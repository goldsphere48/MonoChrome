﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    interface IZIndex
    {
        event EventHandler<EventArgs> ZIndexChanged;
        int ZIndex { get; set; }
    }
}
