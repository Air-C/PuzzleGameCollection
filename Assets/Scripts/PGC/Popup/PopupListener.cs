using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PGC.Popup
{
    public class PopupListener : MonoBehaviour
    {
        [SerializeField]
        private List<Button> buttonList = new ();

        private Dictionary<string, Action> onClickActions = new Dictionary<string, Action>();

        public List<Button> ButtonList
        {
            get => buttonList;
            set => buttonList = value;
        }

        public void Init(Dictionary<string, Action> actions)
        {
            onClickActions = actions;
            
            foreach (var button in buttonList)
            {
                if (onClickActions.ContainsKey(button.name))
                {
                    button.onClick.AddListener(() =>
                    {
                        onClickActions[button.name].Invoke();
                    });
                }
            }
        }

        private void Awake()
        {
            
        }

    }
}