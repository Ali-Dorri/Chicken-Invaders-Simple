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

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public int DetermineDirection()
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

    public void CheckShoot(UnityAction shoot)
    {
        if (Input.GetKey(shootKey))
        {
            if (canShoot)
            {
                StartCoroutine(ShootRepeatedly(shoot));
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

    public void CheckStopShooting()
    {
        if (Input.GetKeyUp(shootKey))
        {
            isShooting = false;
        }
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
