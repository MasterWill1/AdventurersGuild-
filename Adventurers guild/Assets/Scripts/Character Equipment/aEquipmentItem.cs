using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aEquipmentItem : MonoBehaviour
{
    public equipmentItem thisEquipmentItem;
    public Text equipmentNameText, assignedUserText;
    public Button thisItemButton;

    AllCharacterStorage AllCharacterStorage;
    CharacterEquipmentStorage CharacterEquipmentStorage;
    GameObject AllCharacterStorageGameObject, EquipmentHandlerGameObject, characterDetailsScreenGO, availableEquipmentFacadeGO;
    characterDetailsScreen characterDetailsScreenScript;
    AvailableEquipmentFacade AvailableEquipmentFacade;

    int itemForCharId;
    CharacterEquipmentTypes.itemType itemForEquipmentType;


    void Awake()
    {
        AllCharacterStorageGameObject = GameObject.FindWithTag("allCharStorage");
        AllCharacterStorage = AllCharacterStorageGameObject.GetComponent<AllCharacterStorage>();

        thisEquipmentItem = gameObject.AddComponent<equipmentItem>();
        thisEquipmentItem.setNullVariables();
        //equipmentNameText = gameObject.transform.Find("equipmentNameText").gameObject.GetComponent<Text>();
        //assignedUserText = gameObject.transform.Find("assignedUserText").gameObject.GetComponent<Text>();

        Button thisItemBtn = thisItemButton.GetComponent<Button>();
        thisItemBtn.onClick.AddListener(chooseItem);

        characterDetailsScreenGO = GameObject.FindWithTag("characterDetails");
        characterDetailsScreenScript = characterDetailsScreenGO.GetComponent<characterDetailsScreen>();

        availableEquipmentFacadeGO = GameObject.FindWithTag("availableEquipmentFacadeGO");
        AvailableEquipmentFacade = availableEquipmentFacadeGO.GetComponent<AvailableEquipmentFacade>();
    }


    public void setSelf(equipmentItem equipmentItem, int forCharId, CharacterEquipmentTypes.itemType forItemType)
    {
        itemForCharId = forCharId;
        itemForEquipmentType = forItemType;

        AllCharacterStorageGameObject = GameObject.FindWithTag("allCharStorage");
        AllCharacterStorage = AllCharacterStorageGameObject.GetComponent<AllCharacterStorage>();

        EquipmentHandlerGameObject = GameObject.FindWithTag("EquipmentHandler");
        CharacterEquipmentStorage = EquipmentHandlerGameObject.GetComponent<CharacterEquipmentStorage>();

        thisEquipmentItem = equipmentItem;
        updateVisuals();
    }

    public void updateVisuals()
    {
        equipmentNameText.text = CharacterEquipmentStorage.equipmentItemToStringDescription(thisEquipmentItem);

        string userText = "";

        //if the equipment is not in use by any character
        if (thisEquipmentItem.charId <= 0)
        {
            userText = "Unassigned";
        }
        else //display the character that the equipment is being used by
        {
            Character thisboi = AllCharacterStorage.findAliveCharacterFromID(thisEquipmentItem.charId);
            userText = thisboi.characterDetails.charName;
        }
        assignedUserText.text = userText;
    }

    void chooseItem()
    {
        if(thisEquipmentItem.itemType == CharacterEquipmentTypes.itemType.none)
        {
            CharacterEquipmentStorage.removeItemFromUserSlot(itemForCharId, itemForEquipmentType);
        }
        else
        {
            CharacterEquipmentStorage.swapUserItem(itemForCharId, thisEquipmentItem);
        }

        characterDetailsScreenScript.setVisuals(itemForCharId);
        AvailableEquipmentFacade.closeMenu();
    }

}
