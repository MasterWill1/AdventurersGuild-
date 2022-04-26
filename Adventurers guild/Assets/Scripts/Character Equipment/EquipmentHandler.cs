using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as midpoint for Equipment functions. Call here to access from other subsystems
/// rather than having to have lots of different calls
/// </summary>
public class EquipmentHandler : MonoBehaviour
{

    EquipmentCostCalcs EquipmentCostCalcs;
    CharacterEquipmentStorage CharacterEquipmentStorage;

    public GameObject equipmentListObject;
    AvailableEquipmentFacade AvailableEquipmentFacade;

    private void Start()
    {
        EquipmentCostCalcs = gameObject.GetComponent<EquipmentCostCalcs>();
        CharacterEquipmentStorage = gameObject.GetComponent<CharacterEquipmentStorage>();
        AvailableEquipmentFacade = equipmentListObject.GetComponent<AvailableEquipmentFacade>();
    }
    public CharacterEquipment createRandomStartingEquipment(int charId, int charLevel, CharacterTypes.CharacterClass characterClass)
    {
        return CharacterEquipmentStorage.createRandomStartingEquipment(charId, charLevel, characterClass);
    }
    public List<CharacterEquipment> getAllCharEquipmentList()
    {
        return CharacterEquipmentStorage.allCharEquipmentList;
    }
    public int calculateTotalEquipmentCost(CharacterEquipment characterEquipment)
    {
        return EquipmentCostCalcs.calculateTotalEquipmentCost(characterEquipment);
    }
    public void showEquipmentList(CharacterEquipmentTypes.itemType filter, int forCharId)
    {
        AvailableEquipmentFacade.showEquipmentList(filter, forCharId);
    }

}
