using UnityEngine;

namespace Core.Common
{
    public static class Vector2Extension
    {
        /// <summary>
        /// Convert vector to horizontal vector   
        /// </summary>
        /// <param name="value">source value</param>
        /// <param name="valueToNormalize"> result vector length</param>
        /// <param name="threshold">value that define if vector angle value enough to convert to horizontal</param>
        /// <returns></returns>
        public static Vector2 NormalizeToHorizontalDirection(
            this Vector2 value, 
            float valueToNormalize = 1, 
            float threshold = 0.5f)
        {
            if (value == Vector2.zero)
                return value;
            Vector2 normalValue = value.normalized;
            if (normalValue.x >= threshold) return Vector2.right * valueToNormalize;
            if (normalValue.x < -threshold) return Vector2.left * valueToNormalize;
            return Vector2.zero;
        }
        
        /// <summary>
        /// Convert vector to right angle vector 
        /// </summary>
        /// <param name="value">source value</param>
        /// <param name="threshold">value that define if vector angle value enough to convert</param>
        /// <returns></returns>
        public static Vector2 NormalizeToRightAngleDirection(this Vector2 value, float threshold = 0.5f)
        {
            if (value == Vector2.zero)
                return value;
            
            Vector2 normalValue = value.normalized;
            
            if (normalValue.y >= threshold) return Vector2.up;
            if (normalValue.y < -threshold) return Vector2.down;
            if (normalValue.x >= threshold) return Vector2.right;
            if (normalValue.x < -threshold) return Vector2.left;
            return Vector2.zero;
        }
    }
}