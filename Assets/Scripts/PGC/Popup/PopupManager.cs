using System.Collections.Generic;
using PGC.Enum;
using UnityEngine;

namespace PGC.Popup
{
    public class PopupManager
    {
        // private GameObject pause;
        private Dictionary<PopupEnum, GameObject> popups = new Dictionary<PopupEnum, GameObject>();
        
        private GameObject canvas;
        public PopupListener GetPopup(GameContext ctx, PopupEnum popupType)
        {
            if (popups.ContainsKey(popupType))
            {
                popups[popupType].SetActive(true);
                return popups[popupType].GetComponent<PopupListener>();
            }
            canvas = GameObject.FindWithTag("MainCanvas");
            Debug.Log($"PopupManager::GetPopup::{popupType}");
            ctx.assetModule.GetPopup(popupType, out var prefab);      
            popups.Add(popupType, Object.Instantiate(prefab, canvas.transform));
            return popups[popupType].GetComponent<PopupListener>();
        }

        public void Close(PopupEnum popupType)
        {
            popups[popupType].SetActive(false);
        }
    }
}