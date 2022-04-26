using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aFactionBar : MonoBehaviour
{
    Faction thisFaction;
    public Text FactionNameText;
    public Button thisFactionButton;
    GameObject factionDetailsScreenGO;
    factionDetailsScreen factionDetailsScreen;

    private void Awake()
    {
        factionDetailsScreenGO = GameObject.FindWithTag("factionDetailsScreen");
        factionDetailsScreen = factionDetailsScreenGO.GetComponent<factionDetailsScreen>();

        Button thisFactionBtn = thisFactionButton.GetComponent<Button>();
        thisFactionBtn.onClick.AddListener(viewFaction);
    }
    public void setSelf(Faction faction)
    {
        thisFaction = faction;

        FactionNameText.text = thisFaction.factionName;
    }
    void viewFaction()
    {
        factionDetailsScreen.setVisuals(thisFaction);
    }
}
