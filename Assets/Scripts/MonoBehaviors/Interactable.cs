using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Interactable : MonoBehaviour
{
    public Entity entity;
    public GameObject cameraContainer;
    public GameObject adornee;
    public GameObject canvas;
    public GameObject button;
    public GameObject textLabel;

    double offsetDistance;

    string _nameText = "";
    string _distanceText = "";

    TextMeshProUGUI label;
    void Awake()
    {
        if (textLabel)
        {
            label = textLabel.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("Could not find textLabel");
        }

        SetName("");
        SetDistance(0);
    }
    void Start()
    {
        canvas.GetComponent<Canvas>().worldCamera = cameraContainer.GetComponent<Camera>();
    }

    void Update()
    {
        Update_Screen();
    }
    void Update_Screen()
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
                    SetDistance(distM);
                }
            }
        }

        canvas.SetActive(!hide);
    }
    void Update_VR()
    {
        if (IsVisible())
        {
            canvas.SetActive(true);
            UpdatePosition();
            UpdateRotation();
            UpdateDistance();
        }
        else
        {
            canvas.SetActive(false);
        }
    }
    Vector3 OffsetToAdornee()
    {
        Vector3 fromPos = cameraContainer.transform.position;
        Vector3 targetPos = adornee.transform.position;
        Vector3 offset = targetPos - fromPos;
        return offset;
    }
    Vector3 DirectionToAdornee()
    {
        return Vector3.Normalize(OffsetToAdornee());
    }
    bool IsVisible()
    {
        Vector3 lookDir = cameraContainer.transform.forward;
        Vector3 direction = DirectionToAdornee();
        return Vector3.Dot(lookDir, direction) > 0f;
    }
    void UpdatePosition()
    {
        Vector3 direction = DirectionToAdornee();
        transform.position = cameraContainer.transform.position + (direction * 12f);
    }
    void UpdateRotation()
    {
        Vector3 direction = DirectionToAdornee();
        transform.rotation = Quaternion.LookRotation(direction);
    }
    void UpdateDistance()
    {
        Vector3 offset = OffsetToAdornee();
        float distance = offset.magnitude;
        double distance_m = (distance * Numbers.UnitsToMeters) - offsetDistance;
        SetDistance(distance_m);
    }

    public void Interact()
    {
        WarpManager.Instance.InitializeWarp(PlayerManager.Instance.CharacterStructure, entity);
   }

    public void SetName(string nameText)
    {
        _nameText = nameText;

        UpdateTextLabel();
    }

    public void SetDistance(double distM)
    {
        if (distM < 0) distM = 0;
        
        double distKM = distM / 1000d;
        double distAU = distKM / Numbers.AUToKM;
        double distLY = distKM / Numbers.LightYearToKM;

        if (distLY >= 0.1)
        {
            _distanceText = (System.Math.Round(distLY, 1).ToString() + " ly");
        }
        else if (distAU >= 0.1)
        {
            _distanceText = (System.Math.Round(distAU, 1).ToString() + " au");
        }
        else if (distKM >= 10)
        {
            _distanceText = (System.Math.Round(distKM, 0).ToString() + " km");
        }
        else if (distM > 0)
        {
            _distanceText = (System.Math.Round(distM, 0).ToString() + " m");
        }
        else
        {
            _distanceText = "";
        }

        UpdateTextLabel();
    }

    public void UpdateTextLabel()
    {
        string setText = _nameText + " " + _distanceText;
        if (label)
        {
            label.SetText(setText);
        }
        else
        {
            Debug.Log("Could not set label to: " + setText);
        }
    }

    public void SetOffsetDistance(double dist)
    {
        offsetDistance = dist;
    }
}
