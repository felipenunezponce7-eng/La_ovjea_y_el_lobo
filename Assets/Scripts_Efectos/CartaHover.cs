using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CartaHover :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public Image glow;

    private Vector3 escalaOriginal;
    private Vector3 posicionOriginal;

    private bool seleccionada;

    private void Start()
    {
        escalaOriginal =
            transform.localScale;

        posicionOriginal =
            transform.localPosition;

        if (glow != null)
        {
            glow.enabled = false;
        }
    }



    public void Reiniciar()
    {
        seleccionada = false;

        transform.localScale =
            escalaOriginal;

        transform.localPosition =
            posicionOriginal;

        if (glow != null)
        {
            glow.enabled = false;
        }
    }

    public void OnPointerEnter(
        PointerEventData eventData
    )
    {
        if (seleccionada)
            return;

        transform.localScale =
            escalaOriginal * 1.15f;

        transform.localPosition =
            posicionOriginal +
            new Vector3(0, 25f, 0);

        if (glow != null)
        {
            glow.enabled = true;
        }
    }

    public void OnPointerExit(
        PointerEventData eventData
    )
    {
        if (seleccionada)
            return;

        Reiniciar();
    }

    public void OnPointerClick(
        PointerEventData eventData
    )
    {
        seleccionada = true;

        transform.localScale =
            escalaOriginal * 1.2f;

        transform.localPosition =
            posicionOriginal +
            new Vector3(0, 40f, 0);

        if (glow != null)
        {
            glow.enabled = true;
        }
    }

}