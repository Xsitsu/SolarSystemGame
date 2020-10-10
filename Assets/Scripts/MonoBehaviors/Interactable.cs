using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Interactable : MonoBehaviour
{
    public GameObject adornee;
    public GameObject canvas;
    public GameObject button;
    public GameObject textLabelName;
    public GameObject textLabelDistance;

    TextMeshProUGUI labelName;
    TextMeshProUGUI labelDistance;
    void Awake()
    {
        if (textLabelName)
        {
            labelName = textLabelName.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("Could not find labelName");
        }

        if (textLabelDistance)
        {
            labelDistance = textLabelDistance.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("Could not find labelDistance");
        }

        SetNameLabel("");
        SetDistanceLabel("");
    }
    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(Interact);
    }

    void Update()
    {
        bool hide = true;
        if (adornee != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(adornee.transform.position);
            if (screenPosition.z > 0)
            {
                transform.position = new Vector3(screenPosition.x, screenPosition.y, 0);
                hide = false;
            }

        }

        canvas.SetActive(!hide);
    }

    public void Interact()
    {
        Debug.Log("Interact!");

        // test stuff
        OrbitalBodyMono mono = adornee.GetComponent<OrbitalBodyMono>();
        GameObject character = PlayerManager.Instance.character;
        character.transform.SetParent(mono.satellites.transform);
        character.transform.localPosition = mono.display.transform.localScale * 0.6f;
    }

    public void SetNameLabel(string setText)
    {
        if (labelName)
        {
            labelName.SetText(setText);
        }
        else
        {
            Debug.Log("Could not set name label to: " + setText);
        }
    }

    public void SetDistanceLabel(string setText)
    {
        if (labelDistance)
        {
            labelDistance.SetText(setText);
        }
        else
        {
            Debug.Log("Could not set distance label to: " + setText);
        }
    }
}
