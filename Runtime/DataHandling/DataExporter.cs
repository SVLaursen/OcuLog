using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using oculog.Core;
using UnityEngine;

namespace oculog
{
    public static class DataExporter
    {
        private const string CSV_SUFFIX = ".csv";
        private const string JSON_SUFFIX = ".json";
        
        private static string _dataPath = Application.persistentDataPath;
        
        /// <summary>
        /// Exports the given data to the default persistent data path of the application in the format given.
        /// </summary>
        /// <param name="exportType">Based one the <typeparamref name="EExportType"/></param>
        /// <param name="data">List of data containers</param>
        public static void ExportData(EExportType exportType, List<DataContainer> data)
        {
            ExportData(exportType, data, "oculog");
        }
        
        /// <summary>
        /// Exports the given data to a custom folder at the persistent data path of the application in the format given.
        /// </summary>
        /// <param name="exportType">Based one the <typeparamref name="EExportType"/> enum</param>
        /// <param name="data">List of data containers</param>
        /// <param name="folderName">Custom folder path</param>
        public static void ExportData(EExportType exportType, List<DataContainer> data, string folderName)
        {
            var filePath = _dataPath + $"/{folderName}";
            switch (exportType)
            {
                case EExportType.CSV:
                    ExportCSV(filePath, data);
                    break;
                case EExportType.JSON:
                    ExportJSON(filePath, data);
                    break;
                case EExportType.None:
                    Debug.LogWarning("OCULOG: Export type is none and the data will therefore not be exported");
                    break;
                default:
                    throw new System.ArgumentException("Invalid Export Type");
            }
        }

        [System.Serializable]
        private struct JsonDataObject
        {
            public List<DataContainer> data;

            public JsonDataObject(List<DataContainer> data)
            {
                this.data = data;
            }
        }

        private static void ExportJSON(string filePath, List<DataContainer> data)
        {
            var dataObject = new JsonDataObject(data);
            var jsonData = JsonUtility.ToJson(dataObject, true);
            var entryIndex = 0;

            Directory.CreateDirectory(filePath);

            for (var i = 0; i < 100; i++)
            {
                if (!File.Exists(filePath + $"/oculog_entry_{i}{JSON_SUFFIX}"))
                {
                    entryIndex = i;
                    break;
                }
            }
            
            var fileName = $"/oculog_entry_{entryIndex}{JSON_SUFFIX}";
            File.WriteAllText(filePath + fileName, jsonData);
            
#if UNITY_EDITOR
            PlayStateNotifier.ShouldNotExit = false;
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private static void ExportCSV(string filePath, List<DataContainer> data)
        {
            //Create the custom or oculog folder
            Directory.CreateDirectory(filePath);
            
            //Create the test participant folder
            var caseNumber = 0;
            
            for (var i = 0; i < 100; i++)
            {
                if (Directory.Exists($"{filePath}/case_{i}")) continue;
                Directory.CreateDirectory($"{filePath}/case_{i}");
                caseNumber = i;
                break;
            }
            
            //Prepare and export the data to the path
            foreach (var container in data)
            {
                var entries = container.GetAllEntries();
                var formattedData = new List<string>();
                
                //Create header for the file
                formattedData.Add(WriteHeaderForCSVFile(entries[0]));
                
                //Format data
                foreach (var entry in entries)
                {
                    var formattedEntry = WriteEntryToCSVFormat(entry);
                    formattedData.Add(formattedEntry);
                }

                //Save File
                var savePath = filePath + $"/case_{caseNumber}/{container.Id}{CSV_SUFFIX}";

                /*using (FileStream fs = File.Create(savePath))
                {
                    foreach(var line in formattedData)
                    {
                        var writeBytes = Encoding.UTF8.GetBytes(line);
                        fs.Write(writeBytes, 0, line.Length);
                    }
                    fs.Close();
                }*/
                
                File.WriteAllLines(savePath, formattedData);
            }
            
#if UNITY_EDITOR
            PlayStateNotifier.ShouldNotExit = false;
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private static string WriteHeaderForCSVFile(DataEntry entry)
        {
            return "id;value;timestamp;loglevel";
        }

        private static string WriteEntryToCSVFormat(DataEntry entry)
        {
            return $"{entry.id};{entry.value};{entry.formattedTimeStamp};{entry.logLevel.ToString()}";
        }
    }
}