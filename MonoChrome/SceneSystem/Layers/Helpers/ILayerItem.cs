using System;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal interface ILayerItem
    {
        int ZIndex { get; set; }

        event EventHandler<ZIndexEventArgs> ZIndexChanged;
    }
}