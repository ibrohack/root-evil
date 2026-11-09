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
	public int maxVidas = 10;
	private int vida;
	public float fuerzaDeSalto;
	private bool puedeSaltar = true;

	void Awake()
	{
		GameManager.Instance.ActualizarVida(maxVidas);
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		saltos = maxSaltos;
		vida = maxVidas;
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
		anim.SetFloat("Speed", Mathf.Abs(input.x));
		if (direccion.x < 0) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
		} else if (direccion.x > 0) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		transform.position += direccion * velocidad * Time.deltaTime;
	}

	void Saltar()
	{
		if (InputSystem.actions["Jump"].WasPressedThisFrame() && puedeSaltar)
		{
			bool esDoble = saltos < maxSaltos;
			anim.SetTrigger(esDoble ? "DoubleJump" : "Jump");
			rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode2D.Impulse);
			saltos--;
			if (saltos <= 0) puedeSaltar = false;
		}
	}

	void Disparar()
	{
		if (InputSystem.actions["Attack"].WasPressedThisFrame()) {
			anim.SetTrigger("Shoot");
			Instantiate(bala, cannon.position, Quaternion.Euler(0, 0, 90));
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Suelo") {
			saltos = maxSaltos;
			puedeSaltar = true;
			anim.SetBool("TouchingGround", true);
		}

		if (collision.gameObject.tag == "Enemy")
		{
			vida--;
			GameManager.Instance.ActualizarVida(vida);
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
	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Suelo"))
			anim.SetBool("TouchingGround", false);
	}

}
