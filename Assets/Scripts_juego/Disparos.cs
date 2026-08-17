using Photon.Pun;
using UnityEngine;

public class Disparos : MonoBehaviourPun
{
    public Camera camaraJugador;
    public ParticleSystem muzzleFlash;
    public float distanciaDisparo = 100f;
    public GameObject efectoImpacto;
    public GameObject SangreEfecto;
    public int dano = 25;
    public AudioSource sonidoDisparo;
    public Transform puntodisparo;
    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (GameManager.instancia.faseCartas)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            Disparar();
        }

    }
    
    public void Disparar()
    {

        Debug.DrawRay(
    camaraJugador.transform.position,
    camaraJugador.transform.forward * distanciaDisparo,
    Color.red,
    1f
); 
       
        RaycastHit hit;

        PhotonNetwork.Instantiate(
    "DisparoFX",
    puntodisparo.position,
    puntodisparo.rotation
        );


        if (
    Physics.Raycast(
        camaraJugador.transform.position,
        camaraJugador.transform.forward,
        out hit,
        distanciaDisparo
    )
)
        {
            
            
            PhotonView objetivo =
    hit.collider.GetComponentInParent<PhotonView>();
            Vida vida =
    hit.collider.GetComponentInParent<Vida>();
            
            if (vida != null)
            {
                Instantiate(
                    SangreEfecto,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
            }
            else
            {
                Instantiate(
                    efectoImpacto,
                    hit.point + hit.normal * 0.02f,
                    Quaternion.LookRotation(hit.normal)
                );
            }
           
            if (objetivo != null)
            {
                objetivo.RPC(
    "RecibirDano",
    objetivo.Owner,
    dano
);
            }
        }

    }
}