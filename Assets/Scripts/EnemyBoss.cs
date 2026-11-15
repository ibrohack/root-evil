
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

    [Header("Ataque")]
    // Distancia al Player a la que el boss se para y ataca
    public float rangoAtaque = 2.5f;
    // Segundos de espera entre un ataque y el siguiente
    public float tiempoEntreAtaques = 1.5f;
    // Hasta donde llega la llamarada por delante del cuerpo del boss
    public float alcanceGolpe = 1.5f;
    // Segundos de la animacion Attack en los que la llamarada hace daño (frames a3 a a5)
    public float inicioGolpe = 0.27f;
    public float finGolpe = 0.67f;

    private int indiceActual = 0;
    private bool esperando = false;
    private bool atacando = false;
    private bool muerto = false;
    private float siguienteAtaque = 0f;
    private float duracionAtaque;
    private float duracionMuerte;
    private Collider2D col;

    void Start()
    {
        vidaActual = maxVidas;

        if (animator == null)
            animator = GetComponent<Animator>();

        col = GetComponent<Collider2D>();
        duracionAtaque = DuracionClip("Attack");
        duracionMuerte = DuracionClip("Death");

        animator.Play("Idle");
    }

    void Update()
    {
        if (muerto || atacando) return;

        if (Time.time >= siguienteAtaque && PlayerEnRango())
        {
            StartCoroutine(Atacar());
            return;
        }

        if (esperando) return;

        Movimiento();
    }

    void Movimiento()
    {
        if (waypoints.Length == 0) return;

        Transform destino = waypoints[indiceActual];

        MirarHacia(destino.position.x);

        animator.Play("Walk");

        transform.position = Vector2.MoveTowards(
            transform.position,
            destino.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, destino.position) < 0.1f)
            StartCoroutine(Esperar());
    }

    // El sprite mira a la izquierda por defecto: se voltea para mirar a la derecha
    void MirarHacia(float x)
    {
        if (x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator Esperar()
    {
        esperando = true;

        animator.Play("Idle");

        yield return new WaitForSeconds(tiempoEspera);

        indiceActual = (indiceActual + 1) % waypoints.Length;

        esperando = false;
    }

    bool PlayerEnRango()
    {
        PlayerMovement jugador = PlayerMovement.Instance;
        if (jugador == null || jugador.vida <= 0) return false;

        Vector2 centro = col != null ? (Vector2)col.bounds.center : (Vector2)transform.position;
        return Vector2.Distance(centro, jugador.transform.position) <= rangoAtaque;
    }

    // Se para, mira al Player y reproduce el ataque una vez.
    // Mientras sale la llamarada, si el Player esta delante le quita una vida (solo una por ataque)
    IEnumerator Atacar()
    {
        atacando = true;

        MirarHacia(PlayerMovement.Instance.transform.position.x);
        animator.Play("Attack", 0, 0f);

        bool haGolpeado = false;
        float tiempo = 0f;
        while (tiempo < duracionAtaque)
        {
            if (!haGolpeado && tiempo >= inicioGolpe && tiempo <= finGolpe && PlayerEnZonaGolpe())
            {
                PlayerMovement.Instance.RecibirDanio();
                haGolpeado = true;
            }

            yield return null;
            tiempo += Time.deltaTime;
        }

        // Si estaba parado en un waypoint sigue en Idle; si no, vuelve a caminar
        animator.Play(esperando ? "Idle" : "Walk");
        siguienteAtaque = Time.time + tiempoEntreAtaques;
        atacando = false;
    }

    // Rectangulo delante del cuerpo del boss, del alto del cuerpo, hacia donde mira
    Rect ZonaGolpe()
    {
        Bounds cuerpo = col.bounds;
        // Con escala x negativa el boss mira a la derecha (ver MirarHacia)
        float frente = transform.localScale.x < 0 ? cuerpo.max.x : cuerpo.min.x - alcanceGolpe;
        return new Rect(frente, cuerpo.min.y, alcanceGolpe, cuerpo.size.y);
    }

    bool PlayerEnZonaGolpe()
    {
        PlayerMovement jugador = PlayerMovement.Instance;
        if (jugador == null || jugador.vida <= 0 || col == null) return false;

        Collider2D colJugador = jugador.GetComponent<Collider2D>();
        if (colJugador == null) return false;

        Bounds b = colJugador.bounds;
        return ZonaGolpe().Overlaps(new Rect(b.min.x, b.min.y, b.size.x, b.size.y));
    }

    // Con el boss seleccionado en el Editor: amarillo = distancia a la que ataca, rojo = zona de la llamarada
    void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(col.bounds.center, rangoAtaque);

        Rect zona = ZonaGolpe();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(zona.center, zona.size);
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

        StartCoroutine(AnimacionMuerte());
    }

    IEnumerator AnimacionMuerte()
    {
        // Mientras muere no bloquea al Player ni recibe mas balas
        if (col != null) col.enabled = false;

        animator.Play("Death", 0, 0f);

        yield return new WaitForSeconds(duracionMuerte);

        // Libera la camara de la zona del boss para que vuelva a seguir al Player hasta la bandera
        CameraFollow cameraFollow = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
        if (cameraFollow != null) cameraFollow.boss = false;

        gameObject.SetActive(false);
    }

    float DuracionClip(string nombre)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == nombre) return clip.length;
        }
        return 0f;
    }
}
