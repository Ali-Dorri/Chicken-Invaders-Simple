using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //
    //Fields
    //

    StackArray<Bullet> stack;
    GameObject bulletPrefab;

    //statics
    static BulletPool playerBulletPool;
    static BulletPool chickenBulletPool;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static BulletPool PlayerBulletPool
    {
        get
        {
            if(playerBulletPool == null)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/Bullet Pool");
                prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                prefab.name = "Player Bullet Pool";
                playerBulletPool = prefab.GetComponent<BulletPool>();
                playerBulletPool.bulletPrefab = AliveEntity.PlayerBulletPrefab;
            }

            return playerBulletPool;
        }
    }

    public static BulletPool ChickenBulletPool
    {
        get
        {
            if (chickenBulletPool == null)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/Bullet Pool");
                prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                prefab.name = "Chicken Egg Pool";
                chickenBulletPool = prefab.GetComponent<BulletPool>();
                chickenBulletPool.bulletPrefab = AliveEntity.ChickenEggPrefab;
            }

            return chickenBulletPool;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Awake()
    {
        stack = new StackArray<Bullet>(10);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public Bullet Create(Vector3 position)
    {
        Bullet bullet;

        if (!stack.IsEmpty)
        {
            bullet = stack.Pop();
            bullet.gameObject.SetActive(true);
            bullet.transform.position = position;
        }
        else
        {
            bullet = Instantiate<GameObject>(bulletPrefab, position, Quaternion.identity).GetComponent<Bullet>();
        }

        return bullet;
    }

    public void Destroy(Bullet bullet)
    {
        stack.Push(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform, true);
    }
}
