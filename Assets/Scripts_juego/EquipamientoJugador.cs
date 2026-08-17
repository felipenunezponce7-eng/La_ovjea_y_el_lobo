using Photon.Pun;
using UnityEngine;

public class EquipamientoJugador : MonoBehaviourPun
{
    public GameObject revolver;
    public GameObject pistolaFogueo;
    public GameObject cuchillo;
    private void Start()
    {
        cuchillo.SetActive(false);
        revolver.SetActive(false);
        pistolaFogueo.SetActive(false);
    }

    public void DarRevolver()
    {
        revolver.SetActive(true);
        pistolaFogueo.SetActive(false);
    }

    public void DarFogueo()
    {
        revolver.SetActive(false);
        pistolaFogueo.SetActive(true);
    }
    public void DarCuchillo()
    {
        cuchillo.SetActive(true);
    }
    public void QuitarArmas()
    {
        revolver.SetActive(false);
        pistolaFogueo.SetActive(false);
        cuchillo.SetActive(false);
    }
}