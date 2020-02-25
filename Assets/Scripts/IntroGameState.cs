
namespace SpaceInvaders {
    public class IntroGameState : IGameState {
        public void Execute() {
            GUIManager.Instance.SetCanvasIntro(true);
        }
    }
}
