using System;
using System.Collections.Generic;
using System.Numerics;
using Entities;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleItem.Model;
using PGC.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace PGC.System
{
    public class RenderSystem
    {
        PopupManager popupManager = new PopupManager();
        private GameContext ctx;

        public RenderSystem(GameContext ctx)
        {
            this.ctx = ctx;
        }
        
        
        public void ShowStartMenu()
        {
            StartMenuListener startMenuListener = ctx.startMenu.GetComponent<StartMenuListener>();
            Debug.Log("enter ShowStartMenu");
            if (startMenuListener != null && startMenuListener.isUnsetEvent)
            {
                Debug.Log("register ShowStartMenu");
                startMenuListener.SettingsButton.onClick.AddListener(ShowSettingsBar);
                startMenuListener.StartGameButton.onClick.AddListener(() => { ctx.eventBus.Publish(new StartGameEvent()); Debug.Log("click start");});
                startMenuListener.ExitGameButton.onClick.AddListener(() => { ctx.eventBus.Publish(new ExitGameEvent());});
                startMenuListener.MissionDropdown.onValueChanged.AddListener((value) => { ctx.eventBus.Publish(new MissionSetEvent(value));});
                startMenuListener.VolumeSlider.onValueChanged.AddListener((value) => { ctx.eventBus.Publish(new VolumeChangeEvent(value));});
                startMenuListener.VolumeSlider.value = 1;
                startMenuListener.BackGroundMusicToggle.onValueChanged.AddListener((value) => { ctx.eventBus.Publish(new BackGroundMusicSwitchEvent(value));});
                startMenuListener.isUnsetEvent = false;
            }
            ctx.startMenu.SetActive(true);
        }

        public void HideStartMenu()
        {
            ctx.startMenu.SetActive(false);
        }

        public void ShowSettingsBar()
        {
            ctx.settingsBar.SetActive(!ctx.settingsBar.activeSelf);
        }

        public void CloseStartMenu()
        {
            ctx.startMenu.SetActive(false);
        }
        
        public void ShowPausePopup()
        {
            Debug.Log("ShowPausePopup");
            PopupListener popup = popupManager.GetPopup(ctx, PopupEnum.PausePopup);
            Dictionary<string, Action> popupActions = new Dictionary<string, Action>();
            foreach (var button in popup.ButtonList)
            {
                Debug.Log($"register Action button:{button.name}");
                popupActions.Add(button.name, () =>
                {
                    ctx.gameSystemState.isRunning = true;
                    popup.gameObject.SetActive(false);
                });
            }
            popup.Init(popupActions);
        }
        
        public void ShowGameOverPopup()
        {
            PopupListener popup = popupManager.GetPopup(ctx, PopupEnum.GameOverPopup);
            Dictionary<string, Action> popupActions = new Dictionary<string, Action>();
            foreach (var button in popup.ButtonList)
            {
                Action onClick;
                if (button.name == ButtonEnum.RestartButton.ToString())
                {
                    onClick = () =>
                    {
                        ctx.eventBus.Publish(new ReInitGameEvent());
                        popup.gameObject.SetActive(false);
                    };
                }
                else if (button.name == ButtonEnum.ReturnButton.ToString())
                {
                    onClick = () =>
                    {
                        ctx.eventBus.Publish(new ReturnMenuEvent());
                        popup.gameObject.SetActive(false);
                    };
                }
                else
                {
                    onClick = () =>
                    {
                        ctx.gameSystemState.isRunning = true;
                        popup.gameObject.SetActive(false);
                    };
                }
                popupActions.Add(button.name, onClick);
            }
            popup.Init(popupActions);
        }
        
        public static void ReRenderSquarePosInGrid(GameContext ctx)
        {
            foreach (var square in ctx.grid.Grid)
            {
                if (square != null)
                {
                    Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X,square.Y));
                    square.SquareObj.transform.position = pos;
                }
            }
        }
        
        
        public static void ReRenderShapePos(GameContext ctx)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            if (ctx.newReachedSquare.Count > 0)
            {
                squares.AddRange(ctx.newReachedSquare);
            }
            if (squares.Count == 0)
            {
                return;
            }

            foreach (var square in squares)
            {
                Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X,square.Y));
                square.SquareObj.transform.position = pos;
            }
        }
        
        
        public void OnResetShapePreview(PreviewShapeChangeEvent e)
        {
            ctx.shapePreviewBar.GetComponent<Image>().sprite = e.shapePreviewSprite;
        }

        public void OnItemBarChanged(ItemBarChangeEvent e)
        {
            ItemBarModel waitForRenderBar = null;
            foreach (var itemBar in ctx.itemsBar)
            {
                if (itemBar.type == e.type)
                {
                    waitForRenderBar = itemBar;
                }
            }
            if (waitForRenderBar == null)
            {
                Debug.LogError($"ItemBarChanged event type:{e.type} not found");
                return;
            }

            if (waitForRenderBar.itemInstance == null)
            {
                waitForRenderBar.itemInstance = Object.Instantiate(e.itemPrefab, waitForRenderBar.itemBar.transform);
                waitForRenderBar.countInstance = waitForRenderBar.itemInstance.transform.GetChild(0);
                Button btn = waitForRenderBar.itemInstance.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    ctx.eventBus.Publish<ItemEffectEvent>(new ItemEffectEvent()
                    {
                        type = waitForRenderBar.type,
                    });
                });
            }
            else if (waitForRenderBar.count == 0)
            {
                Object.Destroy(waitForRenderBar.itemInstance.gameObject);
                waitForRenderBar.itemInstance = null;
                waitForRenderBar.countInstance = null;
                waitForRenderBar.type = ItemAbilityType.None;
                return;
            }
            TextMeshProUGUI tmp = waitForRenderBar.countInstance.GetComponent<TextMeshProUGUI>();
            tmp.text = waitForRenderBar.count.ToString();
            
        }
    }
}