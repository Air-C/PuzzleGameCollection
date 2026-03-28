using System;
using System.Collections.Generic;
using Entities;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleItem.Model;
using PGC.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
        
        public void ShowPausePopup()
        {
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
                        ctx.eventBus.Publish(new RestartGameEvent());
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
        
        public static void RenderDestroySquare(GameContext ctx)
        {
            if (ctx.squaresWaitForDestroy.squares.Count == 0)
            {
                return;
            }
            foreach (var square in ctx.squaresWaitForDestroy.squares)
            {
                Object.Destroy(square.SquareObj);
            }
            ctx.squaresWaitForDestroy.squares.Clear();
            foreach (var square in ctx.grid.Grid)
            {
                if (square != null)
                {
                    Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X,square.Y));
                    square.SquareObj.transform.position = pos;
                }
            }
        }
        
        
        public static void RenderShapePos(GameContext ctx)
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