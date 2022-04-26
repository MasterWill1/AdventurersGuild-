using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterDetailsScreen : MonoBehaviour
{
    public GameObject CharHandler, TraitHandlerAccessor, selfGO, HealthHandlerGameObject, AllCharacterAccessor, MoodHandler, EquipmentHandlerGO;
    TraitHandler TraitHandlerScriptAccessor;
    AllCharacterStorage AllCharacterStorage;
    CharMoodStorage CharMoodStorage;
    CharacterEquipmentStorage CharacterEquipmentStorage;
    public Button closeScreenButton;
    public GameObject RelationshipsScrollContainer, RelationshipScrollItemPrefab, MoodletScrollContainer, MoodletScrollItemPrefab;

    [Header("Text boxes")]
    public GameObject TitleTextbox;
    public GameObject levelTextbox, raceTextbox, classTextbox, xpTextbox, xpTillLvlUpTextbox, genderTextbox, lawfulnessTextbox, goodnessTextbox,
        traitsTextbox, strengthTextbox, nativeBiomeTextbox, valueTextbox, wageTextbox, healthTextbox, moodTextBox,
        headEquipmentItemButton, bodyEquipmentItemButton, handEquipmentItemButton, strongHandEquipmentItemButton, offHandEquipmentItemButton, utilityEquipmentItemButton;

    List<GameObject> charOpinionRows = new List<GameObject>();
    List<GameObject> charMoodRows = new List<GameObject>();

    charEquipmentInteraction charEquipmentInteraction;

    // Start is called before the first frame update
    void Start()
    {
        TraitHandlerScriptAccessor = TraitHandlerAccessor.GetComponent<TraitHandler>();

        AllCharacterStorage = AllCharacterAccessor.GetComponent<AllCharacterStorage>();

        Button closeScreenBtn = closeScreenButton.GetComponent<Button>();
        closeScreenBtn.onClick.AddListener(nullScreen);

        charEquipmentInteraction = gameObject.GetComponent<charEquipmentInteraction>();

        CharMoodStorage = MoodHandler.GetComponent<CharMoodStorage>();

        CharacterEquipmentStorage = EquipmentHandlerGO.GetComponent<CharacterEquipmentStorage>();
    }

    public void setVisuals(int charID)
    {
        nullScreen();
        selfGO.SetActive(true);

        //CharacterDetails thisCharacter = CharStorage.findCharacterFromID(charID);
        //CharacterAttributes thisCharAttributes = TraitHandlerScriptAccessor.getAttributesFromID(charID);
        //TraitReference thisCharTraitsRef = TraitHandlerScriptAccessor.getTraitsReferenceFromID(charID);
        //CharacterHealth thisCharhealth = HealthHandler.findCharHealthByID(charID);

        Character thisWholeCharacter = AllCharacterStorage.findAliveCharacterFromID(charID);
        CharacterDetails thisCharacter = thisWholeCharacter.characterDetails;
        CharacterAttributes thisCharAttributes = thisWholeCharacter.characterAttributes;
        TraitReference thisCharTraitsRef = thisWholeCharacter.traitReference;
        CharacterHealth thisCharhealth = thisWholeCharacter.characterHealth;

        List<Trait> traitsList = TraitHandlerScriptAccessor.getTraitListFromTagList(thisCharTraitsRef.traitList);

        TitleTextbox.GetComponent<Text>().text = thisCharacter.charName;
        levelTextbox.GetComponent<Text>().text = "Level " + HelperFunctions.getCharactersLevel(thisCharacter.XP).ToString();
        raceTextbox.GetComponent<Text>().text = HelperFunctions.CharRaceEnumToString(thisCharacter.race);
        classTextbox.GetComponent<Text>().text = HelperFunctions.CharClassEnumToString(thisCharacter.charClass);
        xpTextbox.GetComponent<Text>().text = "Xp: " + thisCharacter.XP.ToString();
        xpTillLvlUpTextbox.GetComponent<Text>().text = "Xp till lvl up: " + HelperFunctions.xpTillLevelUp(thisCharacter.XP);
        genderTextbox.GetComponent<Text>().text = HelperFunctions.intToGender(thisCharAttributes.gender);
        lawfulnessTextbox.GetComponent<Text>().text = HelperFunctions.lawfulnessIntToString(thisCharAttributes.lawfulness);
        goodnessTextbox.GetComponent<Text>().text = HelperFunctions.goodnessIntToString(thisCharAttributes.goodness);
        traitsTextbox.GetComponent<Text>().text = TraitHandlerScriptAccessor.traitsListAsStringFromID(charID);
        strengthTextbox.GetComponent<Text>().text = "Strength: "+ HelperFunctions.calculateCharacterStrength(thisCharacter, traitsList, thisCharAttributes, null, thisWholeCharacter.characterEquipment).ToString();
        nativeBiomeTextbox.GetComponent<Text>().text = "Native Biome: " + thisCharAttributes.nativeBiome;
        valueTextbox.GetComponent<Text>().text = "Value: " + (thisCharacter.cost);
        wageTextbox.GetComponent<Text>().text = "Wage: " + HelperFunctions.wageFromWorth(thisCharacter.cost);
        healthTextbox.GetComponent<Text>().text = "Health: " + thisCharhealth.currentHealth + "/" + thisCharhealth.maxHealth;
        moodTextBox.GetComponent<Text>().text = "Total Mood: " + AllCharacterStorage.getCharsTotalMood(charID);

        //Set list of relationships
        List<int> charHasOpinionsOf = new List<int>(AllCharacterStorage.getIdsOfCharsCharacterHasOpinionletFor(charID));
        foreach(int targetId in charHasOpinionsOf)
        {
            string charName = AllCharacterStorage.findAliveCharacterFromID(targetId).characterDetails.charName;

            GameObject opinionBar = Instantiate(RelationshipScrollItemPrefab);
            opinionBar.transform.SetParent(RelationshipsScrollContainer.transform, false);
            charOpinionRows.Add(opinionBar);
            aOpinonBar aOpinonBar = opinionBar.GetComponent<aOpinonBar>();
            aOpinonBar.setVariables(charID, targetId, charName);
            aOpinonBar.updateVisuals();
        }

        //Set all equipment
        headEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.headEquipmentItem);
        bodyEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.bodyEquipmentItem);
        handEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.handEquipmentItem);
        strongHandEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.strongHandItem);
        offHandEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.offHandItem);
        utilityEquipmentItemButton.GetComponentInChildren<Text>().text =
            CharacterEquipmentStorage.equipmentItemToStringDescription(thisWholeCharacter.characterEquipment.utilityEquipmentItem);

        //decide whether we should be able to edit the characters equipment
        if (thisWholeCharacter.characterDetails.isRecruited)
        {
            charEquipmentInteraction.clickableEquipmentButtons(charID);
        }
        else
        {
            charEquipmentInteraction.unclickableEquipmentButtons();
        }

        //Set mood
        foreach(Moodlet moodlet in thisWholeCharacter.characterMood.moodletList)
        {
            MoodletDef moodletDef = CharMoodStorage.getMoodletDefFromTag(moodlet.moodEventEnum);

            GameObject moodBar = Instantiate(MoodletScrollItemPrefab);
            moodBar.transform.SetParent(MoodletScrollContainer.transform, false);
            charMoodRows.Add(moodBar);
            aMoodBar aMoodBar = moodBar.GetComponent<aMoodBar>();
            aMoodBar.setSelf(moodlet, moodletDef);

        }
    }

    void nullScreen()
    {
        TitleTextbox.GetComponent<Text>().text = "unset name";
        levelTextbox.GetComponent<Text>().text = "unset level";
        raceTextbox.GetComponent<Text>().text = "unset race";
        classTextbox.GetComponent<Text>().text = "unset class";
        xpTextbox.GetComponent<Text>().text = "unset xp";
        genderTextbox.GetComponent<Text>().text = "unset gender";
        lawfulnessTextbox.GetComponent<Text>().text = "unset lawfulness";
        goodnessTextbox.GetComponent<Text>().text = "unset goodness";
        traitsTextbox.GetComponent<Text>().text = "unset traits";
        nativeBiomeTextbox.GetComponent<Text>().text = "unset Native Biome";
        xpTillLvlUpTextbox.GetComponent<Text>().text = "unset xp till lvl up";
        moodTextBox.GetComponent<Text>().text = "Mood unset";

        if (charOpinionRows.Count != 0)
        {
            foreach (GameObject n in charOpinionRows)
            {
                Destroy(n);
            }
        }       
    }
}
