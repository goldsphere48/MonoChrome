using Microsoft.Xna.Framework;

namespace MonoChrome.Core
{
    public static class Time
    {
        public static double DeltaTime => GameTime.ElapsedGameTime.TotalSeconds;
        public static GameTime GameTime { get; internal set; }
        public static float TimeDelation { get; set; }
    }
}