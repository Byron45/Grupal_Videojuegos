using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 desfase = new Vector3(0, 1.5f, -10f); // Ajusta según tu gusto
    public float suavizado = 5f;

    void LateUpdate()
    {
        // Solo se mueve si ya encontró al jugador local
        if (objetivo != null)
        {
            Vector3 posicionDeseada = objetivo.position + desfase;
            // Usamos Lerp para que el movimiento sea fluido y no brusco
            transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        }
    }
}