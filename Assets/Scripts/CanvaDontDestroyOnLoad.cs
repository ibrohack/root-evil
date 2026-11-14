using UnityEngine;

public class CanvaDontDestroyOnLoad : MonoBehaviour
{
	public static CanvaDontDestroyOnLoad Instance;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
}
