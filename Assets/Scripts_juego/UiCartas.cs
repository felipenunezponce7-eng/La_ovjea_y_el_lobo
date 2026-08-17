using Photon.Pun;
using UnityEngine;
using TMPro;

public class UICartas : MonoBehaviour
{
    private ManoJugador mano;

    public TMP_Text textoPiedra;
    public TMP_Text textoPapel;
    public TMP_Text textoTijera;
    private bool cartaElegida;
    public GameObject cartaPiedra;
    public GameObject cartaPapel;
    public GameObject cartaTijera;
    private void Start()
    {
        ManoJugador[] manos =
            FindObjectsByType<ManoJugador>(
                FindObjectsSortMode.None
            );

        foreach (ManoJugador m in manos)
        {
            PhotonView pv =
                m.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                mano = m;
                break;
            }
        }

        if (mano == null)
        {
            
            return;
        }

        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (mano == null)
            return;

        textoPiedra.text =
            "Piedra: " + mano.piedras;

        textoPapel.text =
            "Papel: " + mano.papeles;

        textoTijera.text =
            "Tijera: " + mano.tijeras;
    }

    public void ElegirPiedra()
    {
        
        if (cartaElegida)
            return;

        cartaElegida = true;
        if (mano == null)
            return;

        if (mano.piedras <= 0)
            return;

        cartaElegida = true;

        mano.piedras--;

        ActualizarUI();
        cartaPapel.SetActive(false);
        cartaTijera.SetActive(false);
        if (DueloCartas.instancia == null)
        {
            
            return;
        }

        DueloCartas.instancia
            .ElegirCarta(Carta.Piedra);

   
    }

    public void ElegirPapel()
    {

        if (cartaElegida)
            return;

        cartaElegida = true;
        if (mano == null)
            return;

        if (mano.papeles <= 0)
            return;
        cartaElegida = true;
        mano.papeles--;

        ActualizarUI();
        cartaPiedra.SetActive(false);
        cartaTijera.SetActive(false);
        if (DueloCartas.instancia == null)
        {
            
            return;
        }

        DueloCartas.instancia
            .ElegirCarta(Carta.Papel);

    }

    public void ElegirTijera()
    {
   
        if (cartaElegida)
            return;

        cartaElegida = true;    
        if (mano == null)
            return;

        if (mano.tijeras <= 0)
            return;
        cartaElegida = true;
        mano.tijeras--;

        ActualizarUI();
        cartaPiedra.SetActive(false);
        cartaPapel.SetActive(false);
        if (DueloCartas.instancia == null)
        {
           
            return;
        }

        DueloCartas.instancia
            .ElegirCarta(Carta.Tijera);

        
    }
    public void OcultarCartas()
    {
        cartaPiedra.SetActive(false);
        cartaPapel.SetActive(false);
        cartaTijera.SetActive(false);
    }
    public void MostrarCartas()
    {
        gameObject.SetActive(true);

        cartaElegida = false;

        CartaHover[] hovers =
            GetComponentsInChildren<CartaHover>(true);

        foreach (CartaHover hover in hovers)
        {
            hover.Reiniciar();
        }

        if (mano.piedras > 0)
            cartaPiedra.SetActive(true);

        if (mano.papeles > 0)
            cartaPapel.SetActive(true);

        if (mano.tijeras > 0)
            cartaTijera.SetActive(true);

        ActualizarUI();

        Cursor.visible = true;

        Cursor.lockState =
            CursorLockMode.None;
    }
    public void ReiniciarSeleccion()
    {
        cartaElegida = false;
    }
}