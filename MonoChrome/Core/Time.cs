using Microsoft.Xna.Framework;

namespace MonoChrome.Core
{
    internal static class Time
    {
        public static GameTime GameTime { get; internal set; }
        public static double DeltaTime => GameTime.ElapsedGameTime.TotalSeconds;
    }
}
