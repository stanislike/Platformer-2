using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiAerial : MonoBehaviour
{
    
    [SerializeField] float casesHautBas = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = Vector2.up * casesHautBas;

        Vector2.Lerp(transform.position, target, 30f); 
    }
}
