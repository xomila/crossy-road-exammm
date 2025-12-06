using UnityEngine;

public class RoadLine : LevelLine
{
    [Header("Car Spawning")]
    [SerializeField] private float _spawnInterval = 4f;
    [SerializeField] private Transform _leftSpawnPoint = null;
    [SerializeField] private Transform _rightSpawnPoint = null;
    [SerializeField] private CarObstacle _carObstaclePrefab = null;

    [Header("Coin Spawning")]
    [SerializeField] private GameObject _coinPrefab = null;
    [SerializeField] private float _coinHeight = 0.5f;
    [SerializeField] private Vector2 _spawnAreaX = new Vector2(-8f, 8f);

    private Transform _selectedSpawnPoint = null;
    private Vector3 _selectedDirection = Vector3.right;
    private float _spawnIntervalDelta = 0f;

    public override void Initialize()
    {

        bool chooseRight = Random.Range(0, 2) == 0;

        if(chooseRight)
        {
            _selectedSpawnPoint = _rightSpawnPoint;
            _selectedDirection = Vector3.left;
        }
        else
        {
            _selectedSpawnPoint = _leftSpawnPoint;
            _selectedDirection = Vector3.right;
        }

        _spawnIntervalDelta = Random.Range(0f, _spawnInterval);

        if (LevelProgressTracker.Instance != null)
        {
            LevelProgressTracker.Instance.LineCrossed();
        }
        if (LevelProgressTracker.Instance != null && LevelProgressTracker.Instance.ShouldSpawnCoin())
        {
            SpawnCoin();
        }
    }

    private void Update()
    {
        _spawnIntervalDelta += Time.deltaTime;

        if (_spawnIntervalDelta < _spawnInterval)
            return;

        _spawnIntervalDelta = 0f;

        CarObstacle car = Instantiate(_carObstaclePrefab, _selectedSpawnPoint);
        car.Initialize(_selectedDirection);
    }


    private void SpawnCoin()
    {
        if (_coinPrefab == null)
        {
            Debug.LogWarning("Coin prefab not assigned on RoadLine!");
            return;
        }
        float randomX = Random.Range(_spawnAreaX.x, _spawnAreaX.y);
        
        Vector3 spawnPosition = transform.position + new Vector3(randomX, _coinHeight, 0f);
        
        Instantiate(_coinPrefab, spawnPosition, Quaternion.identity, transform);
        
        Debug.Log("Coin spawned on RoadLine!");
    }
}