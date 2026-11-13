using UnityEngine;
using UnityEngine.Tilemaps;

public class ZonaOculta : MonoBehaviour
{
	[SerializeField] private GameObject zonaOculta;
	TilemapRenderer tr;

	void Awake()
	{
		tr = zonaOculta.GetComponent<TilemapRenderer>();
		tr.enabled = false;
		zonaOculta.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			zonaOculta.SetActive(true);
			tr.enabled = true;
			gameObject.SetActive(false);
		}
	}
}
