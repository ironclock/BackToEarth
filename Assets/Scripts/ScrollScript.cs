using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    public float speed = 0f;
   // Update is called once per frame
   void Start()
   {        

   }
    void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,(Time.time * speed) % 1); 
    }


}
