using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solidify : MonoBehaviour {

	public GameObject PlatformSolid;

		/*Since i'm just working on making the basics exist, the
		 * semisolid platforms will automatically turn solid if
		 * the player's FloorChecker touches it, and vise versa when it disconnects
		 */

	//Sets floor collision on after player passes through 
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "FloorChecker")
		{
			PlatformSolid.SetActive(true);
		}
	}
   
	//Sets floor collision off after player leaves
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "FloorChecker")
        {
            PlatformSolid.SetActive(false);
        }
    }
   
}
