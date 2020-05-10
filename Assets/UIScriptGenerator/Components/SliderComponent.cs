using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    public class SliderComponent : UIComponent
    {
        public override EUIBindItemType GetBindType()
        {
            return EUIBindItemType.Slider;
        }

        public override string GetGetterStr()
        {
            string getterStr =
@"function {0}:GetSliderValue(slider)
    if slider ~= nil then
        return slider.value;
    end
end";
            return string.Format(getterStr, PrefabName);
        }

        public override string GetSetterStr()
        {
            string setterStr =
@"function {0}:SetSliderValue(slider, newValue)
    if slider ~= nil then
        slider.value = newValue;
    end
end";
            return string.Format(setterStr, PrefabName);
        }
    }
}