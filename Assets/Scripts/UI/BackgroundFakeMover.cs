using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundFakeMover : MonoBehaviour
{
    RawImage image;
    [SerializeField] float xSpeed = 0;
    [SerializeField] float ySpeed = 0;

	void Start ()
    {
        image = GetComponent<RawImage>();
	}
	
	void Update ()
    {
        Rect uvRect = image.uvRect;

        //modify uvRect
        uvRect.x += xSpeed * Time.deltaTime;
        uvRect.y += ySpeed * Time.deltaTime;

        //check x and y not to exceed the 1
        if(uvRect.x >= 1)      
            uvRect.x -= (int)uvRect.x;
        if (uvRect.y >= 1)
            uvRect.y -= (int)uvRect.y;

        //move background
        image.uvRect = uvRect;
    }
}
