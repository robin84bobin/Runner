using UnityEngine;

namespace Common
{
    public static class Vector2Extension
    {
        public static Vector2 NormalizeToHorizontalDirection(this Vector2 value, float valueToNormalize = 1)
        {
            if (value == Vector2.zero)
                return value;
            Vector2 normalValue = value.normalized;
            if (normalValue.x >= 0.5f) return Vector2.right * valueToNormalize;
            if (normalValue.x < -0.5f) return Vector2.left * valueToNormalize;
            return Vector2.zero;
        }
        
        public static Vector2 NormalizeToRightAngleDirection(this Vector2 value)
        {
            if (value == Vector2.zero)
                return value;
            
            Vector2 normalValue = value.normalized;
            
            if (normalValue.y >= 0.5f) return Vector2.up;
            if (normalValue.y < -0.5f) return Vector2.down;
            if (normalValue.x >= 0.5f) return Vector2.right;
            if (normalValue.x < -0.5f) return Vector2.left;
            return Vector2.zero;
        }
    }
}