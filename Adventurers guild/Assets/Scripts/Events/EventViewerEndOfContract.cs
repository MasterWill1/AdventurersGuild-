using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventViewerEndOfContract : MonoBehaviour
{
    public GameObject contractHandler;
    EventViewer EventViewer;
    EventViewerDamage EventViewerDamage;
    ContractStorage ContractStorage;

    private void Start()
    {
        EventViewer = gameObject.GetComponent<EventViewer>();
        EventViewerDamage = gameObject.GetComponent<EventViewerDamage>();
        ContractStorage = contractHandler.GetComponent<ContractStorage>();
    }

    public void createEndOfContractEvent(int contractId, bool success, bool crit, List<int[,]> damageEvents)
    {
        SingleEvent EndOfConEvent = EventViewer.createBlankEvent();
        EndOfConEvent.contractId = contractId;

        finaliseTextAndCreateEndOfContractEventBar(success, crit, contractId, damageEvents, EndOfConEvent);
    }

    //creates the end of contract summary events
    public void finaliseTextAndCreateEndOfContractEventBar(bool success, bool crit, int conId, List<int[,]> damageEvents, SingleEvent EndOfConEvent)
    {
        string introText = "";
        string rewardText = "";
        Contract thisCon = ContractStorage.findContractFromID(conId);

        if (success)
        {
            if (crit)
            {
                introText = "Contract : " + conId + " ended in a success. ";
            }
            else
            {
                introText = "Contract : " + conId + " ended in a critical success. ";
            }
            rewardText = "The agreed " + thisCon.reward + "gp was awarded. ";
        }
        else
        {
            if (crit)
            {
                introText = "Contract : " + conId + " ended in a critical failure. ";
            }
            else
            {
                introText = "Contract : " + conId + " ended in a failure. ";
            }
        }

        string damageSummary = "";
        foreach(int[,] charIdsAndDamage in damageEvents)
        {
            damageSummary = damageSummary + EventViewerDamage.charactersDamageEventString(charIdsAndDamage);
            damageSummary = damageSummary + " fighting the boss";
        }

        EndOfConEvent.eventDescription = introText + damageSummary + rewardText;

        EventViewer.createEventBar(EndOfConEvent);
    }
}
