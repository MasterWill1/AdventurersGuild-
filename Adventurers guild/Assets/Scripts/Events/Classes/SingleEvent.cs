using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEvent : MonoBehaviour
{
    public int eventId; //Id for this event, used so that the events can be put in order
    public int charId; //Id for character the event happened to. 0 If generic event
    public int targetCharId; //Id for the other character involved in event. 0 if no other characters involved
    public int contractId; //Id for contract this event occured on. 0 if didnt occur during contract
    public string eventDescription; //string for the event description. This is what the player will see
    public int eventDate; //date the event occured on

    //a single event is for display and for the user to track what has happened to a character or during a contract
    public SingleEvent(int _eventId, int _charId, int _targetCharId, int _contractId, string _eventDescription, int _eventDate)
    {
        eventId = _eventId;
        charId = _charId;
        targetCharId = _targetCharId;
        contractId = _contractId;
        eventDescription = _eventDescription;
        eventDate = _eventDate;
    }
}
