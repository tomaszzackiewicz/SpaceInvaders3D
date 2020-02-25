using System.Collections;
using UnityEngine;

namespace SpaceInvaders {

    public class BulletController : MonoBehaviour {

        public GameObject explosionEffect;
        public float bulletSpeed = 100.0f;
        public float damage = 10.0f;
        public float lifeTime = 2.0f;

        private Rigidbody _rb;

        public bool IsActivated { get; set; }
        public string Owner { get; set; }

        void Start() {

            StartCoroutine(SetLifeTimeCor());
        }

        IEnumerator SetLifeTimeCor() {
            yield return new WaitForSeconds(lifeTime);
            DestroyBullet();
        }

        void Update() {
            _rb = GetComponent<Rigidbody>();
            if (_rb) {
                _rb.velocity = transform.forward * bulletSpeed;
            }
        }

        void OnCollisionEnter(Collision col) {
            if (col.gameObject.CompareTag("Enemy")) {
                EnemyController enemyController = col.gameObject.GetComponent<EnemyController>();
                if (enemyController) {
                    enemyController.InflictDamage(damage);
                }
                PlayExplosionEffect();
            }

            if (col.gameObject.CompareTag("Flagship")) {
                FlagshipController flagshipController = col.gameObject.GetComponent<FlagshipController>();
                if (flagshipController) {
                    flagshipController.InflictDamage(damage);
                }
                PlayExplosionEffect();
            }

            if (col.gameObject.CompareTag("Shield")) {
                ObjectPooler.Instance.ReturnToPool("Bullet", this.gameObject);

                SEController SECon = col.gameObject.GetComponent<SEController>();
                if (SECon) {
                    SECon.DestroyObstacle();
                }
            }

            if (col.gameObject.CompareTag("Obstacle")) {
                PlayExplosionEffect();

                SEController SECon = col.gameObject.GetComponent<SEController>();
                if (SECon) {
                    SECon.DestroyObstacle();
                }
            }

            if (col.gameObject.CompareTag("Bullet")) {
                PlayExplosionEffect();
            }

            if (col.gameObject.CompareTag("Limitation")) {
                PlayExplosionEffect();
            }
        }

        private void PlayExplosionEffect() {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyBullet());
        }

        IEnumerator DestroyBullet() {
            yield return new WaitForSeconds(0.1f);
            ObjectPooler.Instance.ReturnToPool("Bullet", this.gameObject);
        }
    }

}
