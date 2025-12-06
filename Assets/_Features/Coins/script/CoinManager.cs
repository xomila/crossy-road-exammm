using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _coinText = null;

    private int _currentCoins = 0;

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

        ResetCoins();
    }

    public void AddCoin()
    {
        _currentCoins++;
        UpdateUI();
        Debug.Log("Coins collected: " + _currentCoins);
    }

    public void ResetCoins()
    {
        _currentCoins = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_coinText != null)
        {
            _coinText.text = _currentCoins.ToString();
        }
    }

    public int GetCoinCount()
    {
        return _currentCoins;
    }
}