using UnityEngine;

public class RotarObjeto : MonoBehaviour
{
    [SerializeField] private float velocidadGiro = 100f;

    void Update()
    {
        // El truco está en "Space.World". 
        // Esto le dice: "Gira como un trompo en el suelo", no como una rueda.
        transform.Rotate(0, velocidadGiro * Time.deltaTime, 0, Space.World);
    }
}