using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public int maxVidas;
	private bool nivelTerminado = false;

	public void Awake()
	{
		// Cada nivel tiene su propio GameManager: no sobrevive al cambio de escena
		Instance = this;
		Time.timeScale = 1f;
	}

	public void BloquearPlayer()
	{
		PlayerMovement player = PlayerMovement.Instance;
		if (player != null) player.puedeMoverse = !player.puedeMoverse;
	}

    void Update()
    {
		if (UIManager.Instance != null) UIManager.Instance.ActualizarTextoCronometro();
    }

	public void ActualizarVida(int vida)
	{
		if (UIManager.Instance != null) UIManager.Instance.ActualizarVida(vida);
	}

	// El jugador ha tocado la meta: se guardan vidas y tiempo para el siguiente nivel y empieza la transicion
	public void ActualizarFinish()
	{
		if (nivelTerminado) return;
		nivelTerminado = true;

		AudioManager.Reproducir(Sonido.Meta);
		UIManager.Instance.DetenerCronometro();
		DatosPartida.vidas = PlayerMovement.Instance.vida;
		DatosPartida.tiempo = UIManager.Instance.Tiempo;
		UIManager.Instance.TransicionFinish();
	}

	// La llama el evento de la animacion de transicion (Finish.CambiarEscena).
	// El orden de los niveles es el de la lista de escenas de Build Profiles.
	public void CambioDeEscena()
	{
		int siguiente = SceneManager.GetActiveScene().buildIndex + 1;
		if (siguiente < SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(siguiente);
		}
		else
		{
			Victoria();
		}
	}

	// Ultimo nivel superado: partida nueva desde la primera escena
	public void Victoria()
	{
		DatosPartida.Nueva();
		SceneManager.LoadScene(0);
	}
}
