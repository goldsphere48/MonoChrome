using System;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal interface ILayerItem
    {
        event EventHandler<ZIndexEventArgs> ZIndexChanged;
        int ZIndex { get; set; }
    }
}