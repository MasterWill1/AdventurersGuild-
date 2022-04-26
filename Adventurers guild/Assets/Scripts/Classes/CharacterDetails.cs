using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetails : MonoBehaviour
{
    public int XP;
    public CharacterTypes.CharacterClass charClass;
    public CharacterTypes.CharacterRace race;
    public int ID;
    public int inParty;
    public bool isRecruited; // will need to change to a int, with 0 for not recruited, 1 for recruited by player and negatives represent working for another org
    public string charName;
    public int cost;

    public CharacterDetails(int _ID, string _charName, int _XP, CharacterTypes.CharacterClass _charClass, CharacterTypes.CharacterRace _race, int _inParty, bool _isRecruited,
        int _cost)
    {
        ID = _ID;
        charName = _charName;
        XP = _XP;
        charClass = _charClass;
        race = _race;
        inParty = _inParty;
        isRecruited = _isRecruited;
        cost = _cost;
    }
}
