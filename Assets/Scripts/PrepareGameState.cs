
namespace SpaceInvaders {
    public class PrepareGameState : IGameState {
        public void Execute() {
            if ((!SIGameManager.Instance.IsPlayGame) && (!SIGameManager.Instance.IsEndGame)) {
                GUIManager.Instance.SetCanvasIntro(false);
                SIGameManager.Instance.PrepareGame();
                GUIManager.Instance.SetCanvasPlay(true);
            }
        }
    }
}
