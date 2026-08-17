using Photon.Pun;
using UnityEngine;

public class Movimiento_jugador : MonoBehaviourPun
{
    
    public Joystick joystickMovimiento;
    public Joystick joystickCamara;

    public Transform camaraJugador;
    public float velocidad = 5f;
    public float gravedad = -20f;
    public float fuerzaSalto = 8f;
    private Animator animator;
    private float velocidadVertical;
    public float sensibilidad = 60f;

    private CharacterController controller;

    private float rotacionVertical;
    public bool puedeMoverse = true;
    public bool ganaste;
   



    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (!photonView.IsMine)
        {
            camaraJugador.gameObject.SetActive(false);
            return;
        }

        joystickMovimiento =
            GameObject.Find("JoystickMovimiento")
            ?.GetComponent<Joystick>();

        joystickCamara =
            GameObject.Find("JoystickCamara")
            ?.GetComponent<Joystick>();
    }

    private void Update()
    {
        bool ganeDuelo =
    PhotonNetwork.LocalPlayer.ActorNumber ==
    GameManager.instancia.actorGanadorDuelo;

        bool perdiDuelo =
            PhotonNetwork.LocalPlayer.ActorNumber ==
            GameManager.instancia.actorPerdedorDuelo;
        if (!puedeMoverse)
            return;
        if (!photonView.IsMine)
            return;
        if (GameManager.instancia.faseCartas)
            return;
        if (ganeDuelo)
        {
            animator.SetBool("Sentao", true);
        }
        else if (perdiDuelo)
        {
            animator.SetBool("Sentao", true);
        }
        Movimiento();
        Mirar();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Saltar();


        }
        if (Input.GetMouseButtonDown(0) && perdiDuelo)                    
        {
          animator.SetTrigger("Acuchillar");
        }
        if (Input.GetMouseButtonDown(0) && ganeDuelo)
        {
            animator.SetTrigger("Disparar");
        }
    }
     public void Saltar()
    {
        if (controller.isGrounded)
        {
            velocidadVertical = fuerzaSalto;
            animator.SetTrigger("Jump");
        }
    }
    
    void Movimiento()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        animator.SetFloat("ejeX", h);
        animator.SetFloat("ejeZ", v);
        Vector3 direccion =
            transform.right * h +
            transform.forward * v;

        if (controller.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && controller.isGrounded)
        {
            velocidadVertical = fuerzaSalto;
        }

        velocidadVertical += gravedad * Time.deltaTime;
       

        direccion.y = velocidadVertical;

        controller.Move(
            direccion * velocidad * Time.deltaTime
        );
    }

    void Mirar()
    {
      
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (joystickCamara != null)
        {
            mouseX += joystickCamara.Horizontal;
            mouseY += joystickCamara.Vertical;
        }

        mouseX *= sensibilidad * Time.deltaTime;
        mouseY *= sensibilidad * Time.deltaTime;

        rotacionVertical -= mouseY;

        rotacionVertical = Mathf.Clamp(
            rotacionVertical,
            -80f,
            80f
        );

        camaraJugador.localRotation =
            Quaternion.Euler(rotacionVertical, 0, 0);

        transform.Rotate(
    0,
    mouseX * 5f,
    0
);
    }
}