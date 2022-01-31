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
        CharacterStructure.position = new Vector3d(0, 0, -20000);
        CharacterStructure.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (CharacterStructure.parent == null)
        {
            StarSystemDisplay.Instance.SetStructureAnchor(CharacterStructure);
            StarSystemDisplay.Instance.GetStartingGrid().AddStructure(CharacterStructure);
        }
    }
}
