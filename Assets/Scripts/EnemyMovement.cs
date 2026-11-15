
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidad = 2f;
    public float tiempoEspara = 3f;
    public Animator animator;

    private int indiceActual = 0;
    private bool esperando = false;
    private bool muerto = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!esperando && !muerto)
        {
            Movimiento();
        }
    }

    void Movimiento()
    {
        if (waypoints.Length == 0) return;

        Transform destino = waypoints[indiceActual];

        if (destino.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        transform.position = Vector2.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        if (Vector2.Distance(transform.position, destino.position) < 0.1f)
        {
            StartCoroutine(EsperarWaypoint());
        }
    }

    IEnumerator EsperarWaypoint()
    {
        esperando = true;

        yield return new WaitForSeconds(tiempoEspara);

        indiceActual = (indiceActual + 1) % waypoints.Length;
        esperando = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bala") && !muerto)
        {
            StartCoroutine(Morir());
        }
    }

    IEnumerator Morir()
    {
        muerto = true;
        esperando = true;

        AudioManager.Reproducir(Sonido.EnemigoMuerto);

        if (animator != null)
            animator.Play("Death");

        yield return new WaitForSeconds(30f / 60f);

        Destroy(gameObject);
    }
}
