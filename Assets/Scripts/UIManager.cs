using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // NECESARIO PARA REINICIAR

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _coinText, _livesText;
    [SerializeField] private GameObject _gameOverTexto;

    public void UpdateCoinDisplay(int coins)
    {
        _coinText.text = "Coins: " + coins;
    }

    public void UpdateLivesDisplay(int lives)
    {
        _livesText.text = "Lives: " + lives;
    }

    public void MostrarGameOver()
    {
        if (_gameOverTexto != null)
        {
            _gameOverTexto.SetActive(true);
            // Mostramos el cursor para poder hacer clic en el botón
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // FUNCIÓN PARA EL BOTÓN
    public void ReiniciarJuego()
    {
        // Carga la escena actual de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}