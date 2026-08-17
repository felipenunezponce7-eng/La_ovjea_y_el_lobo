using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiJugador : MonoBehaviour
{
    public static UiJugador instancia;

    public TMP_Text textoVida;
    public Image danoOverlay;

    private float velocidadFade = 2f;

    private void Awake()
    {
        instancia = this;
    }
   
    private void Update()
    {
      
        if (danoOverlay.color.a > 0)
        {
            Color c = danoOverlay.color;

            c.a -= velocidadFade * Time.deltaTime;

            danoOverlay.color = c;
        }
    }

    public void ActualizarVida(int vida)
    {
        textoVida.text =
            "Vida: " + vida;
    }

    public void MostrarDano()
    {

        Debug.Log("Mostrar dano");

        Color c = danoOverlay.color;

        c.a = 0.5f;

        danoOverlay.color = c;
    }
}