using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypes
{
    public enum CharacterRace //TODO - to be merged with ContractTypes.contractTargetTag
    {
        none, Human, Elf, Dwarf, Halfling, HalfOrc
    }
    public enum CharacterClass
    {
        none, Fighter, Wizard, Bard, Ranger, Paladin, Rogue
    }
    public static List<CharacterEquipmentTypes.abilityRequirement> getClassAbilityRequirement(CharacterClass characterClass)
    {
        List<CharacterEquipmentTypes.abilityRequirement> abilityRequirements = new List<CharacterEquipmentTypes.abilityRequirement>();
        switch (characterClass)
        {
            case CharacterClass.Fighter:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.medium);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.heavy);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.martial);
                return abilityRequirements;
            case CharacterClass.Wizard:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.spellcaster);
                return abilityRequirements;
            case CharacterClass.Bard:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.spellcaster);
                return abilityRequirements;
            case CharacterClass.Ranger:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.medium);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.martial);
                return abilityRequirements;
            case CharacterClass.Paladin:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.medium);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.heavy);
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.martial);
                return abilityRequirements;
            case CharacterClass.Rogue:
                abilityRequirements.Add(CharacterEquipmentTypes.abilityRequirement.light);
                return abilityRequirements;
            default:
                Debug.LogError("Invalid class passed in for ability requirement: " + characterClass);
                return abilityRequirements;
        }
    }
}
