using System.Collections;
using UnityEngine;

namespace SpaceInvaders {

    public class GUIManager : MonoBehaviour {

        private static GUIManager _instance;

        public GameObject canvasSplash;
        public GameObject canvasIntro;
        public GameObject canvasPlay;
        public GameObject canvasEnd;
        public GameObject canvasWinner;
        public GameObject canvasQuit;

        public static GUIManager Instance {
            get {
                if (_instance == null) {
                    _instance = GameObject.FindObjectOfType<GUIManager>();
                }

                return _instance;
            }
        }

        public bool IsSplash { get; set; } = false;

        void Start() {
            
            SetCanvasIntro(false);
            SetCanvasEnd(false);
            SetCanvasPlay(false);
            SetCanvasWinner(false);
            StartCoroutine(ShowSplashCor());
        }

        IEnumerator ShowSplashCor() {
            IsSplash = true;
            SetCanvasSplash(true);
            yield return new WaitForSeconds(1.0f);
            SetCanvasSplash(false);
            SetCanvasIntro(true);
            IsSplash = false;
        }

        public void SetCanvasSplash(bool isCanvasSplashShown) {
            if (isCanvasSplashShown) {
                canvasSplash.SetActive(true);
            } else {
                canvasSplash.SetActive(false);
            }
        }

        public void SetCanvasIntro(bool isCanvasIntroShown) {
            if (isCanvasIntroShown) {
                canvasIntro.SetActive(true);
            } else {
                canvasIntro.SetActive(false);
            }
        }

        public void SetCanvasPlay(bool isCanvasPlayShown) {
            if (isCanvasPlayShown) {
                canvasPlay.SetActive(true);
            } else {
                canvasPlay.SetActive(false);
            }
        }

        public void SetCanvasEnd(bool isCanvasEndShown) {
            if (isCanvasEndShown) {
                canvasEnd.SetActive(true);
            } else {
                canvasEnd.SetActive(false);
            }
        }

        public void SetCanvasWinner(bool isCanvasWinnerShown) {
            if (isCanvasWinnerShown) {
                canvasWinner.SetActive(true);
            } else {
                canvasWinner.SetActive(false);
            }
        }

        public void SetCanvasQuit(bool isCanvasQuitShown) {
            if (isCanvasQuitShown) {
                canvasQuit.SetActive(true);
            } else {
                canvasQuit.SetActive(false);
            }
        }
    }

}
