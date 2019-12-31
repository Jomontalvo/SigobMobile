namespace SigobMobile.Helpers
{
    using System;
    using Telerik.XamarinForms.Chart;
    using Xamarin.Forms;

    public static class Palette
    {
        #region Attributes
        static readonly Color clear = ((Color)Application.Current.Resources["clear"]);
        static readonly Color greeniOS = ((Color)Application.Current.Resources["iOSTrafficGreen"]);
        static readonly Color rediOS = ((Color)Application.Current.Resources["iOSTrafficRed"]);
        static readonly Color yellowiOS = ((Color)Application.Current.Resources["iOSTrafficYellow"]);
        static readonly Color grayiOS = ((Color)Application.Current.Resources["gray"]);
        static readonly Color greenDroid = ((Color)Application.Current.Resources["calDarkGreen"]);
        static readonly Color redDroid = ((Color)Application.Current.Resources["calIndigo"]);
        static readonly Color yellowDroid = ((Color)Application.Current.Resources["calMango"]);
        static readonly Color grayDroid = ((Color)Application.Current.Resources["calGray"]);
        static readonly double opacity = 0.9;

        /// <summary>
        /// Traffic Ligth Color
        /// </summary>
        public static Color TrafficLightGreen => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgba(greeniOS.R, greeniOS.G, greeniOS.B, opacity),
            Device.Android => Color.FromRgba(greenDroid.R, greenDroid.G, greenDroid.B, opacity),
            _ => Color.Green
        };
        public static Color TrafficLightRed => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgba(rediOS.R, rediOS.G, rediOS.B, opacity),
            Device.Android => Color.FromRgba(redDroid.R, redDroid.G, redDroid.B, opacity),
            _ => Color.Red
        };
        public static Color TrafficLightGray => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgba(grayiOS.R, grayiOS.G, grayiOS.B, opacity),
            Device.Android => Color.FromRgba(grayDroid.R, grayDroid.G, grayDroid.B, opacity),
            _ => Color.Gray
        };
        public static Color TrafficLightYellow => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgba(yellowiOS.R, yellowiOS.G, yellowiOS.B, opacity),
            Device.Android => Color.FromRgba(yellowDroid.R, yellowDroid.G, yellowDroid.B, opacity),
            _ => Color.Yellow
        };

        /// <summary>
        /// Selected Traffic Light Color
        /// </summary>
        public static Color SelectedTrafficLightGreen => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgb(greeniOS.R, greeniOS.G, greeniOS.B),
            Device.Android => Color.FromRgb(greenDroid.R, greenDroid.G, greenDroid.B),
            _ => Color.DarkGreen
        };
        public static Color SelectedTrafficLightRed => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgb(rediOS.R, rediOS.G, rediOS.B),
            Device.Android => Color.FromRgb(redDroid.R, redDroid.G, redDroid.B),
            _ => Color.DarkRed
        };
        public static Color SelectedTrafficLightGray => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgb(grayiOS.R, grayiOS.G, grayiOS.B),
            Device.Android => Color.FromRgb(grayDroid.R, grayDroid.G, grayDroid.B),
            _ => Color.DarkGray
        };
        public static Color SelectedTrafficLightYellow => Device.RuntimePlatform switch
        {
            Device.iOS => Color.FromRgb(yellowiOS.R, yellowiOS.G, yellowiOS.B),
            Device.Android => Color.FromRgb(yellowDroid.R, yellowDroid.G, yellowDroid.B),
            _ => Color.DarkOrange
        };
        #endregion

        #region Properties
        public static ChartPalette RandomPalette { get; set; }
        public static ChartPalette TrafficLightPalette { get; set; }
        public static ChartPalette SelectedTrafficLightPalette { get; set; }
        #endregion

        #region Constructors
        static Palette()
        {
            RandomPalette = new ChartPalette();
            TrafficLightPalette = new ChartPalette();
            SelectedTrafficLightPalette = new ChartPalette();
            LoadColors();
            LoadTrafficLightColors();
            LoadSelectedTrafficLightColors();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate 30 random colors
        /// </summary>
        private static void LoadColors()
        {
            var rand = new Random();

            // Loads 30 colors
            for (int i = 0; i < 30; i++)
            {
                RandomPalette.Entries.Add(new PaletteEntry
                {
                    StrokeColor = Color.Transparent,
                    FillColor = Color.FromRgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255))
                });
            }
        }

        /// <summary>
        /// Generate traffic light colors (order is key)
        /// </summary>
        private static void LoadTrafficLightColors()
        {
            // Green color (First result of API)
            TrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = clear,
                FillColor = TrafficLightGreen
            });
            //Red color (Second result of API)
            TrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = clear,
                FillColor = TrafficLightRed
            });
            //Gray color (Third result of API)
            TrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = clear,
                FillColor = TrafficLightGray
            });
            //Yellow color (Last result of API)
            TrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = clear,
                FillColor = TrafficLightYellow
            });
        }

        /// <summary>
        /// Generate selected traffic light colors
        /// </summary>
        private static void LoadSelectedTrafficLightColors()
        {
            // Selected Green color (1)
            SelectedTrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = Color.DarkGreen,
                FillColor = SelectedTrafficLightGreen
            });
            // Selected Red color (2)
            SelectedTrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = Color.DarkRed,
                FillColor = SelectedTrafficLightRed
            });
            //Selected Gray color (3)
            SelectedTrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = Color.Black,
                FillColor = SelectedTrafficLightGray
            });
            // Selected Yellow color (4)
            SelectedTrafficLightPalette.Entries.Add(new PaletteEntry
            {
                StrokeColor = Color.DarkOrange,
                FillColor = SelectedTrafficLightYellow
            });
        }
        #endregion
    }
}
