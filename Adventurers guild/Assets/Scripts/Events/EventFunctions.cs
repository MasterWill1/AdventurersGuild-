using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//list of static generic functions that most event systems will use
public static class EventFunctions
{
    //takes a event list and a number x and outputs x number of random events from the list
    public static List<int> getCollectionOfEventsFromEventsList(int numberOfEvents, int sizeOfEventList)
    {
        List<int> events = new List<int>();

        List<int> availableEventIds = new List<int>();
        if (numberOfEvents > sizeOfEventList)
        {
            numberOfEvents = sizeOfEventList;
        }

        //create the list of available Ids
        for (int x = 0; x < sizeOfEventList; x++)
        {
            availableEventIds.Add(x);
        }

        for (int x = 1; x <= numberOfEvents; x++)
        {
            int eventToAdd = Random.Range(0, availableEventIds.Count);

            int thisEvent = availableEventIds[eventToAdd];
            events.Add(thisEvent);
            availableEventIds.Remove(thisEvent);

        }
        return events;
    }
}
