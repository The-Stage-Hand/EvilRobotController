using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastableBlock : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletShot")
        {
            //destroy both shot and block
            GameObject.Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
