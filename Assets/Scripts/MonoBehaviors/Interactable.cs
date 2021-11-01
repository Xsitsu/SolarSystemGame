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

    double offsetDistance;

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

                if (PlayerManager.Instance.character != null)
                {
                    double distU = (PlayerManager.Instance.character.transform.position - adornee.transform.position).magnitude;
                    double distM = (distU * Numbers.UnitsToMeters) - offsetDistance;
                    if (distM < 0) distM = 0;

                    double distKM = distM / 1000d;
                    double distAU = distKM / Numbers.AUToKM;
                    double distLY = distKM / Numbers.LightYearToKM;

                    if (distLY >= 0.1)
                    {
                        SetDistanceLabel(System.Math.Round(distLY, 1).ToString() + " ly");
                    }
                    else if (distAU >= 0.1)
                    {
                        SetDistanceLabel(System.Math.Round(distAU, 1).ToString() + " au");
                    }
                    else if (distKM >= 10)
                    {
                        SetDistanceLabel(System.Math.Round(distKM, 0).ToString() + " km");
                    }
                    else
                    {
                        SetDistanceLabel(System.Math.Round(distM, 0).ToString() + " m");
                    }
                }
            }

        }

        canvas.SetActive(!hide);
    }

    public void Interact()
    {
        GameObject character = PlayerManager.Instance.character;

        // test stuff
        OrbitalBodyMono mono = adornee.GetComponent<OrbitalBodyMono>();
        if (mono != null)
        {
            //character.transform.position = mono.gameObject.transform.position + mono.display.transform.localScale * 0.6f;
            StarSystemDisplay.Instance.SetNewOrbitalAnchor(mono.gameObject);
        }
        else
        {
            //character.transform.position = adornee.transform.position + (new Vector3(1, 1, 1) * 1000);
            StarSystemDisplay.Instance.SetNewOrbitalAnchor(adornee);
        }
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

    public void SetOffsetDistance(double dist)
    {
        offsetDistance = dist;
    }
}
