using System.Collections;
using UnityEngine;

namespace SpaceInvaders {

    public class PlayerController : MonoBehaviour {

        public AudioClip shootAudio;
        public AudioClip deathAudio;
        public float speed = 5.0f;
        public Transform bulletSpawnPoint;

        private Vector3 _pos = Vector3.zero;
        private static PlayerController _instance;
        private AudioSource _audioSource;

        public bool IsGameReady { get; set; } = false;

        public static PlayerController Instance {
            get {
                if (_instance == null) {
                    _instance = GameObject.FindObjectOfType<PlayerController>();
                }

                return _instance;
            }
        }

        void Start() {
            _pos = transform.position;
            _audioSource = GetComponent<AudioSource>();
        }

        public void Shoot() {
            if (IsGameReady) {
                GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                if (bullet) {
                    bullet.GetComponent<BoxCollider>().enabled = true;
                    _audioSource.PlayOneShot(shootAudio);
                }
            }
        }

        public void GoRight() {
            if (IsGameReady) {
                if (transform.position.x < 13) {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }

        public void GoLeft() {
            if (IsGameReady) {
                if (transform.position.x > -13) {
                    transform.Translate(-Vector3.right * speed * Time.deltaTime);
                }
            }
        }

        public void InflictDamage(float damage) {
            StartCoroutine(DestroyEnemyCor());
        }

        IEnumerator DestroyEnemyCor() {
            SIGameManager.Instance.OnEndGameState();
            _audioSource.PlayOneShot(deathAudio);
            yield return new WaitForSeconds(1.7f);
            GameObject.Destroy(gameObject);
        }
    }
}
