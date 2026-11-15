
using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public int maxVidas = 3;
    private int vidaActual;

    public Transform[] waypoints;
    public float velocidad = 2f;
    public float tiempoEspera = 3f;

    public Animator animator;

    private int indiceActual = 0;
    private bool esperando = false;
    private bool muerto = false;

    void Start()
    {
        vidaActual = maxVidas;

        if (animator == null)
            animator = GetComponent<Animator>();

        animator.Play("Idle");
    }

    void Update()
    {
        if (muerto || esperando) return;

        Movimiento();
    }

    void Movimiento()
    {
        if (waypoints.Length == 0) return;

        Transform destino = waypoints[indiceActual];

        if (destino.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        animator.Play("Walk");

        transform.position = Vector2.MoveTowards(
            transform.position,
            destino.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, destino.position) < 0.1f)
            StartCoroutine(Esperar());
    }

    IEnumerator Esperar()
    {
        esperando = true;

        animator.Play("Idle");

        yield return new WaitForSeconds(tiempoEspera);

        indiceActual = (indiceActual + 1) % waypoints.Length;

        esperando = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bala") && !muerto)
        {
            vidaActual--;

            if (vidaActual > 0)
                AudioManager.Reproducir(Sonido.Golpe);
            else
                Morir();
        }
    }

    void Morir()
    {
        muerto = true;

        StopAllCoroutines();

        AudioManager.Reproducir(Sonido.EnemigoMuerto);

        // Libera la camara de la zona del boss para que vuelva a seguir al Player hasta la bandera
        CameraFollow cameraFollow = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
        if (cameraFollow != null) cameraFollow.boss = false;

        gameObject.SetActive(false);
    }
}
