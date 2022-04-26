using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aLocationBar : MonoBehaviour
{
    Location thisLocation;
    public Text LocationNameText, LocationOwnerText;
    public Button thisLocationButton;
    GameObject locationDetailsScreenGO;
    locationDetailsScreen locationDetailsScreen;

    private void Awake()
    {
        locationDetailsScreenGO = GameObject.FindWithTag("locationDetailsScreen");
        locationDetailsScreen = locationDetailsScreenGO.GetComponent<locationDetailsScreen>();

        Button thisLocationBtn = thisLocationButton.GetComponent<Button>();
        thisLocationBtn.onClick.AddListener(viewLocation);
    }
    public void setSelf(Location location)
    {
        thisLocation = location;

        LocationNameText.text = location.locationName;
        LocationOwnerText.text = location.ownerFactionId.ToString();
    }

    void viewLocation()
    {
        locationDetailsScreen.setVisuals(thisLocation);
    }
}
