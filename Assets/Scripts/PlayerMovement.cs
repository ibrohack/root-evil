using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	private PlayerMovement Instance;
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

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		saltos = maxSaltos;
		vida = maxVidas;
	}

	void OnEnable()  { SceneManager.sceneLoaded += AlCargarEscena; }
	void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

	void AlCargarEscena(Scene escena, LoadSceneMode modo)
	{
		GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
		if (spawn != null) transform.position = spawn.transform.position;

		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		if (cam != null) cameraFollow = cam.GetComponent<CameraFollow>();
	}

	void Start()
	{
		GameManager.Instance.ActualizarVida(maxVidas);
		if (cameraFollow == null) cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
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

	public void Saltar()
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
		if (InputSystem.actions["Attack"].WasPressedThisFrame() && puedeDisparar)
		{
			puedeDisparar = false;
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

		if (collision.gameObject.tag == "Enemy" && !invencible)
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

		if (other.gameObject.tag == "Meta")
		{
			SceneManager.LoadScene("TestLevel");
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
