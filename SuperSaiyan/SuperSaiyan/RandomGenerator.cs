using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSaiyan
{
    class RandomGenerator
    {
        #region Variable
        static Random random = new Random();
        #endregion

        #region Public Functions

        // Get a random value of Integer from 0 to max
        public static int Next(int max)
        {
            return random.Next(max);
        }

        // Get a random value of Float
        public static float NextFloat(float max)
        {
            return (float)random.NextDouble() * max;
        }
        #endregion
    }
}
