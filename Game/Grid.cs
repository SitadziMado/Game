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
        struct LengthConstraint
        {
            public LengthConstraint(Tuple<int, int> pair, float length)
            {
                Pair = pair;
                Length = length;
            }

            public Tuple<int, int> Pair { get; set; }
            public float Length { get; set; }
        }

        public void TimeStep(float timeElapsed)
        {
            AccumulateForces();
            Verlet(timeElapsed);
            SatisfyConstraints();
        }

        private void Verlet(float timeElapsed)
        {
            for (int i = 0; i < mX.Count; ++i)
            {
                var x = mX[i];
                var a = mA[i];
                var temp = new Vector3(x.X, x.Y, x.Z);

                x += x - mOldX[i] + a * timeElapsed * timeElapsed;
                mOldX[i] = temp;
            }
        }

        private void SatisfyConstraints()
        {
            for (int j = 0; j < Iterations; ++j)
            {
                for (int i = 0; i < mX.Count; ++i)
                    mX[i] = Vector3.Min(Vector3.Max(mX[i], mFstBound), mSndBound);

                foreach (var c in mLengthConstraints)
                {
                    var fst = mX[c.Pair.Item1];
                    var snd = mX[c.Pair.Item2];
                    var fstMass = mInvMass[c.Pair.Item1];
                    var sndMass = mInvMass[c.Pair.Item2];

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
            for (int i = 0; i < mA.Count; ++i)
                mA[i] = mGravity;
        }

        private const int Iterations = 1;

        private Vector3 mFstBound;
        private Vector3 mSndBound;

        private List<LengthConstraint> mLengthConstraints;

        private List<Vector3> mX;
        private List<Vector3> mOldX;
        private List<Vector3> mA;
        private List<float> mInvMass;
        private Vector3 mGravity = -9.81f * Vector3.UnitZ;
    }
}
