using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AliveEntity : MonoBehaviour, IPausable
{
    //
    //Fields
    //

    [SerializeField] protected float health = 20;
    [SerializeField] protected float bulletSpeed;
    Vector3 shootDeltaPosition = new Vector3(0, 0, 0);
    EntitySoundHandler soundhandler;

    //statics
    protected const float DEFAULT_PLAYER_BULLET_SPEED = 7;
    protected const float DEFAULT_ENEMY_BULLET_SPEED = 5;
    protected static GameObject chickenEggPrefab;
    protected static GameObject playerBulletPrefab;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public virtual float Health
    {
        get
        {
            return health;
        }
        protected set
        {
            //if entity will die
            if (value <= 0)
            {
                health = 0;
                soundhandler.PlayKilled();
                Destroy(gameObject);             
            }
            else
            {
                //if decreasing health
                if(health > value)  
                {
                    soundhandler.PlayDamage();
                }

                health = value;
            }
        }
    }

    public abstract GameObject BulletPrefab { get; }

    protected Vector2 ShootPosition
    {
        get
        {
            return shootDeltaPosition;
        }
        set
        {
            shootDeltaPosition = value;
        }
    }

    public static GameObject ChickenEggPrefab
    {
        get
        {
            if(chickenEggPrefab == null)
            {
                chickenEggPrefab = Resources.Load<GameObject>("Prefabs/Chicken Egg");
            }

            return chickenEggPrefab;
        }
    }

    public static GameObject PlayerBulletPrefab
    {
        get
        {
            if (playerBulletPrefab == null)
            {
                playerBulletPrefab = Resources.Load<GameObject>("Prefabs/Player Bullet");
            }

            return playerBulletPrefab;
        }
    }

    protected abstract BulletPool DesiredBulletPool { get; }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        SetBulletPrefab();
        Initialize();
        
        Transform shootTransform= transform.Find("Shoot Position");
        soundhandler = GetComponent<EntitySoundHandler>();
        if (shootTransform != null)
        {
            shootDeltaPosition = shootTransform.localPosition;
        }
    }

    protected abstract void SetBulletPrefab();

    protected abstract void Initialize();

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOpponentBullet(collision.gameObject))
        {
            Health -= collision.GetComponent<Bullet>().Damage;
        }
    }

    protected abstract bool IsOpponentBullet(GameObject ifBullet);

    protected abstract void Move();

    public void Shoot()
    {
        //GameObject bulletGameObject = Instantiate<GameObject>(BulletPrefab, transform.position + shootDeltaPosition,
        //                                                     Quaternion.identity);
        //Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        Bullet bullet = DesiredBulletPool.Create(transform.position + shootDeltaPosition);
        bullet.Speed = bulletSpeed;
        SetBulletStatus(bullet);

        //shoot sound
        soundhandler.PlayShoot();
    }

    protected abstract void SetBulletStatus(Bullet bullet);

    public virtual void PauseOrResume(bool isPaused)
    {
        enabled = !isPaused;
    }

    protected abstract void SetKillSound(EntitySoundHandler soundhandler);
}
