using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if ((Input.GetKeyDown(KeyCode.E)))
            this.gameObject.transform.parent = collision.gameObject.transform;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
