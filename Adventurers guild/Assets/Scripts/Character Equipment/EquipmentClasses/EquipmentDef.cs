using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class EquipmentDef
{
    public CharacterEquipmentTypes.equipmentName equipmentName;
    public CharacterEquipmentTypes.itemType itemType;
    public string title;
    public string description;
    public int baseCombatScore;
    public int baseCost;
    public CharacterEquipmentTypes.abilityRequirement abilityRequirement;
    public CharacterEquipmentTypes.itemHandRequirement itemHandRequirement;

    public EquipmentDef(XmlNode curEquipmentNode)
    {
        Enum.TryParse(curEquipmentNode.Attributes["label"].Value, out equipmentName);

        Enum.TryParse(curEquipmentNode["ItemType"].InnerText, out itemType);

        title = curEquipmentNode["Title"].InnerText;
        description = curEquipmentNode["Description"].InnerText;
        
        if (curEquipmentNode.SelectSingleNode("BaseCombatScore") != null)
        {
            baseCombatScore = int.Parse(curEquipmentNode["BaseCombatScore"].InnerText);
        }
        if (curEquipmentNode.SelectSingleNode("BaseCost") != null)
        {
            baseCost = int.Parse(curEquipmentNode["BaseCost"].InnerText);
        }

        if (curEquipmentNode.SelectSingleNode("TrainingRequirement") != null)
        {
            Enum.TryParse(curEquipmentNode["TrainingRequirement"].InnerText, out abilityRequirement);
        }
        if (curEquipmentNode.SelectSingleNode("HandRequirement") != null)
        {
            int handRequirementint = int.Parse(curEquipmentNode["HandRequirement"].InnerText);

            itemHandRequirement = CharacterEquipmentTypes.XMLintToHandRequirement(handRequirementint);
        }
    }
}
