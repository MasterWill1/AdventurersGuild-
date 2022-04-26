using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contractResultScreen : MonoBehaviour
{
    public GameObject resultScreenScrollView, titleText, xpGainedText, goldGainedText;
    public GameObject resultScreenCharacterPrefab;

    public Button closeMenuButton;

    [HideInInspector]
    public List<GameObject> CharacterRowList;

    // Start is called before the first frame update
    void Awake()
    {
        Button closeMenuBtn = closeMenuButton.GetComponent<Button>();
        closeMenuBtn.onClick.AddListener(closeMenu);
    }

    public void setVisuals(bool success, int xpGained, int goldGained, List<Character> characters)
    {
        if (success == true)
        {
            titleText.gameObject.GetComponent<Text>().text = "Contract Success";
        }
        else
        {
            titleText.gameObject.GetComponent<Text>().text = "Contract Failure";
        }

        xpGainedText.gameObject.GetComponent<Text>().text = "XP gained: "+ xpGained;
        goldGainedText.gameObject.GetComponent<Text>().text = "Gold gained: " + goldGained;

        foreach(Character c in characters)
        {
            //generate object
            GameObject scrollItemObj = Instantiate(resultScreenCharacterPrefab);

            //generate visuals
            scrollItemObj.transform.SetParent(resultScreenScrollView.transform, false);

            //assign character to object
            aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
            objAccess.thisCharacterID = c.charId;

            //set visuals
            objAccess.updateVisuals();
            CharacterRowList.Add(scrollItemObj);
        }
    }

    void closeMenu()
    {
        titleText.gameObject.GetComponent<Text>().text = "Contract result unset";

        xpGainedText.gameObject.GetComponent<Text>().text = "XP gained: unset" ;
        goldGainedText.gameObject.GetComponent<Text>().text = "Gold gained: unset";

        foreach(GameObject g in CharacterRowList)
        {
            Destroy(g);
        }
    }
}
