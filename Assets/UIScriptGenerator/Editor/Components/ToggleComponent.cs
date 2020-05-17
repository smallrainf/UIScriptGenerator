using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class ToggleComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Toggle;
        }
    }
}
