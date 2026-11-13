using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 3f;
	public float length = 5f;
	private float startPositionX;
	private float startPositionY;
	public bool movX;
	public bool movY;

	void Start()
	{
		startPositionX = transform.position.x;
		startPositionY = transform.position.y;
	}

	void Update()
	{
		float newX = transform.position.x;
		float newY = transform.position.y;

		if (movX) {
			newX = startPositionX + Mathf.PingPong(Time.time * speed, length);
		}

		if (movY) {
			newY = startPositionY + Mathf.PingPong(Time.time * speed, length);
		}

		transform.position = new Vector3(newX, newY, 0);
	}
}
