using UnityEngine;

namespace PGC.ModuleEasing
{
    public class Easing
    {
        public static float EaseInCubic(float target, float origin, float t)
        {
            return origin + (target - origin) * (t * t * t);
        }
        
        public static float EaseInOutElastic(float target, float origin, float t)
        {
            float c5 = (2 * Mathf.PI) / 4.5f;
            float value = t == 0 
                ? 0 
                : t==1 
                    ? 1 
                    : t < 0.5 
                        ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
            
            return origin + (target - origin) * value;
        }
        
        public static float EaseOutElastic(float target, float origin, float t)
        {
            float c4 = (2 * Mathf.PI) / 3;
            float value = t == 0 
                ? 0 
                : t==1 
                    ? 1 
                    : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
            return origin + (target - origin) * value;
        }
    }
}