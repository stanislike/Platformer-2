using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject ennemi;
    [SerializeField] public int nbEnnemi;
    [SerializeField] private int nbEnnemiMax;
    [SerializeField] private float chrono;
    [SerializeField] private float spawnFrequency;
    
    // Start is called before the first frame update
    void Start()
    {
        chrono = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        chrono += Time.deltaTime;

        if (chrono >= spawnFrequency && nbEnnemi < nbEnnemiMax)
        {
            Instantiate(ennemi, transform.position, Quaternion.identity);
            nbEnnemi += 1;
            chrono = 0f;
        }
    }
}
