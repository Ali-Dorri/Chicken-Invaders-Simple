using UnityEngine;

using System.IO;

public struct OptionData
{
    //
    //Concept Definition
    //

    public enum Difficulty { Easy, Normal, Hard }
    public struct Size
    {
        public int x;
        public int y;

        public Size(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    ///////////////////////////////////////////////////////////////////////////// 

    //
    //Fields
    //

    float masterVolume;
    Difficulty difficulty;  //multiplication:    easy = 1, normal = 2, hard = 3
    Size armySize;
    int health;
    bool endless;
    int maxScore;
    int scorePerKill;

    //defaults
    public const float DEFAULT_MASTER_VOLUME = 1;
    public const Difficulty DEFAULT_DIFFICULTY = Difficulty.Normal;
    public const int DEFAULT_ARMY_WIDTH= 8;
    public const int DEFAULT_ARMY_HEIGHT= 3;
    public const int DEFAULT_HEALTH = 20;
    public const bool DEFAULT_ENDLESS = false;
    public const int DEFAULT_MAX_SCORE = 250;

    //save local path
    const string LOCAL_FILE_PATH = "Option\\OptionData.opdat";
    const string LOCAL_FOLDER = "Option";

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    /// <summary>
    /// Create option data with default values.
    /// </summary>
    public OptionData(float masterVolume)
    {
        this.masterVolume = Mathf.Clamp01(masterVolume);
        difficulty = DEFAULT_DIFFICULTY;
        armySize = new Size(DEFAULT_ARMY_WIDTH, DEFAULT_ARMY_HEIGHT);
        health = DEFAULT_HEALTH;
        endless = DEFAULT_ENDLESS;
        maxScore = DEFAULT_MAX_SCORE;
        scorePerKill = (-health + (armySize.x * armySize.y)) * ((int)difficulty + 1);
    }

    public OptionData(float masterVolume, Difficulty difficulty, Size armySize, int health, 
                    bool isEndless, int maxScore, int scorePerKill)
    {
        this.masterVolume = Mathf.Clamp01(masterVolume);
        this.difficulty = difficulty;
        this.armySize = armySize;
        this.health = health;
        this.endless = isEndless;
        this.maxScore = maxScore;
        this.scorePerKill = scorePerKill;
    }

    public OptionData(float masterVolume, Difficulty difficulty, Size armySize, int health,
                    bool isEndless, int maxScore)
    {
        this.masterVolume = Mathf.Clamp01(masterVolume);
        this.difficulty = difficulty;
        this.armySize = armySize;
        this.health = health;
        this.endless = isEndless;
        this.maxScore = maxScore;
        scorePerKill = (-health + (armySize.x * armySize.y)) * ((int)difficulty + 1);
    }

    /// <summary>
    /// Default initialization
    /// </summary>
    public void Initialize()
    {
        masterVolume = DEFAULT_MASTER_VOLUME;
        difficulty = DEFAULT_DIFFICULTY;
        armySize = new Size(DEFAULT_ARMY_WIDTH, DEFAULT_ARMY_HEIGHT);
        health = DEFAULT_HEALTH;
        endless = DEFAULT_ENDLESS;
        maxScore = DEFAULT_MAX_SCORE;
        scorePerKill = (-health + (armySize.x * armySize.y)) * ((int)difficulty + 1);
    }

    public void Initialize(float masterVolume, Difficulty difficulty, Size armySize, int health,
                    bool isEndless, int maxScore, int scorePerKill)
    {
        this.masterVolume = Mathf.Clamp01(masterVolume);
        this.difficulty = difficulty;
        this.armySize = armySize;
        this.health = health;
        this.endless = isEndless;
        this.maxScore = maxScore;
        this.scorePerKill = scorePerKill;
    }

    public void Initialize(float masterVolume, Difficulty difficulty, Size armySize, int health,
                    bool isEndless, int maxScore)
    {
        this.masterVolume = Mathf.Clamp01(masterVolume);
        this.difficulty = difficulty;
        this.armySize = armySize;
        this.health = health;
        this.endless = isEndless;
        this.maxScore = maxScore;
        scorePerKill = (-health + (armySize.x * armySize.y)) * ((int)difficulty + 1);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public static int ScorePerKillByNewDifficulty(int scorePerKill, Difficulty prevDifficulty, Difficulty newDifficulty)
    {
        return scorePerKill /((int)prevDifficulty + 1) * ((int)newDifficulty + 1);
    }

    public static int ScorePerKillByNewArmyWidth(int scorePerKill, Size prevSize, int newWidth)
    {
        return scorePerKill + (prevSize.y * (newWidth - prevSize.x));
    }

    public static int ScorePerKillByNewArmyHeight(int scorePerKill, Size prevSize, int newHeight)
    {
        return scorePerKill + (prevSize.x * (newHeight - prevSize.y));
    }

    public static int ScorePerKillByNewHealth(int scoreperKill, int prevHealth, int newHealth)
    {
        return scoreperKill + prevHealth - newHealth;
    }

    public void Save()
    {
        //check path
        CheckPath();

        FileStream stream = null;

        try
        {
            stream = File.Open(Path.Combine(Application.persistentDataPath, LOCAL_FILE_PATH), FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);

            //write datas
            writer.Write((double)masterVolume);
            writer.Write((int)difficulty);
            writer.Write(armySize.x);
            writer.Write(armySize.y);
            writer.Write(health);
            writer.Write(endless);
            writer.Write(maxScore);
        }
        finally
        {
            if(stream != null)
            {
                stream.Close();
            }
        }
    }

    public void Load()
    {
        //check path
        CheckPath();

        string path = Path.Combine(Application.persistentDataPath, LOCAL_FILE_PATH);

        if (File.Exists(path))
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open);
                BinaryReader reader = new BinaryReader(stream);

                //read datas
                masterVolume = (float)reader.ReadDouble();
                difficulty = (Difficulty)reader.ReadInt32();
                armySize = new Size(reader.ReadInt32(), reader.ReadInt32());
                health = reader.ReadInt32();
                endless = reader.ReadBoolean();
                maxScore = reader.ReadInt32();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        else
        {
            Initialize();
        }   
    }

    void CheckPath()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_FOLDER);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }   
    }

    public void DisplayDatas()
    {
        Debug.Log("master volume: " + masterVolume.ToString());
        Debug.Log("difficulty: " + difficulty.ToString());
        Debug.Log("army size: " + " x:" + armySize.x.ToString() + " y:" + armySize.y.ToString());
        Debug.Log("health: " + health.ToString());
        Debug.Log("endless: " + endless.ToString());
        Debug.Log("max score: " + maxScore.ToString());
        Debug.Log("score per kill: " + scorePerKill.ToString());
    }
}
