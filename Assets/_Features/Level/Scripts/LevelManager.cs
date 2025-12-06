using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private int _baseOffset = 5;
    [SerializeField] private int _playerOffsetAhead = 8;
    [SerializeField] private float _offsetSize = 2f;
    [SerializeField] private Transform _playerTransform = null;

    [Header("Line Prefabs")]
    [SerializeField] private LevelLine _safePathPrefab = null;
    [SerializeField] private LevelLine _roadPrefab = null;
    [SerializeField] private LevelLine _railwayPrefab = null;

    [Header("Spawn Probabilities (Must add up to 100)")]
    [SerializeField] [Range(0, 100)] private float _safePathProbability = 40f;
    [SerializeField] [Range(0, 100)] private float _roadProbability = 35f;
    [SerializeField] [Range(0, 100)] private float _railwayProbability = 25f;

    [Header("Group Size Ranges")]
    [SerializeField] private Vector2Int _safePathGroupSize = new Vector2Int(1, 2);
    [SerializeField] private Vector2Int _roadGroupSize = new Vector2Int(2, 4);
    [SerializeField] private Vector2Int _railwayGroupSize = new Vector2Int(1, 3);

    [Header("Optimization")]
    [SerializeField] private float _destroyDistanceBehind = 8f;

    private int _currentOffset = 0;
    private LevelLine _currentGroupType = null;
    private int _remainingInGroup = 0;
    private List<LevelLine> _spawnedLines = new List<LevelLine>();

    private void Start()
    {
        _currentOffset = _baseOffset;
        ValidateProbabilities();
    }

    private void Update()
    {
        HandleGeneration();
        CleanupOldLines();
    }

    private void HandleGeneration()
    {
        if (_playerTransform.position.z >= _currentOffset - _playerOffsetAhead)
            SpawnNewLine();
    }

    private void SpawnNewLine()
    {
        _currentOffset++;

        if (_remainingInGroup > 0)
        {
            SpawnLineOfType(_currentGroupType);
            _remainingInGroup--;
        }
        else
        {
            ChooseNewGroup();
            SpawnLineOfType(_currentGroupType);
            _remainingInGroup--;
        }
    }

    private void ChooseNewGroup()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < _safePathProbability)
        {
            _currentGroupType = _safePathPrefab;
            _remainingInGroup = Random.Range(_safePathGroupSize.x, _safePathGroupSize.y + 1);
            Debug.Log($"Starting SafePath group of {_remainingInGroup} lines");
        }
        else if (randomValue < _safePathProbability + _roadProbability)
        {
            _currentGroupType = _roadPrefab;
            _remainingInGroup = Random.Range(_roadGroupSize.x, _roadGroupSize.y + 1);
            Debug.Log($"Starting Road group of {_remainingInGroup} lines");
        }
        else
        {
            _currentGroupType = _railwayPrefab;
            _remainingInGroup = Random.Range(_railwayGroupSize.x, _railwayGroupSize.y + 1);
            Debug.Log($"Starting Railway group of {_remainingInGroup} lines");
        }
    }

    private void SpawnLineOfType(LevelLine linePrefab)
    {
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is null! Make sure all line prefabs are assigned in the Inspector.");
            return;
        }

        Vector3 spawnPosition = new Vector3(0f, 0f, _currentOffset * _offsetSize);
        LevelLine line = Instantiate(linePrefab, spawnPosition, Quaternion.identity, transform);
        line.Initialize();

        _spawnedLines.Add(line);
    }

    private void CleanupOldLines()
    {
        float destroyThreshold = _playerTransform.position.z - _destroyDistanceBehind;

        for (int i = _spawnedLines.Count - 1; i >= 0; i--)
        {
            if (_spawnedLines[i] == null)
            {
                _spawnedLines.RemoveAt(i);
            }
            else if (_spawnedLines[i].transform.position.z < destroyThreshold)
            {
                Debug.Log($"Destroying line at Z: {_spawnedLines[i].transform.position.z} (Player at: {_playerTransform.position.z})");
                Destroy(_spawnedLines[i].gameObject);
                _spawnedLines.RemoveAt(i);
            }
        }
    }

    private void ValidateProbabilities()
    {
        float total = _safePathProbability + _roadProbability + _railwayProbability;
        
        if (Mathf.Abs(total - 100f) > 0.1f)
        {
            Debug.LogWarning($"Spawn probabilities don't add up to 100%! Current total: {total}%. " +
                           "Adjust the values in the Inspector.");
        }
    }

    [ContextMenu("Validate Probabilities")]
    private void ValidateProbabilitiesMenu()
    {
        ValidateProbabilities();
        Debug.Log($"SafePath: {_safePathProbability}%, Road: {_roadProbability}%, Railway: {_railwayProbability}% = {_safePathProbability + _roadProbability + _railwayProbability}%");
    }
}