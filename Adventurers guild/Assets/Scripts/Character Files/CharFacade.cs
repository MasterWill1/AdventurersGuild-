using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all the visuals related to characters
public class CharFacade : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> recruitableCharactersRows, employeeCharactersRows;

    public GameObject CharacterHandler;
    CharStorage CharStorage;

    [Header("Recruitable Characters Screen")]
    public GameObject rcScrollContent;
    public GameObject rcScrollItemPrefab;
    [Header("Employed Characters Screen")]
    public GameObject employeeScrollContent;
    public GameObject employeeScrollItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CharStorage = CharacterHandler.GetComponent<CharStorage>();
    }

    //Displays a recruitable character
    public void displayRecuitable(CharacterDetails gendChar)
    {
        //generate object
        GameObject scrollItemObj = Instantiate(rcScrollItemPrefab);

        //generate visuals
        scrollItemObj.transform.SetParent(rcScrollContent.transform, false);

        //assign character to object
        aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
        objAccess.thisCharacterID = gendChar.ID;

        RecruitableCharacter rcObjAccess = scrollItemObj.GetComponent<RecruitableCharacter>();
        rcObjAccess.setCharCostVisual();

        //set visuals
        objAccess.updateVisuals();
        recruitableCharactersRows.Add(scrollItemObj);
    }

    //called when recuit character button is pressed.
    //Creates a new object visual for the recuited char. Old visual has already been deleted by this point
    public void displayEmployee(int newEmployeeID)
    {
        //generate object
        GameObject scrollItemObj = Instantiate(employeeScrollItemPrefab);
        employeeCharactersRows.Add(scrollItemObj);

        //set parent
        scrollItemObj.transform.SetParent(employeeScrollContent.transform, false);

        //assign character to object
        aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
        objAccess.thisCharacterID = newEmployeeID;

        //set visuals
        objAccess.updateVisuals();
    }

    public void ClearAllData()
    {
        foreach (GameObject n in recruitableCharactersRows)
        {
            Destroy(n);
        }

        foreach (GameObject n in employeeCharactersRows)
        {
            Destroy(n);
        }

        recruitableCharactersRows.Clear();
        employeeCharactersRows.Clear();
    }

    //loops through each employed character row to see if the add to party button needs to be reactivated
    public void allSetAddToPartyButton()
    {
        foreach (GameObject n in employeeCharactersRows)
        {
            n.GetComponent<EmployeeCharacter>().decideOnReActivateAddToPartyButton();
        }
    }

    //updates a employed characters visuals by their ID
    public void updateCharVisuals(int ID)
    {
        foreach (GameObject g in employeeCharactersRows)
        {
            if (g.GetComponent<aCharacter>().thisCharacterID == ID)
            {
                g.GetComponent<aCharacter>().updateVisuals();
                break;
            }
        }
    }
}
