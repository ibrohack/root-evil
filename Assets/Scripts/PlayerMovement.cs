using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	public CameraFollow cameraFollow;
	public GameObject bala;
	public Transform cannon;
	public float velocidad;
	public int maxSaltos = 2;
	private int saltos;
	public float fuerzaDeSalto;
	private bool puedeSaltar = true;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		saltos = maxSaltos;
	}

	// Update is called once per frame
	void Update()
	{
		Movimiento();
		Saltar();
		Disparar();
	}

	void Movimiento()
	{
		Vector2 input = InputSystem.actions["Move"].ReadValue<Vector2>();
		Vector3 direccion = new Vector3(input.x, 0, 0);
		if (direccion.x < 0) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
		} else {
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		transform.position += direccion * velocidad * Time.deltaTime;
	}

	void Saltar()
	{
		if (InputSystem.actions["Jump"].WasPressedThisFrame()) {
			if (puedeSaltar) {
				rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode2D.Impulse);
				saltos--;
				if (saltos <= 0)
				{
					puedeSaltar = false;
				}
			}
		}
	}

	void Disparar()
	{
		if (InputSystem.actions["Attack"].WasPressedThisFrame()) {
			Instantiate(bala, cannon.position, Quaternion.Euler(0, 0, 90));
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Suelo") {
			saltos = maxSaltos;
			puedeSaltar = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "TriggerCamara")
		{
			if (cameraFollow.seguir)
			{
				cameraFollow.seguir = false;
			} else
			{
				cameraFollow.seguir = true;
			}
		}
	}

}
