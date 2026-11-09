using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField] private UIManager uIManager;
	public int maxVidas;

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		} else Destroy(this);
	}

    void Update()
    {
		uIManager.ActualizarTextoCronometro();
    }

	public void ActualizarVida(int vida)
	{
		uIManager.ActualizarVida(vida);
	}
}
