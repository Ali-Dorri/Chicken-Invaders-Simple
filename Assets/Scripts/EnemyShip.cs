using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : AliveEntity, IEnemy
{
    //
    //Fields
    //

    new Transform transform;
    float shootTimeGap = 0;
    float shootTimeCounter = 0;
    int columnIndex;
    int rowIndex;
    [SerializeField] float speed = 4;
    bool isSetInPosition = false;
    Animator animator;

    //static
    static System.Random random = new System.Random();
    const float MAX_SHOOT_TIME_GAP = 8;
    const float MIN_SHOOT_TIME_GAP = 6;
    const float DEFAULT_SPEED = 4;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public int ColumnIndex
    {
        get
        {
            return columnIndex;
        }
        set
        {
            columnIndex = value;
        }
    }

    public int RowIndex
    {
        get
        {
            return rowIndex; 
        }
        set
        {
            rowIndex = value;
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
            if(health <= 0)
            {
                ChickenArmyController.Singleton.RemoveChicken(columnIndex, rowIndex);
            }
        }
    }

    protected override GameObject BulletPrefab
    {
        get
        {
            return chickenEggPrefab;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    protected override void SetBulletPrefab()
    {
        if(chickenEggPrefab == null)
        {
            chickenEggPrefab = Resources.Load<GameObject>("Prefabs/Chicken Egg");
        }   
    }

    protected override void Initialize()
    {
        transform = base.transform;
        if(bulletSpeed <= 0)
        {
            bulletSpeed = DEFAULT_ENEMY_BULLET_SPEED;
        }
        shootTimeGap = GetRandomTime();
        if(speed <= 0)
        {
            speed = DEFAULT_SPEED;
        }
        animator = transform.GetChild(0).GetComponent<Animator>();

        //get to position
        StartCoroutine(GetToPosition());
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    void Update ()
    {
        Move();
        ShootRandomly();
    }

    public virtual void ShootRandomly()
    {
        shootTimeCounter += Time.deltaTime;

        if (shootTimeCounter >= shootTimeGap)
        {
            Shoot();
            shootTimeCounter = 0;
            shootTimeGap = GetRandomTime();
        }
    }

    float GetRandomTime()
    {
        int rnd = random.Next(0, 10);
        return MIN_SHOOT_TIME_GAP + (rnd * (MAX_SHOOT_TIME_GAP - MIN_SHOOT_TIME_GAP) / 10);
    }

    protected override void Move()
    {
        //nothing
    }

    IEnumerator GetToPosition()
    {
        ChickenArmyController chickenArmy = ChickenArmyController.Singleton;

        Vector2 purposePos = chickenArmy.GetPosition(columnIndex, rowIndex);
        while (Vector2.Distance(purposePos, transform.position) > speed * Time.deltaTime)
        {
            transform.position += ((Vector3)purposePos - transform.position).normalized * speed * Time.deltaTime;
            purposePos = chickenArmy.GetPosition(columnIndex, rowIndex);
            yield return null;
        }

        isSetInPosition = true;
        chickenArmy.AddChicken(this, columnIndex, rowIndex);
    }

    protected override void SetBulletStatus(Bullet bullet)
    {
        bullet.TeamSide = Bullet.Group.Enemy;
    }

    protected override bool IsOpponentBullet(GameObject ifBullet)
    {
        Bullet bullet = ifBullet.GetComponent<Bullet>();

        if (bullet != null)
        {
            if (bullet.TeamSide == Bullet.Group.Friend)
            {
                return true;
            }
        }

        return false;
    }

    public override void PauseOrResume(bool isPaused)
    {
        base.PauseOrResume(isPaused);

        if (isPaused)
        {
            //stop animation
            animator.StartPlayback();
        }
        else
        {
            //resume animation
            animator.StopPlayback();
        }

        if (!isSetInPosition)
        {
            if (isPaused)
            {
                StopAllCoroutines();
            }
            else
            {
                StartCoroutine(GetToPosition());
            }   
        }
    }
}
