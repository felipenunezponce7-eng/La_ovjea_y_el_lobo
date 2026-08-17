using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;
public class Efecto_sangre : MonoBehaviour
{
    public GameObject sangre;

    private List<ParticleCollisionEvent> colievents;

    void Start()
    {
        colievents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {    
        if (other.CompareTag("Terreno"))
        {
            ParticleSystem part = GetComponent<ParticleSystem>();
            int numcolision = part.GetCollisionEvents(other, colievents);
            for (int i = 0; i < numcolision; i++)

            {
                Vector3 posicionimp = colievents[i].intersection;
                Vector3 direcionimp = colievents[i].normal;
                Quaternion rotacionimp = Quaternion.LookRotation(direcionimp);
                posicionimp.y += -1f;

                direcionimp.x = 0;

                Instantiate(sangre, posicionimp, rotacionimp);
                
            }
        }
     


        
    }
}
