using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RuletaRusaManager : MonoBehaviourPun
{
    public static RuletaRusaManager instancia;

    [Header("UI")]
    public GameObject panelRuleta;
    public GameObject panelCartas;
    public TMP_Text textoRuleta;
    public Button botonDisparar;
    public AudioSource sonidoDisparo;
    public AudioSource SonidoTambor;

    private int posicionBala;
    private int disparoActual;

    private int actorTurno;

    private void Awake()
    {
        instancia = this;
    }

    public void ComenzarRuleta()
    {
        panelRuleta.SetActive(true);
        panelCartas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState =
            CursorLockMode.None;
        disparoActual = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            posicionBala = Random.Range(1, 7);

            photonView.RPC(
                nameof(SincronizarRuleta),
                RpcTarget.All,
                posicionBala
            );
        }
    }

    [PunRPC]
    private void SincronizarRuleta(
        int bala
    )
    {
        posicionBala = bala;

        actorTurno =
            PhotonNetwork.PlayerList[0]
            .ActorNumber;

        ActualizarTurno();
    }

    void ActualizarTurno()
    {
        bool esMiTurno =
            PhotonNetwork.LocalPlayer.ActorNumber
            == actorTurno;

        botonDisparar.interactable =
            esMiTurno;

        if (esMiTurno)
        {
            textoRuleta.text =
                "TU TURNO";
        }
        else
        {
            textoRuleta.text =
                "TURNO DEL RIVAL";
        }
    }

    public void Disparar()
    {
        if (
            PhotonNetwork.LocalPlayer.ActorNumber
            != actorTurno
        )
        {
            return;
        }

        photonView.RPC(
            nameof(RPC_Disparar),
            RpcTarget.All
        );
    }

    [PunRPC]
    private void RPC_Disparar()
    {
        disparoActual++;

        if (
            disparoActual ==
            posicionBala
        )
        {
            textoRuleta.text =
                "BANG";
            sonidoDisparo.Play();
            Invoke(
                nameof(FinalizarRuleta),
                2f
            );

            return;
        }

        textoRuleta.text =
            "CLICK";
        SonidoTambor.Play();
        Invoke(
            nameof(CambiarTurno),
            1.5f
        );
    }

    private void CambiarTurno()
    {
        if (
            actorTurno ==
            PhotonNetwork.PlayerList[0]
            .ActorNumber
        )
        {
            actorTurno =
                PhotonNetwork.PlayerList[1]
                .ActorNumber;
        }
        else
        {
            actorTurno =
                PhotonNetwork.PlayerList[0]
                .ActorNumber;
        }

        ActualizarTurno();
    }

    private void FinalizarRuleta()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(
            nameof(ResultadoRuleta),
            RpcTarget.All,
            actorTurno
        );
    }

    [PunRPC]
    private void ResultadoRuleta(
    int actorMuerto
)
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        if (
            PhotonNetwork.LocalPlayer.ActorNumber
            == actorMuerto
        )
        {
            SceneManager.LoadScene(
               "Pantalla_Victoria"
            );
        }
        else
        {
            SceneManager.LoadScene(
               "Pantalla_Derrota"
            );
        }
    }
}