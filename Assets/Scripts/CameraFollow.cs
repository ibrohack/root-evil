using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;
	private Vector3 newPosition;
	public bool seguir;

	void LateUpdate()
    {
		if (target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}
        if (target != null && seguir)
        {
			newPosition = new Vector3(target.position.x, -0.2f, 0);
            Vector3 desiredPosition = newPosition + offset;
            // Interpola suavemente entre la posición actual y la deseada
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
