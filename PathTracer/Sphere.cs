using System.Numerics;
using PathTracer.Surfaces;

namespace PathTracer
{
    public class Sphere
    {
        public Sphere(Vector3 position, float radius, ISurface surface, Vector3 emissionColor)
        {
            Position = position;
            Radius = radius;
            EmissionColor = emissionColor;
            Surface = surface;
        }

        public Vector3 Position { get; private set; }
        public float Radius { get; private set; }
        public Vector3 EmissionColor { get; private set; }
        private ISurface Surface { get; set; }
        public Vector3 GetColor(Vector3 n)
        {
            return Surface.GetTextureColor(n);
        }
    }
}