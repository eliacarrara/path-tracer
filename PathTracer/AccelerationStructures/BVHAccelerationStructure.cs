using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.AccelerationStructures
{
    class BVHNode
    {
        public BVHNode(BVHNode l, BVHNode r, int sphere)
        {
            Left = l;
            Right = r;
            SphereIndex = sphere;
        }

        public BVHNode Left { get; private set; }
        public BVHNode Right { get; private set; }
        public int SphereIndex { get; private set; }
    }

    public class BVHAccelerationStructure : IAccelerationStructure
    {
        readonly BVHNode mRoot;
        readonly Sphere[] mSpheres;
        readonly int mNaturalSphereCount;

        public BVHAccelerationStructure(List<Sphere> spheres)
        {
            mSpheres = new Sphere[spheres.Count * 2];
            mNaturalSphereCount = spheres.Count;
            mRoot = Populate(spheres);
        }

        private BVHNode Populate(List<Sphere> spheres)
        {
            var nodes = new Dictionary<int, BVHNode>(spheres.Count);
            for (int i = 0; i < spheres.Count; i++)
            {
                nodes.Add(i, new BVHNode(null, null, i));
                mSpheres[i] = spheres[i];
            }

            var keyNodes = -1;
            var keySpheres = mNaturalSphereCount;
            while (nodes.Count > 1)
            {
                int nodeA = -1, nodeB = -1;
                int sphereA, sphereB;
                Sphere a, b;
                var minRadius = float.PositiveInfinity;

                var keys = nodes.Keys;
                var j = 0;
                foreach (var itr1 in keys)
                {
                    var i = 0;
                    sphereA = nodes[itr1].SphereIndex;
                    foreach (var itr2 in keys)
                    {
                        if (j <= i++)
                            break;
                        if (itr1 != itr2)
                        {
                            sphereB = nodes[itr2].SphereIndex;
                            a = mSpheres[sphereA];
                            b = mSpheres[sphereB];
                            float radius = (Vector3.Distance(a.Position, b.Position) + a.Radius + b.Radius) / 2;
                            if (radius < minRadius)
                            {
                                nodeA = itr1;
                                nodeB = itr2;
                                minRadius = radius;
                            }
                        }
                    }
                    j++;
                }

                sphereA = nodes[nodeA].SphereIndex;
                sphereB = nodes[nodeB].SphereIndex;
                a = mSpheres[sphereA];
                b = mSpheres[sphereB];

                Vector3 c = a.Position + Vector3.Normalize(b.Position - a.Position) * (minRadius - a.Radius);

                mSpheres[keySpheres] = new Sphere(c, minRadius, null, Vector3.Zero);
                nodes.Add(keyNodes, new BVHNode(nodes[nodeA], nodes[nodeB], keySpheres));

                nodes.Remove(nodeA);
                nodes.Remove(nodeB);

                keySpheres++;
                keyNodes--;
            }

            return nodes[keyNodes + 1];
        }

        private HitPoint GetHitPointR(Vector3 d, Vector3 OP, BVHNode node)
        {
            if (node.Left != null && node.Right != null)
            {
                float lambdaLeft = Helper.VectorSphereHitPoint(d, OP, mSpheres[node.Left.SphereIndex]);
                float lambdaRight = Helper.VectorSphereHitPoint(d, OP, mSpheres[node.Right.SphereIndex]);

                if (lambdaLeft == HitPoint.NoHitPoint.Lambda && lambdaRight == HitPoint.NoHitPoint.Lambda)
                {
                    return HitPoint.NoHitPoint;
                }
                else if (lambdaLeft != HitPoint.NoHitPoint.Lambda && lambdaRight != HitPoint.NoHitPoint.Lambda)
                {
                    HitPoint hpLeft = GetHitPointR(d, OP, node.Left);
                    HitPoint hpRight = GetHitPointR(d, OP, node.Right);

                    return (hpLeft.Lambda < hpRight.Lambda) ? hpLeft : hpRight;
                }
                else if (lambdaLeft != HitPoint.NoHitPoint.Lambda)
                {
                    return GetHitPointR(d, OP, node.Left);
                }
                else // lambdaRight != HitPoint.NoHitPoint.Lambda
                {
                    return GetHitPointR(d, OP, node.Right);
                }
            }
            else
            {
                Debug.Assert(node.SphereIndex < mNaturalSphereCount);
                float lambda = Helper.VectorSphereHitPoint(d, OP, mSpheres[node.SphereIndex]);

                if (lambda == HitPoint.NoHitPoint.Lambda)
                    return HitPoint.NoHitPoint;
                else
                    return new HitPoint(OP, lambda, d, node.SphereIndex);
            }
        }
        
        public HitPoint GetHitPoint(Vector3 d, Vector3 OP)
        {
            return GetHitPointR(d, OP, mRoot);
        }

        public Sphere this[int index]
        {
            get
            {
                Debug.Assert(index < mNaturalSphereCount);
                return mSpheres[index];
            }
        }
    }
}
