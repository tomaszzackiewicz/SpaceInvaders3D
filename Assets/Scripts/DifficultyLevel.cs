using UnityEngine;

namespace SpaceInvaders {

    [CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
    public class DifficultyLevel : ScriptableObject {

        public DiffLevel enemyType;
        public float difficultyLevel;
    }

}
