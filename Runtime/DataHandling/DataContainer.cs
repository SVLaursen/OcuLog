using System.Collections.Generic;
using UnityEngine;

namespace oculog
{
    [System.Serializable]
    public class DataContainer
    {
        /// <summary>
        /// Unique ID of the data container
        /// </summary>
        public string Id => id;

        [SerializeField] private string id;

        [SerializeField] private List<DataEntry> _data;
        
        public DataContainer(string id)
        {
            this.id = id;
            _data = new List<DataEntry>();
        }

        /// <summary>
        /// Adds a DataEntry to the container
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(DataEntry entry)
        {
            _data.Add(entry);
        }

        /// <summary>
        /// Gets the given entry at the index specified
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataEntry GetEntry(int index)
        {
            return _data[index];
        }

        /// <summary>
        /// Gets all the entries of data in this container
        /// </summary>
        /// <returns></returns>
        public List<DataEntry> GetAllEntries()
        {
            return _data;
        }
    }
}