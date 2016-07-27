using System;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;

namespace WarsawTramsOnline
{
    [Activity(Label = "WarsawTramsOnline", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);


            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += async (sender, e) =>
            //{
            //    string url = @"https://api.um.warszawa.pl/api/action/wsstore_get/?id=c7238cfe-8b1f-4c38-bb4a-de386db7e776&apikey=f1712a99-0dad-42e1-9f12-c660438bd769";
            //    JsonValue json = await Test(url);
            //    Display(json);
            //};
        }

        //private void SetUpMap()
        //{
        //    if (mMap == null)
        //    {
        //        FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        //    }
        //}

        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.AddMarker(new MarkerOptions()
                .SetPosition(new LatLng(0, 0))
                .SetTitle("Marker"));
        }

        //private async Task<JsonValue> Test(string url)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";

        //    // Send the request to the server and wait for the response:
        //    using (WebResponse response = await request.GetResponseAsync())
        //    {
        //        // Get a stream representation of the HTTP web response:
        //        using (Stream stream = response.GetResponseStream())
        //        {
        //            // Use this stream to build a JSON document object:
        //            JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
        //            Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

        //            // Return the JSON document:
        //            return jsonDoc;
        //        }
        //    }
        //}

        //private void Display(JsonValue json)
        //{
        //    //TextView result = FindViewById<TextView>(Resource.Id.textView1);

        //    //result.Text = json.ToString();
        //    //result.MovementMethod = new ScrollingMovementMethod();
        //}


    }
}

