using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _coinText, _livesText;
    [SerializeField] private GameObject _gameOverTexto;

    public void UpdateCoinDisplay(int coins)
    {
        if (_coinText != null) _coinText.text = "Coins: " + coins;
    }

    public void UpdateLivesDisplay(int lives)
    {
        if (_livesText != null) _livesText.text = "Lives: " + lives;
    }

    public void MostrarGameOver()
    {
        if (_gameOverTexto != null)
        {
            _gameOverTexto.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ReiniciarJuego()
    {
        // Si perdemos, salimos de la sala para resetear todo el estado de red
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}