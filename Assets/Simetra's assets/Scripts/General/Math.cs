using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimetraCustomLib
{
    namespace Math
    {
        /// <summary>
        /// Useful class to return random values
        /// </summary>
        public static class Randomizer
        {
            /// <summary>
            /// Returns random boolean value
            /// </summary>
            public static bool RandomBoolean()
            {
                return UnityEngine.Random.Range(1, 3) == 1 ? true : false;
            }

            /// <summary>
            /// Returns a random rotation (quaternion)
            /// </summary>
            public static Quaternion RandomRotation()
            {
                Vector3 eulerRotation = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.
                    Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

                return Quaternion.Euler(eulerRotation);
            }

            /// <summary>
            /// Returns random sign (+1 or -1)
            /// </summary>
            public static int RandomSign()
            {
                return UnityEngine.Random.Range(1, 3) == 1 ? 1 : -1;
            }

            public static int ScatteredRanges()
            {
                return 0;
            }

            /// <summary>
            /// Throws dice and returns results as an array
            /// </summary>
            /// <param name="diceCount">Number of dice to be thrown</param>
            public static int[] ThrowDice(int diceCount)
            {
                if (diceCount <= 0)
                    return null;

                int[] results = new int[diceCount];

                for (int i = 0; i < diceCount; i++)
                    results[i] = UnityEngine.Random.Range(1, 7);

                return results;
            }
        }
    }
}