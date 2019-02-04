using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : AliveEntity, IEnemy
{
    //
    //Fields
    //

    new Transform transform;
    //float shootTimeGap = 0;
    //float shootTimeCounter = 0;
    int columnIndex;
    int rowIndex;
    [SerializeField] float speed = 4;
    bool isSetInPosition = false;

    //static
    const float DEFAULT_SPEED = 4;
    const float KILLED_SOUND_VOLUME = 1;
    const float EASY_BULLET_SPEED = 2.5f;
    const float NORMAL_BULLET_SPEED = 3;
    const float HARD_BULLET_SPEED = 5;

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

    public override float Health
    {
        get
        {
            return base.Health;
        }

        protected set
        {
            float previousHealth = health;
            base.Health = value;

            if (previousHealth > health)
            {
                if (health == 0)
                {
                    //show score on UI
                    InformationIndicator.Singleton.IncreaseScore();

                    //logic
                    ChickenArmyController.Singleton.RemoveChicken(columnIndex, rowIndex);
                    ChickenArmyController.WholeChickenNumber--; 
                }
            }
        }
    }

    public override GameObject BulletPrefab
    {
        get
        {
            return chickenEggPrefab;
        }
    }

    protected override BulletPool DesiredBulletPool
    {
        get
        {
            return BulletPool.ChickenBulletPool;
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

        //set bullet speed
        OptionData.Difficulty difficulty = GameData.Singleton.GameOptionData.GameDifficulty;
        if(difficulty == OptionData.Difficulty.Easy)
        {
            bulletSpeed = EASY_BULLET_SPEED;
        }
        else if(difficulty == OptionData.Difficulty.Normal)
        {
            bulletSpeed = NORMAL_BULLET_SPEED;
        }
        else
        {
            bulletSpeed = HARD_BULLET_SPEED;
        }

        //set speed
        if(speed <= 0)
        {
            speed = DEFAULT_SPEED;
        }

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
