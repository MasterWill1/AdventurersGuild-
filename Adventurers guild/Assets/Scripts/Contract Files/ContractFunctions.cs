using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractFunctions : MonoBehaviour
{
    //gets the success chance according to the party strength and contract difficulty
    public static float getPartyEffectivenessForQuest(int partyStrength, int contractDifficulty)
    {
        float fPartyStrength = partyStrength;
        float fContractDifficulty = contractDifficulty;
        float preDeductionEffectiveness = fPartyStrength / fContractDifficulty;
        Debug.Log("preDeduction effectiveness: " + preDeductionEffectiveness);

        float effectiveness = preDeductionEffectiveness * (float)0.9;
        Debug.Log("effectiveness: " + effectiveness);

        return effectiveness;
    }

    //calculates whether a contract has been completed succesfully
    public static bool CalculateSuccess(float effectiveness)
    {
        double ChanceRollScore = Random.Range(0f, 1);
        Debug.Log("Randomised Final Contract Difficulty roll: " + ChanceRollScore);

        //function of diminishing returns
        float successChance = getSuccessChance(effectiveness);
        Debug.Log("Contract Party final effectiveness: " + successChance);

        //if the contracts success chance is higher than difficulty score then success
        if (ChanceRollScore <= successChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static float getSuccessChance(float effectiveness)
    {
        float successChance = (-1 / (Mathf.Pow(effectiveness + 1f, 4))) + 1;
        return successChance;
    }

    public static bool wasCriticalResult()
    {
        int standardResultChance = 90;
        int criticalResultChance = 10;

        float totalResultWeighting = standardResultChance + criticalResultChance;

        float finalStandardChance = (standardResultChance / totalResultWeighting)*100;

        float resultScore = Random.Range(0, 100f);
        Debug.Log("Chance of normal result: " + finalStandardChance + "%. Result critical score: " + resultScore);

        if (resultScore < finalStandardChance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
