using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    [HideInInspector]
    public int partyID, onQuest;
    [HideInInspector]
    public List<Character> members;
    [HideInInspector]
    public string partyName;

    public Party(int _partyID, string _partyName, List<Character> _members, int _onQuest)
    {
        partyID = _partyID;
        members = _members;
        onQuest = _onQuest; //0 = not on quest, anything else is the contract id they are on
        partyName = _partyName;
    }
}
