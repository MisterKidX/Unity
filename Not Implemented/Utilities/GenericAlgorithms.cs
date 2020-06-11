using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;
using System;

namespace Utilities
{
    public static class GenericAlgorithms
    {
        /// <summary>
        /// This algorithm will set two variables who are codependant on wach other, each having the capacity and priority.
        /// The priority resembles a stack behaviour
        /// </summary>
        /// <param name="x">the first variable, with lower priority (will reduce points from first, and add last)</param>
        /// <param name="xCapacity">the capacity for the first parameter</param>
        /// <param name="y">the second variable, with higher priority (will reduce points from last, and add first)</param>
        /// <param name="yCapcity">the capacity for the second parameter</param>
        /// <param name="amountToAdd">the amount to add to the comined capacities. Note that if the amount exceeds the total capacity of the two parameters, the method will return the max value of both</param>
        public static void SetDuoIntermingledParameters(ref int x, int xCapacity, ref int y, int yCapcity, int amountToAdd)
        {
            for (int i = 0; i < Math.Abs(amountToAdd); i++)
            {
                var newValue = Mathf.Clamp(amountToAdd, -1, 1);
                //subtraction
                if (newValue > 0)
                {
                    if (x > 0)
                    {
                        x -= newValue;
                    }
                    else
                    {
                        y -= newValue;
                    }
                }
                //addition
                else
                {
                    if (y < yCapcity)
                    {
                        y -= newValue;
                    }
                    else
                    {
                        x -= newValue;
                    }
                }
            }
        }
    }
}

