using UnityEngine;
using UnityEngine.UI;

public class AjustesVolumen : MonoBehaviour
{
	[SerializeField] private Slider sliderMusica;
	[SerializeField] private Slider sliderSFX;

	void Awake()
	{
		// Los sliders avisan a este script cada vez que cambian
		// (es lo mismo que conectar On Value Changed en el Inspector)
		if (sliderMusica != null) sliderMusica.onValueChanged.AddListener(CambiarMusica);
		if (sliderSFX != null) sliderSFX.onValueChanged.AddListener(CambiarSFX);
	}

	void OnEnable()
	{
		// Al abrir el panel los sliders muestran el volumen guardado
		if (AudioManager.Instance == null) return;

		if (sliderMusica != null) sliderMusica.SetValueWithoutNotify(AudioManager.Instance.VolumenMusica);
		if (sliderSFX != null) sliderSFX.SetValueWithoutNotify(AudioManager.Instance.VolumenSFX);
	}

	void OnDisable()
	{
		// Al cerrar el panel se escribe el volumen en disco
		if (AudioManager.Instance != null) AudioManager.Instance.GuardarVolumen();
	}

	public void CambiarMusica(float valor)
	{
		if (AudioManager.Instance != null) AudioManager.Instance.VolumenMusica = valor;
	}

	public void CambiarSFX(float valor)
	{
		if (AudioManager.Instance != null) AudioManager.Instance.VolumenSFX = valor;
	}
}
