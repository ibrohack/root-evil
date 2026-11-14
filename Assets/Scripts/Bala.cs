using UnityEngine;

public class Bala : MonoBehaviour
{
	private Rigidbody2D rb;
	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(player.transform.right.x * 1000, 2));
		Destroy(gameObject, 1f);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "ZonaOculta")
		{
			Destroy(gameObject);
		}
	}
}
