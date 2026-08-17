using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResultado : MonoBehaviour
{
    public static UIResultado instancia;

    public GameObject panelResultado;
    public TMP_Text textoResultado;

    public Image fondoNegro;

    private void Awake()
    {
        instancia = this;

        panelResultado.SetActive(false);
    }

   
    public void MostrarResultado(string mensaje)
    {
        

        panelResultado.SetActive(true);

        textoResultado.text = mensaje;
    }

    public void CambiarMensaje(string mensaje)
    {
       

        textoResultado.text = mensaje;
    }

    public void OcultarResultado()
    {
        

        panelResultado.SetActive(false);
    }
}