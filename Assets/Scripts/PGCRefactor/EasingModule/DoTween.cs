using System.Collections;
using PGC.ModuleEasing.Enum;
using UnityEngine;

namespace PGC.EasingModule
{
    public class DoTween
    {
        
        public static IEnumerator ScaleTween(Transform targetTransform,float scaleFactor, Vector3 endScale, float duration = 1, EaseEnum easeType = EaseEnum.EaseOutElastic)
        {
            float time = 0f;
            Vector3 initScale = CalculateInitScale(scaleFactor, endScale);
            targetTransform.localScale = initScale;
            targetTransform.gameObject.SetActive(true);
            while (time <= duration)
            {
                float x = ChooseEase2Exec(easeType, endScale.x, initScale.x, time / duration);
                float y = ChooseEase2Exec(easeType, endScale.y, initScale.y, time / duration);
                targetTransform.localScale = new Vector3(x, y, endScale.z);
                time += Time.deltaTime;
                yield return null;
            }
        }

        static Vector3 CalculateInitScale(float scaleFactor, Vector3 endScale)
        {
            return new Vector3(endScale.x * scaleFactor, endScale.y* scaleFactor, endScale.z* scaleFactor);
        }

        static float ChooseEase2Exec(EaseEnum easeType, float startValue, float endValue, float duration)
        {
            switch(easeType)
            {
                case EaseEnum.EaseInCubic:
                    return Easing.EaseInCubic(startValue, endValue, duration);
                case EaseEnum.EaseInOutElastic:
                    return Easing.EaseInOutElastic(startValue, endValue, duration);
                case EaseEnum.EaseOutElastic:
                    return Easing.EaseOutElastic(startValue, endValue, duration);
                default:
                    return Easing.EaseOutElastic(startValue, endValue, duration);
            }
        }
    }
}