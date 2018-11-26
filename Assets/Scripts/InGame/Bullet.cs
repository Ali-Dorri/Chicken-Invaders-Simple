using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour, IPausable
{
    //
    //Concept Definition
    //

    public enum Group { Friend, Enemy }
    enum BulletLayer { Friend, Enemy }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    [SerializeField] float damage = 5;
    [SerializeField] float speed = 3;
    Type crashableType;
    /// <summary>
    /// The direction the bullet will go in y axis when shooted.
    /// </summary>
    int direction = 0;
    Group group;

    //static
    const float DEFAULT_SPEED = 5;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public float Damage
    {
        get
        {
            return damage;
        }
    }

    protected Type CrashableType
    {
        get
        {
            return crashableType;
        }
        set
        {
            if(value != typeof(IEnemy) && value != typeof(IFriend))
            {
                throw new Exception("The Type that bullet can crash shuld be just IEnemy or IFirend!");
            }

            crashableType = value;
        }
    }

    public Group TeamSide
    {
        get
        {
            return group;
        }
        set
        {
            if(value == Group.Friend)
            {
                CrashableType = typeof(IEnemy);
                direction = 1;
                gameObject.layer = GetBulletLayerNumber(BulletLayer.Friend);
            }
            else
            {
                CrashableType = typeof(IFriend);
                direction = -1;
                gameObject.layer = GetBulletLayerNumber(BulletLayer.Enemy);
            }

            group = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            if(value > 0)
            {
                speed = value;
            }
            else
            {
                speed = DEFAULT_SPEED;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void Update()
    {
        Vector3 position = transform.localPosition;
        position += new Vector3(0, speed * direction * Time.deltaTime, 0);
        transform.localPosition = position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject crasher = collider.gameObject;

        if (crasher.GetComponent(crashableType))
        {
            //Destroy(gameObject);
            if(crashableType == typeof(IFriend))
            {
                BulletPool.ChickenBulletPool.Destroy(this);
            }
            else
            {
                BulletPool.PlayerBulletPool.Destroy(this);
            }
        }
        else if (crasher.tag == "Shredder")
        {
            //Destroy(gameObject);
            if (crashableType == typeof(IFriend))
            {
                BulletPool.ChickenBulletPool.Destroy(this);
            }
            else
            {
                BulletPool.PlayerBulletPool.Destroy(this);
            }
        }
    }

    //static
    static int GetBulletLayerNumber(BulletLayer bulletLayer)
    {
        //Friend Bullet Layer: 8         Enemy Bullet Layer: 9
        if (bulletLayer== BulletLayer.Friend)
        {
            return 8;
        }
        else
        {
            return 9;
        }
    }

    public void PauseOrResume(bool isPaused)
    {
        enabled = !isPaused;
    }
}
