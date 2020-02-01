using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D pObject;
    // Start is called before the first frame update
    void Start()
    {
        pObject = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(h,v,0);
        
    }

    void FixedUpdate()
    {
    }
}
