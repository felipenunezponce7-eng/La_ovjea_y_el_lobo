using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :
    MonoBehaviourPun
{
    public static GameManager instancia;

    public bool faseCartas = true;
    public int actorGanadorDuelo = -1;
    public int actorPerdedorDuelo = -1;
    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        faseCartas = true;

        Cursor.visible = true;

        Cursor.lockState =
            CursorLockMode.None;
    }

    [PunRPC]
    public void FinalizarPartida(int actorMuerto)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.Log(
            "FINALIZAR PARTIDA -> actorMuerto="
            + actorMuerto
            + " | yo="
            + PhotonNetwork.LocalPlayer.ActorNumber
        );

        if (
            PhotonNetwork.LocalPlayer.ActorNumber
            == actorMuerto
        )
        {
            Debug.Log("DERROTA");

            SceneManager.LoadScene(
                "Pantalla_Derrota"
            );
        }
        else
        {
            Debug.Log("VICTORIA");

            SceneManager.LoadScene(
                "Pantalla_Victoria"
            );
        }
    }
}