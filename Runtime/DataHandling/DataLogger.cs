using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace oculog
{
    public static class DataLogger
    {
        public static Action<DataEntry> OnWarningEmitted;
        public static Action<DataEntry> OnErrorEmitted;
        
        private static bool _initialized;

        private static Dictionary<string, DataContainer> _containers;
        
        public static void Init()
        {
            if (_initialized) return;
            _containers = new Dictionary<string, DataContainer>();

            _initialized = true;
        }

        public static void LogEntry(DataEntry entry)
        {
            if (_containers.ContainsKey(entry.id))
                _containers[entry.id].AddEntry(entry);
            else
            {
                var newContainer = new DataContainer(entry.id);
                newContainer.AddEntry(entry);
                _containers.Add(entry.id, newContainer);
            }
        }

        public static DataContainer GetContainer(string id)
        {
            if (_containers.ContainsKey(id))
                return _containers[id];
            
            //If the id does not exist we throw an error since this shouldn't happen
            throw new ArgumentException("DataLogger has no container with the given id");
        }

        public static List<DataEntry> GetEntriesForEntity(string id)
        {
            if (_containers.ContainsKey(id))
                return _containers[id].GetAllEntries();
            
            //If the id does not exist we throw an error since this shouldn't happen
            throw new ArgumentException("DataLogger has no container with the given id");
        }

        public static DataEntry GetEntryAtIndexForEntity(string id, int index)
        {
            if (_containers.ContainsKey(id))
                return _containers[id].GetEntry(index);
            
            //If the id does not exist we throw an error since this shouldn't happen
            throw new ArgumentException("DataLogger has no container with the given id");
        }
    }
}
