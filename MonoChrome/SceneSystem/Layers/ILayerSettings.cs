using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers
{
    public interface ILayerSettings
    {
        bool AllowThroughHandling { get; set; }
        bool CollisionDetectionEnable { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }
        bool HandleInput { get; set; }
        string Name { get; }
        int ZIndex { get; set; }
    }
}
