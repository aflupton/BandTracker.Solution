using System;
using System.Collections.Generic;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Models
{
  public class Band
  {
    //private variables
    private int _id;
    private string _bandName;
    private string _bandGenre;
    private string _bandFrom;
    //band constructor
    public Band(string bandName, string bandGenre, string bandFrom, int id = 0)
    {
      _bandName = bandName;
      _bandGenre = bandGenre;
      _bandFrom = bandFrom;
      _id = id;
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
    public string GetBandFrom()
    {
      return _bandFrom;
    }
    public void SetBandFrom(string newBandFrom)
    {
      _bandFrom = newBandFrom;
    }

    //get band id
    public int GetBandId()
    {
      return _id;
    }

    // Create test override methods
    public override int GetHashCode()
    {
      return this.GetBandId().GetHashCode();
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool nameEquality = (this.GetBandName() == newBand.GetBandName());
        bool genreEquality = (this.GetBandGenre() == newBand.GetBandGenre());
        bool fromEquality = (this.GetBandFrom() == newBand.GetBandFrom());
        return (nameEquality && genreEquality && fromEquality);
      }
    }

    // Create method for 'bands' table
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (band_name, band_genre, band_from) VALUES (@BandName, @BandGenre, @BandFrom);";

      MySqlParameter band_name = new MySqlParameter();
      band_name.ParameterName = "@BandName";
      band_name.Value = this._bandName;
      cmd.Parameters.Add(band_name);

      MySqlParameter band_genre = new MySqlParameter();
      band_genre.ParameterName = "@BandGenre";
      band_genre.Value = this._bandGenre;
      cmd.Parameters.Add(band_genre);

      MySqlParameter band_from = new MySqlParameter();
      band_from.ParameterName = "@BandFrom";
      band_from.Value = this._bandFrom;
      cmd.Parameters.Add(band_from);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    // //Create method for 'join' table
    public void AddVenuesToBand(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = newVenue.GetVenueId();
      cmd.Parameters.Add(venue_id);

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = _id;
      cmd.Parameters.Add(band_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    // //Read method for 'bands' table, singular
    public static List<Band> GetAllBands()
    {
      List<Band> allBands = new List<Band>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from bands;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string band_name = rdr.GetString(1);
        string band_genre = rdr.GetString(2);
        string band_from = rdr.GetString(3);
        Band newBand = new Band(band_name, band_genre, band_from, id);
        allBands.Add(newBand);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }

    // //Read method for 'join' table
    public List<Venue> GetVenuesFromJoin()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.* FROM bands
      JOIN venues_bands ON (bands.id = venues_bands.band_id)
      JOIN venues ON (venues_bands.venue_id = venues.id)
      WHERE bands.id = @BandId;";

      MySqlParameter id = new MySqlParameter();
      id.ParameterName = "@BandId";
      id.Value = _id;
      cmd.Parameters.Add(id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Venue> venues = new List<Venue>{};
      while(rdr.Read())
      {
        int venue_id = rdr.GetInt32(0);
        string venue_name = rdr.GetString(1);
        string venue_address = rdr.GetString(2);
        string venue_hours = rdr.GetString(3);
        Venue newVenue = new Venue(venue_name, venue_address, venue_hours, venue_id);
        venues.Add(newVenue);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return venues;
    }

    // //Find method for 'bands' table
    public static Band FindBands(int band_id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from bands WHERE band_id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = band_id;
      cmd.Parameters.Add(searchId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bandId = 0;
      string bandName = "";
      string bandGenre = "";
      string bandFrom = "";

      while(rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
        bandGenre = rdr.GetString(2);
        bandFrom = rdr.GetString(3);
      }
      Band newBand = new Band (bandName, bandGenre, bandFrom, bandId);

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return newBand;
    }

    // //Update method for 'bands' table, singular
    public void UpdateBand(string bandName, string bandGenre, string bandFrom)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE bands SET band_name=@BandName, band_genre=@BandGenre, band_from=@BandFrom WHERE band_id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter band_name = new MySqlParameter();
      band_name.ParameterName = "@BandName";
      band_name.Value = bandName;
      cmd.Parameters.Add(band_name);

      MySqlParameter band_genre = new MySqlParameter();
      band_genre.ParameterName = "@BandGenre";
      band_genre.Value = bandGenre;
      cmd.Parameters.Add(band_genre);

      MySqlParameter band_from = new MySqlParameter();
      band_from.ParameterName = "@BandFrom";
      band_from.Value = bandFrom;
      cmd.Parameters.Add(band_from);

      cmd.ExecuteNonQuery();
      _bandName = bandName;
      _bandGenre = bandGenre;
      _bandFrom = bandFrom;

      if(conn != null)
      {
        conn.Dispose();
      }
    }

    // //Delete method for 'bands' table, singular
    public void DeleteBand()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands WHERE band_id = @BandId;
      DELETE FROM venues_bands
      WHERE band_id = @BandId;";

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = this.GetBandId();
      cmd.Parameters.Add(band_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    //Delete method for 'bands' table, entire class
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE from bands;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
