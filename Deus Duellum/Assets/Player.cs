using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody rb;
   
    
    int playerId;
    string playerName;
    int avatarId;

	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float yMov = Input.GetAxis("Vertical");
        transform.Translate(xMov, 0, yMov);

        if(xMov != 0 || yMov != 0)
        {
            string msg = "MOVE|" + xMov.ToString() + "|" + yMov.ToString();
            
        }
    }
}
