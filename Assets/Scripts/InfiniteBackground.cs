using System.Collections.Generic;
using UnityEngine;

public class FondoInfinito : MonoBehaviour
{
    [SerializeField] private GameObject fondoPrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private float margen = 2f;

    private float ancho;
    private float origenX;
    private readonly Dictionary<int, GameObject> activos = new Dictionary<int, GameObject>();
    private readonly List<int> paraBorrar = new List<int>();

    void Start()
    {
        if (cam == null) cam = Camera.main;

        ancho = fondoPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        origenX = cam.transform.position.x;
    }

    void LateUpdate()
    {
        float mitadCamara = cam.orthographicSize * cam.aspect;
        float izquierda = cam.transform.position.x - mitadCamara - margen;
        float derecha = cam.transform.position.x + mitadCamara + margen;

        // Rango de índices que deben existir
        int indiceMin = IndiceEn(izquierda);
        int indiceMax = IndiceEn(derecha);

        // Crear los que falten
        for (int i = indiceMin; i <= indiceMax; i++)
        {
            if (!activos.ContainsKey(i))
            {
                Vector3 pos = new Vector3(origenX + i * ancho, 0.2f, 0f);
                activos[i] = Instantiate(fondoPrefab, pos, Quaternion.identity, transform);
            }
        }

        // Destruir los que quedaron fuera del rango
        paraBorrar.Clear();
        foreach (int i in activos.Keys)
        {
            if (i < indiceMin || i > indiceMax)
                paraBorrar.Add(i);
        }
        foreach (int i in paraBorrar)
        {
            Destroy(activos[i]);
            activos.Remove(i);
        }
    }

    // Índice del fondo que contiene la posición X
    private int IndiceEn(float x)
    {
        return Mathf.FloorToInt((x - origenX + ancho / 2f) / ancho);
    }
}