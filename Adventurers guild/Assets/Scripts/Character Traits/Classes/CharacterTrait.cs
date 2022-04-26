using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrait : MonoBehaviour
{
    public int charId;
    public TraitTypes.traitTag traitTag;

    public CharacterTrait(int _charId, TraitTypes.traitTag _traitTag)
    {
        charId = _charId;
        traitTag = _traitTag;
    }

}
