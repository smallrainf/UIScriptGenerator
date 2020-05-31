using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [UIComponentAttirbute("group")]
    public class ReusableLayoutGroupComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.ReusableLayoutGroup;
        }
    }
}