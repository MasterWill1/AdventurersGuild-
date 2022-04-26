using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnEventBar : MonoBehaviour
{
    SingleEvent thisEvent;
    public GameObject eventDescriptionText, eventDateText;


    public void setEvent(SingleEvent singleEvent)
    {
        thisEvent = singleEvent;

        eventDescriptionText.GetComponent<Text>().text = thisEvent.eventDescription;
        eventDateText.GetComponent<Text>().text = "Day: " + thisEvent.eventDate;
    }
}
