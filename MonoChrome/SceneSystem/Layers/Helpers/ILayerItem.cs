using System;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    interface ILayerItem
    {
        event EventHandler<ZIndexEventArgs> ZIndexChanged;
        int ZIndex { get; set; }
    }
}
