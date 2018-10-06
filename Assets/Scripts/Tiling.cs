using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;         // the offset so calculation isn't done when it's already over it

    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;       // used if the object is not tilable

    private float spriteWidth = 0f;         // width of the sprite
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        // Does it still need buddys if not do noting
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            // calculate the cameras extend (half the width) of what the camera can see in world coordinates
            float camHorizantalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position where the camera can see the edge of the sprite(element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizantalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizantalExtend;

            // checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}

    // function that creates another sprite on the side required
    void MakeNewBuddy(int rightOrLeft)
    {
        
        // calculating the new position for a new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y,myTransform.position.z);
        // instantiating our new buddy and storing him in a variable
        Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);

        // if not tilable reverse x size of object to get rid of ugly seams
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        // parent from new buddy set to former sprite
        newBuddy.parent = myTransform.parent;

        // if right/left buddy is created set its left/right buddy side to true so it doesn't create another sprite
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
