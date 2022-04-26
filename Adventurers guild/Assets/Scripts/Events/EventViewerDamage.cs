using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventViewerDamage : MonoBehaviour
{
    public GameObject allCharStorageObject;

    AllCharacterStorage AllCharacterStorage;
    EventViewer EventViewer;
    private void Start()
    {
        EventViewer = gameObject.GetComponent<EventViewer>();
        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();
    }
    public string charactersDamageEventString(int[,] charIdsAndDamage)
    {
        string damageString = "";

        //if id and damage array isnt empty
        if (charIdsAndDamage.GetLength(0) > 0)
        {
            for (int i = 1; i < charIdsAndDamage.GetLength(0); i++)
            {
                damageString = damageString + AllCharacterStorage.findAliveCharacterFromID(charIdsAndDamage[i, 0]).characterDetails.charName +
                    " took " + charIdsAndDamage[i, 1] + "damage, ";

            }
        }

        return damageString;
    }
}
