// ----------------------------------------------------------------------------
// GCIEL - Scriptable Object Variables
// 
// Author: Zachary Segall
// Date:   02/13/2018
//
// Modeled after Game Architecture with Scriptable Objects 
// Author: Ryan Hipple
// ----------------------------------------------------------------------------

using UnityEngine;
namespace GCIEL.Toolkit
{
    [CreateAssetMenu]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public float Value;
        
        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}
