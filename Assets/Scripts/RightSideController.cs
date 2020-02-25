using UnityEngine;

namespace SpaceInvaders {

    public class RightSideController : MonoBehaviour {

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Enemy")) {

                SIGameManager.Instance.RightSideTrigger();
            }

            if (col.gameObject.tag.Contains("Flagship")) {

                SIGameManager.Instance.RightSideFlagship();
            }
        }

        void OnTriggerExit(Collider col) {
            if (col.gameObject.tag.Contains("Flagship")) {

                SIGameManager.Instance.FlagshipExit();
            }
        }
    }

}
