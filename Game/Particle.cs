using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Particle
    {
        public Particle(Vector3 x, float invMass)
        {
            X = x;
            PreviousX = x;
            Acceleration = Vector3.Zero;
            InvMass = invMass;
        }

        public Particle(Vector3 x, Vector3 a, float invMass)
        {
            X = x;
            PreviousX = x;
            Acceleration = a;
            InvMass = invMass;
        }

        public Vector3 X { get; set; }
        public Vector3 PreviousX { get; set; }
        public Vector3 Acceleration { get; set; }
        public float InvMass { get; set; }
    }
}
