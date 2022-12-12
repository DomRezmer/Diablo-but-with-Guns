using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public int currentLevel;
    public int baseEXP = 20;
    public int currentEXP;

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
        InvokeRepeating("AddEXP", 1f, 1f);
    }

    public void AddEXP()
    {
        CalculateLevel(5);
    }

    void CalculateLevel(int amount)
    {
        currentEXP += amount;

        int tmp_cur_level = (int)Mathf.Sqrt(currentEXP / baseEXP);

        if(currentLevel != tmp_cur_level)
        {
            currentLevel= tmp_cur_level;
        }

        expForNextLevel = baseEXP * (currentLevel + 1) * (currentLevel + 1);
        expDifferenceToNextLevel = expForNextLevel - currentEXP;
        totalEXPDifference = expForNextLevel - (baseEXP * currentLevel * currentLevel);

        fillAmount = (float)expDifferenceToNextLevel / (float)totalEXPDifference;
        reversedFillAmount = 1 - fillAmount;

        statPoints = 5 * currentLevel;
        skillPoints = 15 * currentLevel;
    }
}
