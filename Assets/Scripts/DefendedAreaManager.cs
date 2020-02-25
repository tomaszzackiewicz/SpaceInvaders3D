using UnityEngine;

namespace SpaceInvaders {

    public class DefendedAreaManager : MonoBehaviour {

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Enemy")) {
                SIGameManager.Instance.OnEndGameState();
            }
        }

    }

}
