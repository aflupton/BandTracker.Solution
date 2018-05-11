using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Tests
{
  [TestClass]
  public class BandTests : IDisposable
  {
    public BandTests()
    {
      DBConfiguration.ConnectionString = "server=localhost; user id=root; password=root; port=8889; database=band_tracker_test;";
    }
    public void Dispose()
    {
      Band.DeleteAll();
    }
    [TestMethod]
    public void Save_AddsBandToDatabase_BandsTable()
    {
      //Arrange
      Band testBand = new Band("Black Marble", "Post-punk", "New York");
      testBand.Save();
      //Act
      List<Band> testResult = Band.GetAllBands();
      //Assert
      // Console.WriteLine("Bands count: " + testResult.Count);
      Assert.AreEqual(1, testResult.Count);
    }
    [TestMethod]
    public void FindAll_BandsFromDatabase_BandsTable()
    {
      //Arrange
      Band newBand = new Band("Black Marble", "Post-punk", "New York");
      newBand.Save();
      //Act
      Band testResult = Band.FindBands(newBand.GetBandId());
      //Assert
      // Console.WriteLine("Band name: " + testResult.GetBandName());
      Assert.AreEqual("Black Marble", testResult.GetBandName());
    }
    [TestMethod]
    public void Add_VenuesToBand_JoinTable()
    {

    }
    [TestMethod]
    public void Update_SingleBand_VenuesTable()
    {
      //Arrange
      Band newBand = new Band("The Spits", "Punk", "Seattle");
      newBand.Save();
      //Act
      newBand.UpdateBand("Emeralds", "Drone", "Cleveland");
      Band updatedBand = Band.FindBands(newBand.GetBandId());
      //Assert
      Console.WriteLine("Updated band name: " + updatedBand.GetBandName());
      Assert.AreEqual("Emeralds", updatedBand.GetBandName());
    }
    [TestMethod]
    public void Delete_SingleBand_VenuesTable()
    {
      //Arrange
      Band newBand = new Band("Joanna Brouk", "New Age", "Oakland");
      newBand.Save();
      //Act
      newBand.DeleteBand();
      Band deletedBand = Band.FindBands(newBand.GetBandId());
      //Assert
      Console.WriteLine("Name: " + deletedBand.GetBandName());
      Assert.AreEqual("", deletedBand.GetBandName());
    }
  }
}
