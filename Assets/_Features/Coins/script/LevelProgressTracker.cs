using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    public static LevelProgressTracker Instance { get; private set; }

    [Header("Coin Spawn Settings")]
    [SerializeField] private int _initialSpawnRate = 8;
    [SerializeField] private int _maxSpawnRate = 4;
    [SerializeField] private int _linesForMaxRate = 50;

    private int _linesCrossed = 0;
    private int _currentSpawnRate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ResetProgress();
    }

    public void ResetProgress()
    {
        _linesCrossed = 0;
        _currentSpawnRate = _initialSpawnRate;
    }

    public void LineCrossed()
    {
        _linesCrossed++;
        UpdateSpawnRate();
        Debug.Log($"Lines crossed: {_linesCrossed}, Coin spawn rate: 1 every {_currentSpawnRate} lines");
    }

    private void UpdateSpawnRate()
    {
        if (_linesCrossed >= _linesForMaxRate)
        {
            _currentSpawnRate = _maxSpawnRate;
        }
        else
        {
            float progress = (float)_linesCrossed / _linesForMaxRate;
            _currentSpawnRate = Mathf.RoundToInt(Mathf.Lerp(_initialSpawnRate, _maxSpawnRate, progress));
        }
    }

    public bool ShouldSpawnCoin()
    {
        return Random.Range(0, _currentSpawnRate) == 0;
    }

    public int GetLinesCrossed()
    {
        return _linesCrossed;
    }
}