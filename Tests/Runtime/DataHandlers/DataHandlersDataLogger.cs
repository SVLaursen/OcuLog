using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using oculog;

public class DataHandlersDataLogger
{
    [Test]
    public void LogASingleEntryAndGetContainer()
    {
        var entry = new DataEntry("unit-test-1", "good test", 10);
        
        DataLogger.Init();
        DataLogger.LogEntry(entry);

        var container = DataLogger.GetContainer("unit-test-1");
        Assert.AreEqual(typeof(DataContainer), container.GetType());

        var containerEntry = container.GetEntry(0);
        Assert.NotNull(containerEntry);
        Assert.AreEqual("unit-test-1", containerEntry.id);
    }

    [Test]
    public void LogSingleEntryAndCallGetAllEntries()
    {
        var entry = new DataEntry("unit-test-1", "good test", 12);
        
        DataLogger.Init();
        DataLogger.LogEntry(entry);

        var entries = DataLogger.GetEntriesForEntity("unit-test-1");
        Assert.IsNotEmpty(entries);
    }

    [Test]
    public void LogTwoEntriesWithDifferentIDs()
    {
        var entryOne = new DataEntry("unit-test-1", "good test", 12);
        var entryTwo = new DataEntry("unit-test-2", "good test 2", 12);

        DataLogger.Init();
        DataLogger.LogEntry(entryOne);
        DataLogger.LogEntry(entryTwo);

        var containerOne = DataLogger.GetContainer("unit-test-1");
        var containerTwo = DataLogger.GetContainer("unit-test-2");

        Assert.AreEqual(typeof(DataContainer), containerOne.GetType());
        Assert.AreEqual(typeof(DataContainer), containerTwo.GetType());

        var containerOneEntry = containerOne.GetEntry(0);
        var containerTwoEntry = containerTwo.GetEntry(0);
        
        Assert.NotNull(containerOneEntry);
        Assert.AreEqual("unit-test-1", containerOneEntry.id);
        
        Assert.NotNull(containerTwoEntry);
        Assert.AreEqual("unit-test-2", containerTwoEntry.id);
    }

    [Test]
    public void LogFiftyDifferentContainersWithHundredEach()
    {
        DataLogger.Init();

        for (var i = 0; i < 50; i++)
        {
            for (var j = 0; j < 100; j++)
            {
                var entry = new DataEntry($"unit-test-{i}", "bababoie", j);
                DataLogger.LogEntry(entry);
            }
        }

        for (var i = 0; i < 50; i++)
        {
            var container = DataLogger.GetContainer($"unit-test-{i}");
            Assert.NotNull(container);

            var entries = container.GetAllEntries();
            Assert.GreaterOrEqual(entries.Count, 100);
        }
    }

    [Test]
    public void LogWarningLevelEntryAndTriggerAction()
    {
        var entry = new DataEntry("unit-test-1", "warning test", 120, ELogLevel.Warning);

        DataLogger.Init();
        DataLogger.OnWarningEmitted += dataEntry =>
        {
            Assert.AreEqual(dataEntry.logLevel, ELogLevel.Warning);
        };
        DataLogger.LogEntry(entry);

        var defaultEntry = new DataEntry("unit-test-1", "default test", 121, ELogLevel.Default);
        DataLogger.LogEntry(defaultEntry);
    }

    [Test]
    public void LogErrorLevelEntryAndTriggerAction()
    {
        var entry = new DataEntry("unit-test-1", "warning test", 120, ELogLevel.Warning);

        DataLogger.Init();
        DataLogger.OnErrorEmitted += dataEntry =>
        {
            Assert.AreEqual(dataEntry.logLevel, ELogLevel.Error);
        };
        DataLogger.LogEntry(entry);

        var defaultEntry = new DataEntry("unit-test-1", "default test", 121, ELogLevel.Default);
        DataLogger.LogEntry(defaultEntry);
    }
}
