using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_Adjust : MonoBehaviour
{
    public float zOffset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - zOffset);
        //if (zOffset != 0) //GetComponent<Collider>() == null
        //{
        //    Color tmp = GetComponent<SpriteRenderer>().color;
        //    tmp.a = .1f;
        //    GetComponent<SpriteRenderer>().color = tmp;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
