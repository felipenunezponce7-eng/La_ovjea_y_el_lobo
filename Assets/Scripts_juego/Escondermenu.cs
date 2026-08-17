using UnityEngine;

public class Escondermenu : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}