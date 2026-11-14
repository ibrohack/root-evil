using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager Instance;
	private Animator anim;
	[SerializeField] private TMP_Text textoVidas;
	[SerializeField] private TMP_Text textoCronometro;
	[SerializeField] private Image finishImage;
	private bool estaCorriendo = true;
	private float tiempoTranscurrido;

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		} else Destroy(gameObject);
	}

	void Start()
	{
		anim = finishImage.GetComponent<Animator>();
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
}
