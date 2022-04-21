using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiagnosticsUI : MonoBehaviour
{
    string baseText = "Speed: {0}\nMoveDir: {1}\nRotateDir: {2}\nRotateQuat: {3}\nIsDown: {4}\nW.Speed: {5}\nW.MoveDir: {6}";

    public SublightEngineMono sublightEngine;
    public WarpEngineMono warpEngine;
    public TextMeshProUGUI textLabel;
    void Start()
    {
        
    }
    void Update()
    {
        string text = string.Format(baseText, sublightEngine.SpeedOut, sublightEngine.moveDirection,
        sublightEngine.rotateDirection, sublightEngine.rotateQuaternion,
        sublightEngine.useRotateQuaternion, warpEngine.speedCOut, warpEngine.moveDirection);
        textLabel.SetText(text);
    }
}
