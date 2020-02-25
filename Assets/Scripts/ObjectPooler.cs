using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders {

    public class ObjectPooler : MonoBehaviour {
        public static ObjectPooler Instance;

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void Awake() {
            Instance = this;
        }

        void Start() {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (Pool pool in pools) {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++) {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, GameObject parent = null) {
            if (!poolDictionary.ContainsKey(tag) || poolDictionary[tag].Count <= 0) {
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            if (parent) {
                objectToSpawn.transform.SetParent(parent.transform);
            }

            //poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void ReturnToPool(string tag, GameObject obj) {
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }

        public int CheckPoolSize(string tag) {
            foreach (Pool pool in pools) {
                if (pool.tag.Equals(tag)) {
                    return pool.size;
                }
            }

            return 0;
        }

        public Pool GetPool(string tag) {
            foreach (Pool pool in pools) {
                if (pool.tag.Equals(tag)) {
                    return pool;
                }
            }

            return null;
        }

        [System.Serializable]
        public class Pool {
            public string tag;
            public GameObject prefab;
            public int size;
        }
    }
}
