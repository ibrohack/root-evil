using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField] private string escenaMenu = "MainMenu";
	[SerializeField] private string primerNivel = "Level1";
	[SerializeField] private float esperaGameOver = 1.2f;   // segundos para ver la animacion de muerte
	private bool nivelTerminado = false;
	private bool jugadorMuerto = false;
	private bool pausado = false;

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
		if (nivelTerminado || jugadorMuerto) return;
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

	// ---------- Pausa ----------

	public void Pausar()
	{
		if (pausado || nivelTerminado || jugadorMuerto) return;
		pausado = true;

		Time.timeScale = 0f;   // para fisicas, animaciones y cronometro
		AudioManager.PausarMusica(true);
		UIManager.Instance.MostrarPausa(true);
	}

	public void Reanudar()
	{
		if (!pausado) return;
		pausado = false;

		Time.timeScale = 1f;
		AudioManager.PausarMusica(false);
		UIManager.Instance.MostrarPausa(false);
	}

	// Vuelve a empezar el nivel con las vidas y el tiempo que tenia al entrar.
	// DatosPartida todavia guarda esos valores: solo cambian al llegar a la meta.
	public void ReiniciarNivel()
	{
		CargarEscena(SceneManager.GetActiveScene().name);
	}

	public void IrAlMenu()
	{
		DatosPartida.Nueva();
		CargarEscena(escenaMenu);
	}

	// ---------- Game Over ----------

	// La llama PlayerMovement al morir (enemigo, KillZone o caida)
	public void JugadorMuerto()
	{
		if (jugadorMuerto || nivelTerminado) return;
		jugadorMuerto = true;

		UIManager.Instance.DetenerCronometro();
		StartCoroutine(MostrarGameOver());
	}

	IEnumerator MostrarGameOver()
	{
		yield return new WaitForSecondsRealtime(esperaGameOver);

		// Sin panel de Game Over en este HUD: se reintenta directamente
		if (!UIManager.Instance.MostrarGameOver())
		{
			Reintentar();
			yield break;
		}

		AudioManager.PararMusica();
		AudioManager.Reproducir(Sonido.GameOver);
	}

	// Reintentar tras Game Over: vidas al maximo, el tiempo vuelve al que tenia al entrar en el nivel
	public void Reintentar()
	{
		DatosPartida.vidas = 0;
		CargarEscena(SceneManager.GetActiveScene().name);
	}

	// ---------- Victoria ----------

	// Ultimo nivel superado. La cortina negra de la transicion ya cubre la pantalla
	public void Victoria()
	{
		// Sin panel de Victoria en este HUD: se vuelve al menu
		if (!UIManager.Instance.MostrarVictoria())
		{
			IrAlMenu();
			return;
		}

		AudioManager.PararMusica();
		AudioManager.Reproducir(Sonido.Victoria);
	}

	public void JugarDeNuevo()
	{
		DatosPartida.Nueva();
		CargarEscena(primerNivel);
	}

	// ---------- Utilidades ----------

	void CargarEscena(string nombre)
	{
		Time.timeScale = 1f;
		AudioManager.PausarMusica(false);   // si venimos de la pausa, la musica sigue sin cortarse
		SceneManager.LoadScene(nombre);
	}
}
