using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders {

    public class InputManager : MonoBehaviour {

        public static InputManager _instance;

        private bool _isCanvasQuitShow = false;

        public static InputManager Instance {
            get {
                if (_instance == null) {
                    _instance = GameObject.FindObjectOfType<InputManager>();
                }

                return _instance;
            }
        }

        void Update() {

            if (Input.GetKeyDown(KeyCode.Q)) {
                GUIManager.Instance.SetCanvasQuit(true);
                Time.timeScale = 0;
                _isCanvasQuitShow = true;
            }

            if (Input.GetKeyDown(KeyCode.Y)) {
                if (_isCanvasQuitShow) {
                    Application.Quit();
                }
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                if (_isCanvasQuitShow) {
                    GUIManager.Instance.SetCanvasQuit(false);
                    Time.timeScale = 1;
                    _isCanvasQuitShow = false;
                }
            }
            if ((!_isCanvasQuitShow) && (!GUIManager.Instance.IsSplash)) {
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    SIGameManager.Instance.SetDifficultyLevel(DiffLevel.Novice);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    SIGameManager.Instance.SetDifficultyLevel(DiffLevel.Casual);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    SIGameManager.Instance.SetDifficultyLevel(DiffLevel.Master);
                }

                if (Input.GetKeyDown(KeyCode.P)) {
                    SIGameManager.Instance.OnPrepareGameState();
                }

                if (Input.GetKeyDown(KeyCode.R)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                if (Input.GetKey("d") || Input.GetKey("right")) {
                    if (PlayerController.Instance) {
                        PlayerController.Instance.GoRight();
                    }
                }

                if (Input.GetKey("a") || Input.GetKey("left")) {
                    if (PlayerController.Instance) {
                        PlayerController.Instance.GoLeft();
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    if (PlayerController.Instance) {
                        PlayerController.Instance.Shoot();
                    }
                }
            }

        }
    }

}
