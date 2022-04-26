using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMood : MonoBehaviour
{
    public int Id; //character Id
    public List<Moodlet> moodletList; //List of moodlets
    public CharacterMood(int _Id, List<Moodlet> _moodletList)
    {
        Id = _Id;
        moodletList = _moodletList;
    }
}
