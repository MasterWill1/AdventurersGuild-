using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowGameObject : MonoBehaviour
{
    public GameObject objToShow;
    public Button showButton;
    // Start is called before the first frame update
    void Start()
    {
        Button showBtn = showButton.GetComponent<Button>();
        showBtn.onClick.AddListener(showObject);
    }

    void showObject()
    {
        objToShow.SetActive(true);
    }
}
