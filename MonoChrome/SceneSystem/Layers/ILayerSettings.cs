namespace MonoChrome.SceneSystem.Layers
{
    public interface ILayerSettings
    {
        bool AllowThroughHandling { get; set; }
        bool CollisionDetectionEnable { get; set; }
        bool Enabled { get; set; }
        bool HandleInput { get; set; }
        string Name { get; }
        bool Visible { get; set; }
        int ZIndex { get; set; }
    }
}