using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	private Animator anim;
	[SerializeField] private TMP_Text textoVidas;
	[SerializeField] private TMP_Text textoCronometro;
	[SerializeField] private Image finishImage;

	[Header("Paneles (opcionales)")]
	[SerializeField] private GameObject panelPausa;
	[SerializeField] private GameObject panelGameOver;
	[SerializeField] private GameObject panelVictoria;
	[SerializeField] private TMP_Text textoTiempoFinal;
	[SerializeField] private GameObject botonPausa;

	private bool estaCorriendo = true;
	private float tiempoTranscurrido;

	public void Awake()
	{
		// Cada nivel tiene su propio HUD: no sobrevive al cambio de escena
		Instance = this;
		if (finishImage != null) anim = finishImage.GetComponent<Animator>();

		// El cronometro continua desde el tiempo acumulado en los niveles anteriores
		tiempoTranscurrido = DatosPartida.tiempo;

		OcultarPaneles();
	}

	public float Tiempo
	{
		get { return tiempoTranscurrido; }
	}

	public void ActualizarTextoCronometro()
	{
		if (estaCorriendo)
		{
			tiempoTranscurrido += Time.deltaTime;
			textoCronometro.text = FormatoTiempo(tiempoTranscurrido);
		}
	}

	static string FormatoTiempo(float segundosTotales)
	{
		int minutos = Mathf.FloorToInt(segundosTotales / 60);
		int segundos = Mathf.FloorToInt(segundosTotales % 60);
		return string.Format("{0:00}:{1:00}", minutos, segundos);
	}

	public void DetenerCronometro()
	{
		estaCorriendo = false;
	}

	public void ReiniciarCronometro()
	{
		tiempoTranscurrido = 0f;
		estaCorriendo = true;
	}

	public void ActualizarVida(int vida)
	{
		textoVidas.text = "x " + vida;
	}

	public void TransicionFinish()
	{
		if (anim != null) anim.SetTrigger("Change");
	}

	// ---------- Paneles ----------

	void OcultarPaneles()
	{
		if (panelPausa != null) panelPausa.SetActive(false);
		if (panelGameOver != null) panelGameOver.SetActive(false);
		if (panelVictoria != null) panelVictoria.SetActive(false);
		if (botonPausa != null) botonPausa.SetActive(true);
	}

	public void MostrarPausa(bool mostrar)
	{
		if (panelPausa != null) panelPausa.SetActive(mostrar);
		if (botonPausa != null) botonPausa.SetActive(!mostrar);
	}

	// Devuelve false si este HUD no tiene panel de Game Over
	public bool MostrarGameOver()
	{
		if (panelGameOver == null) return false;

		panelGameOver.SetActive(true);
		if (botonPausa != null) botonPausa.SetActive(false);
		return true;
	}

	// Devuelve false si este HUD no tiene panel de Victoria
	public bool MostrarVictoria()
	{
		if (panelVictoria == null) return false;

		if (textoTiempoFinal != null) textoTiempoFinal.text = "TIEMPO " + FormatoTiempo(tiempoTranscurrido);
		panelVictoria.SetActive(true);
		if (botonPausa != null) botonPausa.SetActive(false);
		return true;
	}

	// ---------- Botones del HUD: se conectan en el OnClick de cada boton ----------

	public void BotonSaltar()
	{
		if (PlayerMovement.Instance != null) PlayerMovement.Instance.Saltar();
	}

	public void BotonDisparar()
	{
		if (PlayerMovement.Instance != null) PlayerMovement.Instance.Disparar();
	}

	public void BotonPausa()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.Pausar();
	}

	public void BotonReanudar()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.Reanudar();
	}

	public void BotonReiniciar()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.ReiniciarNivel();
	}

	public void BotonReintentar()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.Reintentar();
	}

	public void BotonJugarDeNuevo()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.JugarDeNuevo();
	}

	public void BotonMenu()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		GameManager.Instance.IrAlMenu();
	}
}
