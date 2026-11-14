using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField] private UIManager uIManager;
	[SerializeField] private PlayerMovement player;
	public int maxVidas;

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		} else Destroy(this);
	}

	void OnEnable()  { SceneManager.sceneLoaded += AlCargarEscena; }

	void AlCargarEscena(Scene escena, LoadSceneMode modo)
	{
		ActualizarFinish();
	}

	public void BloquearPlayer()
	{
		player.puedeMoverse = !player.puedeMoverse;
	}

    void Update()
    {
		uIManager.ActualizarTextoCronometro();
    }

	public void ActualizarVida(int vida)
	{
		uIManager.ActualizarVida(vida);
	}

	public void ActualizarFinish()
	{
		uIManager.TransicionFinish();
	}

	public void CambioDeEscena()
	{
		switch(SceneManager.GetActiveScene().name)
		{
			case "Level1":
				SceneManager.LoadScene("TestLevel");
				break;
			case "TestLevel":
				SceneManager.LoadScene("Level1");
				break;
		}
	}
}
