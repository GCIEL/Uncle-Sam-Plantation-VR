// ----------------------------------------------------------------------------
// GCIEL - Scriptable Object Variables
// 
// Author: Zachary Segall
// Date:   03/06/2018
//
// Modeled after Game Architecture with Scriptable Objects 
// Author: Ryan Hipple
// ----------------------------------------------------------------------------

using UnityEngine;
namespace GCIEL.Toolkit
{
    [CreateAssetMenu]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }
    }
}
