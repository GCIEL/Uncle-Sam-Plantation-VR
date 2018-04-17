// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;


namespace GCIEL.Toolkit
{

    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }

        // In this case, I intend to store Monobehaviour objects in the runtime set.
        // Since these objects only exist at runtime and do not persist across plays,
        // we need to clear the runtime set on startup to make sure that all the garbage 
        // objects previously stored in this asset are deleted
        protected void OnEnable()
        {
            Items = new List<T>();
        }

        private void OnDisable()
        {
            Items = null;
        }

    }
}