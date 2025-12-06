using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _movementDuration = .3f;

    [SerializeField]
    private float _tileScale = 2f;

    [SerializeField]
    private Transform _renderer = null;

    [SerializeField]
    private GameObject _gameOverUI = null;

    private bool _isAlive = true;

    private bool _isMoving = false;
    private Sequence _movementTween = null;

    private void Update()
    {
        if (!_isAlive)
            return;

        HandleInputs();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isAlive) return;

        _isAlive = false;

        if (_movementTween != null)
            _movementTween.Kill(false);

        DOVirtual.Float(1f, 0f, .8f, value => Time.timeScale = value)
            .SetUpdate(true)
            .OnComplete(() => _gameOverUI.SetActive(true));
    }

    private void HandleInputs()
    {
        if (_isMoving)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _isMoving = true;

            _movementTween = DOTween.Sequence()
                .Append(transform.DOMoveZ(transform.position.z + _tileScale, _movementDuration).SetEase(Ease.InOutSine))
                .Join(_renderer.DOMoveY(transform.position.y + _tileScale * .5f, _movementDuration * .5f).SetEase(Ease.InQuad))
                .Join(_renderer.DORotate(Vector3.zero, _movementDuration * .5f).SetEase(Ease.InQuad))
                .Insert(_movementDuration * .5f, _renderer.DOMoveY(transform.position.y, _movementDuration * .5f).SetEase(Ease.OutQuad))
                .OnComplete(ResetMovementCheck);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _isMoving = true;

            _movementTween = DOTween.Sequence()
                .Append(transform.DOMoveZ(transform.position.z - _tileScale, _movementDuration).SetEase(Ease.InOutSine))
                .Join(_renderer.DOMoveY(transform.position.y + _tileScale * .5f, _movementDuration * .5f).SetEase(Ease.InQuad))
                .Join(_renderer.DORotate(new Vector3(0f, 180f, 0f), _movementDuration * .5f).SetEase(Ease.InQuad))
                .Insert(_movementDuration * .5f, _renderer.DOMoveY(transform.position.y, _movementDuration * .5f).SetEase(Ease.OutQuad))
                .OnComplete(ResetMovementCheck);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _isMoving = true;

            _movementTween = DOTween.Sequence()
                .Append(transform.DOMoveX(transform.position.x + _tileScale, _movementDuration).SetEase(Ease.InOutSine))
                .Join(_renderer.DOMoveY(transform.position.y + _tileScale * .5f, _movementDuration * .5f).SetEase(Ease.InQuad))
                .Join(_renderer.DORotate(new Vector3(0f, 90f, 0f), _movementDuration * .5f).SetEase(Ease.InQuad))
                .Insert(_movementDuration * .5f, _renderer.DOMoveY(transform.position.y, _movementDuration * .5f).SetEase(Ease.OutQuad))
                .OnComplete(ResetMovementCheck);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _isMoving = true;

            _movementTween = DOTween.Sequence()
                .Append(transform.DOMoveX(transform.position.x - _tileScale, _movementDuration).SetEase(Ease.InOutSine))
                .Join(_renderer.DOMoveY(transform.position.y + _tileScale * .5f, _movementDuration * .5f).SetEase(Ease.InQuad))
                .Join(_renderer.DORotate(new Vector3(0f, -90f, 0f), _movementDuration * .5f).SetEase(Ease.InQuad))
                .Insert(_movementDuration * .5f, _renderer.DOMoveY(transform.position.y, _movementDuration * .5f).SetEase(Ease.OutQuad))
                .OnComplete(ResetMovementCheck);
        }
    }

    private void ResetMovementCheck()
    {
        _isMoving = false;
    }
}


