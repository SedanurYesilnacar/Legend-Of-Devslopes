using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField]
    private Transform _player;

    private bool _gameOver = false;
    private int _currentLevel;
    private int _finalLevel = 20;
    private int _maxPowerUp = 3;
    private int _totalPowerUp = 0;
    private float _generatedSpawnTime = 1f; // Time between enemy spawns
    private float _currentSpawnTime = 0f;
    private float _timeBetweenPowerUpSpawn = 60f;
    private GameObject _newEnemy;

    private List<EnemyHealth> _enemies = new List<EnemyHealth>();
    private List<EnemyHealth> _killedEnemies = new List<EnemyHealth>();

    [SerializeField]
    private GameObject[] _spawnPoints;
    [SerializeField]
    private GameObject[] _enemyArr;
    [SerializeField]
    private Transform[] _powerUpSpawnPoints;
    [SerializeField]
    private GameObject[] _powerUpObjects;

    [SerializeField]
    private Text _levelTxt;
    [SerializeField]
    private Text _endGameTxt;


    public bool GameOver
    {
        get
        {
            return _gameOver;
        }
    }

    public Transform Player
    {
        get
        {
            return _player;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        _endGameTxt.gameObject.SetActive(false);
    }


    void Start()
    {
        _currentLevel = 1;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    void Update()
    {
        _currentSpawnTime += Time.deltaTime;
    }


    public void PlayerHit(int currentHP)
    {
        if(currentHP > 0)
        {
            _gameOver = false;
        } else
        {
            _gameOver = true;
            StartCoroutine(EndGameRoutine("Defeat !"));
        }
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        _enemies.Add(enemy);
    }

    public void KilledEnemy(EnemyHealth enemy)
    {
        _killedEnemies.Add(enemy);
    }

    private IEnumerator SpawnEnemy()
    {
        if(_currentSpawnTime >= _generatedSpawnTime && _enemies.Count < _currentLevel)
        {
            int enemyNum = Random.Range(0, _enemyArr.Length);
            int spawnPointNum = Random.Range(0, _spawnPoints.Length);
            _newEnemy = Instantiate(_enemyArr[enemyNum], _spawnPoints[spawnPointNum].transform.position,Quaternion.identity) as GameObject;
        }
        if(_killedEnemies.Count == _currentLevel && _currentLevel != _finalLevel)
        {
            _enemies.Clear();
            _killedEnemies.Clear();
            yield return new WaitForSeconds(3f);
            _currentLevel++;
            _levelTxt.text = "Level " + _currentLevel;
        }
        // Win the game
        if(_killedEnemies.Count == _finalLevel)
        {
            StartCoroutine(EndGameRoutine("Victory !"));
        }
        _currentSpawnTime = 0f;
        yield return new WaitForSeconds(_generatedSpawnTime);
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnPowerUp()
    {
        if(!GameOver && _totalPowerUp < _maxPowerUp)
        {
            int powerUpNum = Random.Range(0, _powerUpObjects.Length);
            int powerUpSpawnPoint = Random.Range(0, _powerUpSpawnPoints.Length);
            GameObject newPowerUp = Instantiate(_powerUpObjects[powerUpNum], _powerUpSpawnPoints[powerUpSpawnPoint].position, Quaternion.identity) as GameObject;
            _totalPowerUp++;
            yield return new WaitForSeconds(_timeBetweenPowerUpSpawn);
        }
        yield return null;
        StartCoroutine(SpawnPowerUp());
    }

    private IEnumerator EndGameRoutine(string outcome)
    {
        _endGameTxt.text = outcome;
        if(outcome == "Defeat !")
        {
            _endGameTxt.color = Color.red;
        }
        _endGameTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}