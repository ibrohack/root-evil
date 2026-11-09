using System.Collections.Generic;
using UnityEngine;

public class FondoInfinito : MonoBehaviour
{
    [SerializeField] private GameObject fondoPrefab;   // Prefab con SpriteRenderer
    [SerializeField] private Camera cam;               // Cámara ortográfica
    [SerializeField] private float margen = 2f;        // Anticipación al generar/destruir

    private float ancho;
    private float ultimoX;                             // Centro X del último fondo creado
    private readonly Queue<GameObject> activos = new Queue<GameObject>();

    void Start()
    {
        if (cam == null) cam = Camera.main;

        // bounds ya tiene en cuenta la escala del prefab
        ancho = fondoPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        // Así el primer fondo se crea centrado en la cámara
        ultimoX = cam.transform.position.x - ancho;
    }

    void LateUpdate()
    {
        float mitadCamara = cam.orthographicSize * cam.aspect;
        float bordeDerecho = cam.transform.position.x + mitadCamara;
        float bordeIzquierdo = cam.transform.position.x - mitadCamara;

        // Crear fondos mientras el último no cubra el borde derecho de la cámara
        while (ultimoX + ancho / 2f < bordeDerecho + margen)
        {
            ultimoX += ancho;
            Vector3 pos = new Vector3(ultimoX, 0.2f, 0f);
            GameObject nuevo = Instantiate(fondoPrefab, pos, Quaternion.identity, transform);
            activos.Enqueue(nuevo);
        }

        // Destruir los que ya quedaron completamente fuera por la izquierda
        while (activos.Count > 0 && activos.Peek().transform.position.x + ancho / 2f < bordeIzquierdo - margen)
        {
            Destroy(activos.Dequeue());
        }
    }
}