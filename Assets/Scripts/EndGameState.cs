namespace SpaceInvaders {
    public class EndGameState : IGameState {

        public void Execute() {
            SIGameManager.Instance.AudioSource.pitch = 1.0f;
            PlayerController.Instance.IsGameReady = false;
            SIGameManager.Instance.IsPlayGame = false;
            SIGameManager.Instance.IsEndGame = true;
            SIGameManager.Instance.IsCheckEnemies = false;

            if (SIGameManager.Instance.IsWinner) {
                GUIManager.Instance.SetCanvasWinner(true);
            } else {
                GUIManager.Instance.SetCanvasEnd(true);
            }
        }
    }

}
