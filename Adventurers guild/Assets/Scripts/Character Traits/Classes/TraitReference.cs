using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitReference : MonoBehaviour
{
    public int ID;
    public List<TraitTypes.traitTag> traitList;

    public TraitReference(int _ID, List<TraitTypes.traitTag> _traitList)
    {
        ID = _ID;
        traitList = _traitList;
    }
}
