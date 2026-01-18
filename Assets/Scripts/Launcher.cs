using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject[] personajesDisponibles;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
            }
            else
            {
                PhotonNetwork.JoinOrCreateRoom("SalaJuego", new RoomOptions { MaxPlayers = 25 }, TypedLobby.Default);
            }
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("SalaJuego", new RoomOptions { MaxPlayers = 25 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("¡En la sala!");

        // Asignamos el nombre
        PhotonNetwork.LocalPlayer.NickName = "Jugador " + PhotonNetwork.LocalPlayer.ActorNumber;

        // Limpiamos la referencia vieja para asegurar el respawn
        PhotonNetwork.LocalPlayer.TagObject = null;

        Vector3 spawnPos = new Vector3(-4.65f, 3.5f, 0);
        int indexAleatorio = Random.Range(0, personajesDisponibles.Length);
        string personajeElegido = personajesDisponibles[indexAleatorio].name;

        GameObject miPersonaje = PhotonNetwork.Instantiate(personajeElegido, spawnPos, Quaternion.identity);
        PhotonNetwork.LocalPlayer.TagObject = miPersonaje;
    }

    public override void OnLeftRoom()
    {
        // Forzamos la carga de la escena al salir para limpiar fantasmas
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}