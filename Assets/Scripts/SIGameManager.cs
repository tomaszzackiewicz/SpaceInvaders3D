using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace SpaceInvaders {

    public class SIGameManager : MonoBehaviour {

        private static SIGameManager _instance;

        public delegate void RunLeftDelegate();
        public static event RunLeftDelegate runLeftDelegate;

        public delegate void RunRightDelegate();
        public static event RunRightDelegate runRightDelegate;

        public delegate void FlagshipDelegate(bool isPlayed);
        public static event FlagshipDelegate flagshipDelegate;

        public delegate void ScoredDel(int points);
        public static event ScoredDel scored;

        public GameObject parentBLE;
        public GameObject flagship;

        public GameObject row1;
        public GameObject row2;
        public GameObject row3;
        public GameObject row4;
        public GameObject row5;

        public EnemyStats enemyStatsRow1;
        public EnemyStats enemyStatsRow2;
        public EnemyStats enemyStatsRow3;

        public DifficultyLevel novice;
        public DifficultyLevel casual;
        public DifficultyLevel master;

        public float speed = 10.0f;
        public float flagshipSpeed = 0.5f;
        public float factor = 1.0f;
        public float flagshipFactor = 0.1f;
        public List<GameObject> EnemiesList = new List<GameObject>();
        public GameObject[] Enemies = new GameObject[54];

        private bool _dirRight = true;
        private bool _flagshipRight = true;
        private GameObject _grid;

        private float _nextTime = 0.0f;
        private float _nextTimeCheck = 0.0f;
        private float _nextTimeFlagship = 0.0f;
        private float _factorCheck = 0.5f;
        private IGameState _currentGameState;
        private bool _isGamePrepared = false;
        private bool _isSetSpeed1 = false;
        private bool _isSetSpeed2 = false;
        private bool _isPlayGame;

        private float _offset = 0.0f;
        private float _offset1 = 0.0f;
        private float _offset2 = 0.0f;
        private float _offset3 = 0.0f;
        private float _offset4 = 0.0f;
        private float _offset5 = 0.0f;

        private readonly float _flagshipDelay = 20.0f;

        public AudioSource AudioSource { get; set; } = null;
        public bool IsWinner { get; set; } = false;
        public bool IsCheckEnemies { get; set; } = false;
        public bool IsEndGame { get; set; } = false;

        public bool IsPlayGame {
            get { return _isPlayGame; }

            set {
                _isPlayGame = value;
                if (_isPlayGame == false) {
                    if (flagshipDelegate != null) {
                        flagshipDelegate(false);
                    }
                }
            }
        }

        public static SIGameManager Instance {
            get {
                if (_instance == null) {
                    _instance = GameObject.FindObjectOfType<SIGameManager>();
                }

                return _instance;
            }
        }

        void Awake() {
            _grid = GameObject.FindGameObjectWithTag("Grid");
            AudioSource = GetComponent<AudioSource>();
            OnIntroGameState();
        }

        void Start() {
            StartCoroutine(DelayFlagshipCor());
        }

        void Update() {
            if (IsPlayGame) {
                if (_dirRight) {
                    if (Time.time > _nextTime) {
                        _grid.transform.Translate(Vector2.right * speed * Time.deltaTime);
                        _nextTime = Time.time + factor;
                    }

                } else {
                    if (Time.time > _nextTime) {
                        _grid.transform.Translate(-Vector2.right * speed * Time.deltaTime);
                        _nextTime = Time.time + factor;
                    }
                }
                if (isDelayed) {
                    if (_flagshipRight) {
                        flagship.transform.Translate(Vector2.right * flagshipSpeed * Time.deltaTime);
                        _nextTimeFlagship = Time.time + flagshipFactor;
                    } else {
                        flagship.transform.Translate(-Vector2.right * flagshipSpeed * Time.deltaTime);
                        _nextTimeFlagship = Time.time + flagshipFactor;
                    }
                }
            }

            if (IsCheckEnemies) {
                if (Time.time > _nextTimeCheck) {
                    EnemiesList = EnemiesList.Where(enemy => enemy != null).ToList();
                    if (EnemiesList.Count > 0) {
                        int enemyThatShoot = UnityEngine.Random.Range(0, EnemiesList.Count);
                        if (EnemiesList[enemyThatShoot] != null) {
                            bool isReady = EnemiesList[enemyThatShoot].GetComponent<EnemyController>().CheckIsSpaceNotBlocked();
                            if (isReady) {
                                EnemiesList[enemyThatShoot].GetComponent<EnemyController>().Shoot();
                            }
                        }
                        if (EnemiesList.Count < 54 && !_isSetSpeed1) {
                            factor = 0.1f;
                            AudioSource.pitch = 2.0f;
                            _isSetSpeed1 = true;
                        } else if (EnemiesList.Count < 18 && !_isSetSpeed2) {
                            factor = 0.01f;
                            AudioSource.pitch = 3.0f;
                            _isSetSpeed2 = true;
                        }
                        _nextTimeCheck = Time.time + _factorCheck;
                    } else {
                        IsWinner = true;
                        OnEndGameState();
                    }
                }
            }
        }

        public void PrepareGame() {
            if (!_isGamePrepared) {
                StartCoroutine(PrepareGameCor());
                _isGamePrepared = true;
            }
        }

        IEnumerator PrepareGameCor() {
            yield return new WaitForSeconds(0.1f);
            SpawnBLE();
            yield return new WaitForSeconds(0.1f);
            SpawnRow1Enemies();
            yield return new WaitForSeconds(0.1f);
            SpawnRow2Enemies();
            yield return new WaitForSeconds(0.1f);
            SpawnRow3Enemies();
            yield return new WaitForSeconds(0.1f);
            SpawnRow4Enemies();
            yield return new WaitForSeconds(0.1f);
            SpawnRow5Enemies();
            yield return new WaitForSeconds(0.1f);
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            yield return new WaitForSeconds(0.1f);
            EnemiesList = Enemies.ToList();
            Array.Clear(Enemies, 0, Enemies.Length);
            Enemies = Enemies.Where(enemy => enemy != null).ToArray();

            OnPlayGameState();
        }

        void PlayBGMusic(bool isMusicPlayed) {
            if (isMusicPlayed) {
                AudioSource.Play();
            } else {
                AudioSource.Stop();
            }
        }

        void SpawnBLE() {
            int bleCount = ObjectPooler.Instance.CheckPoolSize("BLE");
            for (int i = 0; i < bleCount; i++) {
                _offset += 0.25f;
                ObjectPooler.Instance.SpawnFromPool("BLE", new Vector3(parentBLE.transform.position.x + _offset, parentBLE.transform.position.y, parentBLE.transform.position.z), Quaternion.identity, parentBLE);
            }
        }

        void SpawnRow1Enemies() {
            int enemiesCount = ObjectPooler.Instance.CheckPoolSize("Enemy1");
            for (int i = 0; i < enemiesCount; i++) {

                GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy1", new Vector3(row1.transform.position.x + _offset1, row1.transform.position.y, row1.transform.position.z), Quaternion.identity, row1);
                _offset1 += 1.3f;
                enemy.GetComponent<EnemyController>().EnemyStats = enemyStatsRow1;
                enemy.GetComponent<EnemyController>().EnemyRow = EnemyRow.Row1;
            }
        }

        void SpawnRow2Enemies() {
            int enemiesCount = ObjectPooler.Instance.CheckPoolSize("Enemy2");
            for (int i = 0; i < enemiesCount; i++) {

                GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy2", new Vector3(row2.transform.position.x + _offset2, row2.transform.position.y, row2.transform.position.z), Quaternion.identity, row2);
                _offset2 += 1.3f;
                enemy.GetComponent<EnemyController>().EnemyStats = enemyStatsRow2;
                enemy.GetComponent<EnemyController>().EnemyRow = EnemyRow.Row2;
            }
        }

        void SpawnRow3Enemies() {
            int enemiesCount = ObjectPooler.Instance.CheckPoolSize("Enemy3");
            for (int i = 0; i < enemiesCount; i++) {

                GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy3", new Vector3(row3.transform.position.x + _offset3, row3.transform.position.y, row3.transform.position.z), Quaternion.identity, row3);
                _offset3 += 1.3f;
                enemy.GetComponent<EnemyController>().EnemyStats = enemyStatsRow2;
                enemy.GetComponent<EnemyController>().EnemyRow = EnemyRow.Row3;
            }
        }

        void SpawnRow4Enemies() {
            int enemiesCount = ObjectPooler.Instance.CheckPoolSize("Enemy4");
            for (int i = 0; i < enemiesCount; i++) {

                GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy4", new Vector3(row4.transform.position.x + _offset4, row4.transform.position.y, row4.transform.position.z), Quaternion.identity, row4);
                _offset4 += 1.3f;
                enemy.GetComponent<EnemyController>().EnemyStats = enemyStatsRow3;
                enemy.GetComponent<EnemyController>().EnemyRow = EnemyRow.Row4;
            }
        }

        void SpawnRow5Enemies() {
            int enemiesCount = ObjectPooler.Instance.CheckPoolSize("Enemy5");
            for (int i = 0; i < enemiesCount; i++) {

                GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy5", new Vector3(row5.transform.position.x + _offset5, row5.transform.position.y, row5.transform.position.z), Quaternion.identity, row5);
                _offset5 += 1.3f;
                enemy.GetComponent<EnemyController>().EnemyStats = enemyStatsRow3;
                enemy.GetComponent<EnemyController>().EnemyRow = EnemyRow.Row5;
            }
        }

        public void PlayGame() {
            IsPlayGame = true;
            PlayBGMusic(true);
            StartCoroutine(PrepareCheckingCor());
        }

        IEnumerator PrepareCheckingCor() {
            yield return new WaitForSeconds(2.0f);
            IsCheckEnemies = true;
        }

        public void LeftSideTrigger() {
            if (IsCheckEnemies) {
                if (runLeftDelegate != null) {
                    runLeftDelegate();
                }
                _grid.transform.position = new Vector3(_grid.transform.position.x, _grid.transform.position.y, _grid.transform.position.z - 0.3f);
                _dirRight = true;
            }
        }

        public void RightSideTrigger() {
            if (IsCheckEnemies) {
                if (runRightDelegate != null) {
                    runRightDelegate();

                }
                _grid.transform.position = new Vector3(_grid.transform.position.x, _grid.transform.position.y, _grid.transform.position.z - 0.3f);
                _dirRight = false;
            }
        }

        public void LeftSideFlagship() {
            if (flagshipDelegate != null) {
                flagshipDelegate(false);
            }

            StartCoroutine(LeftSideFlagshipCor());
        }

        IEnumerator LeftSideFlagshipCor() {
            yield return new WaitForSeconds(0.9f);
            if (!flagship.GetComponent<MeshRenderer>().enabled) {
                flagship.GetComponent<MeshRenderer>().enabled = true;
                flagship.tag = "Flagship";
            }
            _flagshipRight = true;
            isDelayed = false;
            StartCoroutine(DelayFlagshipCor());
        }

        public void RightSideFlagship() {
            if (flagshipDelegate != null) {
                flagshipDelegate(false);
            }

            StartCoroutine(RightSideFlagshipCor());
        }

        IEnumerator RightSideFlagshipCor() {
            yield return new WaitForSeconds(0.9f);
            if (!flagship.GetComponent<MeshRenderer>().enabled) {
                flagship.GetComponent<MeshRenderer>().enabled = true;
                flagship.tag = "Flagship";
            }
            _flagshipRight = false;
            isDelayed = false;
            StartCoroutine(DelayFlagshipCor());
        }
        bool isDelayed = false;

        IEnumerator DelayFlagshipCor() {
            yield return new WaitForSeconds(_flagshipDelay);

            isDelayed = true;
        }

        public void FlagshipExit() {
            if (flagshipDelegate != null) {
                flagshipDelegate(true);
            }
        }

        public void CountPoints(int points) {
            if (scored != null) {
                scored(points);
            }
        }

        public void SetDifficultyLevel(DiffLevel diffLevel) {
            if (!IsPlayGame) {
                switch (diffLevel) {
                    case DiffLevel.Novice:
                        _factorCheck = novice.difficultyLevel;
                        break;
                    case DiffLevel.Casual:
                        _factorCheck = casual.difficultyLevel;
                        break;
                    case DiffLevel.Master:
                        _factorCheck = master.difficultyLevel;
                        break;
                }
            }
        }

        public void OnIntroGameState() {
            _currentGameState = new IntroGameState();
            _currentGameState.Execute();
        }

        public void OnPrepareGameState() {
            _currentGameState = new PrepareGameState();
            _currentGameState.Execute();
        }

        public void OnPlayGameState() {
            _currentGameState = new PlayGameState();
            _currentGameState.Execute();
        }

        public void OnEndGameState() {
            _currentGameState = new EndGameState();
            _currentGameState.Execute();
        }
    }
}

