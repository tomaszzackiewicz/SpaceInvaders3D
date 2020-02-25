using UnityEngine;

namespace SpaceInvaders {

    public class BLEController : MonoBehaviour {

        public void OnHit() {
            ObjectPooler.Instance.ReturnToPool("BLE", this.gameObject);
        }
    }
}
