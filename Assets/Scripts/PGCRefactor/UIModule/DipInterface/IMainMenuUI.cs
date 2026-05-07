using System;
namespace PGCRefactor.UIModule.DipInterface
{
    public interface IMainMenuUI
    {
        void ShowMainMenu();
        void HideMainMenu();
        void SetStartGameAction(Action action);
    }
}