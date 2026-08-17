using Photon.Pun;
using UnityEngine;

public class SpawnJugadores : MonoBehaviour
{
    public Transform spawn1;
    public Transform spawn2;

    void Start()
    {


        Vector3 posicion;
        Quaternion rotacion;

        if (PhotonNetwork.LocalPlayer.ActorNumber % 2 == 1)
        {
            posicion = spawn1.position;
            rotacion = spawn1.rotation;
        }
        else
        {
            posicion = spawn2.position;
            rotacion = spawn2.rotation;
        }

        GameObject jugador =
    PhotonNetwork.Instantiate(
        "Jugador",
        posicion,
        rotacion
    );

        Movimiento_jugador movimiento =
            jugador.GetComponent<Movimiento_jugador>();

        if (movimiento != null)
        {
            movimiento.puedeMoverse = false;
        }

        

    }
}