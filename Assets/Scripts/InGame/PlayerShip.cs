using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : AliveEntity, IFriend
{
    //
    //Fields
    //

    ControlsManager controlsManager;
    EntityMovePhysics physics;
    [SerializeField] GameObject explosion;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    protected override GameObject BulletPrefab
    {
        get
        {
            return playerBulletPrefab;
        }
    }

    protected override float Health
    {
        get
        {
            return base.Health;
        }

        set
        {
            base.Health = value;

            if (health == 0)
            {
                WinLoseExecuter.Singleton.Lose();
                BackgroundMusicPlayer.Singleton.PlayLoseTrack();
                //create explosion
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    protected override void SetBulletPrefab()
    {
        if(playerBulletPrefab == null)
        {
            playerBulletPrefab = Resources.Load<GameObject>("Prefabs/Player Bullet");
        }      
    }

    protected override void Initialize()
    {
        controlsManager = GetComponent<ControlsManager>();
        physics = GetComponent<EntityMovePhysics>();
        if(bulletSpeed <= 0)
        {
            bulletSpeed = DEFAULT_PLAYER_BULLET_SPEED;
        }       
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    void Update ()
    {
        Move();
        controlsManager.CheckShoot(Shoot);
        controlsManager.CheckStopShooting();
        if (controlsManager.IsShotMissile())
        {
            ShootMissile();
        }
    }

    protected override void Move()
    {
        int direction = controlsManager.DetermineDirection();
        physics.Move(new Vector2(direction, 0));
    }

    protected override void SetBulletStatus(Bullet bullet)
    {
        bullet.TeamSide = Bullet.Group.Friend;
    }

    void ShootMissile()
    {

    }

    protected override bool IsOpponentBullet(GameObject ifBullet)
    {
        Bullet bullet = ifBullet.GetComponent<Bullet>();

        if(bullet != null)
        {
            if (bullet.TeamSide == Bullet.Group.Enemy)
            {
                return true;
            }
        }

        return false;
    }
}
