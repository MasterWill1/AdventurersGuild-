using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [HideInInspector]
    public List<CharacterHealth> AllCharactersHealths;

    public GameObject CharacterStorageObject, allCharacterHandler;
    CharStorage CharStorage;
    AllCharacterStorage AllCharacterStorage;

    private void Start()
    {
        CharStorage = CharacterStorageObject.GetComponent<CharStorage>();
        AllCharacterStorage = allCharacterHandler.GetComponent<AllCharacterStorage>();
    }

    public CharacterHealth generateCharHealth(int level, CharacterTypes.CharacterClass charClass, int charID)
    {
        int charHealth = 0;

        switch (charClass)
        {
            case CharacterTypes.CharacterClass.Fighter: //fighter
                charHealth = 10;
                break;
            case CharacterTypes.CharacterClass.Wizard: //wizard
                charHealth = 6;
                break;
            case CharacterTypes.CharacterClass.Bard: //bard
                charHealth = 8;
                break;
            case CharacterTypes.CharacterClass.Ranger: //ranger
                charHealth = 8;
                break;
            case CharacterTypes.CharacterClass.Paladin: //paladin
                charHealth = 10;
                break;
            case CharacterTypes.CharacterClass.Rogue: //rogue
                charHealth = 8;
                break;
        }

        for (int i = 1; i < level; i++)
        {
            charHealth = healthIncreasedByLevelUp(charHealth, charClass);
        }

        return buildCharHealth(charID, charHealth, charHealth);
    }

    int healthIncreasedByLevelUp(int health, CharacterTypes.CharacterClass charClass)
    {
        switch (charClass)
        {
            case CharacterTypes.CharacterClass.Fighter: //fighter
                health = health + 7 + Random.Range(1,3);
                break;
            case CharacterTypes.CharacterClass.Wizard: //wizard
                health = health + 4 + Random.Range(1, 3);
                break;
            case CharacterTypes.CharacterClass.Bard: //bard
                health = health + 5 + Random.Range(1, 3);
                break;
            case CharacterTypes.CharacterClass.Ranger: //ranger
                health = health + 5 + Random.Range(1, 3);
                break;
            case CharacterTypes.CharacterClass.Paladin: //paladin
                health = health + 7 + Random.Range(1, 3);
                break;
            case CharacterTypes.CharacterClass.Rogue: //rogue
                health = health + 5 + Random.Range(1, 3);
                break;
        }
        return health;
    }

    public void levelUpHealth(int charID)
    {
        CharacterHealth characterHealth = findCharHealthByID(charID);
        CharacterTypes.CharacterClass charClass = CharStorage.findCharacterFromID(charID).charClass;
        int healthIncrease = healthIncreasedByLevelUp(characterHealth.maxHealth, charClass);

        characterHealth.maxHealth = characterHealth.maxHealth + healthIncrease;
        characterHealth.currentHealth = characterHealth.currentHealth + healthIncrease;
    }

    public void damageCharacter(int charId, int damage)
    {
        Character character = AllCharacterStorage.findAliveCharacterFromID(charId);
        character.characterHealth.currentHealth = character.characterHealth.currentHealth - damage;

        Debug.Log("Character Id: " + charId + " (" + character.characterDetails.charName + ") was damaged for " + damage + "hp. New health: " + character.characterHealth.currentHealth);

        if (character.characterHealth.currentHealth <= 0)
        {
            AllCharacterStorage.allAliveCharactersList.Remove(character);
            AllCharacterStorage.allDeadCharacterList.Add(character);
        }
        else
        {
            //they are alive but injured, so set their mood accordingly
            decideWhatInjuryMoodDebuff(charId, character.characterHealth.currentHealth, character.characterHealth.maxHealth);
        }
    }

    //fix the character injured debuf by making function that decides what they should be on

    //heals the character by the amount specified
    public void healCharacter(int charId, int healing)
    {
        Character character = AllCharacterStorage.findAliveCharacterFromID(charId);
        int newHealthAfterHeal = character.characterHealth.currentHealth + healing;

        if (character.characterHealth.currentHealth == character.characterHealth.maxHealth)
        {
            Debug.Log("Cant heal: Character Id: " + charId + " (" + character.characterDetails.charName + ") as they were already on max health.");
            return;
        }

        //only heal character to max health and not above
        if (newHealthAfterHeal >= character.characterHealth.maxHealth)
        {
            character.characterHealth.currentHealth = character.characterHealth.maxHealth;
        }
        else
        {
            character.characterHealth.currentHealth = newHealthAfterHeal;
        }
        Debug.Log("Character Id: " + charId + " (" + character.characterDetails.charName + ") was healed for " + healing + "hp. New health: " + character.characterHealth.currentHealth);
        //decide what mood injury debuff they may now be on (if any)
        decideWhatInjuryMoodDebuff(charId, character.characterHealth.currentHealth, character.characterHealth.maxHealth);
    }

    public void decideWhatInjuryMoodDebuff(int charId, int charHealth, int charMaxHealth)
    {
        double charHealthPercent = (double)(charHealth) / (double)(charMaxHealth);
        if (charHealthPercent == 1)
        {
            //if character is now at max health, remove their injured mood buffs
            AllCharacterStorage.removeMoodletFromChar(charId, MoodTypes.moodletTag.injuredMinor);
            AllCharacterStorage.removeMoodletFromChar(charId, MoodTypes.moodletTag.injuredMajor);
        }
        else
        if (charHealthPercent < 1 && charHealthPercent > 0.7) //minor injury
        {
            AllCharacterStorage.addMoodletToChar(charId, MoodTypes.moodletTag.injuredMinor);
        }else
        if (charHealthPercent < 0.7 && charHealthPercent > 0) //major injury
        {
            AllCharacterStorage.addMoodletToChar(charId, MoodTypes.moodletTag.injuredMajor);
        }
    }

    public void dailyHealRecovery()
    {
        Debug.Log("--Routine Daily Healing begins--");
        foreach (Character c in AllCharacterStorage.allAliveCharactersList)
        {
            healCharacter(c.charId, 1);
        }
    }

    public CharacterHealth findCharHealthByID(int charID)
    {
        foreach(CharacterHealth characterHealth in AllCharactersHealths)
        {
            if(characterHealth.ID == charID)
            {
                return characterHealth;
            }
        }
        Debug.LogError("Invalid ID passed when getting character health, ID: " + charID);
        return null;
    }

    public CharacterHealth buildCharHealth(int ID, int currenthealth, int maxHealth)
    {
        CharacterHealth characterHealth = gameObject.AddComponent<CharacterHealth>();
        characterHealth.ID = ID;
        characterHealth.currentHealth = currenthealth;
        characterHealth.maxHealth = maxHealth;

        AllCharactersHealths.Add(characterHealth);
        return characterHealth;
    }
}
