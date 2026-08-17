using Photon.Pun;
using UnityEngine;
using System.Collections;

public class DueloCartas : MonoBehaviourPun
{
    public static DueloCartas instancia;
    
    private Carta? cartaJugador1;
    private Carta? cartaJugador2;
    private int actor1;
    private int actor2;
    public int ultimoGanador;
    public GameObject mirilla;
    private void Awake()
    {
        instancia = this;
    }

    public void ElegirCarta(Carta carta)
    {
        photonView.RPC(
            nameof(RecibirCarta),
            RpcTarget.MasterClient,
            PhotonNetwork.LocalPlayer.ActorNumber,
            (int)carta
        );
    }

    [PunRPC]
    private void RecibirCarta(
        int actorNumber,
        int carta
    )
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (actor1 == 0)
        {
            actor1 = actorNumber;
        }
        else if (actor2 == 0 && actorNumber != actor1)
        {
            actor2 = actorNumber;
        }

        if (actorNumber == actor1)
        {
            cartaJugador1 = (Carta)carta;
        }
        else
        {
            cartaJugador2 = (Carta)carta;
        }

        

        


        if (
            cartaJugador1.HasValue &&
            cartaJugador2.HasValue
        )
        {
            ResolverDuelo();
        }
    }

    private void ResolverDuelo()
    {
        Carta j1 = cartaJugador1.Value;
        Carta j2 = cartaJugador2.Value;

        int ganador = 0;

        if (j1 == j2)
        {
            ganador = 0;
        }
        else if (
            (j1 == Carta.Piedra && j2 == Carta.Tijera) ||
            (j1 == Carta.Papel && j2 == Carta.Piedra) ||
            (j1 == Carta.Tijera && j2 == Carta.Papel)
        )
        {
            ganador = actor1;
        }
        else
        {
            ganador = actor2;
        }
        
        photonView.RPC(
            nameof(ResultadoDuelo),
            RpcTarget.All,
            ganador,
            actor1,
            actor2,
            (int)j1,
            (int)j2
        );
    }

    [PunRPC]
    private void ResultadoDuelo(
        int actorGanador,
        int actor1,
        int actor2,
        int cartaJ1,
        int cartaJ2
    )

    {
        ultimoGanador = actorGanador;
        if (actorGanador != 0)
        {
            GameManager.instancia.actorGanadorDuelo =
                actorGanador;

            if (actorGanador == actor1)
            {
                GameManager.instancia.actorPerdedorDuelo =
                    actor2;
            }
            else
            {
                GameManager.instancia.actorPerdedorDuelo =
                    actor1;
            }
        }
        EquipamientoJugador equipoLocal = null;

        EquipamientoJugador[] equipos =
            FindObjectsByType<EquipamientoJugador>(
                FindObjectsSortMode.None
            );

        foreach (EquipamientoJugador equipo in equipos)
        {
            PhotonView pv =
                equipo.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                equipoLocal = equipo;
                break;
            }
        }

        Carta miCarta;
        Carta cartaRival;

        if (
            PhotonNetwork.LocalPlayer.ActorNumber
            == actor1
        )
        {
            miCarta = (Carta)cartaJ1;
            cartaRival = (Carta)cartaJ2;
        }
        else
        {
            miCarta = (Carta)cartaJ2;
            cartaRival = (Carta)cartaJ1;
        }

       

        StartCoroutine(
            IniciarPartida(
                actorGanador,
                equipoLocal,
                miCarta,
                cartaRival
            )
        );

    }

    private IEnumerator IniciarPartida(
    int actorGanador,
    EquipamientoJugador equipoLocal,
    Carta miCarta,
    Carta cartaRival
)
    {
        bool gane =
            PhotonNetwork.LocalPlayer.ActorNumber
            == actorGanador;

        if (actorGanador == 0)
        {
            UIResultado.instancia
                ?.MostrarResultado(
                    "EMPATE"
                );

            yield return new WaitForSeconds(2f);

            if (AmbosSinCartas())
            {
                UIResultado.instancia
                    ?.CambiarMensaje(
                        "Sin cartas...\nSorteando destino"
                    );

                yield return new WaitForSeconds(3f);

                if (PhotonNetwork.IsMasterClient)
                {
                    int ganadorAleatorio;

                    if (Random.Range(0, 2) == 0)
                    {
                        ganadorAleatorio = actor1;
                    }
                    else
                    {
                        ganadorAleatorio = actor2;
                    }

                    photonView.RPC(
                        nameof(ResultadoDuelo),
                        RpcTarget.All,
                        ganadorAleatorio,
                        actor1,
                        actor2,
                        (int)Carta.Piedra,
                        (int)Carta.Piedra
                    );
                }

                yield break;
            }

            UIResultado.instancia
                ?.CambiarMensaje(
                    "Las cartas fueron consumidas\nElijan nuevamente"
                );

            yield return new WaitForSeconds(2f);

            UIResultado.instancia
                ?.OcultarResultado();

            UICartas ui =
                FindFirstObjectByType<UICartas>();

            if (ui != null)
            {
                ui.ReiniciarSeleccion();
                ui.MostrarCartas();
            }

            cartaJugador1 = null;
            cartaJugador2 = null;

            this.actor1 = 0;
            this.actor2 = 0;

            yield break;
        }

        if (gane)
        {
            UIResultado.instancia
                ?.MostrarResultado(
                    "GANASTE EL DUELO"
                );
        }
        else
        {
            UIResultado.instancia
                ?.MostrarResultado(
                    "PERDISTE EL DUELO"
                );
        }

        yield return new WaitForSeconds(2f);

        UIResultado.instancia
            ?.CambiarMensaje(
                "Tu carta: " + miCarta +
                "\nCarta rival: " + cartaRival
            );

        yield return new WaitForSeconds(2f);

        if (gane)
        {
            UIResultado.instancia
                ?.CambiarMensaje(
                    "ASESÍNALO"
                );
        }
        else
        {
            UIResultado.instancia
                ?.CambiarMensaje(
                    "HUYE"
                );
        }

        yield return new WaitForSeconds(2f);

        if (!gane)
        {
            equipoLocal?.DarFogueo();
         

        }
        UICartas uiCartas =
    FindFirstObjectByType<UICartas>();

        if (uiCartas != null)
        {
            uiCartas.OcultarCartas();
        }
        GameManager.instancia
            .faseCartas = false;

        Cursor.visible = false;

        Cursor.lockState =
            CursorLockMode.Locked;

        if (gane)
        {
            StartCoroutine(
                RetrasarGanador(
                    equipoLocal
                )
            );
        }
        else
        {
            ActivarMovimientoLocal();

            yield return new WaitForSeconds(2f);

            UIResultado.instancia
                ?.OcultarResultado();
        }

        cartaJugador1 = null;
        cartaJugador2 = null;

        this.actor1 = 0;
        this.actor2 = 0;
    }
    private bool AmbosSinCartas()
    {
        ManoJugador[] manos =
            FindObjectsByType<ManoJugador>(
                FindObjectsSortMode.None
            );

        int sinCartas = 0;

        foreach (ManoJugador mano in manos)
        {
            if (mano.SinCartas())
            {
                sinCartas++;
            }
        }

        return sinCartas >= 2;
    }
    private void ActivarMovimientoLocal()
    {
        Movimiento_jugador[] jugadores =
            FindObjectsByType<Movimiento_jugador>(
                FindObjectsSortMode.None
            );

        foreach (Movimiento_jugador jugador in jugadores)
        {
            PhotonView pv =
                jugador.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                jugador.puedeMoverse = true;

               

                break;
            }
        }
    }

    private IEnumerator RetrasarGanador(
    EquipamientoJugador equipoLocal
)
    {
        Movimiento_jugador jugador = null;

        Movimiento_jugador[] jugadores =
            FindObjectsByType<Movimiento_jugador>(
                FindObjectsSortMode.None
            );

        foreach (Movimiento_jugador j in jugadores)
        {
            PhotonView pv =
                j.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                jugador = j;
                break;
            }
        }

        

        UIResultado.instancia
            ?.MostrarResultado(
                "PREPARA EL REVÓLVER"
            );
        if (jugador != null)
        {
            jugador.puedeMoverse = false;
        }
        yield return new WaitForSeconds(5f);

        equipoLocal?.DarRevolver();

        UIResultado.instancia
            ?.CambiarMensaje(
                "COMIENZA LA CACERÍA"
            );
        
       

        yield return new WaitForSeconds(1.5f);

        UIResultado.instancia
            ?.OcultarResultado();
        mirilla.SetActive( true );
        if (jugador != null)
        {
            jugador.puedeMoverse = true;
        }
        
        photonView.RPC(
    nameof(IniciarCaceriaRPC),
    RpcTarget.All,
    ultimoGanador
);

    }

    [PunRPC]
    private void IniciarCaceriaRPC(
    int ganador
)
    {
        if (
            RondaManager.instancia != null
        )
        {
            RondaManager.instancia
                .IniciarRonda(
                    ganador
                );
        }
    }
}