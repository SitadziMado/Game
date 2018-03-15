using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
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
}
