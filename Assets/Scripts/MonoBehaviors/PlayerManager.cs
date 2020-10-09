using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject character;
    static private PlayerManager _instance;
	static public PlayerManager Instance { get { return _instance; } }
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
        SolarSystemDisplay display = SolarSystemDisplay.Instance;
        if (display && character)
        {
            SolarSystemDisplay.SolarSystemObject sso = display.GetStrongestGravity(character.transform.position);
            
            OrbitalBodyMono mono = sso.Object.GetComponent<OrbitalBodyMono>();
            character.transform.SetParent(mono.satellites.transform);
        }
    }
}
