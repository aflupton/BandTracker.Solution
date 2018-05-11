using System;
using System.Collections.Generic;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Models
{
  public class Venue
  {
    //private variables
    private int _id;
    private string _venueName;
    private string _venueAddress;
    private string _venueHours;
    //constructor
    public Venue(string venueName, string venueAddress, string venueHours, int id = 0)
    {
      _venueName = venueName;
      _venueAddress = venueAddress;
      _venueHours = venueHours;
      _id = id;
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
    public string GetVenueHours()
    {
      return _venueHours;
    }
    public void SetVenueHours(string newVenueHours)
    {
      _venueHours = newVenueHours;
    }

    //get venue id
    public int GetVenueId()
    {
      return _id;
    }
    //handle assertion equation
    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        return this.GetVenueName().Equals(newVenue.GetVenueName());
      }
    }

    public override int GetHashCode()
    {
         return this.GetVenueName().GetHashCode();
    }

    //Create method for 'venues' table
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues (venue_name, venue_address, venue_hours) VALUES (@VenueName, @VenueAddress, @VenueHours);";

      MySqlParameter venue_name = new MySqlParameter();
      venue_name.ParameterName = "@VenueName";
      venue_name.Value = this._venueName;
      cmd.Parameters.Add(venue_name);

      MySqlParameter venue_address = new MySqlParameter();
      venue_address.ParameterName = "@VenueAddress";
      venue_address.Value = this._venueAddress;
      cmd.Parameters.Add(venue_address);

      MySqlParameter venue_hours = new MySqlParameter();
      venue_hours.ParameterName = "@VenueHours";
      venue_hours.Value = this._venueHours;
      cmd.Parameters.Add(venue_hours);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    //Create method for 'join' table
    public void AddBandsToVenues(Band newBand)
    {

    }
    //Read method for 'venues' table, singular
    public static List<Venue> GetAllVenues()
    {
      List<Venue> allVenues = new List<Venue>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from venues;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string venue_name = rdr.GetString(1);
        string venue_address = rdr.GetString(2);
        string venue_hours = rdr.GetString(3);
        Venue newVenue = new Venue(venue_name, venue_address, venue_hours, id);
        allVenues.Add(newVenue);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allVenues;
    }
    //Read method for 'join' table
    // public List<Band> GetBandsFromJoin()
    // {
    //
    // }
    // Find method for 'venues' table
    public static Venue FindVenues(int venue_id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from venues WHERE venue_id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = venue_id;
      cmd.Parameters.Add(searchId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      int venueId = 0;
      string venueName = "";
      string venueAddress = "";
      string venueHours = "";

      while(rdr.Read())
      {
        venueId = rdr.GetInt32(0);
        venueName = rdr.GetString(1);
        venueAddress = rdr.GetString(2);
        venueHours = rdr.GetString(3);
      }
      Venue newVenue = new Venue (venueName, venueAddress, venueHours, venueId);

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return newVenue;
    }
    //Update method for 'venues' table, singular
    public void UpdateVenue(string venueName, string venueAddress, string venueHours)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET venue_name=@VenueName, venue_address=@VenueAddress, venue_hours=@VenueHours WHERE venue_id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter venue_name = new MySqlParameter();
      venue_name.ParameterName = "@VenueName";
      venue_name.Value = venueName;
      cmd.Parameters.Add(venue_name);

      MySqlParameter venue_address = new MySqlParameter();
      venue_address.ParameterName = "@VenueAddress";
      venue_address.Value = venueAddress;
      cmd.Parameters.Add(venue_address);

      MySqlParameter venue_hours = new MySqlParameter();
      venue_hours.ParameterName = "@VenueHours";
      venue_hours.Value = venueHours;
      cmd.Parameters.Add(venue_hours);

      cmd.ExecuteNonQuery();
      _venueName = venueName;
      _venueAddress = venueAddress;
      _venueHours = venueHours;

      if(conn != null)
      {
        conn.Dispose();
      }
    }
    //Delete method for 'venues' table, singular
    public void DeleteVenue()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE venue_id = @VenueId;
      DELETE FROM venues_bands
      WHERE venue_id = @VenueId;";

      MySqlParameter venueId = new MySqlParameter();
      venueId.ParameterName = "@VenueId";
      venueId.Value = this.GetVenueId();
      cmd.Parameters.Add(venueId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    //Delete method for 'venues' table, entire class
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE from venues;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
