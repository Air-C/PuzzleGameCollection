using PGCRefactor.Enum;

namespace PGCRefactor.Until
{
    public class PGCUntil
    {
        public static PrefabEnum ToPrefabEnum(PrefabEnumPopup prefabEnumPopup)
        {
            return prefabEnumPopup switch
            {
                PrefabEnumPopup.PausePopup => PrefabEnum.PausePopup,
                PrefabEnumPopup.GameOverPopup => PrefabEnum.GameOverPopup,
                _ => throw new System.ArgumentOutOfRangeException(nameof(prefabEnumPopup), prefabEnumPopup, null)
            };
        }
    }
}