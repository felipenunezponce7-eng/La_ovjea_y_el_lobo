using UnityEngine;

public class ManoJugador : MonoBehaviour
{
    public int piedras = 2;
    public int papeles = 2;
    public int tijeras = 2;

    public bool SinCartas()
    {
        return piedras <= 0 &&
               papeles <= 0 &&
               tijeras <= 0;
    }
}

