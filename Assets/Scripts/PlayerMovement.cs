using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement Instance;
	private Rigidbody2D rb;
	private Animator anim;
	public CameraFollow cameraFollow;
	public GameObject bala;
	public Transform cannon;
	private Coroutine corrutinaDisparoRapido;
	private Coroutine corrutinaInvencible;
	public float velocidad;
	public int maxSaltos = 2;
	private int saltos;
	public int maxVidas;
	public int vida;
	public float fuerzaDeSalto;
	private bool puedeSaltar = true;
	private bool puedeDisparar = true;
	public bool invencible = false;
	private int direccionMirada = 0;
	public bool puedeMoverse = false;
	public float limiteCaida = -8f;

	void Awake()
	{
		// Cada nivel tiene su propio Player: no sobrevive al cambio de escena
		Instance = this;

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		saltos = maxSaltos;

		// Las vidas vienen del nivel anterior. 0 significa partida nueva: se empieza con el maximo
		vida = DatosPartida.vidas > 0 ? DatosPartida.vidas : maxVidas;
	}

	void Start()
	{
		GameManager.Instance.ActualizarVida(vida);
		if (cameraFollow == null) cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
	}

	// Update is called once per frame
	void Update()
	{
		// Si cae por debajo del nivel muere, igual que al tocar la KillZone
		if (vida > 0 && transform.position.y < limiteCaida)
		{
			rb.linearVelocity = Vector2.zero;
			rb.simulated = false;
			Morir();
			return;
		}

		if (PuedeActuar())
		{
			Movimiento();

			// Teclado y mando. Los botones tactiles del HUD llaman a Saltar() y Disparar() directamente
			if (InputSystem.actions["Jump"].WasPressedThisFrame()) Saltar();
			if (InputSystem.actions["Attack"].WasPressedThisFrame() && !PunteroSobreUI()) Disparar();
		}
	}

	bool PuedeActuar()
	{
		return vida > 0 && puedeMoverse && Time.timeScale > 0f;
	}

	// Evita disparar al hacer clic con el raton sobre un boton de la interfaz
	static bool PunteroSobreUI()
	{
		return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
	}

	void Movimiento()
	{
		Vector2 input = InputSystem.actions["Move"].ReadValue<Vector2>();
		Vector3 direccion = new Vector3(input.x, 0, 0);
		anim.SetFloat("Speed", Mathf.Abs(input.x));
		if (direccion.x < 0) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
			direccionMirada = -1;
		} else if (direccion.x > 0) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
			direccionMirada = 1;
		}
		transform.position += direccion * velocidad * Time.deltaTime;

		if (rb.linearVelocity.y < 0)
		{
			anim.SetBool("TouchingGround", false);
		}
	}

	public void Saltar()
	{
		if (!PuedeActuar() || !puedeSaltar) return;

		bool esDoble = saltos < maxSaltos;
		anim.SetTrigger(esDoble ? "DoubleJump" : "Jump");
		AudioManager.Reproducir(esDoble ? Sonido.DobleSalto : Sonido.Salto);
		anim.SetBool("TouchingGround", false);
		rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode2D.Impulse);
		saltos--;
		if (saltos <= 0) puedeSaltar = false;
		if (gameObject.transform.parent != null) gameObject.transform.SetParent(null);
	}


	public void ActivarInvencibilidad()
	{
		if (corrutinaInvencible != null) StopCoroutine(corrutinaInvencible);
		corrutinaInvencible = StartCoroutine(PlayerInvencible());
	}

	public IEnumerator PlayerInvencible()
	{
		invencible = true;
		yield return new WaitForSeconds(5f);

		invencible = false;
	}

	public void ActivarDisparoRapido()
	{
		if (corrutinaDisparoRapido != null) StopCoroutine(corrutinaDisparoRapido);
		corrutinaDisparoRapido = StartCoroutine(DisparoRapido());
	}

	public IEnumerator DisparoRapido()
	{
		anim.SetFloat("ShootSpeed", 5f);
		yield return new WaitForSeconds(5f);

		anim.SetFloat("ShootSpeed", 1f);
	}

	public void Disparar()
	{
		if (!PuedeActuar() || !puedeDisparar) return;

		puedeDisparar = false;
		anim.SetTrigger("Shoot");
	}

	public void InstanciasBala()
	{
		Instantiate(bala, cannon.position, direccionMirada > 0 ? Quaternion.identity : Quaternion.Euler(0, 0, 180));
		AudioManager.Reproducir(Sonido.Disparo);
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

		if (collision.gameObject.tag == "Enemy" && !invencible && vida > 0)
		{
			vida--;
			GameManager.Instance.ActualizarVida(vida);
			if (vida <= 0)
			{
				collision.collider.enabled = false;
				Morir();
			}
			else
			{
				AudioManager.Reproducir(Sonido.Golpe);
			}
		}

		if (collision.gameObject.tag == "KillZone" && vida > 0)
		{
			collision.collider.enabled = false;
			Morir();
		}
	}

	// Unico punto de muerte: enemigo, KillZone o caida fuera del nivel
	void Morir()
	{
		vida = 0;
		GameManager.Instance.ActualizarVida(vida);
		rb.gravityScale = 0.2f;
		anim.SetTrigger("Dead");
		AudioManager.Reproducir(Sonido.Muerte);
		GameManager.Instance.JugadorMuerto();
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

		if (other.gameObject.tag == "TriggerBoss")
		{
			cameraFollow.boss = !cameraFollow.boss;
		}

		if (other.gameObject.tag == "Meta" && vida > 0)
		{
			GameManager.Instance.ActualizarFinish();
		}
	}
	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Suelo"))
		{
			anim.SetBool("TouchingGround", false);
		}
	}

	public void recuperarDisparo()
	{
		puedeDisparar = true;
	}

}
