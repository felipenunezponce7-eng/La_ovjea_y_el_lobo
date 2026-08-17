using UnityEngine;

public class Destruir_Efecto : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}