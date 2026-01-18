using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool _yaRecogida = false;

    [Header("Ajustes Visuales")]
    [SerializeField] private float velocidadGiro = 100f;
    [SerializeField] private GameObject efectoRecoger;

    [Header("Sonido")]
    [SerializeField] private AudioClip _sonidoMoneda;

    void Update()
    {
        // Rotación integrada: Gira como un trompo
        transform.Rotate(0, velocidadGiro * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_yaRecogida)
        {
            _yaRecogida = true; // Bloqueo de seguridad inmediato

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddCoin();

                if (efectoRecoger != null)
                {
                    Instantiate(efectoRecoger, transform.position, Quaternion.identity);
                }

                if (_sonidoMoneda != null)
                {
                    AudioSource.PlayClipAtPoint(_sonidoMoneda, transform.position, 1.0f);
                }

                Destroy(this.gameObject);
            }
        }
    }
}