using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarLogEntry : MonoBehaviour
{
    public GameObject IconLabel;
    public GameObject TextLabel;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SetIconColor(Color value)
    {
        IconLabel.GetComponent<Image>().color = value;
    }
    public void SetText(string value)
    {
        TextLabel.GetComponent<Text>().text = value;
    }
}
