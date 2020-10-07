using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public GameObject adornee;
    public GameObject canvas;
    public GameObject button;
    void Start()
    {
        
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
    }
}
