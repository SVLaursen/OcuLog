using System;
using UnityEngine;

namespace oculog
{
    [Serializable]
    public class DataEntry
    {
        public string id;
        public string value;
        public string formattedTimeStamp;
        
        public ELogLevel logLevel;

        private float _timeStamp;

        /// <summary>
        /// Generates a data entry with the given properties
        /// </summary>
        /// <param name="id">Used by the data container that the entry will be stored in</param>
        /// <param name="value">Description of entry</param>
        /// <param name="timeStamp">The time at which the entry was logged in application runtime</param>
        /// <param name="logLevel">Message level</param>
        public DataEntry(string id, string value, float timeStamp, ELogLevel logLevel)
        {
            this.id = id;
            this.value = value;
            this.logLevel = logLevel;
            _timeStamp = timeStamp;
            formattedTimeStamp = GetTimeStamp();
        }

        /// <summary>
        /// Generates a data entry with the given properties
        /// </summary>
        /// <param name="id">Used by the data container that the entry will be stored in</param>
        /// <param name="value">Description of entry</param>
        /// <param name="timeStamp">The time at which the entry was logged in application runtime</param>
        public DataEntry(string id, string value, float timeStamp)
        {
            this.id = id;
            this.value = value;
            _timeStamp = timeStamp;
            logLevel = ELogLevel.Default;
            formattedTimeStamp = GetTimeStamp();
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
            
            var timeSpan = TimeSpan.FromSeconds(_timeStamp);
            var hours = GetTimeStringValue(timeSpan.Hours);
            var minutes = GetTimeStringValue(timeSpan.Minutes);
            var seconds = GetTimeStringValue(timeSpan.Seconds);
            var milliseconds = GetTimeStringValue(timeSpan.Milliseconds);

            return $"T{hours}:{minutes}:{seconds}:{milliseconds}";
        }

        /// <summary>
        /// Gets the timestamp property saved in the data entry
        /// </summary>
        /// <returns>Timestamp in seconds</returns>
        public float GetTimestampInSeconds() => _timeStamp;
    }
}