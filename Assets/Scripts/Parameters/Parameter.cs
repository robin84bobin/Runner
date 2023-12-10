using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Parameters
{
    public enum ParamName
    {
        NOPE,
        HEIGHT,
        SPEED,
    }

    public class Parameter
    {
        public ParamName Name { get; private set; }

        public float Value { get; private set; }
        public float MinValue { get; private set; } = 0f;
        public float MaxValue { get; private set; } = 100f;
        public float DefaultValue { get; private set; } = 100f;

        public event Action onMaxValue;
        public event Action onMinValue;
        public ValueChangeEvent OnValueChange;


        public Parameter (ParamName name, float value, float maxValue = float.MaxValue, float minValue = float.MinValue)
        {
            Name = name;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
            DefaultValue = value;
            InitValues();
        }

        public void InitValues()
        {
            if (MaxValue < MinValue)
            {
                Debug.LogWarning(string.Format("Skill:{0}  max value:{1} lower than min value {2}",
                    Name.ToString(), MaxValue.ToString(), MinValue.ToString()));
                MaxValue = MinValue;
            }
            if (DefaultValue > MaxValue) {
                Debug.LogWarning(string.Format("Skill:{0}  default value:{1} higher than max value {2}",
                    Name.ToString(), DefaultValue.ToString(), MaxValue.ToString()));
                DefaultValue = Mathf.Clamp(DefaultValue, MinValue, MaxValue);
            }
            if (DefaultValue < MinValue) {
                Debug.LogWarning(string.Format("Skill:{0}  default value:{1} lower than min value {2}",
                    Name.ToString(), DefaultValue.ToString(), MinValue.ToString()));
                DefaultValue = Mathf.Clamp(DefaultValue, MinValue, MaxValue);
            }
        
            Value = DefaultValue;
        }

        public void ResetToDefaultValue()
        {
            ChangeValue(DefaultValue);
        }
        
        /// <summary>
        /// Change param value and returns remainder
        /// </summary>
        public float ChangeValue(float amount)
        {
            return SetValue(Value + amount);
        }

        /// <summary>
        /// Set param and returns remainder
        /// </summary>
        public float SetValue(float newValue)
        {
            float remainder = 0;
            float oldValue = Value;

            if (newValue > MaxValue)
            {
                Value = MaxValue;
                onMaxValue?.Invoke();
                OnValueChange?.Invoke(oldValue, Value);
                remainder = newValue - MaxValue;
                return remainder;
            }
            else
            if (newValue <= MinValue)
            {
                Value = MinValue;
                onMinValue?.Invoke();
                OnValueChange?.Invoke(oldValue, Value);
                remainder = newValue - MinValue; 
                return remainder;
            }

            Value = newValue;
            OnValueChange?.Invoke(oldValue, Value);

            return remainder;
        }

        public void Release()
        {
            OnValueChange = null;
            onMinValue = null;
            onMaxValue = null;
        }
    }

    public delegate Action<float,float> ValueChangeEvent(float oldValue, float newValue);
}