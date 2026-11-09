using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sr;
	public CameraFollow cameraFollow;
	public GameObject bala;
	public float velocidad;
	public float fuerzaDeSalto;
	private bool puedeSaltar = true;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		
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
			sr.flipX = true;
		} else {
			sr.flipX = false;
		}
		transform.position += direccion * velocidad * Time.deltaTime;
	}

	void Saltar()
	{
		if (InputSystem.actions["Jump"].WasPressedThisFrame()) {
			if (puedeSaltar) {
				rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode2D.Impulse);
				puedeSaltar = false;
			}
		}
	}

	void Disparar()
	{
		if (InputSystem.actions["Attack"].WasPressedThisFrame()) {
			Instantiate(bala, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 90));
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Suelo") {
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
