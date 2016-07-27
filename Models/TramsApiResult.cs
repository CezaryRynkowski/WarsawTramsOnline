using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WarsawTramsOnline.Models
{
    public class TramsApiResult
    {
        public List<Trams> result { get; set; } 
    }

    public class Trams
    {
        public string Status { get; set; }
        public int FirstLine { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Lines { get; set; }
        public DateTime Time { get; set; }
        public bool LowFloor { get; set; }
        public string Brigade { get; set; }
    }
}