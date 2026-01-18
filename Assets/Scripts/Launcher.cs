using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject[] personajesDisponibles;

    void Start()
    {
        // VERIFICACIÓN: Si ya estamos conectados (por un reinicio de escena), 
        // no intentamos conectar de nuevo, solo verificamos si ya estamos en una sala.
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Ya estábamos conectados. Verificando estado...");
            if (PhotonNetwork.InRoom)
            {
                // Si ya estamos en la sala, llamamos directamente a la lógica de spawn
                OnJoinedRoom();
            }
            else
            {
                // Si estamos conectados pero fuera de la sala, intentamos entrar
                PhotonNetwork.JoinOrCreateRoom("SalaJuego", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
            }
        }
        else
        {
            Debug.Log("Conectando...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al Master.");
        PhotonNetwork.JoinOrCreateRoom("SalaJuego", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("¡En la sala!");

        // 1. Posición fija sobre la primera plataforma
        // Usamos los mismos valores que definiste para el Respawn
        Vector3 spawnPos = new Vector3(-4.65f, 3.5f, 0);

        // 2. Elegir un personaje al azar de tu lista
        int indexAleatorio = Random.Range(0, personajesDisponibles.Length);
        string personajeElegido = personajesDisponibles[indexAleatorio].name;

        // 3. Crear al personaje en la red
        PhotonNetwork.Instantiate(personajeElegido, spawnPos, Quaternion.identity);
    }
}