using Unity.VisualScripting;
using UnityEditor.Animations;
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
	public int maxVidas;
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
		if (vida > 0)
		{
			Movimiento();
			Saltar();
			Disparar();
		}
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

		if (rb.linearVelocity.y < 0)
		{
			anim.SetBool("TouchingGround", false);
		}
	}

	void Saltar()
	{
		if (InputSystem.actions["Jump"].WasPressedThisFrame() && puedeSaltar)
		{
			bool esDoble = saltos < maxSaltos;
			anim.SetTrigger(esDoble ? "DoubleJump" : "Jump");
			anim.SetBool("TouchingGround", false);
			rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode2D.Impulse);
			saltos--;
			if (saltos <= 0) puedeSaltar = false;
			if (gameObject.transform.parent != null) gameObject.transform.SetParent(null);
		}
	}

	void Disparar()
	{
		if (InputSystem.actions["Attack"].WasPressedThisFrame()) {
			anim.SetTrigger("Shoot");
		}
	}

	public void InstanciasBala()
	{
		Instantiate(bala, cannon.position, Quaternion.identity);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Suelo") {
			saltos = maxSaltos;
			puedeSaltar = true;
			anim.SetBool("TouchingGround", true);

			if (collision.gameObject.name.Contains("Plataforma"))
			{
				gameObject.transform.SetParent(collision.gameObject.transform);
			}
		}

		if (collision.gameObject.tag == "Enemy")
		{
			vida--;
			GameManager.Instance.ActualizarVida(vida);
			if (vida <= 0)
			{
				rb.gravityScale = 0.2f;
				collision.collider.enabled = false;
				Muerte();
			}
		}

		if (collision.gameObject.tag == "KillZone")
		{
			vida = 0;
			GameManager.Instance.ActualizarVida(vida);
			rb.gravityScale = 0.2f;
			collision.collider.enabled = false;
			Muerte();
		}
	}

	void Muerte()
	{
		anim.SetTrigger("Dead");
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Suelo")
		{
			anim.SetBool("TouchingGround", true);
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
		{
			anim.SetBool("TouchingGround", false);
		}
	}

}
