using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{
    public static class MathUtils
    {
        public static int GetWeightRandom(Dictionary<int, int> weightMap)
        {
            int sumWeight = weightMap.Values.Sum();
            float randomWeight = new Random().Next(sumWeight);
        
            float prevValue = 0f;
            foreach (var weight in weightMap)
            {
                if (randomWeight > prevValue && randomWeight < prevValue + weight.Value)
                    return weight.Key;
		
                prevValue += weight.Value;
            }
            return weightMap.FirstOrDefault().Key;
        }
    }
}