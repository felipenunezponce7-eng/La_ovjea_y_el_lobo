using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiFinal : MonoBehaviourPunCallbacks
{
    private bool volverMenu;

    public void VolverMenu()
    {
        volverMenu = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene("Mainmenu");
        }
    }

    public void JugarOtraVez()
    {
        volverMenu = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene("Mainmenu");
        }
    }

    public override void OnLeftRoom()
    {
        if (volverMenu)
        {
            SceneManager.LoadScene("Mainmenu");
        }
    }
}