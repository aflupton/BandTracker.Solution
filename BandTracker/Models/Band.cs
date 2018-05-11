using System;
using System.Collections.Generic;
using BandTracker;
using MySql.Data.MySqClient;

namespace BandTracker.Models
{
  public class BandTracker
  {
    //private variables
    private int _bandId;
    private string _bandName;
    private string _bandGenre;
    private string _bandVenues;
    //band constructor
    public Band(string bandName, string bandGenre, string bandVenues)
    {
      _bandName = bandName;
      _bandGenre = bandGenre;
      _bandVenues = bandVenues;
      _bandId = bandId;
    }

    //get and set a band's name
    public string GetBandName()
    {
      return _bandName;
    }
    public void SetBandName(string newBandName)
    {
      _bandName = newBandName;
    }

    //get and set genres associated with a band
    public string GetBandGenre()
    {
      return _bandGenre;
    }
    public void SetBandGenre(string newBandGenre)
    {
      _bandGenre = newBandGenre;
    }

    //get and set venues that a band has played
    public string GetBandVenues()
    {
      return _bandVenues;
    }
    public void SetBandVenues(string newBandVenues)
    {
      _bandVenues = newBandVenues;
    }
    
    //get band id
    public void GetBandId()
    {
      return _bandId;
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
    public static Band Find(int id)
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
