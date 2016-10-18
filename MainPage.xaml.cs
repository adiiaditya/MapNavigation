using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Device;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Tasks;



namespace taxilocator
{
    public partial class Page4 : PhoneApplicationPage
    {
        PhoneCallTask phoneTask = null;
        GeoCoordinateWatcher watcher;
        public Page4()
        {
            InitializeComponent();
            phoneTask = new PhoneCallTask();
            map.ZoomBarVisibility = System.Windows.Visibility.Visible;   // This sets the Zoom bar in the map visible
            map.LogoVisibility = Visibility.Collapsed;  // The Bing map logo is set not to appear in the map
            map.CopyrightVisibility = Visibility.Collapsed;  //  The bing map copyright text is hidden
            map.Mode = new AerialMode();  // The map is set to load with default view mode as aerial.
            if (watcher == null)
            {
                //---To get the highest accuracy--
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
                {
                    //---the minimum distance (in meters) to travel before the next location updates---
                    MovementThreshold = 10
                };
                //when a new position is obtained---
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                //---event to fire when there is a status change in the location 
                // service API---
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.Start();
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            var task = new Microsoft.Phone.Tasks.BingMapsDirectionsTask();
            task.Start = new Microsoft.Phone.Tasks.LabeledMapLocation(textBox1.Text, new System.Device.Location.GeoCoordinate());
            task.End = new Microsoft.Phone.Tasks.LabeledMapLocation(textBox2.Text, new System.Device.Location.GeoCoordinate());
            task.Show();
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Debug.WriteLine("({0},{1})", e.Position.Location.Latitude, e.Position.Location.Longitude);
            map.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // the user has changed the settings in phone.
                        Debug.WriteLine("Location is disabled.Change Settings");
                    }
                    else
                    {
                        Debug.WriteLine("Location is not functioning on this phone");
                    }
                    break;
                case GeoPositionStatus.Initializing: Debug.WriteLine("initializing");
                    Debug.WriteLine("finding location");
                    break;
                case GeoPositionStatus.NoData: Debug.WriteLine("nodata");
                    break;
                case GeoPositionStatus.Ready: Debug.WriteLine("ready");
                    break;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PrivacyPolicy.xaml", UriKind.Relative));
        }
    
    }
}
