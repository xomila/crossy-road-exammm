using UnityEngine;

public class CarObstacle : Obstacle
{
	[SerializeField]
	private float _movementSpeed = 1f;

	[SerializeField]
	private float _lifeSpan = 10f;

	[SerializeField]
	private Transform _renderer = null;

	private Vector3 _direction = Vector3.zero;

	public void Initialize(Vector3 direction)
	{
		_direction = direction;

		if(direction == Vector3.right)
			_renderer.Rotate(0f, 90f, 0f);
		else
			_renderer.Rotate(0f, -90f, 0f);

		Destroy(gameObject, _lifeSpan);
	}

	private void Update()
	{
		transform.Translate(_movementSpeed * Time.deltaTime * _direction, Space.World);
	}
}
