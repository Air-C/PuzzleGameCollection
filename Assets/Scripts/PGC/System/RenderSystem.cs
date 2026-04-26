using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Entities;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleEasing;
using PGC.ModuleEasing.Enum;
using PGC.ModuleItem.Model;
using PGC.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
            if (startMenuListener != null && startMenuListener.isUnsetEvent)
            {
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
            if (ctx.settingsBar.activeSelf)
            {
                ctx.settingsBar.transform.DOScale(1f, 0.5f);
            }
            else
            {
                ctx.settingsBar.transform.DOScale(1.5f, 0.5f);
            }
            ctx.settingsBar.SetActive(!ctx.settingsBar.activeSelf);
        }

        public void CloseStartMenu()
        {
            ctx.startMenu.SetActive(false);
        }
        
        public void ShowPausePopup()
        {
            PopupListener popup = popupManager.GetPopup(ctx, PopupEnum.PausePopup);
            popup.transform.DOScale(1.5f, 0.5f);
            RegisterButtonOnClick(popup);
        }
        
        public void ShowGameOverPopup()
        {
            PopupListener popup = popupManager.GetPopup(ctx, PopupEnum.GameOverPopup);
            popup.transform.DOScale(1.2f, 0.5f);
            RegisterButtonOnClick(popup);
        }

        // 新按钮添加在此添加注册方法
        private void RegisterButtonOnClick(PopupListener popup)
        {
            Dictionary<string, Action> popupActions = new Dictionary<string, Action>();
            Action onClick;
            foreach (var button in popup.ButtonList)
            {
                if (button.name == ButtonEnum.ExitButton.ToString())
                {
                    onClick = () =>
                    {
                        ctx.eventBus.Publish(new ReturnMenuEvent());
                        popup.gameObject.SetActive(false);
                        popup.transform.DOScale(1f, 0.5f);
                    };
                }
                else if (button.name == ButtonEnum.RestartButton.ToString())
                {
                    onClick = () =>
                    {
                        ctx.eventBus.Publish(new ReInitGameEvent());
                        popup.gameObject.SetActive(false);
                        popup.transform.DOScale(1f, 0.5f);
                    };
                }
                else
                {
                    onClick = () =>
                    {
                        ctx.gameSystemState.isRunning = true;
                        popup.gameObject.SetActive(false);
                        popup.transform.DOScale(1f, 0.5f);
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