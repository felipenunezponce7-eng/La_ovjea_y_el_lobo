using UnityEngine;
using UnityEngine.SceneManagement;

public class DestruirMusicaMenu : MonoBehaviour
{
    Scene scene;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (scene.buildIndex >= 2)
        {
            Destroy(gameObject);
        }
    }
}
