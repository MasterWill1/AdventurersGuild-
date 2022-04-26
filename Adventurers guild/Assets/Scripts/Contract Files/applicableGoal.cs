using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applicableGoal
{
    public ContractTypes.contractGoalTag contractGoalTag;
    public int difficultyMin;
    public int difficultyMax;

    public applicableGoal(ContractTypes.contractGoalTag _contractGoalTag, int _difficultyMin, int _difficultyMax)
    {
        contractGoalTag = _contractGoalTag;
        difficultyMin = _difficultyMin;
        difficultyMax = _difficultyMax;
    }
}
