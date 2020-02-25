using UnityEngine;

namespace SpaceInvaders {

    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemyStats : ScriptableObject {

        public EnemyType enemyType;
        public Color color;
        public int points;
    }

}
