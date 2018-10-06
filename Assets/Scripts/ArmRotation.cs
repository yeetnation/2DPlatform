using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 0;
    bool armRotationRight = true;
	// Update is called once per frame
	void Update () {
        // subtracting the position of the player from the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;  
        difference.Normalize();         // normalizing the vector. Meaning that the sum of the vector will be equal to 1

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if ((rotZ > 90 || rotZ < -90) && armRotationRight == true)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            armRotationRight = false;
        }
        else if((rotZ < 90 && rotZ > -90) && armRotationRight == false)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            armRotationRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
        }
    }
}
