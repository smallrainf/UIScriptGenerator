using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class RectTransformComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.RectTransform;
        }
    }
}