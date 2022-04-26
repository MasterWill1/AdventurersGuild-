using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableEquipmentFacade : MonoBehaviour
{
    public GameObject EquipmentHandler, availableEquipmentScreen;
    CharacterEquipmentStorage CharacterEquipmentStorage;
    public List<GameObject> equipmentItemBars = new List<GameObject>();

    public Button closeMenuButton;

    equipmentItem blankEquipmentItem;

    public GameObject equipmentBarPrefab, scrollAreaContainer;

    AllCharacterStorage AllCharacterStorage;
    public GameObject AllCharacterStorageHandler;

    PartyStorage PartyStorage;
    public GameObject PartyHandler;

    // Start is called before the first frame update
    void Awake()
    {
        CharacterEquipmentStorage = EquipmentHandler.GetComponent<CharacterEquipmentStorage>();
        AllCharacterStorage = AllCharacterStorageHandler.GetComponent<AllCharacterStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();

        Button closeMenuBtn = closeMenuButton.GetComponent<Button>();
        closeMenuBtn.onClick.AddListener(closeMenu);

        blankEquipmentItem = gameObject.AddComponent<equipmentItem>();
        blankEquipmentItem.setNullVariables();
    }

    //can also take in a int "forCharId" so we know who to swap out item for, which can be 0 for no char
    public void showEquipmentList(CharacterEquipmentTypes.itemType filter, int forCharId)
    {
        //reset the view
        closeMenu();
        availableEquipmentScreen.SetActive(true);


        //if forCharId /= 0, then it is for replacement and will need to create a blank equipment
        if(forCharId != 0)
        {
            instantiateNewBar(blankEquipmentItem, filter, forCharId);
        }

        foreach (equipmentItem item in CharacterEquipmentStorage.allRealEquipmentItems)
        {
            bool doCreateBar = true;

            //if item doesnt match filter and there is a filter, dont create
            if (item.itemType != filter && filter != CharacterEquipmentTypes.itemType.none) 
            {
                doCreateBar = false;
            }
            //Dont show the item if the intended user is already using it
            if(item.charId == forCharId)
            {
                doCreateBar = false;
            }

            if (doCreateBar)
            {
                //if the item is in store and not in use, show it
                if (item.charId == 0)
                {
                    instantiateNewBar(item, filter, forCharId);
                }
                else
                {
                    //Only show item if the owner of the item works at the guild
                    Character ItemOwner = AllCharacterStorage.findAliveCharacterFromID(item.charId);
                    if (ItemOwner.characterDetails.isRecruited == true)
                    {
                        //If the owner is not in a party then they are not on contract so show item
                        if (ItemOwner.characterDetails.inParty == 0)
                        {
                            instantiateNewBar(item, filter, forCharId);
                        }
                        //else only show item if owners party is not on quest
                        else if (PartyStorage.findPartyFromID(ItemOwner.characterDetails.inParty).onQuest == 0)
                        {
                            instantiateNewBar(item, filter, forCharId);
                        }
                    }
                }
            }
        }


    }

    void instantiateNewBar(equipmentItem equipmentItem, CharacterEquipmentTypes.itemType itemTypeToReplace, int forCharId)
    {
        GameObject newEquipmentBar = Instantiate(equipmentBarPrefab);
        newEquipmentBar.transform.SetParent(scrollAreaContainer.transform, false);
        equipmentItemBars.Add(newEquipmentBar);

        aEquipmentItem aEquipmentItem = newEquipmentBar.GetComponent<aEquipmentItem>();
        aEquipmentItem.setSelf(equipmentItem, forCharId, itemTypeToReplace);
    }

    public void closeMenu()
    {
        foreach(GameObject bar in equipmentItemBars)
        {
            Destroy(bar);
        }
        equipmentItemBars.Clear();
    }
}
