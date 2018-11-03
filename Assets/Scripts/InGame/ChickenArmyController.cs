using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenArmyController : MonoBehaviour, IPausable
{
    //
    //Fields
    //

    //army array variables
    [SerializeField] int armyColumns;
    [SerializeField] int armyRows;
    int firstColumnIndex = 0;  
    int lastColumnIndex = 0;    
    bool[] isColumnsExist;
    /// <summary>
    /// The first dimension is columns and next is transforms in each column.
    /// </summary>
    Transform[,] chickenArmy;

    //move and size variables
    float leftMoveXLimit;  
    float rightMoveXLimit; 
    [SerializeField] float xGap;
    [SerializeField] float yGap;
    Vector2 chickenSize;
    [SerializeField] float speed = 2;
    bool isGoingRight = true;

    //other variables
    [SerializeField] GameObject chickenPrefab;
    [SerializeField] float spawnGapTime = 0.2f;
    bool isSpawningDone = false;
    int spawnerChickenCounter = 0;

    //static
    static ChickenArmyController singleton = null;
    static int wholeChickenNumber = 0;
    const int DEFAULT_COLUMNS_NUMER = 10;
    const int DEFAULT_ROWS_NUMBER = 5;
    const float DEFAULT_COLUMN_GAP = 1;
    const float DEFAULT_ROW_GAP = 1;
    const float DEFAULT_CHICKEN_WIDTH = 1;
    const float DEFAULT_CHICKEN_HEIGHT = 1;
    const float DEFAULT_SPEED = 2;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public Vector2 ChickenSize
    {
        get
        {
            return chickenSize;
        }
        set
        {
            if (value.x <= 0)
            {
                value.x = DEFAULT_CHICKEN_WIDTH;
            }
            if (value.y <= 0)
            {
                value.y = DEFAULT_CHICKEN_HEIGHT;
            }

            chickenSize = value;
        }
    }

    int FirstColumnIndex
    {
        set
        {
            firstColumnIndex = value;
            SetMoveXLimits();
        }
    }

    int LastColumnIndex
    {
        set
        {
            lastColumnIndex = value;
            SetMoveXLimits();
        }
    }

    public static ChickenArmyController Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = FindObjectOfType<ChickenArmyController>();
                if(singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Chicken Army");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<ChickenArmyController>();
                } 
            }

            return singleton;
        }
    }

    public static int WholeChickenNumber
    {
        get
        {
            return wholeChickenNumber;
        }
        set
        {
            if(value <= 0)
            {
                wholeChickenNumber = 0;
                WinLoseExecuter.Singleton.Win();
                BackgroundMusicPlayer.Singleton.PlayWinTrack();
            }
            else
            {
                wholeChickenNumber = value;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Awake()
    {
        //set army array
        if (armyColumns <= 0)
        {
            armyColumns = DEFAULT_COLUMNS_NUMER;
        }
        if (armyRows <= 0)
        {
            armyRows = DEFAULT_ROWS_NUMBER;
        }
        chickenArmy = new Transform[armyColumns, armyRows];
        
        //initialize column existances
        isColumnsExist = new bool[armyColumns];
        for (int i = 0; i < isColumnsExist.Length; i++)
        {
            isColumnsExist[i] = false;
        }

        //set gap sizes
        if(xGap <= 0)
        {
            xGap = DEFAULT_COLUMN_GAP;
        }
        if (yGap <= 0)
        {
            yGap = DEFAULT_ROW_GAP;
        }
    }

    private void Start()
    {
        // if is not singleton destroy it
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            if(this != singleton)
            {
                Destroy(gameObject);
            }
        }

        if(speed <= 0)
        {
            speed = DEFAULT_SPEED;
        }

        //spawn chickens
        chickenSize = chickenPrefab.transform.localScale * chickenPrefab.GetComponent<BoxCollider2D>().size;
        xGap = chickenSize.x + xGap;
        yGap = chickenSize.y + yGap;
        SetArmyPosition();
        SetMoveXLimits();
        StartCoroutine(SpawnChickens());
    }

    void SetArmyPosition()
    {
        float cameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;
        float topLength = yGap * (armyRows - 1) / 2f;
        Vector3 position = transform.position;
        position.y = cameraTop - topLength - chickenSize.y / 2;
        transform.position = position;
    }

    IEnumerator SpawnChickens()
    {
        int chickensNumberToBeCreated = armyColumns * armyRows;
        wholeChickenNumber = chickensNumberToBeCreated;

        for (; spawnerChickenCounter < chickensNumberToBeCreated; spawnerChickenCounter++)
        {
            GameObject chicken = Instantiate<GameObject>(chickenPrefab, new Vector3(8, 8, 0), Quaternion.identity);
            Chicken enemy = chicken.GetComponent<Chicken>();
            enemy.ColumnIndex = spawnerChickenCounter / armyRows;
            enemy.RowIndex = spawnerChickenCounter % armyRows;

            yield return new WaitForSeconds(spawnGapTime);
        }
        
        if(spawnerChickenCounter >= chickensNumberToBeCreated)
        {
            isSpawningDone = true;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void Update()
    {
        if (isGoingRight)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            
            //if it reaches the rigth limit
            if(transform.position.x >= rightMoveXLimit)
            {
                isGoingRight = false;
            }
        }
        else
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            
            //if it reaches the left limit
            if (transform.position.x <= leftMoveXLimit)
            {
                isGoingRight = true;
            }
        }
    }

    void SetMoveXLimits()
    {
        //camera limits
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float rightWorldLimit = Camera.main.transform.position.x + cameraHalfWidth;
        float leftWorldLimit = Camera.main.transform.position.x - cameraHalfWidth;

        //right limit
        float rightLength = xGap * ((armyColumns - 1) / 2f - firstColumnIndex) + chickenSize.x / 2;
        rightMoveXLimit = rightWorldLimit - rightLength;

        //left limit
        float leftLength = xGap * ((armyColumns - 1) / 2f - (armyColumns - 1 - lastColumnIndex)) + chickenSize.x / 2;
        leftMoveXLimit = leftWorldLimit + leftLength;
    }

    public Vector2 GetPosition(int columnIndex, int rowIndex)
    {
        float xDelta = xGap * ((armyColumns - 1) / 2f - columnIndex);
        float yDelta = yGap * ((armyRows - 1) / 2f - rowIndex);
        return transform.position + new Vector3(xDelta, yDelta, 0);
    }

    public bool AddChicken(Chicken enemy, int columnIndex, int rowIndex)
    {
        if(chickenArmy[columnIndex,rowIndex] == null)
        {
            chickenArmy[columnIndex, rowIndex] = enemy.transform;
            enemy.transform.SetParent(transform, true);
            isColumnsExist[columnIndex] = true;

            if (columnIndex > lastColumnIndex)
            {
                LastColumnIndex = columnIndex;
            }
            if (!isColumnsExist[firstColumnIndex])
            {
                ChangeLastOrFirstColumn(firstColumnIndex);
            }
            
            enemy.transform.position = GetPosition(columnIndex, rowIndex);

            return true;
        }

        return false;
    }

    public void RemoveChicken(int columnIndex, int rowIndex)
    {
        //release the enemy
        if(chickenArmy[columnIndex,rowIndex] != null)
        {
            chickenArmy[columnIndex, rowIndex].transform.SetParent(null, true);
            chickenArmy[columnIndex, rowIndex] = null;

            //check if column is empty
            int length = chickenArmy.GetLength(1);
            bool isColumnEmpty = true;
            for (int i = 0; i < length; i++)
            {
                if (chickenArmy[columnIndex, i] != null)
                {
                    isColumnEmpty = false;
                    break;
                }
            }

            //change the lastColumn or firsColumn if needed
            if (isColumnEmpty)
            {
                ChangeLastOrFirstColumn(columnIndex);
            }
        } 
    }

    void ChangeLastOrFirstColumn(int columnIndex)
    {
        isColumnsExist[columnIndex] = false;
        int length;

        if (columnIndex == firstColumnIndex)
        {
            length = chickenArmy.GetLength(0);
            int i = columnIndex + 1;
            for (; i < length && !isColumnsExist[i]; i++) ;
            if (i == length)
            {
                //it means all the colums are empty
                i = 0;
            }
            FirstColumnIndex = i;
        }

        if (columnIndex == lastColumnIndex)
        {
            length = -1;
            int i = columnIndex - 1;
            for (; i > length && !isColumnsExist[i]; i--) ;
            if (i == length)
            {
                //it means all the colums are empty
                i = 0;
            }
            LastColumnIndex = i;
        }
    }

    private void OnDestroy()
    {
        if(singleton != null)
        {
            if(this == singleton)
            {
                singleton = null;
                wholeChickenNumber = 0;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 0.5f);
    //    Gizmos.DrawWireSphere(transform.position + new Vector3((armyColumns - 1) / 2f, 0, 0), chickenSize.x);
    //    Gizmos.DrawWireSphere(transform.position - new Vector3((armyColumns - 1) / 2f, 0, 0), chickenSize.x);
    //}

    public void PauseOrResume(bool isPaused)
    {
        enabled = !isPaused;

        if(!isSpawningDone)
        {
            if (isPaused)
            {
                StopAllCoroutines();
            }
            else
            {
                spawnerChickenCounter++;
                StartCoroutine(SpawnChickens());
            }
        }
        
    }
}
