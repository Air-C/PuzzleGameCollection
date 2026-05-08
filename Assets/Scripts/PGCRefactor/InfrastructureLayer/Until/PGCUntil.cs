using System;
using PGCRefactor.Enum;

namespace PGCRefactor.AssetsLoadModule.Until
{
    public class PGCUntil
    {
        public static PrefabEnum ToPrefabEnum(PrefabEnumPopup prefabEnumPopup)
        {
            return prefabEnumPopup switch
            {
                PrefabEnumPopup.PausePopup => PrefabEnum.PausePopup,
                PrefabEnumPopup.GameOverPopup => PrefabEnum.GameOverPopup,
                _ => throw new ArgumentOutOfRangeException(nameof(prefabEnumPopup), prefabEnumPopup, null)
            };
        }
    }
}