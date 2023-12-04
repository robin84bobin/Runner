using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum ParamName
{
    NOPE,
    HEALTH,
    SPEED,
    ARMOR_EQUIP,
    SPEED_EQUIP
}

[Serializable]
public class Parameter
{
    [FormerlySerializedAs("name")] public ParamName Name;

    public float minValue = 0f;
    public float maxValue = 100f;
    public float defaultValue = 100f;

    public UnityEvent onMaxValue;
    public UnityEvent onMinValue;
    public SkillChangeEvent onSkillChange;

    [NonSerialized]
    public float value;

    public Parameter (ParamName name, float value, float maxValue = float.MaxValue, float minValue = float.MinValue)
    {
        Name = name;
        this.maxValue = maxValue;
        this.minValue = minValue;
        defaultValue = value;
        InitValues();
    }

    public void InitValues()
    {
        if (maxValue < minValue)
        {
            Debug.LogWarning(string.Format("Skill:{0}  max value:{1} lower than min value {2}",
                Name.ToString(), maxValue.ToString(), minValue.ToString()));
            maxValue = minValue;
        }
        if (defaultValue > maxValue) {
            Debug.LogWarning(string.Format("Skill:{0}  default value:{1} higher than max value {2}",
                Name.ToString(), defaultValue.ToString(), maxValue.ToString()));
            defaultValue = Mathf.Clamp(defaultValue, minValue, maxValue);
        }
        if (defaultValue < minValue) {
            Debug.LogWarning(string.Format("Skill:{0}  default value:{1} lower than min value {2}",
                Name.ToString(), defaultValue.ToString(), minValue.ToString()));
            defaultValue = Mathf.Clamp(defaultValue, minValue, maxValue);
        }
        
       value = defaultValue;
    }

    /// <summary>
    /// Change skill value and returns remainder
    /// </summary>
    public float ChangeValue(float amount)
    {
        return SetValue(value + amount);
    }

    /// <summary>
    /// Set skill value and returns remainder
    /// </summary>
    public float SetValue(float newValue)
    {
        float remainder = 0;
        float oldValue = value;

        if (newValue > maxValue)
        {
            value = maxValue;
            onMaxValue.Invoke();
            onSkillChange.Invoke(oldValue, value);
            remainder = newValue - maxValue;
            return remainder;
        }
        else
        if (newValue <= minValue)
        {
            value = minValue;
            onMinValue.Invoke();
            onSkillChange.Invoke(oldValue, value);
            remainder = newValue - minValue; 
            return remainder;
        }

        value = newValue;
        onSkillChange.Invoke(oldValue, value);

        return remainder;
    }

    public void Release()
    {
        onSkillChange.RemoveAllListeners();
        onMinValue.RemoveAllListeners();
        onMaxValue.RemoveAllListeners();
    }
}

[Serializable]
public class SkillChangeEvent : UnityEvent<float,float> { };
