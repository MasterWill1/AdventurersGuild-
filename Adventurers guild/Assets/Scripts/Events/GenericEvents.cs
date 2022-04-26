using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic events that could happen any time
public class GenericEvents : MonoBehaviour
{
    public GameObject AllCharacterStorageHandler, ContractHandler, KeyWorldDetailsHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractStorage ContractStorage;
    keyWorldDetailsHandler keyWorldDetailsHandler;

    private void Start()
    {
        AllCharacterStorage = AllCharacterStorageHandler.GetComponent<AllCharacterStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        keyWorldDetailsHandler = KeyWorldDetailsHandler.GetComponent<keyWorldDetailsHandler>();
    }

    public void getSmallBonusGoldByContractDifficulty(int conId)
    {
        Contract contract = ContractStorage.findContractFromID(conId);
        int bonusgold = Mathf.RoundToInt(contract.reward / 5);
        keyWorldDetailsHandler.changeTotalGold(bonusgold);
    }

    public void getLargeBonusGoldByContractDifficulty(int conId)
    {
        Contract contract = ContractStorage.findContractFromID(conId);
        int bonusgold = Mathf.RoundToInt(contract.reward / 2);
        keyWorldDetailsHandler.changeTotalGold(bonusgold);
    }

    public int[,] damageXCharactersWithXDamageByContractDifficulty(int contractId, List<int> charIds, EventTypes.damageType damageType)
    {
        int contractDifficulty = ContractStorage.findContractFromID(contractId).difficulty;
        int[,] charIdsAndDamage = DamageEvents.getDamageForCharsIds(charIds, damageType, contractDifficulty);

        for (int i = 0; i < charIdsAndDamage.GetLength(0); i++)
        {
            AllCharacterStorage.damageCharacter(charIdsAndDamage[i, 0], charIdsAndDamage[i, 1]);
        }
        return charIdsAndDamage;
    }

    //for when you want to specify the exact difficulty to get damage from rather than get it from a contract for example
    public int[,] damageXCharactersWithXDamageByExactDifficulty(int damageDifficulty, List<int> charIds, EventTypes.damageType damageType)
    {
        int[,] charIdsAndDamage = DamageEvents.getDamageForCharsIds(charIds, damageType, damageDifficulty);

        for (int i = 0; i < charIdsAndDamage.GetLength(0); i++)
        {
            AllCharacterStorage.damageCharacter(charIdsAndDamage[i, 0], charIdsAndDamage[i, 1]);
        }
        return charIdsAndDamage;
    }



}
