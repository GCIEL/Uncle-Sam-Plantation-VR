// ----------------------------------------------------------------------------
// GCIEL - Scriptable Object Variables
// 
// Author: Zachary Segall
// Date:   02/13/2018
//
// Modeled after Game Architecture with Scriptable Objects 
// Author: Ryan Hipple
// ----------------------------------------------------------------------------

using System;

namespace GCIEL.Toolkit
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value;  }
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}