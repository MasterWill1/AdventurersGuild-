using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRelationships : MonoBehaviour
{
    public int Id; //character Id
    public List<Opinionlet> opinionList; //List of opinions of other characters
    public CharacterRelationships(int _Id, List<Opinionlet> _opinionList)
    {
        Id = _Id;
        opinionList = _opinionList;
    }
}
