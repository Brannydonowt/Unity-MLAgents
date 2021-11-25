using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AcademyUI : MonoBehaviour
{
    public TextMeshProUGUI classType, className, camName;

    public void SetClassType(string s)
    {
        classType.text = s;
    }

    public void SetClassName(string s)
    {
        className.text = s;
    }

    public void SetCamName(string s)
    {
        camName.text = s;
    }
}
