using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyScript : MonoBehaviour {


    public void Attack()
    {
        gameObject.transform.localScale = new Vector3(2f,2f,2f);
    }
}
