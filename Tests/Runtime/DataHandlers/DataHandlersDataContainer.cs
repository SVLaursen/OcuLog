using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using oculog;

public class DataHandlersDataContainer
{
    private DataContainer _singleContainer;
    private List<DataContainer> _containers = new List<DataContainer>();
    
    [Test]
    public void CreateDataContainerAndGetPublicProperties()
    {
        _singleContainer = new DataContainer("singleContainer");
        var containerId = _singleContainer.Id;
        var containerData = _singleContainer.GetAllEntries();

        Assert.IsEmpty(containerData);
        Assert.IsNotEmpty(containerId);
        Assert.AreEqual("singleContainer", containerId);
    }

    [Test]
    public void CreateTwoHundredDataContainersAndFindSpecific()
    {
        for (var i = 0; i < 200; i++)
        {
            var newContainer = new DataContainer($"container-{i}");
            _containers.Add(newContainer);
        }
        
        Assert.AreEqual(200, _containers.Count);

        foreach (var container in _containers)
        {
            Assert.IsEmpty(container.GetAllEntries());
            Assert.IsNotEmpty(container.Id);
        }
    }

    [Test]
    public void FillDataContainerWithTenEntries()
    {
        var testContainer = new DataContainer("test-container");
        
        for (var i = 0; i < 10; i++)
        {
            testContainer.AddEntry(new DataEntry($"id-{i}", "unit test", i));
        }
        
        Assert.AreEqual(10, testContainer.GetAllEntries().Count);
    }

    [Test]
    public void FillDataContainerWithHundredEntries()
    {
        for (var i = 0; i < 100; i++)
        {
            _singleContainer.AddEntry(new DataEntry($"id-{i}", "unit test", i));
        }
        
        Assert.AreEqual(100, _singleContainer.GetAllEntries().Count);
    }
    
    [Test]
    public void GetEntryAtIndexTest()
    {
        DataEntry entryOne = _singleContainer.GetEntry(2);
        DataEntry entryTwo = _singleContainer.GetEntry(10);
        DataEntry entryThree = _singleContainer.GetEntry(33);

        Assert.IsNotNull(entryOne);
        Assert.IsNotNull(entryTwo);
        Assert.IsNotNull(entryThree);
        
        Assert.AreEqual(typeof(DataEntry), entryOne.GetType());
        Assert.AreEqual(typeof(DataEntry), entryTwo.GetType());
        Assert.AreEqual(typeof(DataEntry), entryThree.GetType());
    }
}