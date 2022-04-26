using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideGOOnClick : MonoBehaviour
{
    public GameObject objToHide;
    public Button Button;

    // Start is called before the first frame update
    void Start()
    {
        Button showBtn = Button.GetComponent<Button>();
        showBtn.onClick.AddListener(showObject);
    }

    void showObject()
    {
        objToHide.SetActive(false);
    }
}
