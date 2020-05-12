using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    static class Time
    {
        public static GameTime GameTime { get; internal set; }
        public static double DeltaTime => GameTime.ElapsedGameTime.TotalSeconds;
    }
}
