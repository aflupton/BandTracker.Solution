using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Tests
{
  [TestClass]
  public class VenueTests : IDisposable
  {
    public VenueTests()
    {
      DBConfiguration.ConnectionString = "server=localhost; user id=root;password=root;port=8889;database=band_tracker_test;";
    }
    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }
    [TestMethod]
    public void Save_AddsVenueToDatabase_VenuesTable()
    {
      //Arrange
      Venue newVenue = new Venue("The Comet", "Pike Street, Seattle", "2pm-2am");
      newVenue.Save();
      //Act
      List<Venue> testResult = Venue.GetAllVenues();
      //Assert
      // Console.WriteLine("Venue count: " + testResult.Count);
      Assert.AreEqual(1, testResult.Count);
    }
    [TestMethod]
    public void FindAll_VenuesFromDatabase_VenuesTable()
    {
      //Arrange
      Venue newVenue = new Venue("The Redwood", "East Olive Way", "2pm-2am");
      newVenue.Save();
      //Act
      Venue testResult = Venue.FindVenues(newVenue.GetVenueId());
      //Assert
      // Console.WriteLine("Venue name: " + testResult.GetVenueName());
      Assert.AreEqual("The Redwood", testResult.GetVenueName());
    }
    [TestMethod]
    public void Add_BandsToVenue_JoinTable()
    {

    }
    [TestMethod]
    public void Update_SingleVenue_VenuesTable()
    {
      //Arrange
      Venue newVenue = new Venue("Lo-Fi", "Eastlake", "7pm-2am");
      newVenue.Save();
      //Act
      newVenue.UpdateVenue("Lo-Fi", "Eastlake Ave East", "5pm-2am");
      Venue updatedVenue = Venue.FindVenues(newVenue.GetVenueId());
      //Assert
      // Console.WriteLine("Updated venue address: " + updatedVenue.GetVenueAddress());
      Assert.AreEqual("Eastlake Ave East", updatedVenue.GetVenueAddress());
    }
    [TestMethod]
    public void Delete_SingleVenue_VenuesTable()
    {
      //Arrange
      Venue newVenue = new Venue("The Paramount", "911 Pine Street", "Evenings");
      newVenue.Save();
      //Act
      newVenue.DeleteVenue();
      Venue deletedVenue = Venue.FindVenues(newVenue.GetVenueId());
      //Assert
      Console.WriteLine("Name: " + deletedVenue.GetVenueName());
      Assert.AreEqual("", deletedVenue.GetVenueName());
    }
  }
}
