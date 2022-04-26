using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class charEquipmentInteraction : MonoBehaviour
{
    public Button headEquipmentButton, bodyEquipmentButton, handEquipmentButton, strongHandEquipmentButton, offHandEquipmentButton, utilityEquipmentButton;

    public GameObject EquipmentHandlerObject;
    EquipmentHandler EquipmentHandler;

    int charIdForButtons;
    
    // Start is called before the first frame update
    void Start()
    {
        charIdForButtons = 0;

        EquipmentHandler = EquipmentHandlerObject.GetComponent<EquipmentHandler>();
        headEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.headEquipment, charIdForButtons); });
        bodyEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.bodyEquipment, charIdForButtons); });
        handEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.handEquipment, charIdForButtons); });
        strongHandEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.strongHandEquipment, charIdForButtons); });
        offHandEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.offHandEquipment, charIdForButtons); });
        utilityEquipmentButton.onClick.AddListener
            (delegate { EquipmentHandler.showEquipmentList(CharacterEquipmentTypes.itemType.utilityEquipment, charIdForButtons); });
    }

    public void unclickableEquipmentButtons()
    {
        charIdForButtons = 0;

        headEquipmentButton.interactable = false;
        bodyEquipmentButton.interactable = false;
        handEquipmentButton.interactable = false;
        strongHandEquipmentButton.interactable = false;
        offHandEquipmentButton.interactable = false;
        utilityEquipmentButton.interactable = false;        
    }
    public void clickableEquipmentButtons(int charId)
    {
        charIdForButtons = charId;

        headEquipmentButton.interactable = true;
        bodyEquipmentButton.interactable = true;
        handEquipmentButton.interactable = true;
        strongHandEquipmentButton.interactable = true;
        offHandEquipmentButton.interactable = true;
        utilityEquipmentButton.interactable = true;

    }
}
