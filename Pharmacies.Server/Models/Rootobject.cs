using System;
using System.Collections.Generic;
using System.Text;

namespace Pharmacies.Server.Models
{
    /// <summary>
    /// Object returned by searchnearbyplace api. Contains array of searched places.
    /// </summary>
    public class Rootobject
    {
        public object[] html_attributions { get; set; }
        public Result[] results { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public string business_status { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public Plus_Code plus_code { get; set; }
        public float rating { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public string[] types { get; set; }
        public int user_ratings_total { get; set; }
        public string vicinity { get; set; }
        public Opening_Hours opening_hours { get; set; }
        public Photo[] photos { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Northeast
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Plus_Code
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }

    public class Opening_Hours
    {
        public bool open_now { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public string[] html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }



}





