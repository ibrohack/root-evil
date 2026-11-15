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
	private bool estaCorriendo = true;
	private float tiempoTranscurrido;

	public void Awake()
	{
		// Cada nivel tiene su propio HUD: no sobrevive al cambio de escena
		Instance = this;
		if (finishImage != null) anim = finishImage.GetComponent<Animator>();

		// El cronometro continua desde el tiempo acumulado en los niveles anteriores
		tiempoTranscurrido = DatosPartida.tiempo;
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

			int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
			int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

			textoCronometro.text = string.Format("{0:00}:{1:00}", minutos, segundos);
		}
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

	// Botones tactiles del HUD: se conectan en el OnClick de cada boton
	public void BotonSaltar()
	{
		if (PlayerMovement.Instance != null) PlayerMovement.Instance.Saltar();
	}

	public void BotonDisparar()
	{
		if (PlayerMovement.Instance != null) PlayerMovement.Instance.Disparar();
	}
}
