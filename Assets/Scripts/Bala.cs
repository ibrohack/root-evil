using UnityEngine;

public class Bala : MonoBehaviour
{
	private Rigidbody2D rb;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(1000, 2));
		Destroy(gameObject, 1f);
	}
}
