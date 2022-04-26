using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public int charID;
    public int gender;
    public int lawfulness;
    public int goodness;
    public LocationTypes.locationBiomeTag nativeBiome;

    //to add: sexuality, home location
    public CharacterAttributes(int _charID, int _gender, int _lawfulness, int _goodness, LocationTypes.locationBiomeTag _nativeBiome)
    {
        charID = _charID;
        gender = _gender;
        lawfulness = _lawfulness; //0= chaotic, 10=lawful
        goodness = _goodness; //0=evil, 10= good
        nativeBiome = _nativeBiome;
    }
}
