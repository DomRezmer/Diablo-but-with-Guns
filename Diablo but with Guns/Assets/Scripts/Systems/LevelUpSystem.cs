using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSystem : MonoBehaviour
{
    public int currentLevel;
    public int baseEXP = 20;
    public int currentEXP;
    public Text lvlText;
    public Image expBarImage;

    public int expForNextLevel;
    public int expDifferenceToNextLevel;
    public int totalEXPDifference;

    public float fillAmount;
    public float reversedFillAmount;

    public int statPoints;
    public int skillPoints;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("AddEXP", 1f, 1f);
    }

    public static int ExpNeedToLvlUp(int currentLevel)
    {
        if (currentLevel == 0)
        {
            return 0;
        }
        return (currentLevel * currentLevel + currentLevel) + 5;
    }

    public void AddEXP()
    {
        CalculateLevel(25);
    }

    public void AddBossEXP()
    {
        CalculateLevel(100);
    }

    void CalculateLevel(int amount)
    {
        float previousExp = ExpNeedToLvlUp(currentLevel - 1);

        currentEXP += amount;

        int tmp_cur_level = (int)Mathf.Sqrt(currentEXP / baseEXP) + 1;
        if (currentLevel != tmp_cur_level)
        {
            currentLevel = tmp_cur_level;
            lvlText.text = currentLevel.ToString("");
            previousExp = ExpNeedToLvlUp(currentLevel - 1);
        }

        expForNextLevel = baseEXP * currentLevel * currentLevel;
        expDifferenceToNextLevel = expForNextLevel - currentEXP;
        totalEXPDifference = expForNextLevel - (baseEXP * (currentLevel - 1) * (currentLevel - 1));

        fillAmount = (float)expDifferenceToNextLevel / (float)totalEXPDifference;
        reversedFillAmount = 1 - fillAmount;

        expBarImage.fillAmount = (currentEXP - previousExp) / (expForNextLevel - previousExp);

        if (expBarImage.fillAmount == 1)
        {
            expBarImage.fillAmount = 0;
        }

        statPoints = 5 * (currentLevel - 1);
        skillPoints = 15 * (currentLevel - 1);
    }
}