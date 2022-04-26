using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecodeID : MonoBehaviour
{
    public string CharRace(int raceID)
    {
        switch (raceID)
        {
            case 1:
                return "Human";
            case 2:
                return "Elf";
            case 3:
                return "Dwarf";
            case 4:
                return "Halfing";
            case 5:
                return "Half-Orc";
        }
        return "error: " + raceID.ToString();
    }

    public string CharClass(int classID)
    {
        switch (classID)
        {
            case 1:
                return "Fighter";
            case 2:
                return "Wizard";
            case 3:
                return "Bard";
            case 4:
                return "Ranger";
            case 5:
                return "Paladin";
        }
        return "error: " + classID.ToString();
    }
}
