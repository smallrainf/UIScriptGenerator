using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class ImageComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Image;
        }

        public override string GetGetterStr()
        {
            string getterStr =
@"function {0}:GetSprite(image)
    if image ~= nil then
        return image.sprite;
    end
end";
            return string.Format(getterStr, PrefabName);
        }

        public override string GetSetterStr()
        {
            string setterStr =
@"function {0}:SetSprite(image, sprite)
    if image ~= nil then
        image.sprite = sprite;
    end
end";
            return string.Format(setterStr, PrefabName);
        }
    }
}
