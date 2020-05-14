using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public struct PolygonCollisionResult
    {
        // Are the polygons going to intersect forward in time?
        public bool WillIntersect;
        // Are the polygons currently intersecting?
        public bool Intersect;
        // The translation to apply to the first polygon to push the polygons apart.
        public Vector2 MinimumTranslationVector;
    }
}
