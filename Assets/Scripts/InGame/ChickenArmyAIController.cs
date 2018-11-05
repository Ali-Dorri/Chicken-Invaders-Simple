using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenArmyAIController : ChickenArmyController
{
    //
    //Fields
    //

    Transform playerTransform;
    int lastColumnShootedIndex = -1;
    List<DataWithIndex<List<DataWithIndex<Chicken>>>> aliveChickens;

    //single chicken shooting variables
    float shootTimeGap = 0;
    float shootTimeCounter = 0;
    Chicken randomChicken;

    //static
    static System.Random random = new System.Random();
    const float MAX_SHOOT_TIME_GAP = 3;
    const float MIN_SHOOT_TIME_GAP = 1;
    //(For Later) will be used when creating stack lists for controlling creating and destroying bullets
    const int MAX_EGGS_NUMBER = 10;


    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    protected override void InitializeInStart()
    {
        base.InitializeInStart();

        playerTransform = FindObjectOfType<PlayerShip>().transform;
        aliveChickens = new List<DataWithIndex<List<DataWithIndex<Chicken>>>>(ArmyColumnsNumber);


        //single chicken shooting initialization
        shootTimeGap = GetRandomTime();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    protected override void UpdateArmy()
    {
        base.UpdateArmy();
        ColumnShootProcess();
        RandomShootForSingleChickens();
    }

    void RandomShootForSingleChickens()
    {
        shootTimeCounter += Time.deltaTime;

        if (shootTimeCounter >= shootTimeGap)
        {
            //random chicken shoots
            randomChicken = FindRandomChicken();
            if(randomChicken != null)
            {
                randomChicken.Shoot();
            }
                
            //set time for next random
            shootTimeCounter = 0;
            shootTimeGap = GetRandomTime();
        }
    }

    Chicken FindRandomChicken()
    {
        int columns = aliveChickens.Count;
        if(columns > 0)
        {
            List<DataWithIndex<Chicken>> selectedColumn = aliveChickens[random.Next(0, columns - 1)].Data;
            int columnLength = selectedColumn.Count;
            if(columnLength > 0)
            {
                return selectedColumn[random.Next(0, columnLength - 1)].Data;
            } 
        }

        return null;
    }

    void ColumnShootProcess()
    {
        //each 0.2 second do these
            //check player position (if it's x position is different with lastColumnShooted's x position) (if{})
                //if we are allowed: calculate the probablity according to current column and last column x distance
                    //calculate random for which chicken in column will shoot
                        //if more than one chicken should shoot, other allowed chickens should shoot later by random time
    }

    static float GetRandomTime()
    {
        int rnd = random.Next(0, 10);
        return MIN_SHOOT_TIME_GAP + (rnd * (MAX_SHOOT_TIME_GAP - MIN_SHOOT_TIME_GAP) / 10);
    }

    public override bool AddChicken(Chicken enemy, int columnIndex, int rowIndex)
    {
        bool chickenDidntExist = base.AddChicken(enemy, columnIndex, rowIndex);

        if (chickenDidntExist)  // == a new chicken was added to the army
        {
            //if a column was added
            if (rowIndex == 0)  //even for the first chicken(there had been no columns in aliveChickens yet)
            {
                aliveChickens.Add(new DataWithIndex<List<DataWithIndex<Chicken>>>(new List<DataWithIndex<Chicken>>(ArmyRowsNumber)
                                  , columnIndex));
            }

            foreach(DataWithIndex<List<DataWithIndex<Chicken>>> existentColumn in aliveChickens)
            {
                if(existentColumn.Index == columnIndex)
                {
                    existentColumn.Data.Add(new DataWithIndex<Chicken>(enemy, rowIndex));
                }
            }  
        }

        return chickenDidntExist;
    }

    public override void RemoveChicken(int columnIndex, int rowIndex)
    {
        base.RemoveChicken(columnIndex, rowIndex);

        //check if the chicken was added to the aliveChickens
        if (WasChickenAdded(columnIndex, rowIndex))
        {
            //find the column that have the chicken to be removed
            List<DataWithIndex<Chicken>> removedFromColumn = null;
            DataWithIndex<List<DataWithIndex<Chicken>>> removedFromColumnWithIndex =
                                        new DataWithIndex<List<DataWithIndex<Chicken>>>();
            foreach (DataWithIndex<List<DataWithIndex<Chicken>>> listData in aliveChickens)
            {
                if (listData.Index == columnIndex)
                {
                    removedFromColumnWithIndex = listData;
                    removedFromColumn = listData.Data;
                    break;
                }
            }

            //remove the chicken from alive chicken list(remove from removedFromColumn)
            foreach (DataWithIndex<Chicken> dataWithIndex in removedFromColumn)
            {
                if (dataWithIndex.Index == rowIndex)
                {
                    removedFromColumn.Remove(dataWithIndex);
                    break;
                }
            }

            //if column has no chicken in it
            if (removedFromColumn.Count == 0)
            {
                //remove column
                aliveChickens.Remove(removedFromColumnWithIndex);
            }
        }      
    }

    bool WasChickenAdded(int columnIndex, int rowIndex)
    {
        int existentColumnsNumber = aliveChickens.Count;

        for (int i = 0; i < existentColumnsNumber; i++)
        {
            if(aliveChickens[i].Index == columnIndex)
            {
                List<DataWithIndex<Chicken>> column = aliveChickens[i].Data;
                int columnLength = column.Count;

                for(int j = 0; j < columnLength; j++)
                {
                    if(column[j].Index == rowIndex)
                    {
                        return true;
                    }
                }

                break;
            }
        }

        return false;
    }
}
