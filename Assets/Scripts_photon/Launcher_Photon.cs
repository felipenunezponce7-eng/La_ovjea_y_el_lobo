using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Launcher_Photon : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public TMP_Text textoEstado;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Conectar()
    {
        if (PhotonNetwork.IsConnected)
            return;

        ActualizarEstado(
            "Conectando a Photon..."
        );

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ActualizarEstado(
            "Conectado al servidor"
        );

        PhotonNetwork.AutomaticallySyncScene = true;

        SceneManager.LoadScene("Mainmenu");
    }

    public override void OnDisconnected(
        DisconnectCause cause
    )
    {
        ActualizarEstado(
            "Desconectado: " + cause
        );
    }

    public void BuscarPartida()
    {
        ActualizarEstado(
            "Buscando partida..."
        );

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(
        short returnCode,
        string message
    )
    {
        ActualizarEstado(
            "Creando sala..."
        );

        RoomOptions opciones =
            new RoomOptions();

        opciones.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(
            null,
            opciones
        );
    }

    public override void OnCreatedRoom()
    {
        ActualizarEstado(
            "Sala creada. Esperando rival..."
        );
    }

    public override void OnJoinedRoom()
    {
        int jugadores =
            PhotonNetwork.CurrentRoom.PlayerCount;

        if (jugadores == 1)
        {
            ActualizarEstado(
                "Esperando rival..."
            );
        }
        else
        {
            ActualizarEstado(
                "Jugadores listos"
            );
        }

        Debug.Log(
            "Entró a sala. Jugadores: " +
            jugadores +
            "/2"
        );
    }

    public override void OnPlayerEnteredRoom(
        Player newPlayer
    )
    {
        ActualizarEstado(
            "Jugador encontrado"
        );

        if (
            PhotonNetwork.CurrentRoom.PlayerCount
            == 2
        )
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ActualizarEstado(
                    "Iniciando partida..."
                );

                PhotonNetwork.LoadLevel(
                    "Mapa"
                );
            }
        }
    }

    public void ActualizarEstado(
        string mensaje
    )
    {
        Debug.Log(mensaje);

        if (textoEstado == null)
        {
            Menu menu =
                FindFirstObjectByType<Menu>();

            if (menu != null)
            {
                textoEstado =
                    menu.textoEstado;
            }
        }

        if (textoEstado != null)
        {
            textoEstado.text =
                mensaje;
        }
    }
}