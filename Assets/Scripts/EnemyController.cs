using System.Collections;
using UnityEngine;

namespace SpaceInvaders {

    public class EnemyController : MonoBehaviour {

        public Transform bulletSpawnPoint;
        public float factor = 1.0f;
        public EnemyType enemyType;
        public int points = 0;
        public AudioClip hitAudio;

        private MeshRenderer ren;
        private bool isShootReady = false;
        private AudioSource audioSource;

        public EnemyRow EnemyRow { get; set; }
        public EnemyStats EnemyStats { get; set; }

        void Awake() {
            ren = GetComponentInChildren<MeshRenderer>();
            audioSource = GetComponent<AudioSource>();
        }

        void Start() {
            StartCoroutine(InitCor());
        }

        IEnumerator InitCor() {
            yield return new WaitForSeconds(1.0f);
            if (EnemyStats) {
                enemyType = EnemyStats.enemyType;
                ren.material.color = EnemyStats.color;
                points = EnemyStats.points;
            }
        }

        public bool CheckIsSpaceNotBlocked() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, Mathf.Infinity)) {
                Debug.DrawRay(transform.position, -transform.forward * 100, Color.white, 2.0f);
                if (hit.collider.gameObject.CompareTag("Enemy")) {
                    isShootReady = false;
                } else {
                    isShootReady = true;
                }
            }
            return isShootReady;
        }

        public void Shoot() {
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("EBullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            if (bullet) {
                bullet.GetComponent<BoxCollider>().enabled = true;
            }
        }

        public void InflictDamage(float damage) {
            StartCoroutine(DestroyEnemyCor());
        }

        IEnumerator DestroyEnemyCor() {
            audioSource.PlayOneShot(hitAudio);
            SIGameManager.Instance.CountPoints(points);
            yield return new WaitForSeconds(0.1f);

            SIGameManager.Instance.EnemiesList.Remove(this.gameObject);
            this.gameObject.SetActive(false);
            switch (EnemyRow) {
                case EnemyRow.Row1:
                    ObjectPooler.Instance.ReturnToPool("Enemy1", this.gameObject);
                    break;
                case EnemyRow.Row2:
                    ObjectPooler.Instance.ReturnToPool("Enemy2", this.gameObject);
                    break;
                case EnemyRow.Row3:
                    ObjectPooler.Instance.ReturnToPool("Enemy3", this.gameObject);
                    break;
                case EnemyRow.Row4:
                    ObjectPooler.Instance.ReturnToPool("Enemy4", this.gameObject);
                    break;
                case EnemyRow.Row5:
                    ObjectPooler.Instance.ReturnToPool("Enemy5", this.gameObject);
                    break;

            }
        }

        void OnColliderEnter(Collider col) {
            if (col.gameObject.CompareTag("Obstacle")) {
                GameObject.Destroy(col.gameObject);
            }
        }

    }

}


