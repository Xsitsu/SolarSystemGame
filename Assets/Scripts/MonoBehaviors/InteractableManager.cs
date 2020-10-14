using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public GameObject interactablePrefab;
    public GameObject canvas;
    static private InteractableManager _instance;
	static public InteractableManager Instance { get { return _instance; } }
    void Awake()
	{
		_instance = this;
	}

    void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Register(GameObject gameObject)
    {
        GameObject intGameObj = Instantiate(interactablePrefab);
        Interactable interactable = intGameObj.GetComponent<Interactable>();

        interactable.adornee = gameObject;
        intGameObj.transform.SetParent(canvas.transform);
    }

    public void Unregister(GameObject gameObject)
    {
        foreach (Transform ch in canvas.transform)
        {
            GameObject child = ch.gameObject;
            if (child.GetComponent<Interactable>().adornee = gameObject)
            {
                Destroy(child);
            }
        }
    }

    public Interactable GetInteractable(GameObject gameObject)
    {
        foreach (Transform ch in canvas.transform)
        {
            Interactable interactable = ch.gameObject.GetComponent<Interactable>();
            if (interactable)
            {
                if (interactable.adornee == gameObject)
                {
                    return interactable;
                }
            }
            else
            {
                Debug.Log("Have child with no interactable!");
            }
        }
        return null;
    }

    public void SetVisible(bool visible)
    {
        canvas.SetActive(visible);
    }

    public bool GetVisible()
    {
        return canvas.activeSelf;
    }
}
