using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vida : MonoBehaviourPun
{
    public int vida = 100;
    private bool muerto = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            UiJugador.instancia
                ?.ActualizarVida(vida);
        }
    }
    [PunRPC]
    public void RecibirDano(int dano)
    {
        if (!photonView.IsMine)
            return;

        vida -= dano;

        UiJugador.instancia
            ?.ActualizarVida(vida);

        UiJugador.instancia
            ?.MostrarDano();

        if (vida <= 0 && !muerto)
        {
            muerto = true;
            Morir();
        }
    }
    private void Morir()
    {
        Debug.Log(
            "MORIR -> local="
            + PhotonNetwork.LocalPlayer.ActorNumber
            + " owner="
            + photonView.Owner.ActorNumber
            + " IsMine="
            + photonView.IsMine
        );

        if (!photonView.IsMine)
            return;

        GameManager.instancia.photonView.RPC(
            "FinalizarPartida",
            RpcTarget.All,
            photonView.Owner.ActorNumber
        );
    }


}