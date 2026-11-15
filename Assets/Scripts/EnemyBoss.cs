using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
	public int maxVidas;
	private int vidaActual;

	void Start()
	{
		if (maxVidas == 0) maxVidas = 1;
		vidaActual = maxVidas;
	}

	void Update()
	{
		if (vidaActual > 0)
		{
			
		} else
		{
			CameraFollow cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
			cameraFollow.boss = false;
			gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bala")
		{
			vidaActual--;
		}
	}
}
