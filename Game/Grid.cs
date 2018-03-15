using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Grid
    {
        public Grid()
        {
            mFstBound = new Vector3(-100.0f, -100.0f, 0.0f);
            mSndBound = new Vector3(100.0f, 100.0f, 100.0f);
        }

        public Grid(Particle[] particles, LengthConstraint[] constraints)
        {
            mParticles = new List<Particle>(particles);
            mLengthConstraints = new List<LengthConstraint>(constraints);
        }

        public void TimeStep(float timeElapsed)
        {
            AccumulateForces();
            Verlet(timeElapsed);
            SatisfyConstraints();
        }

        private void Verlet(float timeElapsed)
        {
            for (int i = 0; i < mParticles.Count; ++i)
            {
                var x = mParticles[i].X;
                var a = mParticles[i].Acceleration;
                var temp = new Vector3(x.X, x.Y, x.Z);

                x += x - mParticles[i].PreviousX + a * timeElapsed * timeElapsed;
                mParticles[i].PreviousX = temp;
            }
        }

        private void SatisfyConstraints()
        {
            for (int j = 0; j < Iterations; ++j)
            {
                for (int i = 0; i < mParticles.Count; ++i)
                    mParticles[i].X = Vector3.Min(Vector3.Max(mParticles[i].X, mFstBound), mSndBound);

                foreach (var c in mLengthConstraints)
                {
                    var fst = mParticles[c.Pair.Item1].X;
                    var snd = mParticles[c.Pair.Item2].X;
                    var fstMass = mParticles[c.Pair.Item1].InvMass;
                    var sndMass = mParticles[c.Pair.Item2].InvMass;

                    var delta = snd - fst;
                    var deltaLen = delta.Length();
                    var diff = (deltaLen - c.Length) / (deltaLen * (fstMass + sndMass));

                    delta = fstMass * delta * diff;

                    fst += delta;
                    snd -= delta;
                }
            }
        }

        private void AccumulateForces()
        {
            for (int i = 0; i < mParticles.Count; ++i)
                mParticles[i].Acceleration = mGravity;
        }

        public IEnumerable<Vector3> Points { get { return from v in mParticles select v.X; } }

        private const int Iterations = 1;

        private Vector3 mFstBound;
        private Vector3 mSndBound;

        private List<LengthConstraint> mLengthConstraints;

        /*private List<Vector3> mX;
        private List<Vector3> mOldX;
        private List<Vector3> mA;
        private List<float> mInvMass;*/
        private List<Particle> mParticles;
        private Vector3 mGravity = -9.81f * Vector3.UnitZ;
    }
}
