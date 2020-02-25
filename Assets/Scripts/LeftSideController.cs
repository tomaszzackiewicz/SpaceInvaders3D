using UnityEngine;

namespace SpaceInvaders {

    public class LeftSideController : MonoBehaviour {

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Enemy")) {

                SIGameManager.Instance.LeftSideTrigger();
            }
            if (col.gameObject.tag.Contains("Flagship")) {

                SIGameManager.Instance.LeftSideFlagship();
            }
        }

        void OnTriggerExit(Collider col) {
            if (col.gameObject.tag.Contains("Flagship")) {

                SIGameManager.Instance.FlagshipExit();
            }
        }
    }
}
