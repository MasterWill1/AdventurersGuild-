using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventViewer : MonoBehaviour
{
    public GameObject keyworldDetailsHandlerObject, allCharStorageObject, eventPrefab, eventViewerScroller;
    keyWorldDetailsHandler keyWorldDetailsHandler;
    [HideInInspector]
    public AllCharacterStorage AllCharacterStorage;
    EventViewerSocial EventViewerSocial;
    EventViewerEndOfContract EventViewerEndOfContract;

    int EventIdCounter = 0;
    List<SingleEvent> singleEventList;
    List<GameObject> EventRows;

    private void Start()
    {
        keyWorldDetailsHandler = keyworldDetailsHandlerObject.GetComponent<keyWorldDetailsHandler>();
        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();
        EventViewerSocial = gameObject.GetComponent<EventViewerSocial>();
        EventViewerEndOfContract = gameObject.GetComponent<EventViewerEndOfContract>();

        singleEventList = new List<SingleEvent>();
        EventRows = new List<GameObject>();
    }
    public SingleEvent createBlankEvent()
    {
        SingleEvent singleEvent = gameObject.AddComponent<SingleEvent>();

        EventIdCounter++;
        singleEvent.eventId = EventIdCounter;
        singleEvent.eventDate = keyWorldDetailsHandler.getDate();

        //set settable variables to null:
        singleEvent.contractId = 0;
        singleEvent.charId = 0;
        singleEvent.targetCharId = 0;

        singleEventList.Add(singleEvent);

        return singleEvent;
    }

    public void createEventBar(SingleEvent socialEvent)
    {
        GameObject eventBar = Instantiate(eventPrefab);
        eventBar.transform.SetParent(eventViewerScroller.transform, false);
        EventRows.Add(eventBar);
        AnEventBar thisEventBar = eventBar.GetComponent<AnEventBar>();
        thisEventBar.setEvent(socialEvent);
    }
    /// <summary>
    /// creates the visual event card for positive on contract events
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="damageTaken"></param>
    /// <param name="badSocialEvents"></param>
    public void createAGoodSocialEvent(int charId, int targetCharId, int contractId, EventTypes.goodSocialEvents goodSocialEvent)
    {
        EventViewerSocial.createAGoodSocialEvent(charId, targetCharId, contractId,  goodSocialEvent);
    }

    /// <summary>
    /// creates the visual event card for negative on contract events
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="damageTaken"></param>
    /// <param name="badSocialEvents"></param>
    public void createABadOnContractSocialEvent(int charId, int targetCharId, int contractId, EventTypes.badSocialEvents badSocialEvent, int[,] damageTaken = null)
    {
        EventViewerSocial.createABadOnContractSocialEvent(charId, targetCharId, contractId, damageTaken, badSocialEvent);
    }

    public void createAEndOfContractEvent(int contractId, bool success, bool crit, List<int[,]> damageEvents)
    {
        EventViewerEndOfContract.createEndOfContractEvent(contractId, success, crit,  damageEvents);
    }


}
