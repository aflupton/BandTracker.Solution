using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using BandTracker;

namespace BandTracker.Controllers
{
  public class VenueController : Controller
  {
    [HttpGet("/Venues")]
    public ActionResult Index()
    {
      List<Venue> allVenues = Venue.GetAllVenues();
      return View(allVenues);
    }

    [HttpGet("/Venues/CreateForm")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/Venues/New")]
    public ActionResult Create()
    {
      Venue newVenue = new Venue(Request.Form["venue-name"], Request.Form["venue-address"], Request.Form["venue-hours"]);
      newVenue.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/Venues/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Venue selectedVenue = Venue.FindVenues(id);
      List<Band> venueBands = selectedVenue.GetBandsFromJoin();
      model.Add("selectedVenue", selectedVenue);
      model.Add("venueBands", venueBands);
      return View("Details", model);
    }

    [HttpGet("/Venues/{id}/Update")]
    public ActionResult Update(int id)
    {
      Venue selectedVenue = Venue.FindVenues(id);
      return View("VenueUpdate", selectedVenue);
    }

    [HttpPost("/Venues/{id}/Updated")]
    public ActionResult Updated(int id)
    {
      Venue selectedVenue = Venue.FindVenues(id);
      selectedVenue.UpdateVenue(Request.Form["venue-name"], Request.Form["venue-address"], Request.Form["venue-hours"]);
      return RedirectToAction("Index");
    }

    [HttpPost("/Venues/{id}/Delete")]
    public ActionResult DeleteBook(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Venue selectedVenue = Venue.FindVenues(id);
      selectedVenue.DeleteVenue();
      return View("Index", model);
    }
  }
}
