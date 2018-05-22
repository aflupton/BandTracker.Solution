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

    // Create test override methods
    public override int GetHashCode()
    {
      return this.GetVenueId().GetHashCode();
    }

    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool nameEquality = (this.GetVenueName() == newVenue.GetVenueName());
        bool addressEquality = (this.GetVenueAddress() == newVenue.GetVenueAddress());
        bool hoursEquality = (this.GetVenueHours() == newVenue.GetVenueHours());
        return (nameEquality && addressEquality && hoursEquality);
      }
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
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    //Create method for 'join' table
    public void AddBandsToVenues(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = newBand.GetBandId();
      cmd.Parameters.Add(band_id);

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = _id;
      cmd.Parameters.Add(venue_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
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
    public List<Band> GetBandsFromJoin()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT bands.* FROM venues
      JOIN venues_bands ON (venues.id = venues_bands.venue_id)
      JOIN bands ON (venues_bands.band_id = bands.id)
      WHERE venues.id = @VenueId;";

      MySqlParameter id = new MySqlParameter();
      id.ParameterName = "@VenueId";
      id.Value = _id;
      cmd.Parameters.Add(id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Band> bands = new List<Band>{};
      while(rdr.Read())
      {
        int band_id = rdr.GetInt32(0);
        string band_name = rdr.GetString(1);
        string band_genre = rdr.GetString(2);
        string band_from = rdr.GetString(3);
        Band newBand = new Band(band_name, band_genre, band_from, band_id);
        bands.Add(newBand);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return bands;
    }

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
      string venue_name = "";
      string venue_address = "";
      string venue_hours = "";

      while(rdr.Read())
      {
        venueId = rdr.GetInt32(0);
        venue_name = rdr.GetString(1);
        venue_address = rdr.GetString(2);
        venue_hours = rdr.GetString(3);
      }
      Venue newVenue = new Venue (venue_name, venue_address, venue_hours, venueId);

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

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = this.GetVenueId();
      cmd.Parameters.Add(venue_id);

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
