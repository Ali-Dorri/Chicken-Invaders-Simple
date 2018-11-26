using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ControlsManager : MonoBehaviour, IPausable
{
    //
    //Fields
    //

    bool isLeftPressed = false;
    bool isRightPressed = false;
    [SerializeField] KeyCode leftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode rightKey = KeyCode.RightArrow;
    [SerializeField] KeyCode shootKey = KeyCode.Space;
    [SerializeField] KeyCode missileKey = KeyCode.LeftControl;
    [SerializeField] float shootGapTime = 0.2f;   
    int direction;
    bool isShooting = false;
    bool canShoot = true;

    //kinect variables
    [SerializeField] bool isUsingKinect = false;
    KinectPositionFinder kinectPositionFinder;
    float previousXPos;
    int currentXDirection; 

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsKinectEnabled
    {
        get
        {
            return isUsingKinect;
        }
        set
        {
            if(value != isUsingKinect)
            {
                isUsingKinect = value;
                isShooting = false;
                canShoot = true;

                if (isUsingKinect)
                {
                    PlayerShip playerShip = FindObjectOfType<PlayerShip>();

                    //disable keyboard effects
                    isLeftPressed = false;
                    isRightPressed = false;
                    playerShip.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                    //reset kinect status
                    previousXPos = playerShip.transform.position.x;
                    currentXDirection = 0;
                }
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        kinectPositionFinder = FindObjectOfType<KinectPositionFinder>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public int DetermineDirection()
    {
        if (isUsingKinect)
        {
            return DetermineDirectionByKinect();
        }
        else
        {
            return DetermineDirectionByKeyboard();
        }
    }

    int DetermineDirectionByKeyboard()
    {
        if (Input.GetKey(leftKey))
        {
            if (!isLeftPressed)
            {
                direction = -1;
                isLeftPressed = true;
            }
        }
        else
        {
            if (isLeftPressed)
            {
                isLeftPressed = false;

                if (isRightPressed)
                {
                    direction = 1;
                }
                else
                {
                    direction = 0;
                }
            }
        }

        if (Input.GetKey(rightKey))
        {
            if (!isRightPressed)
            {
                direction = 1;
                isRightPressed = true;
            }
        }
        else
        {
            if (isRightPressed)
            {
                isRightPressed = false;

                if (isLeftPressed)
                {
                    direction = -1;
                }
                else
                {
                    direction = 0;
                }
            }
        }

        return direction;
    }

    int DetermineDirectionByKinect()
    {
        if(kinectPositionFinder.RightHandPos.x != previousXPos)
        {
            float direction = kinectPositionFinder.RightHandPos.x - previousXPos;
            if(direction > 0)
            {
                currentXDirection = 1;
            }
            else if(direction < 0)
            {
                currentXDirection = -1;
            }
            else
            {
                currentXDirection = 0;
            }
        }

        return currentXDirection;
    }

    public void CheckShoot(UnityAction shoot)
    {
        //if Keyboard enabled
        if (!isUsingKinect)
        {
            if (Input.GetKey(shootKey))
            {
                if (canShoot)
                {
                    StartCoroutine(ShootRepeatedly(shoot));
                }
            }
            else if (Input.GetKeyUp(shootKey))
            {
                isShooting = false;
            }
        }
        //if Kinect enabled
        else
        {
            if(kinectPositionFinder.LeftHandPos.y > kinectPositionFinder.HeadPos.y)
            {
                if (canShoot)
                {
                    StartCoroutine(ShootRepeatedly(shoot));
                }
            }
            else
            {
                isShooting = false;
            }
        }    
    }

    IEnumerator ShootRepeatedly(UnityAction shoot)
    {
        isShooting = true;
        canShoot = false;

        while (isShooting)
        {
            shoot();
            yield return new WaitForSeconds(shootGapTime);
        }

        canShoot = true;
    }

    public bool IsShotMissile()
    {
        return Input.GetKeyDown(missileKey);
    }

    public void PauseOrResume(bool isPaused)
    {
        if (isPaused)
        {
            isLeftPressed = false;
            isRightPressed = false;
            direction = 0;
            isShooting = false;
        }
    }
}
