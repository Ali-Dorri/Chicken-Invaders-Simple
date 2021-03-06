﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectPositionFinder : MonoBehaviour, IPausable
{
    //
    //Fields
    //

    public KinectWrapper.NuiSkeletonPositionIndex trackedRightHand = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex trackedLeftHand = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    public KinectWrapper.NuiSkeletonPositionIndex trackedHead = KinectWrapper.NuiSkeletonPositionIndex.Head;

    Vector3 rightHandPos;
    Vector3 leftHandPos;
    Vector3 headPos;
    ControlsManager controlsManager;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public Vector3 RightHandPos
    {
        get
        {
            return rightHandPos;
        }
    }

    public Vector3 LeftHandPos
    {
        get
        {
            return leftHandPos;
        }
    }

    public Vector3 HeadPos
    {
        get
        {
            return headPos;
        }
    }

    public bool IsEnabled
    {
        get
        {
            return controlsManager.IsKinectEnabled;
        }
        set
        {
            controlsManager.IsKinectEnabled = value;
            enabled = value;

            //set positions according to the current situation
            if (value)
            {
                Transform player = FindObjectOfType<PlayerShip>().transform;

                rightHandPos.x = player.position.x;
                rightHandPos.y = player.position.y;

                // let the left hand be under the head not to let the player ship shoot
                leftHandPos.y = 0;
                headPos.y = 1;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    void Start()
    {
        //distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
        controlsManager = FindObjectOfType<ControlsManager>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    void Update()
    {
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            if (manager.IsUserDetected())
            {
                FindPosition(manager, trackedRightHand, ref rightHandPos);
                FindPosition(manager, trackedLeftHand, ref leftHandPos);
                FindPosition(manager, trackedHead, ref headPos);
            }
        }
            
    }


    private void FindPosition(KinectManager manager, KinectWrapper.NuiSkeletonPositionIndex TrackedJoint
                                , ref Vector3 positionInWorldSpace)
    {       
        int iJointIndex = (int)TrackedJoint;
        uint userId = manager.GetPlayer1ID();

        if (manager.IsJointTracked(userId, iJointIndex))
        {
            Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);

            if (posJoint != Vector3.zero)
            {
                // 3d position to depth
                Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);

                // depth pos to color pos
                Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);

                //(scaleX, scaleY, distanceToCamera) == viewportPoint
                float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;

                positionInWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, 0));           
            }
        }
    }

    public void PauseOrResume(bool isPaused)
    {
        enabled = !isPaused;
    }
}
