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
using WarsawTramsOnline.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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

        public void OnMapReady(GoogleMap googleMap)
        {
            string url = @"https://api.um.warszawa.pl/api/action/wsstore_get/?id=c7238cfe-8b1f-4c38-bb4a-de386db7e776&apikey=f1712a99-0dad-42e1-9f12-c660438bd769";

            var json = GetTramsValue(url);

            var result = JsonConvert.DeserializeObject<TramsApiResult>(json);

            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(52.23153, 21.00403), 11); //Warsaw
            googleMap.MoveCamera(camera);

            foreach (var tram in result.result)
            {
                googleMap.AddMarker(new MarkerOptions()
                .SetPosition(new LatLng(tram.Lat, tram.Lon))
                .SetTitle(tram.FirstLine.ToString())
                .SetSnippet("Niskopodłogowy: " + tram.LowFloor)
                .Draggable(false));
            }

        }

        private string GetTramsValue(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var responseString = responseContent.ReadAsStringAsync().Result;
                    return responseString;
                }
            }
            return "error";
        }

        private void Display(JsonValue json)
        {
            //TextView result = FindViewById<TextView>(Resource.Id.textView1);

            //result.Text = json.ToString();
            //result.MovementMethod = new ScrollingMovementMethod();
        }


    }
}

