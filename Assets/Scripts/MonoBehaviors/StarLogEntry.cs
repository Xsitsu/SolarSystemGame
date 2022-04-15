using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarLogEntry : MonoBehaviour
{
    public GameObject IconLabel;
    public GameObject TextLabel;
    public GameObject Button;
    public Entity entity;
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
    public void Init()
    {
        Button.GetComponent<Button>().onClick.AddListener(OnClick);

        transform.GetComponent<ManualActivator>().ActivateEvent += OnClick;
    }
    public void OnClick()
    {
        WarpManager.Instance.InitializeWarp(PlayerManager.Instance.CharacterStructure, entity);
    }
}
