using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoBienvenida;
    public TMP_Text textoEstado;
    public Button botonBuscar;

    private void Start()
    {
        Debug.Log("MENU START");

        if (textoBienvenida != null)
        {
            textoBienvenida.text =
                "Bienvenido " +
                PhotonNetwork.NickName;
        }

        Launcher_Photon launcher =
            FindFirstObjectByType<Launcher_Photon>();

        Debug.Log(
            "Launcher encontrado: " +
            launcher
        );

        if (launcher != null)
        {
            launcher.textoEstado =
                textoEstado;

            Debug.Log(
                "TextoEstado asignado al Launcher"
            );
        }
        else
        {
            Debug.LogError(
                "No se encontró Launcher_Photon"
            );
        }
    }

    public void BuscarPartida()
    {
        botonBuscar.interactable = false;

        if (textoEstado != null)
        {
            textoEstado.text =
                "Buscando partida...";
        }

        Launcher_Photon launcher =
            FindFirstObjectByType<Launcher_Photon>();

        Debug.Log(
            "Launcher encontrado al buscar: " +
            launcher
        );

        if (launcher != null)
        {
            launcher.BuscarPartida();
        }
        else
        {
            Debug.LogError(
                "Launcher_Photon es NULL"
            );
        }
    }

    public void SalirJuego()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}