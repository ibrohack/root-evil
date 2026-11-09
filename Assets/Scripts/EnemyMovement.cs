using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private Transform[] waypoints;
	[SerializeField] private float velocidad = 2f;
	private int indiceActual = 0;
	private GameObject player;
	private bool seguimiento = false;

	void Update()
	{
		Movimiento();
	}

	void Movimiento()
	{
		if (!seguimiento)
		{
			if (waypoints.Length == 0) return;

			Transform destino = waypoints[indiceActual];
			transform.position = Vector2.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

			// Si llega al waypoint actual, pasa al siguiente
			if (Vector2.Distance(transform.position, destino.position) < 0.1f)
			{
				indiceActual = (indiceActual + 1) % waypoints.Length;
			}
		} else
		{
			// Sigue al jugador en caso de detectarlo
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocidad * Time.deltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bala")
		{
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			player = collision.gameObject;
			seguimiento = true;
		}
	}
}
