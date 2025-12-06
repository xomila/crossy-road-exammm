using UnityEngine;

public class SafePathLine : LevelLine
{
    [Header("Coin Spawning")]
    [SerializeField] private GameObject _coinPrefab = null;
    [SerializeField] private float _coinHeight = 0.5f;
    [SerializeField] private Vector2 _spawnAreaX = new Vector2(-8f, 8f);

    public override void Initialize()
    {
        if (LevelProgressTracker.Instance != null)
        {
            LevelProgressTracker.Instance.LineCrossed();
        }

        if (LevelProgressTracker.Instance != null && LevelProgressTracker.Instance.ShouldSpawnCoin())
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        if (_coinPrefab == null)
        {
            Debug.LogWarning("Coin prefab not assigned on SafePathLine!");
            return;
        }

        float randomX = Random.Range(_spawnAreaX.x, _spawnAreaX.y);
        
        Vector3 spawnPosition = transform.position + new Vector3(randomX, _coinHeight, 0f);
        
        Instantiate(_coinPrefab, spawnPosition, Quaternion.identity, transform);
        
        Debug.Log("Coin spawned on SafePathLine!");
    }
}