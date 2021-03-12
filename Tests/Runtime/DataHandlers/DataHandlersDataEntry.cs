using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using oculog;
using UnityEngine;
using UnityEngine.TestTools;

public class DataHandlersDataEntry
{
    [Test]
    public void CreateADatAEntry()
    {
        var entry = new DataEntry("unit-test", "this is a test", 1000);
        var id = entry.id;
        var description = entry.description;
        var logLevel = entry.logLevel;
        
        Assert.IsNotNull(entry);
        Assert.AreEqual("unit-test", id);
        Assert.AreEqual("this is a test", description);
        Assert.AreEqual(ELogLevel.Default, logLevel);
    }

    [Test]
    public void CreateADataEntryWithLogLevel()
    {
        var entry = new DataEntry("unit-test", "this is a test", 1000, ELogLevel.Error);
        var id = entry.id;
        var description = entry.description;
        var logLevel = entry.logLevel;
        
        Assert.IsNotNull(entry);
        Assert.AreEqual("unit-test", id);
        Assert.AreEqual("this is a test", description);
        Assert.AreEqual(ELogLevel.Error, logLevel);
    }

    [Test]
    public void GetTimeStampOfDataEntry()
    {
        var entry = new DataEntry("unit-test", "this is a test", 1000.12f);
        var timeStamp = entry.GetTimeStamp();
        
        Assert.IsNotEmpty(timeStamp);
        Assert.AreEqual("T00:16:40:12", timeStamp);
    }

    [Test]
    public void GetLongSessionTimeStampOfDataEntry()
    {
        var entry = new DataEntry("unit-test", "this is a test", 100000.12f);
        var timeStamp = entry.GetTimeStamp();
        
        Assert.IsNotEmpty(timeStamp);
        Assert.AreEqual("T03:46:40:11", timeStamp);
    }
}
