using Photon.Pun;
using TMPro;
using UnityEngine;

public class Login_Manager : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_InputField inputNombre;
    public Launcher_Photon launcher;

    public void Login()
    {
        string nombre = inputNombre.text.Trim();

        if (string.IsNullOrEmpty(nombre))
        {
           
            return;
        }

        PhotonNetwork.NickName = nombre;

        PlayerPrefs.SetString("NombreJugador", nombre);

        launcher.Conectar();
    }

    private void Start()
    {
        inputNombre.text =
            PlayerPrefs.GetString("NombreJugador", "");
    }
}