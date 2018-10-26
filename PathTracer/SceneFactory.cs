using PathTracer.AccelerationStructures;
using PathTracer.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    class SceneFactory
    {
        public static Scene CreateThisThing()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(0, 0, 0), 1, new TextureSurface(Properties.Resources.cmbr), new Vector3(0.01f)),
                new Sphere(new Vector3(0, 1001f, 0), 1000, new SingleColorSurface(new Vector3(0.7f, 0.7f, 0.7f)), Vector3.Zero), // bottom
                new Sphere(new Vector3(0, 0, 1001), 1000, new SingleColorSurface(new Vector3(1, 1, 1)), Vector3.Zero), // back
                new Sphere(new Vector3(5, -5, -5), 2.5f, new SingleColorSurface(Vector3.One), Vector3.One) // light
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(-2f, -2f, -4);
            var lookAt = new Vector3(0, 0, 0);
            var fov = Helper.DegToRad(40);
            var baseColor = Vector3.Zero;
            return new Scene(new Camera(up, eye, lookAt, fov), new NoAcceleration(spheres));
        }

        public static Scene CreateCornellBoxScene()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-1001, 0, 0), 1000, new SingleColorSurface(new Vector3(1, 0, 0), 0.7f), Vector3.Zero), // left
                new Sphere(new Vector3(1001, 0, 0), 1000, new SingleColorSurface(new Vector3(0, 0, 1), 0.7f), Vector3.Zero), // right
                new Sphere(new Vector3(0, 0, 1001), 1000, new SingleColorSurface(new Vector3(1, 1, 1), 0.9f), Vector3.Zero), // back
                new Sphere(new Vector3(0, -1001, 0), 1000, new SingleColorSurface(new Vector3(1, 1, 1), 0.9f), Vector3.Zero), // top
                new Sphere(new Vector3(0, 1001, 0), 1000, new SingleColorSurface(new Vector3(1, 1, 1), 0.9f), Vector3.Zero), // bottom
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, new SingleColorSurface(new Vector3(1, 1, 0), 0.9f), Vector3.Zero),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, new SingleColorSurface(new Vector3(1, 1, 1), 0.9f), Vector3.Zero),
                new Sphere(new Vector3(0, -15, 0), 14.005f, new SingleColorSurface(Vector3.One), Vector3.One) // Light
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);

            return new Scene(new Camera(up, eye, lookAt, fov), new NoAcceleration(spheres));
        }

        public static Scene CreateCornellBoxSceneTexture()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-1001, 0, 0), 1000, new SingleColorSurface(new Vector3(1, 0, 0), 0.8f), Vector3.Zero), // left
                new Sphere(new Vector3(1001, 0, 0), 1000, new SingleColorSurface(new Vector3(0, 0, 1), 0.8f), Vector3.Zero), // right
                new Sphere(new Vector3(0, 0, 1001), 1000, new SingleColorSurface(Vector3.One, 0.8f), Vector3.Zero), // back
                new Sphere(new Vector3(0, -1001, 0), 1000, new SingleColorSurface(Vector3.One, 0.8f), Vector3.Zero), // top
                new Sphere(new Vector3(0, 1001, 0), 1000, new SingleColorSurface(Vector3.One, 0.8f), Vector3.Zero), // bottom
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, new TextureSurface(Properties.Resources.moon, 0.8f), Vector3.Zero),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, new TextureSurface(Properties.Resources.cmbr, 0.8f), Vector3.Zero),
                new Sphere(new Vector3(0, -15, 0), 14.005f, new SingleColorSurface(Vector3.One), Vector3.One) // Light
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);

            return new Scene(new Camera(up, eye, lookAt, fov), new NoAcceleration(spheres));
        }

        public static Scene CreateRandomSphereScene(int count, float size)
        {
            Random r = new Random();
            var spheres = new List<Sphere>();

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4.5f);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1);
                Vector3 color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                spheres.Add(new Sphere(pos, size, new SingleColorSurface(color), Vector3.Zero));
            }

            spheres.Add(new Sphere(new Vector3(0, -4, -1), 0.5f, new SingleColorSurface(Vector3.One), Vector3.One));

            return new Scene(new Camera(up, eye, lookAt, fov), new BVHAccelerationStructure(spheres));
        }
    }
}
