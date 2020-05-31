using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [UIComponentAttirbute("btn")]
    public class ButtonComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Button;
        }

        public string GetRegisterClickStr()
        {
            return string.Format("self.csharp:AddSelfClick(self.{0}.gameObject, self.{1}, self);", UIName, "On" + UIName);
        }

        public string GetClickStr()
        {
            string clickStr =
@"function {0}:{1}(go)

end";
            return string.Format(clickStr, PrefabName, "On" + UIName);
        }
    }
}
