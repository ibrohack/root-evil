using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PickUp : MonoBehaviour
{
	public enum Tipo
	{
		invencible,
		vida,
		disparo
	}

	private SpriteRenderer sr;
	[SerializeField] private Tipo tipo = Tipo.invencible;
	[SerializeField] private Sprite invencible;
	[SerializeField] private Sprite vida;
	[SerializeField] private Sprite disparo;

	void Awake()
	{
		sr = gameObject.GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		switch (tipo)
			{
				case Tipo.invencible:
					sr.sprite = invencible;
					break;
				case Tipo.vida:
					sr.sprite = vida;
					break;
				case Tipo.disparo:
					sr.sprite = disparo;
					break;
			}
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
			switch (tipo)
			{
				case Tipo.invencible:
					player.ActivarInvencibilidad();
					break;
				case Tipo.vida:
					player.vida++;
					GameManager.Instance.ActualizarVida(player.vida);
					break;
				case Tipo.disparo:
					player.ActivarDisparoRapido();
					break;
			}
				Destroy(gameObject);
		}
	}
}
