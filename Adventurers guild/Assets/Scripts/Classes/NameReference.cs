using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameReference : MonoBehaviour
{
    public int ID, FN, SN;

    public NameReference(int _ID, int _FN, int _SN)
    {
        ID = _ID;
        FN = _FN;
        SN = _SN;
    }
}
