using UnityEngine;
public class TrainObstacle : Obstacle
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _lifeSpan = 15f;
    [SerializeField] private Transform _renderer = null;

    private Vector3 _direction = Vector3.zero;

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        if (_renderer != null)
        {
            _renderer.rotation = Quaternion.LookRotation(direction);
        }
        Destroy(gameObject, _lifeSpan);
    }

    private void Update()
    {
        transform.position += _direction * _movementSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Train hit player!");
        }
    }
}