using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKinectEnabler : MonoBehaviour
{
    KinectPositionFinder positionFinder;

	void Start ()
    {
        positionFinder = FindObjectOfType<KinectPositionFinder>();
	}

    public void EnableKinect()
    {
        positionFinder.IsEnabled = true;
    }

    public void DiasbleKinect()
    {
        positionFinder.IsEnabled = false;
    }
}
