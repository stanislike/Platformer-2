using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBroke : MonoBehaviour
{

    [SerializeField] private float renderTime = 0.5f;
    [SerializeField] private bool onColPlat = false;
    [SerializeField] private string playerTag = "Player";
    private float chrono = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        chrono = renderTime;
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();

        if (onColPlat)
        {
            chrono -= Time.deltaTime;
            textMesh.text = (Mathf.Round(chrono * 10.0f)* 0.1f).ToString() + "s";
        }
        else
        {
            textMesh.text = renderTime.ToString() + "s";
        }
        
        if (chrono <= 0)
        {
            Destroy(gameObject);
        }
        
        Debug.Log(chrono);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == playerTag)
        {
            Vector2 normal = -other.GetContact(0).normal;
            Debug.DrawRay(other.GetContact(0).point, normal, Color.red, 1f);

            if (normal.y > 0)
            {
                onColPlat = true;
            }
        }
    }
}
