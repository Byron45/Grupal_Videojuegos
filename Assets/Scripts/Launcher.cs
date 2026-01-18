using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    // Cambiamos de una sola variable a un "Array" (lista) de personajes
    public GameObject[] personajesDisponibles;

    void Start()
    {
        Debug.Log("Conectando...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al Master.");
        PhotonNetwork.JoinOrCreateRoom("SalaJuego", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("¡En la sala!");

        // 1. Elegir un punto de aparición aleatorio para no caer uno encima de otro
        float randomX = Random.Range(-5f, 5f);
        Vector3 spawnPos = new Vector3(randomX, 2f, 0);

        // 2. Elegir un personaje al azar de la lista que tú pusiste
        int indexAleatorio = Random.Range(0, personajesDisponibles.Length);
        string personajeElegido = personajesDisponibles[indexAleatorio].name;

        // 3. Crear al personaje en la red
        PhotonNetwork.Instantiate(personajeElegido, spawnPos, Quaternion.identity);
    }
}