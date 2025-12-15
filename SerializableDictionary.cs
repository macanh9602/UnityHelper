using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTLTools
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // Save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // Load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new Exception($"Serialization Error: {nameof(keys)} and {nameof(values)} have different counts ({keys.Count} vs {values.Count}).");

            for (int i = 0; i < keys.Count; i++)
            {
                this[keys[i]] = values[i];
            }
        }
    }
}
