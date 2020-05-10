using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class TextComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Text;
        }

        public override string GetGetterStr()
        {
            string getterStr =
@"function {0}:GetText(text)
    if text ~= nil then
        return text.text;
    end
end";
            return string.Format(getterStr, PrefabName);
        }

        public override string GetSetterStr()
        {
            string setterStr =
@"function {0}:SetText(text, newText)
    if text ~= nil then
        text.text = newText;
    end
end";
            return string.Format(setterStr, PrefabName);
        }
    }
}
