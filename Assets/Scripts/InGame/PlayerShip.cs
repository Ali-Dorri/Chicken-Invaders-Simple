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

    //statics
    const float KILEED_SOUND_VOLUME = 0.1f;

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

    public override float Health
    {
        get
        {
            return base.Health;
        }

        protected set
        {
            base.Health = value;

            //show health on UI
            InformationIndicator.Singleton.SetHealth(value);

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

    protected override void SetKillSound(EntitySoundHandler soundhandler)
    {
        soundhandler.PlayKilled(KILEED_SOUND_VOLUME);
    }
}
