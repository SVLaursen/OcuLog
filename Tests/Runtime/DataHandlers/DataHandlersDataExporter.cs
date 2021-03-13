using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using oculog;

public class DataHandlersDataExporter
{
    [Test]
    public void ExportDataInCSVFormat()
    {
        var expectedFilePath = Application.persistentDataPath + @"\oculog\case_0\unit-test.csv";
        DataLogger.Init();
        
        var entry = new DataEntry("unit-test", "this is a test", 1000);
        DataLogger.LogEntry(entry);

        var container = DataLogger.GetContainer("unit-test");
        DataExporter.ExportData(EExportType.CSV, new List<DataContainer>{container});

        var fileCreated = File.Exists(expectedFilePath);
        Assert.AreEqual(true, fileCreated);

        if (fileCreated)
            File.Delete(expectedFilePath);
        
        if(Directory.Exists(Application.persistentDataPath + @"\oculog\case_0"))
            Directory.Delete(Application.persistentDataPath + @"\oculog\case_0");
    }

    [Test]
    public void ExportDataInCSVFormatWithFolderPath()
    {
        var expectedFilePath = Application.persistentDataPath + @"\custom\case_0\unit-test.csv";
        DataLogger.Init();
        
        var entry = new DataEntry("unit-test", "this is a test", 1000);
        DataLogger.LogEntry(entry);

        var container = DataLogger.GetContainer("unit-test");
        DataExporter.ExportData(EExportType.CSV, new List<DataContainer>{container}, "custom");

        var fileCreated = File.Exists(expectedFilePath);
        Assert.AreEqual(true, fileCreated);

        if (fileCreated)
            File.Delete(expectedFilePath);
        
        if(Directory.Exists(Application.persistentDataPath + @"\custom\case_0"))
            Directory.Delete(Application.persistentDataPath + @"\custom\case_0");
    }

    [Test]
    public void ExportLargeDataQuantityInCSVFormat()
    {
        var containers = new List<DataContainer>();
        DataLogger.Init();

        for (var i = 0; i < 100; i++)
        {
            var entryId = $"unit-test-{i}";
            for (var j = 0; j < 100; j++)
            {
                var entry = new DataEntry(entryId, "this is a test", j);
                DataLogger.LogEntry(entry);
            }

            var data = DataLogger.GetContainer(entryId);
            containers.Add(data);
        }
        
        DataExporter.ExportData(EExportType.CSV,containers);

        for (var i = 0; i < 100; i++)
        {
            var expectedFilePath = Application.persistentDataPath + @$"\oculog\case_0\unit-test-{i}.csv";
            var fileExists = File.Exists(expectedFilePath);

            Assert.AreEqual(true, fileExists);
            
            if(fileExists)
                File.Delete(expectedFilePath);
        }
        
        if(Directory.Exists(Application.persistentDataPath + @"\custom\case_0"))
            Directory.Delete(Application.persistentDataPath + @"\custom\case_0");
    }

    [Test]
    public void ExportDataInJSONFormat()
    {
        var expectedFilePath = Application.persistentDataPath + @"\oculog\oculog_entry_0.json";
        DataLogger.Init();
        
        var entry = new DataEntry("unit-test", "this is a test", 1000);
        DataLogger.LogEntry(entry);

        var container = DataLogger.GetContainer("unit-test");
        DataExporter.ExportData(EExportType.JSON, new List<DataContainer>{container});

        var fileCreated = File.Exists(expectedFilePath);
        Assert.AreEqual(true, fileCreated);

        if (fileCreated)
            File.Delete(expectedFilePath);
    }

    [Test]
    public void ExportDataInJSONFormatWithFolderPath()
    {
        var expectedFilePath = Application.persistentDataPath + @"\custom\oculog_entry_0.json";
        DataLogger.Init();
        
        var entry = new DataEntry("unit-test", "this is a test", 1000);
        DataLogger.LogEntry(entry);

        var container = DataLogger.GetContainer("unit-test");
        DataExporter.ExportData(EExportType.JSON, new List<DataContainer>{container}, "custom");

        var fileCreated = File.Exists(expectedFilePath);
        Assert.AreEqual(true, fileCreated);

        if (fileCreated)
            File.Delete(expectedFilePath);
    }

    [Test]
    public void ExportLargeDataQuantityInJSONFormat()
    {
        var containers = new List<DataContainer>();
        DataLogger.Init();

        for (var i = 0; i < 100; i++)
        {
            var entryId = $"unit-test-{i}";
            for (var j = 0; j < 100; j++)
            {
                var entry = new DataEntry(entryId, "this is a test", j);
                DataLogger.LogEntry(entry);
            }

            var data = DataLogger.GetContainer(entryId);
            containers.Add(data);
        }
        
        DataExporter.ExportData(EExportType.JSON,containers);
        
        var expectedFilePath = Application.persistentDataPath + @$"\oculog\oculog_entry_0.json";
        var fileExists = File.Exists(expectedFilePath);
        Assert.AreEqual(true, fileExists);
        
        if(fileExists)
            File.Delete(expectedFilePath);
    }
}