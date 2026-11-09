using UnityEngine;

public class Bala : MonoBehaviour
{
	private Rigidbody2D rb;
	public float direction;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(1000 * direction, 2));
		Destroy(gameObject, 1f);
	}
}
