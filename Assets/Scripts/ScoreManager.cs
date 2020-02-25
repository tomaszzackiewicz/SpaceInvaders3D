using UnityEngine;
using TMPro;

namespace SpaceInvaders {

    public class ScoreManager : MonoBehaviour {

        public TextMeshProUGUI totalScore1;
        public TextMeshProUGUI totalHIScore1;

        private int _totalScore = 0;

        void OnEnable() {
            SIGameManager.scored += SetScore;
        }

        void SetScore(int points) {

            _totalScore += points;

            if (_totalScore < 10) {
                totalScore1.text = "0000" + _totalScore.ToString();
            } else if (_totalScore < 100) {
                totalScore1.text = "000" + _totalScore.ToString();
            } else if (_totalScore < 1000) {
                totalScore1.text = "00" + _totalScore.ToString();
            } else if (_totalScore < 10000) {
                totalScore1.text = "0" + _totalScore.ToString();
            }
        }

        void OnDisable() {
            SIGameManager.scored -= SetScore;
        }
    }
}
