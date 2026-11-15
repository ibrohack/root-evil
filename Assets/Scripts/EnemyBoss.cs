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
			AudioManager.Reproducir(Sonido.EnemigoMuerto);
			CameraFollow cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
			cameraFollow.boss = false;
			gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bala" && vidaActual > 0)
		{
			vidaActual--;
			// El ultimo impacto lo cubre el sonido de muerte en Update
			if (vidaActual > 0) AudioManager.Reproducir(Sonido.Golpe);
		}
	}
}
