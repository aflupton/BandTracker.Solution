using System;
using System.Collections.Generic;
using BandTracker;
using MySql.Data.MySqClient;

namespace BandTracker.Models
{
  public class Venue
  {
    //private variables
    private int _venueId;
    private string _venueName;
    private string _venueAddress;
    private string _venueBands;
    //constructor
    public Venue(string venueName, string venueAddress, string venueBands, int venueId = 0)
    {
      _venueName = venueName;
      _venueAddress = venueAddress;
      _venueBands = venueBands;
      _venueId = venueId;
    }

    //get and set venue name
    public string GetVenueName()
    {
      return _venueName;
    }
    public void SetVenueName(string newVenueName)
    {
      _venueName = newVenueName;
    }

    //get and set venue address
    public string GetVenueAddress()
    {
      return _venueAddress;
    }
    public void SetVenueAddress(string newVenueAddress)
    {
      _venueAddress = newVenueAddress;
    }

    //get and set bands that have played this venue
    public string GetVenueBands()
    {
      return _venueBands;
    }
    public void SetVenueBands(string newVenueBands)
    {
      _venueBands = newVenueBands;
    }

    //get venue id
    public void GetVenueId()
    {
      return _venueId;
    }
    
    //Create method for 'bands' table
    public void Save()
    {

    }
    //Create method for 'join' table
    public void AddVenuesToBand(Venue newVenue)
    {

    }
    //Read method for 'bands' table, singular
    public static List<Band> GetAll()
    {

    }
    //Read method for 'join' table
    public List<Venue> GetVenues()
    {

    }
    //Find method for 'bands' table
    public static Band Find(int venueId)
    {

    }
    //Update method for 'bands' table, singular
    public void UpdateBand(string bandName, string bandGenre, string bandVenues)
    {

    }
    //Delete method for 'bands' table, singular
    public void DeleteBand()
    {

    }
    //Delete method for 'bands' table, entire class
    public static void DeleteAll()
    {

    }
  }
}
