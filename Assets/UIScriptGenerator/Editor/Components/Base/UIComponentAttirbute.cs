using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class UIComponentAttirbute : Attribute
{
    public string UIName { get; private set; }

    public UIComponentAttirbute(string uiName)
    {
        UIName = uiName;
    }
}
