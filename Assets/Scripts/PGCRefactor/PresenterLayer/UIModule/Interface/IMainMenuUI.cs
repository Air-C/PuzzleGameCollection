using System;
namespace PGCRefactor.PresenterLayer.UIModule.Interface
{
    public interface IMainMenuUI
    {
        void ShowMainMenu();
        void HideMainMenu();
        void SetStartGameAction(Action action);
    }
}