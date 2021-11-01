using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject character;
    public GameObject setParent;

    public Structure CharacterStructure;

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
        if (setParent != null)
        {
            character.transform.SetParent(setParent.transform);
        }

        CharacterStructure = new Structure();
        CharacterStructure.name = "Spaceship";

        ModuleCargo cargoBay = new ModuleCargo();
        cargoBay.Name = "Cargo Bay";
        cargoBay.MaxVolume = 10000;
        CharacterStructure.AddModule(cargoBay);
    }

    void Update()
    {
        if (CharacterStructure.parent == null)
        {
            StarSystemDisplay.Instance.SetOrbitalAnchor(CharacterStructure);
            Star star = StarSystemDisplay.Instance.GetStar();
            CharacterStructure.orbitRadius = star.radius * 1.6;
            star.AddSatellite(CharacterStructure);
        }
    }
}
