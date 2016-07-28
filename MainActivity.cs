using System;
using System.Collections.Generic;
using System.Json;
using System.Net;
using Android.App;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using WarsawTramsOnline.Models;
using System.Net.Http;
using Android.Content.Res;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace WarsawTramsOnline
{
    [Activity(Label = "WarsawTramsOnline", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/CustomActionBarTheme")]
    public class MainActivity : FragmentActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private TramsApiResult mLastApiResult;
        private string url = @"https://api.um.warszawa.pl/api/action/wsstore_get/?id=c7238cfe-8b1f-4c38-bb4a-de386db7e776&apikey=f1712a99-0dad-42e1-9f12-c660438bd769";

        private DrawerLayout mDrawerLayout;
        List<string> mLeftItems = new List<string>();
        private ArrayAdapter mLeftAdapter;
        private ListView mLeftDrawer;
        private ActionBarDrawerToggle mDrawerToogle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);


            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);

            mLeftItems.Add("Główna mapa");
            mLeftItems.Add("Wyszukaj");

            mLeftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, mLeftItems);
            mLeftDrawer.Adapter = mLeftAdapter;

            mDrawerToogle = new MyActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.navs_icon, Resource.String.open_drawer, Resource.String.close_drawer);

            mDrawerLayout.SetDrawerListener(mDrawerToogle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(true);

            var btnRefresh = FindViewById<Button>(Resource.Id.refresh);
            btnRefresh.Click += (sender, ea) =>
            {
                RefreshTrams();
            };

            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_bar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToogle.SyncState();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (mDrawerToogle.OnOptionsItemSelected(item))
            {
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToogle.OnConfigurationChanged(newConfig);
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

