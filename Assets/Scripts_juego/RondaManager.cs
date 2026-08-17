using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RondaManager : MonoBehaviourPun
{
    public static RondaManager instancia;

    public TMP_Text textoTiempo;

    private float tiempoActual;
    private bool rondaActiva;
    private bool cuchilloEntregado;
    private bool ruletaIniciada;
    double tiempoinicio;

    private int actorGanador;
    public Transform spawnRuleta1;
    public Transform spawnRuleta2;
    private void Awake()
    {
        instancia = this;
    }

    public void IniciarRonda(
    int ganador
)
    {
        

        actorGanador = ganador;
        if (PhotonNetwork.IsMasterClient)
        {
     
            photonView.RPC(nameof(SincronizarInicio), RpcTarget.All, PhotonNetwork.Time);
        }
       
        tiempoActual = 60f;

        rondaActiva = true;

        cuchilloEntregado = false;
    }

    private void Update()
    {

        if (!rondaActiva)
            return;

       

        tiempoActual = 60F - (float)(PhotonNetwork.Time - tiempoinicio);
        textoTiempo.text = Mathf.CeilToInt(tiempoActual).ToString();
        if (
            tiempoActual <= 30f &&
            !cuchilloEntregado
        )
        {
            cuchilloEntregado = true;

            DarCuchilloAlPerdedor();
        }

        if (tiempoActual <= 0) 
        {
            rondaActiva = false;

           if (PhotonNetwork.IsMasterClient)
            {
                FinalizarTiempo();
            }
        }
    }
    [PunRPC]
    private void ActualizarTiempoRPC(
    int tiempo
)
    {
        if (textoTiempo != null)
        {
            textoTiempo.text =
                tiempo.ToString();
        }
    }


    [PunRPC]

    private void SincronizarInicio(double inicio)
    {
        tiempoinicio = inicio;
        rondaActiva = true;
    }
    private void DarCuchilloAlPerdedor()
    {
        EquipamientoJugador[] equipos =
            FindObjectsByType<EquipamientoJugador>(
                FindObjectsSortMode.None
            );

        foreach (EquipamientoJugador equipo in equipos)
        {
            PhotonView pv =
                equipo.GetComponent<PhotonView>();

            if (
                pv.Owner.ActorNumber
                != actorGanador
            )
            {
                equipo.DarCuchillo();
            }
        }

        
    }

    private void FinalizarTiempo()
    {
        if (ruletaIniciada)
            return;

        ruletaIniciada = true;

       

        photonView.RPC(
            nameof(IniciarRuleta),
            RpcTarget.All
        );
    }

    [PunRPC]
    private void IniciarRuleta()
    {
       

        StartCoroutine(
            SecuenciaRuleta()
        );
    }
    private IEnumerator SecuenciaRuleta()
    {
        EquipamientoJugador[] equipos =
            FindObjectsByType<EquipamientoJugador>(
                FindObjectsSortMode.None
            );

        foreach (EquipamientoJugador e in equipos)
        {
            e.QuitarArmas();
        }

        TeletransportarJugadores();

        UIResultado.instancia
            ?.MostrarResultado(
                "SE ACABÓ EL TIEMPO"
            );

        yield return new WaitForSeconds(3f);

        UIResultado.instancia
            ?.CambiarMensaje(
                "LA SUERTE DECIDIRÁ"
            );

        yield return new WaitForSeconds(3f);

        RuletaRusaManager.instancia
            ?.ComenzarRuleta();
    }
    private void TeletransportarJugadores()
    {
        Movimiento_jugador[] jugadores =
            FindObjectsByType<Movimiento_jugador>(
                FindObjectsSortMode.None
            );

        foreach (Movimiento_jugador jugador in jugadores)
        {
            PhotonView pv =
                jugador.GetComponent<PhotonView>();

            CharacterController cc =
                jugador.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            if (
                pv.Owner.ActorNumber ==
                PhotonNetwork.PlayerList[0]
                .ActorNumber
            )
            {
                jugador.transform.position =
                    spawnRuleta1.position;

                jugador.transform.rotation =
                    spawnRuleta1.rotation;
            }
            else
            {
                jugador.transform.position =
                    spawnRuleta2.position;

                jugador.transform.rotation =
                    spawnRuleta2.rotation;
            }

            if (cc != null)
            {
                cc.enabled = true;
            }

            jugador.puedeMoverse = false;
        }
    }

}