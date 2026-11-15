using UnityEngine;

public class BossZone : MonoBehaviour
{
	[SerializeField] private GameObject pared;

	void Start()
	{
		pared.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			pared.SetActive(true);
			Destroy(gameObject);
		}
	}
}
