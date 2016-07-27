using System;
using System.Json;
using System.Net;
using Android.App;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using WarsawTramsOnline.Models;
using System.Net.Http;
using Android.Widget;
using Newtonsoft.Json;

namespace WarsawTramsOnline
{
    [Activity(Label = "WarsawTramsOnline", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.NoActionBar.Fullscreen")]
    public class MainActivity : FragmentActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private TramsApiResult mLastApiResult;
        private string url = @"https://api.um.warszawa.pl/api/action/wsstore_get/?id=c7238cfe-8b1f-4c38-bb4a-de386db7e776&apikey=f1712a99-0dad-42e1-9f12-c660438bd769";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var btnRefresh = FindViewById<Button>(Resource.Id.btnRefreshTrams);

            btnRefresh.Click += (sender, ea) =>
            {
                RefreshTrams();
            };

            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

        }

        private void RefreshTrams()
        {
            mMap.Clear();
            var result = JsonConvert.DeserializeObject<TramsApiResult>(GetTramsValue(url));

            foreach (var tram in result.result)
            {
                mMap.AddMarker(new MarkerOptions()
                .SetPosition(new LatLng(tram.Lat, tram.Lon))
                .SetTitle(tram.FirstLine.ToString())
                .SetSnippet("Niskopodłogowy: " + tram.LowFloor)
                .Draggable(false));
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            var json = GetTramsValue(url);

            var result = JsonConvert.DeserializeObject<TramsApiResult>(json);
            mLastApiResult = result;

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
    }
}

