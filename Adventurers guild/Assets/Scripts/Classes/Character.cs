using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int charId;
    public CharacterDetails characterDetails;
    public CharacterAttributes characterAttributes;
    public TraitReference traitReference;
    public CharacterHealth characterHealth;
    public NameReference nameReference;
    public CharacterMood characterMood;
    public CharacterRelationships characterRelationships;
    public CharacterEquipment characterEquipment;

    public Character(int _charId, CharacterDetails _characterDetails, CharacterAttributes _characterAttributes, TraitReference _traitReference,
        CharacterHealth _characterHealth, NameReference _nameReference, CharacterMood _characterMood, CharacterRelationships _characterRelationships,
        CharacterEquipment _characterEquipment)
    {
        characterDetails = _characterDetails;
        characterAttributes = _characterAttributes;
        traitReference = _traitReference;
        characterHealth = _characterHealth;
        nameReference = _nameReference;
        charId = _charId;
        characterMood = _characterMood;
        characterRelationships = _characterRelationships;
        characterEquipment = _characterEquipment;
    }
}
