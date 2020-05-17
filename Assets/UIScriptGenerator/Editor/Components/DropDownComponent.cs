using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class DropDownComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.DropDown;
        }
    }
}