using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [UIComponentAttirbute("item")]
    public class CommonItemScrollViewComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.CommonItemScrollView;
        }
    }
}
