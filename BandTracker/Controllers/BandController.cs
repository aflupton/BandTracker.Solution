using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using BandTracker;

namespace BandTracker.Controllers
{
  public class BandController : Controller
  {
    [HttpGet("/Bands")]
    public ActionResult Index()
    {
      List<Band> allBands = Band.GetAllBands();
      return View("Index", allBands);
    }

    [HttpGet("/Bands/CreateForm")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/Bands/New")]
    public ActionResult Create()
    {
      Band newBand = new Band(Request.Form["band-name"], Request.Form["band-genre"], Request.Form["band-from"]);
      newBand.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/Bands/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Band selectedBand = Band.FindBands(id);
      List<Venue> bandVenues = selectedBand.GetVenuesFromJoin();
      model.Add("selectedBand", selectedBand);
      model.Add("bandVenues", bandVenues);
      return View("Details", model);
    }

    [HttpGet("/Band/{id}/Update")]
    public ActionResult Update(int id)
    {
      Band selectedBand = Band.FindBands(id);
      return View("BandUpdate", selectedBand);
    }

    [HttpPost("/Bands/{id}/Updated")]
    public ActionResult Updated(int id)
    {
      Band selectedBand = Band.FindBands(id);
      selectedBand.UpdateBand(Request.Form["band-name"], Request.Form["band-genre"], Request.Form["band-from"]);
      return RedirectToAction("Index");
    }

    [HttpPost("/Bands/{id}/Delete")]
    public ActionResult DeleteBook(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Band selectedBand = Band.FindBands(id);
      selectedBand.DeleteBand();
      return View("Index", model);
    }

    [HttpPost("/Bands/DeleteAll")]
    public ActionResult DeleteAll()
    {
      Band.DeleteAll();
      return View("Index");
    }
  }
}
