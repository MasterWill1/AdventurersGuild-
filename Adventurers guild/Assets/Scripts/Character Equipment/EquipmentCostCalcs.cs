using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCostCalcs : MonoBehaviour
{

    public int calculateEquipmentItemCost(equipmentItem EquipmentItem)
    {
        //calculate the base cost of item
        int cost = CharacterEquipmentTypes.equipmentBaseCost(EquipmentItem);

        //TODO change cost according to magic enchantment

        return cost;
    }
    /// <summary>
    /// Takes in CharacterEquipment and calculates total cost of all equipment
    /// </summary>
    /// <param name="characterEquipment"></param>
    /// <returns></returns>
    public int calculateTotalEquipmentCost(CharacterEquipment characterEquipment)
    {
        int cost = calculateEquipmentItemCost(characterEquipment.handEquipmentItem);
        cost = cost + calculateEquipmentItemCost(characterEquipment.bodyEquipmentItem);
        cost = cost + calculateEquipmentItemCost(characterEquipment.handEquipmentItem);
        cost = cost + calculateEquipmentItemCost(characterEquipment.strongHandItem);
        cost = cost + calculateEquipmentItemCost(characterEquipment.offHandItem);
        cost = cost + calculateEquipmentItemCost(characterEquipment.utilityEquipmentItem);

        return cost;
    }
}
