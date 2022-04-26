using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipmentItem : MonoBehaviour
{
    public int itemId;
    public int charId;
    public CharacterEquipmentTypes.itemType itemType;
    public CharacterEquipmentTypes.equipmentName equipmentName;
    public CharacterEquipmentTypes.abilityRequirement abilityRequirement;
    public CharacterEquipmentTypes.itemHandRequirement itemHandRequirement; //change to a list of itemTags? where itemTags are onehanded, twohanded, ranged, melee etc
    public CharacterEquipmentTypes.equipmentQuality equipmentQuality;
    public CharacterEquipmentTypes.magicEffect magicEffect;
    public int baseCost;
    public int baseCombatScore;

    public equipmentItem(int _itemId, int _charId,
        CharacterEquipmentTypes.equipmentName _equipmentName,CharacterEquipmentTypes.equipmentQuality _equipmentQuality,
        CharacterEquipmentTypes.magicEffect _magicEffect)
    {
        itemId = _itemId;
        charId = _charId;
        equipmentName = _equipmentName;
        equipmentQuality = _equipmentQuality;
        magicEffect = _magicEffect;
    }
    public void setConsitentParameters(EquipmentDef equipmentDef)
    {
        itemType = equipmentDef.itemType;
        abilityRequirement = equipmentDef.abilityRequirement;
        itemHandRequirement = equipmentDef.itemHandRequirement;
        baseCost = equipmentDef.baseCost;
        baseCombatScore = equipmentDef.baseCombatScore;
    }
    public void setNullVariables()
    {
        itemId = 0;
        charId = 0;
        itemType = CharacterEquipmentTypes.itemType.none;
        equipmentName = CharacterEquipmentTypes.equipmentName.none;
        abilityRequirement = CharacterEquipmentTypes.abilityRequirement.none;
        itemHandRequirement = CharacterEquipmentTypes.itemHandRequirement.none; //change to a list of itemTags? where itemTags are onehanded, twohanded, ranged, melee etc
        equipmentQuality = CharacterEquipmentTypes.equipmentQuality.none;
        magicEffect = CharacterEquipmentTypes.magicEffect.none;
        baseCost = 0;
        baseCombatScore = 0;
    }
}
