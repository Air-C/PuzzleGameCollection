using System;
using System.Collections.Generic;
using Custom.Tool;
using Entities;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.Pool;
using PGC.Popup;
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
            if (ctx.SquaresWaitForDestory.squares.Count == 0)
            {
                return;
            }
            foreach (var square in ctx.SquaresWaitForDestory.squares)
            {
                Object.Destroy(square.SquareObj);
            }

            ctx.SquaresWaitForDestory.isRender = true;
            if (ctx.SquaresWaitForDestory.AllIsCheck())
            {
                ctx.SquaresWaitForDestory.squares.Clear();
            }

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
            if (ctx.NewReacdhedSquare.Count > 0)
            {
                squares.AddRange(ctx.NewReacdhedSquare);
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
    }
}