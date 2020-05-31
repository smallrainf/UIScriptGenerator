using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [UIComponentAttirbute("img")]
    public class ImageComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Image;
        }

        public override string GetGetterStr()
        {
            string getterStr =
@"function {0}:get_{1}_sprite()
    if self.{1} ~= nil then
        return self.{1}.sprite;
    end
end";
            return string.Format(getterStr, PrefabName, UIName);
        }

        public override string GetSetterStr()
        {
            string setterStr =
@"function {0}:set_{1}_sprite(sprite)
    if self.{1} ~= nil then
        self.{1}.sprite = sprite;
    end
end";
            return string.Format(setterStr, PrefabName, UIName);
        }
    }
}
