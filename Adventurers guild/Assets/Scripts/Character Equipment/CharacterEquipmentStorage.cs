using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharacterEquipmentStorage : MonoBehaviour
{
    [HideInInspector]
    public List<CharacterEquipment> allCharEquipmentList;

    [HideInInspector]
    public List<equipmentItem> allRealEquipmentItems, allBlankEquipmentItems;

    CharStorage CharStorage;
    public GameObject CharStorageHandler;

    int equipmentIdCounter = 0;

    public IDictionary<int, EquipmentDef> equipmentDefDictionary = new Dictionary<int, EquipmentDef>();

    XmlDocument targetXmlDocument;

    //seperate each armour type into light medium heavy. add tool to find what each call can use.
    //locally, concatonate equipment lists class can use and choose from there. means we wont have list for every classs and every equipment type
    List<CharacterEquipmentTypes.equipmentName> allHeadEquipmentNames, allBodyEquipmentNames, allHandEquipmentNames, allStrongHandEquipmentNames,
        allOffHandEquipmentNames, allUtilityEquipmentNames,
        lightHeadEquipment, mediumHeadEquipment, heavyHeadEquipment, spellcasterHeadEquipment,
     lightBodyEquipment, mediumBodyEquipment, heavyBodyEquipment, spellcasterBodyEquipment,
     lightHandEquipment, mediumHandEquipment, heavyHandEquipment, spellcasterHandEquipment,
     noRequirementStrongHandItems, martialStrongHandItems, spellcasterStrongHandItems; 

    // Start is called before the first frame update
    void Start()
    {
        CharStorage = CharStorageHandler.GetComponent<CharStorage>();

        Debug.Log("---Started creating equipment defs.---");
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/Equipment");

        targetXmlDocument = new XmlDocument();
        targetXmlDocument.LoadXml(xmlTextAsset.text);

        XmlNodeList targets = targetXmlDocument.SelectNodes("/Defs/Equipment");

        foreach (XmlNode target in targets)
        {
            createEquipmentDatum(target);
        }

        Debug.Log("---Finished creating targets. Created " + equipmentDefDictionary.Count + " equipment defs---");

        startupChecks();

        setAllEquipementItemListsByArmourType();



    }
    public EquipmentDef getEquipmentFromDictionary(CharacterEquipmentTypes.equipmentName equipmentName)
    {
        EquipmentDef equipmentDef;
        equipmentDefDictionary.TryGetValue((int)equipmentName, out equipmentDef);

        if (equipmentDef == null)
        {
            Debug.LogError("Failed to get a item for item name: " + equipmentName);
        }

        return equipmentDef;
    }

    void createEquipmentDatum(XmlNode equipment)
    {
        EquipmentDef newEquipment = new EquipmentDef(equipment);

        equipmentDefDictionary.Add((int)newEquipment.equipmentName, newEquipment);
        Debug.Log("Created equipment: " + newEquipment.equipmentName);
    }

    /// <summary>
    /// Pass in a type and return all item names of that type
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public List<CharacterEquipmentTypes.equipmentName> getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType itemType)
    {
        List<CharacterEquipmentTypes.equipmentName> output = new List<CharacterEquipmentTypes.equipmentName>();

        foreach (CharacterEquipmentTypes.equipmentName equipmentName in CharacterEquipmentTypes.getAllEquipmentNames())
        {
            if (getEquipmentFromDictionary(equipmentName).itemType == itemType)
            {
                output.Add(equipmentName);
            }
        }
        //dont allow none items to be added to list
        if (output.Contains(CharacterEquipmentTypes.equipmentName.none))
        {
            output.Remove(CharacterEquipmentTypes.equipmentName.none);
        }
        return output;
    }
    /// <summary>
    /// Loops through a list of item names and takes out all those that are not of a certain ability type
    /// </summary>
    /// <param name="abilityRequirement"></param>
    /// <param name="equipmentList"></param>
    /// <returns></returns>
    public List<CharacterEquipmentTypes.equipmentName> getAllEquipmentByAbilityRequirementFromList
        (CharacterEquipmentTypes.abilityRequirement abilityRequirement, List<CharacterEquipmentTypes.equipmentName> equipmentList)
    {
        List<CharacterEquipmentTypes.equipmentName> refinedEquipmentList = new List<CharacterEquipmentTypes.equipmentName>();

        foreach (CharacterEquipmentTypes.equipmentName equipment in equipmentList)
        {
            if (getEquipmentFromDictionary(equipment).abilityRequirement == abilityRequirement)
            {
                refinedEquipmentList.Add(equipment);
            }
        }
        //dont allow none items to be added to list
        if (refinedEquipmentList.Contains(CharacterEquipmentTypes.equipmentName.none))
        {
            refinedEquipmentList.Remove(CharacterEquipmentTypes.equipmentName.none);
        }
        return refinedEquipmentList;
    }

    /// <summary>
    /// Creates a random equipment loadout for a character.
    /// </summary>
    /// <param name="charId"></param>
    /// <returns></returns>
    public CharacterEquipment createRandomStartingEquipment(int charId, int charLevel, CharacterTypes.CharacterClass characterClass)
    {
        CharacterEquipment characterEquipment = gameObject.AddComponent<CharacterEquipment>();

        characterEquipment.charId = charId;

        //Creating random starting head equipment for character
        bool hasEquipment = doesCharHaveItemByLevel(charLevel);
        if (hasEquipment)
        {
            characterEquipment.headEquipmentItem = 
                createRandomEquipmentItemByLevelClassAndItemType(charId, charLevel, CharacterEquipmentTypes.itemType.headEquipment, characterClass);
        }
        else
        {
            characterEquipment.headEquipmentItem = createBlankEquipmentItem(charId);
        }

        //Creating random starting body equipment for character
        hasEquipment = doesCharHaveItemByLevel(charLevel);
        if (hasEquipment)
        {
            characterEquipment.bodyEquipmentItem =
                createRandomEquipmentItemByLevelClassAndItemType(charId, charLevel, CharacterEquipmentTypes.itemType.bodyEquipment, characterClass);
        }
        else
        {
            characterEquipment.bodyEquipmentItem = createBlankEquipmentItem(charId);
        }

        //Creating random starting hand equipment for character
        hasEquipment = doesCharHaveItemByLevel(charLevel);
        if (hasEquipment)
        {
            characterEquipment.handEquipmentItem =
                createRandomEquipmentItemByLevelClassAndItemType(charId, charLevel, CharacterEquipmentTypes.itemType.handEquipment, characterClass);
        }
        else
        {
            characterEquipment.handEquipmentItem = createBlankEquipmentItem(charId);
        }

        //creating random start strong hand item for character
        hasEquipment = doesCharHaveItemByLevel(charLevel);
        if (hasEquipment)
        {
            //placeholder until way of choosing which type of strong hand weapon is created
            characterEquipment.strongHandItem =
                createRandomEquipmentItemByLevelClassAndItemType(charId, charLevel, CharacterEquipmentTypes.itemType.strongHandEquipment, characterClass);
        }
        else
        {
            characterEquipment.strongHandItem = createBlankEquipmentItem(charId);
        }

        characterEquipment.offHandItem = createBlankEquipmentItem(charId);
        characterEquipment.utilityEquipmentItem = createBlankEquipmentItem(charId);

        allCharEquipmentList.Add(characterEquipment);
        return characterEquipment;
    }
    /// <summary>
    /// outputs a boolean according to chance, which is dependant on level passed in 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    bool doesCharHaveItemByLevel(int level)
    {
        int noChance = 60; //out of 100

        if (level > 3 && level < 7) //levels 4-6
        {
            noChance = 30;
        }
        else //levels 7 and above
        {
            noChance = 10;
        }

        int chanceRoll = Random.Range(0, 100);
        if (chanceRoll < noChance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public equipmentItem createRandomEquipmentItemByLevelClassAndItemType(int charId, int level, CharacterEquipmentTypes.itemType itemType,
        CharacterTypes.CharacterClass characterClass)
    {
        CharacterEquipmentTypes.equipmentName equipmentName = CharacterEquipmentTypes.equipmentName.none;
        CharacterEquipmentTypes.abilityRequirement abilityRequirement = CharacterEquipmentTypes.abilityRequirement.none;

        List<CharacterEquipmentTypes.abilityRequirement> classAbilityRequirement = CharacterTypes.getClassAbilityRequirement(characterClass);

        if (itemType == CharacterEquipmentTypes.itemType.headEquipment || itemType == CharacterEquipmentTypes.itemType.bodyEquipment
            || itemType == CharacterEquipmentTypes.itemType.handEquipment)
        {
            abilityRequirement =
                getRandomArmourTypeByLevel(level, classAbilityRequirement);

            if(itemType == CharacterEquipmentTypes.itemType.headEquipment)
            {
                equipmentName = getRandomHeadEquipmentByArmourType(abilityRequirement);
            }
            else if (itemType == CharacterEquipmentTypes.itemType.bodyEquipment)
            {
                equipmentName = getRandomBodyEquipmentByArmourType(abilityRequirement);

            }
            else if(itemType == CharacterEquipmentTypes.itemType.handEquipment)
            {
                equipmentName = getRandomHandEquipmentByArmourType(abilityRequirement);
            }
        }
        else if(itemType == CharacterEquipmentTypes.itemType.strongHandEquipment)
        {
            abilityRequirement = getRandomWeaponProficiencyByLevel(level, classAbilityRequirement);

            equipmentName = getRandomStrongHandItemByUserRequirement(abilityRequirement);
        }

        if(equipmentName == CharacterEquipmentTypes.equipmentName.none)
        {
            Debug.LogError("A proper equipmentName of itemType: " + itemType + " was unable to be generated");
        }

        CharacterEquipmentTypes.equipmentQuality equipmentQuality = getRandomEquipmentQualityByLevel(level);
        return buildEquipmentItem(increaseEquipmentIdCounterAndReturn(), charId, equipmentName,  equipmentQuality, CharacterEquipmentTypes.magicEffect.none);
    }

    public CharacterEquipmentTypes.abilityRequirement getRandomArmourTypeByLevel
        (int level, List<CharacterEquipmentTypes.abilityRequirement> availableArmourTypes)
    {
        int lightChance = 60;
        int mediumChance = 30;
        int heavyChance = 5;
        int spellcasterChance = 5;
        if (level > 3 && level < 7) //levels 4-6
        {
            lightChance = 50;
            mediumChance = 30;
            heavyChance = 10;
            spellcasterChance = 10;
        }
        else if(level >= 7) //levels 7 and above
        {
            lightChance = 20;
            mediumChance = 50;
            heavyChance = 20;
            spellcasterChance = 10;

        }
        //if heavy isnt a available armour type to gen, give weighting to next best: medium
        if (!availableArmourTypes.Contains(CharacterEquipmentTypes.abilityRequirement.heavy))
        {
            mediumChance = mediumChance + heavyChance;
            heavyChance = 0;
        }
        //if medium isnt a available armour type to gen, give weighting to next best: light
        if (!availableArmourTypes.Contains(CharacterEquipmentTypes.abilityRequirement.medium))
        {
            lightChance = lightChance + mediumChance;
            mediumChance = 0;
        }

        //if spellcaster isnt available then give the chance to 
        if (!availableArmourTypes.Contains(CharacterEquipmentTypes.abilityRequirement.spellcaster))
        {
            lightChance = lightChance + spellcasterChance;
            spellcasterChance = 0;
        }
        else
        {
            lightChance = lightChance / 2;
            spellcasterChance = (100 - lightChance - mediumChance - heavyChance);
        }

        if ((lightChance+mediumChance+heavyChance+spellcasterChance) != 100)
        {
            Debug.LogError("Total armour type chances do not equal 100. LightChance = " +
                + lightChance + ", mediumChance = " + mediumChance + ", heavyChance = " + heavyChance + ", spellcasterChance: "+spellcasterChance);
        }

        int chanceRoll = Random.Range(0, 100);
        if (chanceRoll < lightChance)
        {
            return CharacterEquipmentTypes.abilityRequirement.light;
        }
        else if (chanceRoll < lightChance + mediumChance)
        {
            return CharacterEquipmentTypes.abilityRequirement.medium;
        }
        else if (chanceRoll < lightChance + mediumChance + heavyChance)
        {
            return CharacterEquipmentTypes.abilityRequirement.heavy;
        }
        else if (chanceRoll < lightChance + mediumChance + heavyChance + spellcasterChance)
        {
            return CharacterEquipmentTypes.abilityRequirement.spellcaster;
        }
        Debug.LogError("Error, ability requirement was not returned. ChanceRoll: " + chanceRoll + ", LightChance = " +
                + lightChance + ", mediumChance = " + mediumChance + ", heavyChance = " + heavyChance + ", spellcasterChance: " + spellcasterChance);
        return CharacterEquipmentTypes.abilityRequirement.none;
    }
    public CharacterEquipmentTypes.abilityRequirement getRandomWeaponProficiencyByLevel(int level, 
        List<CharacterEquipmentTypes.abilityRequirement> availableUserRequirements)
    {
        int noneChance = 60;
        //if it is only possible to return none, return it
        if(!availableUserRequirements.Contains(CharacterEquipmentTypes.abilityRequirement.martial) &&
            !availableUserRequirements.Contains(CharacterEquipmentTypes.abilityRequirement.spellcaster))
        {
            return CharacterEquipmentTypes.abilityRequirement.none;
        }

        //get chance of having basic equipment
        if (level > 3 && level < 7) //levels 4-6
        {
            noneChance = 50;
        }
        else //levels 7 and above
        {
            noneChance = 20;
        }

        int chanceRoll = Random.Range(0, 100);
        if (chanceRoll < noneChance)
        {
            return CharacterEquipmentTypes.abilityRequirement.none;
        }
        else
        {
            if (availableUserRequirements.Contains(CharacterEquipmentTypes.abilityRequirement.martial))
            {
                return CharacterEquipmentTypes.abilityRequirement.martial;
            }
            else
            {
                return CharacterEquipmentTypes.abilityRequirement.spellcaster;
            }
        }
    }

    public CharacterEquipmentTypes.equipmentQuality getRandomEquipmentQualityByLevel(int level)
    {
        //for levels 1-3
        int basicChance = 70;
        int advancedChance = 25;
        //int supremeChance = 5;
        if (level > 3 && level <= 6) //levels 4-6
        {
            basicChance = 50;
            advancedChance = 40;
            //supremeChance = 10;
        }
        else if (level > 6)//levels 7 and above
        {
            basicChance = 20;
            advancedChance = 60;
            //supremeChance = 20;
        }

        int chanceRoll = Random.Range(0, 100);
        if(chanceRoll < basicChance)
        {
            return CharacterEquipmentTypes.equipmentQuality.basic;
        }
        else if(chanceRoll< basicChance + advancedChance)
        {
            return CharacterEquipmentTypes.equipmentQuality.advanced;
        }
        else
        {
            return CharacterEquipmentTypes.equipmentQuality.supreme;
        }
    }

    public CharacterEquipmentTypes.equipmentName getRandomHeadEquipmentByArmourType(CharacterEquipmentTypes.abilityRequirement armourType)
    {
        switch (armourType)
        {
            case CharacterEquipmentTypes.abilityRequirement.light:
                return lightHeadEquipment[Random.Range(0, lightHeadEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.medium:
                return mediumHeadEquipment[Random.Range(0, mediumHeadEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.heavy:
                return heavyHeadEquipment[Random.Range(0, heavyHeadEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.spellcaster:
                return spellcasterHeadEquipment[Random.Range(0, spellcasterHeadEquipment.Count)];
            default:
                Debug.LogError("Invalid armour type passed in for equipment get: " + armourType);
                return CharacterEquipmentTypes.equipmentName.none;
        }
    }
    public CharacterEquipmentTypes.equipmentName getRandomBodyEquipmentByArmourType(CharacterEquipmentTypes.abilityRequirement armourType)
    {
        switch (armourType)
        {
            case CharacterEquipmentTypes.abilityRequirement.light:
                return lightBodyEquipment[Random.Range(0, lightBodyEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.medium:
                return mediumBodyEquipment[Random.Range(0, mediumBodyEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.heavy:
                return heavyBodyEquipment[Random.Range(0, heavyBodyEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.spellcaster:
                return spellcasterBodyEquipment[Random.Range(0, spellcasterBodyEquipment.Count)];
            default:
                Debug.LogError("Invalid armour type passed in for equipment get: " + armourType);
                return CharacterEquipmentTypes.equipmentName.none;
        }
    }
    public CharacterEquipmentTypes.equipmentName getRandomHandEquipmentByArmourType(CharacterEquipmentTypes.abilityRequirement armourType)
    {
        switch (armourType)
        {
            case CharacterEquipmentTypes.abilityRequirement.light:
                return lightHandEquipment[Random.Range(0, lightHandEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.medium:
                return mediumHandEquipment[Random.Range(0, mediumHandEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.heavy:
                return heavyHandEquipment[Random.Range(0, heavyHandEquipment.Count)];
            case CharacterEquipmentTypes.abilityRequirement.spellcaster:
                return spellcasterHandEquipment[Random.Range(0, spellcasterHandEquipment.Count)];
            default:
                Debug.LogError("Invalid armour type passed in for equipment get: " + armourType);
                return CharacterEquipmentTypes.equipmentName.none;
        }
    }
    public CharacterEquipmentTypes.equipmentName getRandomStrongHandItemByUserRequirement(CharacterEquipmentTypes.abilityRequirement userRequirement)
    {
        switch (userRequirement)
        {
            case CharacterEquipmentTypes.abilityRequirement.none:
                return noRequirementStrongHandItems[Random.Range(0, noRequirementStrongHandItems.Count)];
            case CharacterEquipmentTypes.abilityRequirement.martial:
                return martialStrongHandItems[Random.Range(0, martialStrongHandItems.Count)];
            case CharacterEquipmentTypes.abilityRequirement.spellcaster:
                return spellcasterStrongHandItems[Random.Range(0, spellcasterStrongHandItems.Count)];
            default:
                Debug.LogError("invalid user requirement passed in for equipment get: " + userRequirement);
                return CharacterEquipmentTypes.equipmentName.none;
        }
    }

    /// <summary>
    /// creates a blank equipment item
    /// </summary>
    /// <param name="charId"></param>
    public equipmentItem createBlankEquipmentItem(int charId)
    {
        return buildEquipmentItem(0, charId, CharacterEquipmentTypes.equipmentName.none,
            CharacterEquipmentTypes.equipmentQuality.none, CharacterEquipmentTypes.magicEffect.none);
    }

    /// <summary>
    /// Builds a equipment item and sets its fixed parameters
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="charId"></param>
    /// <param name="equipmentName"></param>
    /// <param name="equipmentQuality"></param>
    /// <param name="magicEffect"></param>
    /// <returns></returns>
    public equipmentItem buildEquipmentItem(int itemId, int charId, CharacterEquipmentTypes.equipmentName equipmentName,
        CharacterEquipmentTypes.equipmentQuality equipmentQuality, CharacterEquipmentTypes.magicEffect magicEffect)
    {
        equipmentItem equipmentItem = gameObject.AddComponent<equipmentItem>();
        equipmentItem.setNullVariables();

        equipmentItem.itemId = itemId;
        equipmentItem.charId = charId;
        equipmentItem.equipmentName = equipmentName;
        equipmentItem.equipmentQuality = equipmentQuality;
        equipmentItem.magicEffect = magicEffect;

        if(equipmentName != CharacterEquipmentTypes.equipmentName.none)
        {
            EquipmentDef equipmentDef = getEquipmentFromDictionary(equipmentName);
            equipmentItem.setConsitentParameters(equipmentDef);
        }

        if (equipmentItem.equipmentName == CharacterEquipmentTypes.equipmentName.none)
        {
            allBlankEquipmentItems.Add(equipmentItem);
        }
        else
        {
            allRealEquipmentItems.Add(equipmentItem);
        }
        return equipmentItem;
    }

    public equipmentItem findRealEquipmentItemByItemId(int itemId)
    {
        foreach(equipmentItem equipmentItem in allRealEquipmentItems)
        {
            if(itemId == equipmentItem.itemId)
            {
                return equipmentItem;
            }
        }
        Debug.Log("Cannot find Equipment Item with Id: " + itemId);
        return null;
    }

    equipmentItem findRealEquipmentItemByCharIdAndType(int charId, CharacterEquipmentTypes.itemType itemType)
    {
        foreach (equipmentItem equipmentItem in allRealEquipmentItems)
        {
            if (charId == equipmentItem.itemId)
            {
                if (itemType == equipmentItem.itemType)
                {
                    return equipmentItem;
                }
            }
        }
        Debug.Log("Cannot find Equipment Item with char Id: " + charId);
        return null;
    }

    public CharacterEquipment findCharacterEquipmentByCharId(int charId)
    {
        foreach(CharacterEquipment characterEquipment in allCharEquipmentList)
        {
            if(charId == characterEquipment.charId)
            {
                return characterEquipment;
            }
        }
        Debug.Log("Cannot find Character Equipment with char Id: " + charId);
        return null;
    }

    public void swapUserItem(int charId, equipmentItem itemToGive)
    {       
        //If the item to give is already in use, remove it from their possesion
        if(itemToGive.charId != 0)
        {
            removeItemFromUserSlot(itemToGive.charId, itemToGive.itemType);
        }

        //remove the current item in the chars slot
        removeItemFromUserSlot(charId, itemToGive.itemType);

        addItemToChar(itemToGive, charId);
    }

    public void removeItemFromUserSlot(int charId, CharacterEquipmentTypes.itemType itemType)
    {
        CharacterEquipment characterEquipment = findCharacterEquipmentByCharId(charId);
        
        switch (itemType)
        {
            case CharacterEquipmentTypes.itemType.headEquipment:
                characterEquipment.headEquipmentItem.charId = 0;
                characterEquipment.headEquipmentItem = createBlankEquipmentItem(charId);
                break;
            case CharacterEquipmentTypes.itemType.bodyEquipment:
                characterEquipment.bodyEquipmentItem.charId = 0;
                characterEquipment.bodyEquipmentItem = createBlankEquipmentItem(charId); 
                break;
            case CharacterEquipmentTypes.itemType.handEquipment:
                characterEquipment.handEquipmentItem.charId = 0;
                characterEquipment.handEquipmentItem = createBlankEquipmentItem(charId);
                break;
            case CharacterEquipmentTypes.itemType.strongHandEquipment:
                characterEquipment.strongHandItem.charId = 0;
                characterEquipment.strongHandItem = createBlankEquipmentItem(charId);
                break;
            case CharacterEquipmentTypes.itemType.offHandEquipment:
                characterEquipment.offHandItem.charId = 0;
                characterEquipment.offHandItem = createBlankEquipmentItem(charId);
                break;
            case CharacterEquipmentTypes.itemType.utilityEquipment:
                characterEquipment.utilityEquipmentItem.charId = 0;
                characterEquipment.utilityEquipmentItem = createBlankEquipmentItem(charId);
                break;
            default:
                Debug.LogError("invalid item type passed in: " + itemType);
                break;
        }
    }

    void addItemToChar(equipmentItem item, int charId)
    {
        if(item.charId != 0)
        {
            Debug.LogError("Item already has a user! Item Id: " + item.itemId);
        }

        CharacterEquipment characterEquipment = findCharacterEquipmentByCharId(charId);

        switch (item.itemType)
        {
            case CharacterEquipmentTypes.itemType.headEquipment:
                characterEquipment.headEquipmentItem = item;
                break;
            case CharacterEquipmentTypes.itemType.bodyEquipment:
                characterEquipment.bodyEquipmentItem = item;
                break;
            case CharacterEquipmentTypes.itemType.handEquipment:
                characterEquipment.handEquipmentItem = item;
                break;
            case CharacterEquipmentTypes.itemType.strongHandEquipment:
                characterEquipment.strongHandItem = item;
                break;
            case CharacterEquipmentTypes.itemType.offHandEquipment:
                characterEquipment.offHandItem = item;
                break;
            case CharacterEquipmentTypes.itemType.utilityEquipment:
                characterEquipment.utilityEquipmentItem = item;
                break;
            default:
                Debug.LogError("invalid item passed in: " + item.itemId);
                break;
        }
        item.charId = charId;
    }

    public string equipmentItemToStringDescription(equipmentItem item)
    {
        if (item.equipmentName == CharacterEquipmentTypes.equipmentName.none)
        {
            return "None";
        }

        EquipmentDef equipmentDef = getEquipmentFromDictionary(item.equipmentName);

        string output = CharacterEquipmentTypes.equipmentQualityToString(item.equipmentQuality) + " " +
            equipmentDef.title;       

        return output;
    }

    void setAllEquipementItemListsByArmourType()
    {
        allHeadEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.headEquipment);
        allBodyEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.bodyEquipment);
        allHandEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.handEquipment);
        allStrongHandEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.strongHandEquipment);
        allOffHandEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.offHandEquipment);
        allUtilityEquipmentNames = getAllEquipmentNameByItemType(CharacterEquipmentTypes.itemType.utilityEquipment);

        lightHeadEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.light, allHeadEquipmentNames));
        mediumHeadEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.medium, allHeadEquipmentNames));
        heavyHeadEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.heavy, allHeadEquipmentNames));
        spellcasterHeadEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.spellcaster, allHeadEquipmentNames));

        lightBodyEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.light, allBodyEquipmentNames));
        mediumBodyEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.medium, allBodyEquipmentNames));
        heavyBodyEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.heavy, allBodyEquipmentNames));
        spellcasterBodyEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.spellcaster, allBodyEquipmentNames));

        lightHandEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.light, allHandEquipmentNames));
        mediumHandEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.medium, allHandEquipmentNames));
        heavyHandEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.heavy, allHandEquipmentNames));
        spellcasterHandEquipment = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.spellcaster, allHandEquipmentNames));

        noRequirementStrongHandItems = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.none, allStrongHandEquipmentNames));
        if (noRequirementStrongHandItems.Contains(CharacterEquipmentTypes.equipmentName.none))
        {
            noRequirementStrongHandItems.Remove(CharacterEquipmentTypes.equipmentName.none); //remove none item as we do not want it
        }

        martialStrongHandItems = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.martial, allStrongHandEquipmentNames));
        
        spellcasterStrongHandItems = new List<CharacterEquipmentTypes.equipmentName>
            (getAllEquipmentByAbilityRequirementFromList(CharacterEquipmentTypes.abilityRequirement.spellcaster, allStrongHandEquipmentNames));

    }

    /// <summary>
    /// Loops through each charId to create their CharacterEquipment to add to allCharEquipmentList, ready to be built into a character in AllCharacterStorage.rebuildAllCharacters.
    /// If no equipment item exists, a blank one is created
    /// </summary>
    public void rebuildCharacterEquipment()
    {
        foreach(CharacterDetails characterDetails in CharStorage.characterDetailsList)
        {
            int charId = characterDetails.ID;

            CharacterEquipment characterEquipment = gameObject.AddComponent<CharacterEquipment>();
            characterEquipment.charId = charId;

            characterEquipment.headEquipmentItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.headEquipment);
            if(characterEquipment.headEquipmentItem == null)
            {
                characterEquipment.headEquipmentItem = createBlankEquipmentItem(charId);
            }
            characterEquipment.bodyEquipmentItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.bodyEquipment);
            if (characterEquipment.bodyEquipmentItem == null)
            {
                characterEquipment.bodyEquipmentItem = createBlankEquipmentItem(charId);
            }
            characterEquipment.handEquipmentItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.handEquipment);
            if (characterEquipment.handEquipmentItem == null)
            {
                characterEquipment.handEquipmentItem = createBlankEquipmentItem(charId);
            }
            characterEquipment.strongHandItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.strongHandEquipment);
            if (characterEquipment.strongHandItem == null)
            {
                characterEquipment.strongHandItem = createBlankEquipmentItem(charId);
            }
            characterEquipment.offHandItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.offHandEquipment);
            if (characterEquipment.offHandItem == null)
            {
                characterEquipment.offHandItem = createBlankEquipmentItem(charId);
            }
            characterEquipment.utilityEquipmentItem = findRealEquipmentItemByCharIdAndType(charId, CharacterEquipmentTypes.itemType.utilityEquipment);
            if (characterEquipment.utilityEquipmentItem == null)
            {
                characterEquipment.utilityEquipmentItem = createBlankEquipmentItem(charId);
            }

            allCharEquipmentList.Add(characterEquipment);
        }
    }

    public void setCounters()
    {
        equipmentIdCounter = 0;
        if(allRealEquipmentItems.Count != 0)
        {
            foreach(equipmentItem item in allRealEquipmentItems)
            {
                if(item.itemId > equipmentIdCounter)
                {
                    equipmentIdCounter = item.itemId;
                }
            }
        }
        equipmentIdCounter++;
    }
    /// <summary>
    /// increases the equipment Id counter and returns it
    /// </summary>
    /// <returns></returns>
    int increaseEquipmentIdCounterAndReturn()
    {
        equipmentIdCounter++;
        return equipmentIdCounter;
    }

    void startupChecks()
    {
        if (equipmentDefDictionary.Count != CharacterEquipmentTypes.getAllEquipmentNames().Count)
        {
            Debug.LogError("Mismatch between number of equipments generated and number of equipment names. Equipments generated: " + equipmentDefDictionary.Count
                + ", equipment names: " + CharacterEquipmentTypes.getAllEquipmentNames().Count);
            foreach (CharacterEquipmentTypes.equipmentName equipmentName in CharacterEquipmentTypes.getAllEquipmentNames())
            {
                equipmentDefDictionary.TryGetValue((int)equipmentName, out EquipmentDef thisEquipment);
                if (thisEquipment == null)
                {
                    Debug.LogError(equipmentName);
                }
            }
        }
    }
}
