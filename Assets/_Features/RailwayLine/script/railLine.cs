using UnityEngine;

public class RailLine : LevelLine
{
    [Header("Spawn Points")]
    [SerializeField] private Transform _leftSpawnPoint = null;
    [SerializeField] private Transform _rightSpawnPoint = null;
    
    [Header("Prefabs")]
    [SerializeField] private TrainObstacle _trainObstaclePrefab = null;
    
    [Header("Warning Light")]
    [SerializeField] private GameObject _redLight = null;
    
    [Header("Settings")]
    [SerializeField] private Vector2 _spawnIntervalRange = new Vector2(6f, 12f);
    [SerializeField] private float _firstTrainDelay = 1f;
    [SerializeField] private float _warningTime = 2f; // Time before train that light turns on

    private Transform _selectedSpawnPoint = null;
    private Vector3 _selectedDirection = Vector3.right;
    
    private float _nextSpawnTime = 0f;
    private bool _warningActive = false;
    private float _trainSpawnTime = 0f;

    public override void Initialize()
{
    Debug.Log("Rail line initialized");

    if (LevelProgressTracker.Instance != null)
    {
        LevelProgressTracker.Instance.LineCrossed();
    }

    if (_redLight != null)
    {
        _redLight.SetActive(false);
    }

    ChooseRandomDirection();

    _nextSpawnTime = Time.time + _firstTrainDelay;
}

    private void Update()
    {
        if (!_warningActive && Time.time >= _nextSpawnTime - _warningTime)
        {
            ActivateWarning();
        }

        if (_warningActive && Time.time >= _trainSpawnTime)
        {
            SpawnTrain();
        }
    }

    private void ChooseRandomDirection()
    {
        bool chooseRight = Random.Range(0, 2) == 0;

        if (chooseRight)
        {
            _selectedSpawnPoint = _rightSpawnPoint;
            _selectedDirection = Vector3.left;
        }
        else
        {
            _selectedSpawnPoint = _leftSpawnPoint;
            _selectedDirection = Vector3.right;
        }
    }

    private void ActivateWarning()
    {
        _warningActive = true;
        _trainSpawnTime = Time.time + _warningTime;

        if (_redLight != null)
        {
            _redLight.SetActive(true);
            Debug.Log("WARNING: Train incoming in " + _warningTime + " seconds!");
        }
    }

    private void SpawnTrain()
    {
        if (_redLight != null)
        {
            _redLight.SetActive(false);
        }

        Quaternion spawnRotation = Quaternion.LookRotation(_selectedDirection);
        TrainObstacle train = Instantiate(
            _trainObstaclePrefab,
            _selectedSpawnPoint.position,
            spawnRotation);

        train.Initialize(_selectedDirection);

        Debug.Log("Train spawned moving " + _selectedDirection);

        ChooseRandomDirection();

        _nextSpawnTime = Time.time + Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y);
        
        _warningActive = false;
    }
}