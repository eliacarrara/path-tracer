using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.Surfaces
{
    class TextureSurface : ISurface
    {
        readonly Vector3[] texture = null;
        readonly int width = 0;
        readonly int height = 0;
        public TextureSurface(Bitmap bitmap, float colorCorrectionFactor = 1.0f)
        {
            if (bitmap != null)
            {
                width = bitmap.Width;
                height = bitmap.Height;
                texture = Helper.LoadBitmap(bitmap, colorCorrectionFactor);
            }
        }
        public Vector3 GetTextureColor(Vector3 n)
        {
            if (texture == null)
                return Vector3.Zero;

            float s = (float)((Math.Atan2(n.X, n.Z) + Math.PI) / (2 * Math.PI));
            float t = (float)(Math.Acos(n.Y) / Math.PI);

            Debug.Assert(s >= 0.0f && s <= 1.0f);
            Debug.Assert(t >= 0.0f && t <= 1.0f);

            int x = (int)((width - 1) * s);
            int y = (int)((height - 1) * t);

            return texture[y * width + x];
        }
    }
}
