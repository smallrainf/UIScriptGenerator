using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [UIComponentAttirbute("slider")]
    public class SliderComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Slider;
        }

        public override string GetGetterStr()
        {
            string getterStr =
@"function {0}:get_{1}_value()
    if self.{1} ~= nil then
        return self.{1}.value;
    end
end";
            return string.Format(getterStr, PrefabName, UIName);
        }

        public override string GetSetterStr()
        {
            string setterStr =
@"function {0}:set_{1}_value(newValue)
    if self.{1} ~= nil then
        self.{1}.value = newValue;
    end
end";
            return string.Format(setterStr, PrefabName, UIName);
        }
    }
}