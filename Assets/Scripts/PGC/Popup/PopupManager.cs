using PGC.Enum;
using UnityEngine;

namespace PGC.Popup
{
    public class PopupManager
    {
        private GameObject pause;
        private GameObject canvas;
        public PopupListener GetPopup(GameContext ctx, PopupEnum popupType)
        {
            if (pause != null)
            {
                pause.SetActive(true);
                return pause.GetComponent<PopupListener>();
            }
            canvas = GameObject.FindWithTag("MainCanvas");
            ctx.assetModule.GetPopup(popupType, out var prefab);            
            pause =  Object.Instantiate(prefab, canvas.transform);
            return pause.GetComponent<PopupListener>();
        }

        public void Close()
        {
            pause.SetActive(false);
        }
    }
}