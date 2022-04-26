using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyFunctions : MonoBehaviour
{
    public GameObject traitHandlerObject, CharacterHandler, PartyHandler, allCharStorage;
    TraitHandler TraitHandler;
    CharFunctions CharFunctions;
    PartyStorage PartyStorage;
    AllCharacterStorage AllCharacterStorage;

    // Start is called before the first frame update
    void Awake()
    {
        TraitHandler = traitHandlerObject.GetComponent<TraitHandler>();
        CharFunctions = CharacterHandler.GetComponent<CharFunctions>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        AllCharacterStorage = allCharStorage.GetComponent<AllCharacterStorage>();
    }

    /// <summary>
    /// Loops through each character in a party to calculate strength
    /// </summary>
    /// <param name="partyID"></param>
    /// <param name="conID"></param>
    /// <returns></returns>
    public int CalculatePartyStrength(int partyID, int conID)
    {
        int partyStrength = 0;
        Party party = PartyStorage.findPartyFromID(partyID);

        foreach (Character character in party.members)
        {
            //get the individual character strength
            int thisCharsStrength = CharFunctions.getCharsStrengthForContract(character, conID);

            //add it to total party strength
            partyStrength = partyStrength + thisCharsStrength;
        }

        Debug.Log("This Parties strength before party modifiers: " + partyStrength);
        partyStrength = HelperFunctions.partyStrengthAfterModifiers(party, partyStrength);
        Debug.Log("This Parties final strength: " + partyStrength);
        return partyStrength;
    }
}
