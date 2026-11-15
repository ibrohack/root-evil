using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
	[SerializeField] private GameObject panelPrincipal;
	[SerializeField] private GameObject panelNiveles;
	[SerializeField] private GameObject panelOpciones;
	[SerializeField] private string primerNivel = "Level1";

	void Start()
	{
		Time.timeScale = 1f;
		MostrarPanel(panelPrincipal);
	}

	// Boton Jugar: partida nueva desde el primer nivel
	public void Jugar()
	{
		CargarNivel(primerNivel);
	}

	// Botones de seleccion de nivel: el nombre de la escena se escribe en el OnClick del boton
	public void CargarNivel(string nombreEscena)
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		DatosPartida.Nueva();
		SceneManager.LoadScene(nombreEscena);
	}

	public void AbrirNiveles()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		MostrarPanel(panelNiveles);
	}

	public void AbrirOpciones()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		MostrarPanel(panelOpciones);
	}

	public void Volver()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		MostrarPanel(panelPrincipal);
	}

	public void Salir()
	{
		AudioManager.Reproducir(Sonido.ClickUI);
		Debug.Log("Salir: en el editor no se cierra el juego, en el movil si");
		Application.Quit();
	}

	// Activa el panel indicado y oculta los otros dos
	void MostrarPanel(GameObject panel)
	{
		if (panelPrincipal != null) panelPrincipal.SetActive(panel == panelPrincipal);
		if (panelNiveles != null) panelNiveles.SetActive(panel == panelNiveles);
		if (panelOpciones != null) panelOpciones.SetActive(panel == panelOpciones);
	}
}
