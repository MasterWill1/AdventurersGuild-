using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageEvents
{


    //takes in a list of char ids and assigns damage for each according to damage severity and contract difficulty
    //returns a 2d array [charId, damage] repeated for number of charIds
    public static int[,] getDamageForCharsIds(List<int> charIds, EventTypes.damageType damagePower, int contractDifficulty)
    {
        int[,] idAndDamageArray = new int[charIds.Count, 2];
        for(int counter = 0; counter < charIds.Count; counter++)
        {
            int damageToGive = 0;
            switch (damagePower)
            {
                case EventTypes.damageType.low:
                    damageToGive = getLowDamageAccordingToContractDifficulty(contractDifficulty);
                    break;
                case EventTypes.damageType.medium:
                    damageToGive = getMediumDamageAccordingToContractDifficulty(contractDifficulty);
                    break;
                case EventTypes.damageType.high:
                    damageToGive = getHighDamageAccordingToContractDifficulty(contractDifficulty);
                    break;
            }

            idAndDamageArray[counter, 0] = charIds[counter];
            idAndDamageArray[counter, 1] = damageToGive;
        }
        return idAndDamageArray;
    }

    static int getLowDamageAccordingToContractDifficulty(int contractDifficulty)
    {
        //divides difficulty by 10 and then adds or takes away half of that
        //a contract with difficulty 30 would be 3 damage +- 1.5
        float diffOver10 = Mathf.RoundToInt(contractDifficulty / 10);
        int damage = Mathf.RoundToInt(diffOver10 + Mathf.RoundToInt(UnityEngine.Random.Range(-diffOver10 / 2f, diffOver10 / 2f)));
        return damage;
    }

    static int getMediumDamageAccordingToContractDifficulty(int contractDifficulty)
    {
        //divides difficulty by 10 and then adds between half to that again
        //a contract with difficulty 30 would be 3 damage + 1.5 to 3
        float diffOver10 = Mathf.RoundToInt(contractDifficulty / 10);
        int damage = Mathf.RoundToInt(diffOver10 + Mathf.RoundToInt(UnityEngine.Random.Range(diffOver10 / 2f, diffOver10)));
        return damage;
    }

    static int getHighDamageAccordingToContractDifficulty(int contractDifficulty)
    {
        //divides difficulty by 10 and then adds between that and 1.5 that again
        //a contract with difficulty 30 would be 3 damage + 3 + 1.5
        float diffOver10 = Mathf.RoundToInt(contractDifficulty / 10);
        int damage = Mathf.RoundToInt(diffOver10 + Mathf.RoundToInt(UnityEngine.Random.Range(diffOver10, diffOver10 + diffOver10 / 2f)));
        return damage;
    }
}
