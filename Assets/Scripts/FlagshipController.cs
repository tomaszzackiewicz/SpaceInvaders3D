using System.Collections;
using UnityEngine;
using TMPro;

namespace SpaceInvaders {

    public class FlagshipController : MonoBehaviour {

        public AudioClip flagshipAudio;
        public AudioClip hitAudio;
        public GameObject canvas;

        private int _points = 0;
        private AudioSource _audioSource;

        void Awake() {
            _audioSource = GetComponent<AudioSource>();
        }

        void Start() {
            canvas.SetActive(false);
        }

        void OnEnable() {
            SIGameManager.flagshipDelegate += FlagshipAudio;
            _points = Random.Range(100, 1000);
        }

        public void InflictDamage(float damage) {
            StartCoroutine(DestroyEnemyCor());
        }

        IEnumerator DestroyEnemyCor() {
            _audioSource.PlayOneShot(hitAudio);
            SIGameManager.Instance.CountPoints(_points);
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.tag = "FlagshipOff";
            StartCoroutine(ManageCanvasCor());
            FlagshipAudio(false);
            _audioSource.PlayOneShot(hitAudio);
        }

        public void FlagshipAudio(bool isPlay) {
            if (isPlay) {
                _audioSource.Play();
            } else {
                _audioSource.Stop();
            }
        }

        IEnumerator ManageCanvasCor() {
            canvas.transform.parent = null;
            canvas.SetActive(true);
            TextMeshProUGUI textMeshProUGUI = canvas.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshProUGUI) {
                textMeshProUGUI.text = _points.ToString();
            }
            yield return new WaitForSeconds(1.0f);
            canvas.transform.SetParent(this.gameObject.transform, true);
            canvas.transform.localPosition = Vector3.zero;
            canvas.SetActive(false);
        }

        void OnDisable() {
            SIGameManager.flagshipDelegate -= FlagshipAudio;
        }
    }

}
