using Photon.Pun;
using UnityEngine;

public class Cuchillo : MonoBehaviourPun
{

    public int dano = 35;


   
    private void OnTriggerEnter(
        Collider other
    )
    {
        if (!photonView.IsMine)
            return;

        Vida vida =
            other.GetComponent<Vida>();

        if (vida == null)
            return;

        vida.photonView.RPC(
            "RecibirDano",
            RpcTarget.All,
            dano
        );
    }
}
