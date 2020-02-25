using UnityEngine;

namespace SpaceInvaders {

    public class SEController : MonoBehaviour {
        public void DestroyObstacle() {
            GameObject.Destroy(gameObject);
        }
    }
}
