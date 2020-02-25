
namespace SpaceInvaders {
    public class PlayGameState : IGameState {
        public void Execute() {
            GUIManager.Instance.SetCanvasPlay(false);
            PlayerController.Instance.IsGameReady = true;
            SIGameManager.Instance.PlayGame();
        }
    }
}
