using System;
using UnityEngine;

namespace oculog
{
    public struct DataEntry
    {
        public string id;
        public string description;
        
        public ELogLevel logLevel;

        private float timeStamp;

        /// <summary>
        /// Generates a data entry with the given properties
        /// </summary>
        /// <param name="id">Used by the data container that the entry will be stored in</param>
        /// <param name="description">Description of entry</param>
        /// <param name="timeStamp">The time at which the entry was logged in application runtime</param>
        /// <param name="logLevel">Message level</param>
        public DataEntry(string id, string description, float timeStamp, ELogLevel logLevel)
        {
            this.id = id;
            this.description = description;
            this.logLevel = logLevel;
            this.timeStamp = timeStamp;
        }

        /// <summary>
        /// Generates a data entry with the given properties
        /// </summary>
        /// <param name="id">Used by the data container that the entry will be stored in</param>
        /// <param name="description">Description of entry</param>
        /// <param name="timeStamp">The time at which the entry was logged in application runtime</param>
        public DataEntry(string id, string description, float timeStamp)
        {
            this.id = id;
            this.description = description;
            this.timeStamp = timeStamp;
            logLevel = ELogLevel.Default;
        }

        /// <summary>
        /// Gets the timestamp of the entry
        /// </summary>
        /// <returns>Timestamp in THH:MM:SS:MS format</returns>
        public string GetTimeStamp()
        {
            string GetTimeStringValue(int value)
            {
                var result = value < 10 ? "0" + value : value.ToString();
                result = result.Length > 2 ? result.Substring(0, 2) : result;
                return result;
            }
            
            var timeSpan = TimeSpan.FromSeconds(timeStamp);
            var hours = GetTimeStringValue(timeSpan.Hours);
            var minutes = GetTimeStringValue(timeSpan.Minutes);
            var seconds = GetTimeStringValue(timeSpan.Seconds);
            var milliseconds = GetTimeStringValue(timeSpan.Milliseconds);

            Debug.Log($"T{hours}:{minutes}:{seconds}:{milliseconds}");
            return $"T{hours}:{minutes}:{seconds}:{milliseconds}";
        }

        /// <summary>
        /// Gets the timestamp property saved in the data entry
        /// </summary>
        /// <returns>Timestamp in seconds</returns>
        public float GetTimestampInSeconds() => timeStamp;
    }
}