using UnityEngine;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    //
    //Fields
    //

    //UI game data components
    [SerializeField] Slider masterVolume;
    [SerializeField] Button easy;
    [SerializeField] Button normal;
    [SerializeField] Button hard;
    [SerializeField] InputField armyWidth;
    [SerializeField] InputField armyHeight;
    [SerializeField] InputField health;
    [SerializeField] Button endless;
    [SerializeField] InputField maxScore;
    [SerializeField] Text scorePerKill;
    [SerializeField] Image tick;

    //Action Buttons
    [SerializeField] Button defaultBtn;
    [SerializeField] Button applyBtn;

    //data
    OptionData tempOptionData;
    Color defaultDifficultyColor;
    Color defaultEndlessBtnColor;
    GameData gameData;
    BackgroundMusicPlayer backgroundMusic;

    //data bounadries
    [SerializeField] int maxArmyWidth = MAX_ARMY_WIDTH;
    [SerializeField] int minArmyWidth = MIN_ARMY_WIDTH;
    [SerializeField] int maxArmyHeight = MAX_ARMY_HEIGHT;
    [SerializeField] int minArmyHeight = MIN_ARMY_HEIGHT;
    [SerializeField] int maxHealth = MAX_HEALTH;
    [SerializeField] int minHealth = MIN_HEALTH;
    [SerializeField] int maxMaxScore = MAX_MAX_SCORE;
    [SerializeField] int minMaxScore = MIN_MAX_SCORE;

    //constants
    const int MAX_ARMY_WIDTH = 8;
    const int MIN_ARMY_WIDTH = 3;
    const int MAX_ARMY_HEIGHT = 4;
    const int MIN_ARMY_HEIGHT = 2;
    const int MAX_HEALTH = 40;
    const int MIN_HEALTH = 10;
    const int MAX_MAX_SCORE = 6000;
    const int MIN_MAX_SCORE = 200;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        //fill data
        gameData = FindObjectOfType<GameData>();
        tempOptionData = gameData.GameOptionData;
        backgroundMusic = FindObjectOfType<BackgroundMusicPlayer>();

        //default color preinitialization
        defaultDifficultyColor = easy.colors.normalColor;
        defaultEndlessBtnColor = endless.colors.normalColor;

        //fill data in UI components
        masterVolume.value = gameData.GameOptionData.masterVolume;
        if (tempOptionData.GameDifficulty == OptionData.Difficulty.Easy)
        {
            OnClickEasy();
        }
        else if (tempOptionData.GameDifficulty == OptionData.Difficulty.Normal)
        {
            OnClickNormal();
        }
        else
        {
            OnClickHard();
        }
        armyWidth.text = tempOptionData.ArmySize.x.ToString();
        armyHeight.text = tempOptionData.ArmySize.y.ToString();
        health.text = tempOptionData.Health.ToString(); 
        maxScore.text = tempOptionData.maxScore.ToString();
        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
        SetEndless(tempOptionData.endless);

        //sign in to option UI events
        masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        easy.onClick.AddListener(OnClickEasy);
        normal.onClick.AddListener(OnClickNormal);
        hard.onClick.AddListener(OnClickHard);
        armyWidth.onEndEdit.AddListener(OnEndEditArmyWidth);
        armyHeight.onEndEdit.AddListener(OnEndEditArmyHeight);
        health.onEndEdit.AddListener(OnEndEditHealth);
        endless.onClick.AddListener(OnClickEndless);
        maxScore.onEndEdit.AddListener(OnEndEditMaxScore);
        defaultBtn.onClick.AddListener(OnClickDefault);
        applyBtn.onClick.AddListener(OnClickApply);
        //sign in to back button click event
        Button back = GameObject.Find("Back").transform.GetChild(0).GetComponent<Button>();
        back.onClick.AddListener(OnBackClick);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    void OnMasterVolumeChanged(float value)
    {
        backgroundMusic.Volume = value;
        tempOptionData.masterVolume = value;
    }

    void OnClickEasy()
    {
        //gui
        if(tempOptionData.GameDifficulty == OptionData.Difficulty.Normal)
        {
            //set normal's normal color to default color
            ColorBlock colors = normal.colors;
            colors.normalColor = defaultDifficultyColor;
            normal.colors = colors;
        }
        else if(tempOptionData.GameDifficulty == OptionData.Difficulty.Hard)
        {
            //set hard's normal color to default color
            ColorBlock colors = hard.colors;
            colors.normalColor = defaultDifficultyColor;
            hard.colors = colors;
        }
        //set easy's normal color to it's pressed color
        ColorBlock easyColors = easy.colors;
        easyColors.normalColor = easy.colors.pressedColor;
        easy.colors = easyColors;

        //logic
        tempOptionData.GameDifficulty = OptionData.Difficulty.Easy;

        //score per kill gui
        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void OnClickNormal()
    {
        //gui
        if (tempOptionData.GameDifficulty == OptionData.Difficulty.Easy)
        {
            //set easy's normal color to default color
            ColorBlock colors = easy.colors;
            colors.normalColor = defaultDifficultyColor;
            easy.colors = colors;
        }
        else if (tempOptionData.GameDifficulty == OptionData.Difficulty.Hard)
        {
            //set hard's normal color to default color
            ColorBlock colors = hard.colors;
            colors.normalColor = defaultDifficultyColor;
            hard.colors = colors;
        }
        //set normal's normal color to it's pressed color
        ColorBlock normalColors = normal.colors;
        normalColors.normalColor = normal.colors.pressedColor;
        normal.colors = normalColors;

        //logic
        tempOptionData.GameDifficulty = OptionData.Difficulty.Normal;

        //score per kill gui
        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void OnClickHard()
    {
        //gui
        if (tempOptionData.GameDifficulty == OptionData.Difficulty.Easy)
        {
            //set easy's normal color to default color
            ColorBlock colors = easy.colors;
            colors.normalColor = defaultDifficultyColor;
            easy.colors = colors;
        }
        else if (tempOptionData.GameDifficulty == OptionData.Difficulty.Normal)
        {
            //set normal's normal color to default color
            ColorBlock colors = normal.colors;
            colors.normalColor = defaultDifficultyColor;
            normal.colors = colors;
        }
        //set hard's normal color to it's pressed color
        ColorBlock easyColors = hard.colors;
        easyColors.normalColor = hard.colors.pressedColor;
        hard.colors = easyColors;

        //logic
        tempOptionData.GameDifficulty = OptionData.Difficulty.Hard;

        //score per kill gui
        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void ChangeButtonColor(Button button, Color color)
    {
        //set easy's normal color to default color
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }

    void OnEndEditArmyWidth(string text)
    {        
        if (text == "")
        {
            //gui
            armyWidth.text = tempOptionData.ArmyWidth.ToString();
        }
        else
        {
            //logic
            int value = int.Parse(text);
            int restrictedValue = Mathf.Clamp(value, minArmyWidth, maxArmyWidth);
            tempOptionData.ArmyWidth = restrictedValue;

            //gui
            if (restrictedValue != value)
            {
                armyWidth.text = restrictedValue.ToString();
            }
        }

        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void OnEndEditArmyHeight(string text)
    {
        if (text == "")
        {
            //gui
            armyHeight.text = tempOptionData.ArmyHeight.ToString();
        }
        else
        {
            //logic
            int value = int.Parse(text);
            int restrictedValue = Mathf.Clamp(value, minArmyHeight, maxArmyHeight);
            tempOptionData.ArmyHeight = restrictedValue;

            //gui
            if (restrictedValue != value)
            {
                armyHeight.text = restrictedValue.ToString();
            }
        }

        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void OnEndEditHealth(string text)
    {
        if (text == "")
        {
            //gui
            health.text = tempOptionData.Health.ToString();
        }
        else
        {
            //logic
            int value = int.Parse(text);
            int restrictedValue = Mathf.Clamp(value, minHealth, maxHealth);
            tempOptionData.Health = restrictedValue;

            //gui
            if (restrictedValue != value)
            {
                health.text = restrictedValue.ToString();
            }
        }

        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void OnClickEndless()
    {
        SetEndless(!tempOptionData.endless);
    }

    void SetEndless(bool isEndless)
    {
        tempOptionData.endless = isEndless;

        //gui
        tick.enabled = isEndless;
        if (isEndless)
        {
            ColorBlock colors = endless.colors;
            colors.normalColor = endless.colors.highlightedColor;
            endless.colors = colors;
        }
        else
        {
            ColorBlock colors = endless.colors;
            colors.normalColor = defaultEndlessBtnColor;
            endless.colors = colors;
        }
    }

    void OnEndEditMaxScore(string text)
    {
        if (text == "")
        {
            //gui
            maxScore.text = tempOptionData.maxScore.ToString();
        }
        else
        {
            //logic
            int value = int.Parse(text);
            int restrictedValue = Mathf.Clamp(value, minMaxScore, maxMaxScore);
            tempOptionData.maxScore = restrictedValue;

            //gui
            if (restrictedValue != value)
            {
                maxScore.text = restrictedValue.ToString();
            }
        }
    }

    void OnClickDefault()
    {
        //logic
        tempOptionData.Initialize();

        //gui
        masterVolume.value = tempOptionData.masterVolume;
        SetDefaultDifficultyColor();
        armyWidth.text = tempOptionData.ArmyWidth.ToString();
        armyHeight.text = tempOptionData.ArmyHeight.ToString();
        health.text = tempOptionData.Health.ToString();
        SetEndless(tempOptionData.endless);
        maxScore.text = tempOptionData.maxScore.ToString();
        scorePerKill.text = tempOptionData.ScorePerKill.ToString();
    }

    void SetDefaultDifficultyColor()
    {
        ChangeButtonColor(easy, defaultDifficultyColor);
        ChangeButtonColor(normal, defaultDifficultyColor);
        ChangeButtonColor(hard, defaultDifficultyColor);
        switch (tempOptionData.GameDifficulty)
        {
            case OptionData.Difficulty.Easy:
                ChangeButtonColor(easy, easy.colors.pressedColor);
                break;
            case OptionData.Difficulty.Normal:
                ChangeButtonColor(normal, normal.colors.pressedColor);
                break;
            default:
                ChangeButtonColor(hard, hard.colors.pressedColor);
                break;
        }
    }

    void OnClickApply()
    {
        gameData.SaveOption(tempOptionData);
    }

    void OnBackClick()
    {
        //set background music to what option data has
        //(it should be preserved only if we apply the changes)
        backgroundMusic.Volume = gameData.GameOptionData.masterVolume;
    }
}
