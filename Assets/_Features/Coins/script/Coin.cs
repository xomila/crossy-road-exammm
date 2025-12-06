using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [Header("Idle Animation")]
    [SerializeField] private float _idleRotationSpeed = 50f;

    [Header("Collection Animation")]
    [SerializeField] private float _collectUpwardDistance = 2f;
    [SerializeField] private float _collectAnimationDuration = 0.5f;

    private bool _isCollected = false;

    private void Update()
    {
        if (!_isCollected)
        {
            transform.Rotate(Vector3.up, _idleRotationSpeed * Time.deltaTime);
        }
    }

 private void OnTriggerEnter(Collider other)
{
    Debug.Log("Something touched the coin! Object name: " + other.gameObject.name + ", Tag: " + other.tag);
    
    if (!_isCollected && other.CompareTag("Player"))
    {
        Debug.Log("Player detected! Collecting coin...");
        CollectCoin();
    }
    else
    {
        Debug.Log("Not the player. isCollected: " + _isCollected);
    }
}   
    private void CollectCoin()
    {
        _isCollected = true;
        CoinManager.Instance.AddCoin();
        PlayCollectionAnimation();
    }
    private void PlayCollectionAnimation()
    {
        transform.DORotate(new Vector3(0f, 360f * 3, 0f), _collectAnimationDuration, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.OutQuad);

        transform.DOMoveY(transform.position.y + _collectUpwardDistance, _collectAnimationDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(gameObject));

        transform.DOScale(0f, _collectAnimationDuration)
            .SetEase(Ease.InQuad);
    }
}