using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public int charId;
    public equipmentItem headEquipmentItem;
    public equipmentItem bodyEquipmentItem;
    public equipmentItem handEquipmentItem;
    public equipmentItem strongHandItem;
    public equipmentItem offHandItem;
    public equipmentItem utilityEquipmentItem;

    public CharacterEquipment(int _charId, equipmentItem _headEquipmentItem, equipmentItem _bodyEquipmentItem,
                                equipmentItem _handEquipmentItem, equipmentItem _strongHandItem, equipmentItem _offHandItem,
                                     equipmentItem _utilityEquipmentItem)
    {
        charId = _charId;
        headEquipmentItem = _headEquipmentItem;
        bodyEquipmentItem = _bodyEquipmentItem;
        handEquipmentItem = _handEquipmentItem;
        strongHandItem = _strongHandItem;
        offHandItem = _offHandItem;
        utilityEquipmentItem = _utilityEquipmentItem;
    }
}
