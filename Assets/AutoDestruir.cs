using UnityEngine;
public class AutoDestruir : MonoBehaviour
{
    void Start() { Destroy(gameObject, 2.0f); } // Se borra a los 2 segundos
}