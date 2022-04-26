using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int ID;
    public int maxHealth;
    public int currentHealth;
    //public List<int> perminantInjuries;

    public CharacterHealth(int _ID, int _maxHealth, int _currentHealth)
    {
        ID = _ID;
        maxHealth = _maxHealth;
        currentHealth = _currentHealth;
        //perminantInjuries = _perminantInjuries;
    }
}
