using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUITab : MonoBehaviour
{
    public GameObject objToShow;
    public Button tabButton;
    public bool closeIfOpen;

    GameObject[] openTabs;

    // Start is called before the first frame update
    void Start()
    {
        Button closeBtn = tabButton.GetComponent<Button>();
        closeBtn.onClick.AddListener(ShowTab);
    }

    void ShowTab()
    {
        if (objToShow.activeInHierarchy == false)
        {
            openTabs = GameObject.FindGameObjectsWithTag("UI Tab");
            foreach (GameObject tab in openTabs)
            {
                tab.SetActive(false);
            }
            objToShow.SetActive(true);
        }
        else
        {
            if (closeIfOpen == true)
            {
                objToShow.SetActive(false);
            }
        }
    }
}
