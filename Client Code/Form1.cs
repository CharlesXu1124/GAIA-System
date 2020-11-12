using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using GMap.NET.MapProviders;
using GMap.NET;

using System.Threading;
using System.Timers;
using System.Diagnostics;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Collections;
using System.IO.Ports;
using System.Media;
using System.Drawing.Imaging;

//web request imports
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Net.Http;


// Firebase import dependencies
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Diagnostics.Tracing;
using System.Text.Json;
using FireSharp.Extensions;

// speech recognition imports
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using Timer = System.Timers.Timer;
using Windows.Devices.Geolocation;


// imports for GPS location acquiring and querying
using System.Device.Location;
using GMap.NET.WindowsForms;
using Windows.Foundation;
using Windows.UI.Composition;
using System.Security.Policy;
using Blimp_GCS;
using Windows.Web.Http;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace SearchGCS
{
    public partial class Form1 : Form
    {
        Random rng = new Random();

        // variable for keeping track of missile left
        int num_missiles = 5;

        // Definition for device GPS
        Bitmap bitmap;
        VideoCaptureDevice cam;
        FilterInfoCollection captureDevices;
        static GMapProvider[] mapProviders;
        Double LONGITUDE;
        Double LATITUDE;
        int picHeight;
        int picWidth;
        Bitmap bitmap2;
        int xAction;
        int yAction;
        int zAction;
        int rotAction;
        int frame_counter;
        static bool _continue;
        static SerialPort comport;
        static bool isConnected;
        double a;
        int audio_counter;
        int action_counter;
        Stopwatch stopWatch = new Stopwatch();
        String obj1, obj2, obj3, obj4, obj5;
        double xpos1, xpos2, xpos3, xpos4, xpos5, ypos1, ypos2, ypos3, ypos4, ypos5;
        double w1, h1, w2, h2, w3, h3, w4, h4, w5, h5;
        PointLatLng pointBase;
        PointLatLng pointTarget;

        GMapRoute trajectory;
        GMapOverlay routes;
        GMapOverlay markers;
        GMapMarker marker_base;
        GMarkerGoogle plane_marker;
        GMapOverlay overlayPlane;



        GMap.NET.WindowsForms.GMapMarker marker_target;
        Double distance_to_target;
        GMapOverlay newMarkerOverlay = new GMapOverlay("markers");

        // Definition of variables for Radar interface
        Timer t = new Timer(50);
        int WIDTH = 200, HEIGHT = 200, HAND = 100;
        int u; // in degrees
        int cx, cy;     //center of the circle
        int x, y;       //HAND coordinate
        int tx, ty, lim = 20;
        Bitmap bitmap_radar;
        Pen p_radar;
        Pen p_missile;
        Graphics g_radar;
        int numSatellites = 0;


        // define weather rocket
        Blimp_GCS.Missile missile;

        // variable for space plane
        SpacePlane sp;


        public Bitmap resized_plane;

        // define adjusted x and y positions
        int xpos1_adj, ypos1_adj, w1_adj, h1_adj;
        int xpos2_adj, ypos2_adj, w2_adj, h2_adj;

        private void MainMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Double lat = MainMap.FromLocalToLatLng(e.X, e.Y).Lat;
                Double lng = MainMap.FromLocalToLatLng(e.X, e.Y).Lng;
                var newMarker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                    new PointLatLng(lat, lng),
                    GMarkerGoogleType.yellow_dot);
                MainMap.Overlays.Add(newMarkerOverlay);
                newMarkerOverlay.Markers.Add(newMarker);

            }
            // mapclick
        }

        private void MainMap_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        int xpos3_adj, ypos3_adj, w3_adj, h3_adj;

        private void btnVoice_Click(object sender, EventArgs e)
        {
            btnVoice.Enabled = false;
            clist.Add(new string[] { "connect to database", "turn left", "turn right", "ascent", "descend", "left", "right"
            ,"close camera", "open camera", "what did you see", "update satellites", "forward", "backward",
                "stop", "lock area", "launch rocket", "focus area", "update weather"});
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            try
            {
                
                sre.RequestRecognizerUpdate();
                sre.LoadGrammar(gr);
                sre.SpeechRecognized += sre_SpeechRecognized;
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
                ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text.ToString())
            {
                case "connect to database":
                    ss.SpeakAsync("connecting to database");
                    client = new FireSharp.FirebaseClient(config);
                    button3.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case "open camera":
                    ss.SpeakAsync("webcam opened");
                    
                    cam = new VideoCaptureDevice(captureDevices[cbo.SelectedIndex].MonikerString);
                    cam.NewFrame -= Handle_New_Frame;
                    cam.NewFrame += new NewFrameEventHandler(Handle_New_Frame);
                    cam.Start();
                    BtnStart.Hide();
                    BtnStop.Show();
                    break;
                case "close camera":
                    ss.SpeakAsync("webcam paused");
                    
                    if (cam.IsRunning == true)
                    {
                        cam.NewFrame -= Handle_New_Frame;
                        cam.SignalToStop();
                        cam = null;
                    }
                    BtnStart.Show();
                    BtnStop.Hide();
                    break;
                case "what did you see":
                    if (obj1 == "null")
                    {
                        ss.SpeakAsync("I did not see anything special");
                    }
                    else
                    {
                        ss.SpeakAsync($"I saw a {obj1}");
                    }
                    break;
                case "update satellites":
                    // making the query to count the number of satellites above me
                    String satResult;
                    SatelliteModel sm = new SatelliteModel
                    {
                        latitude = pointTarget.Lat,
                        longitude = pointTarget.Lng,
                        elevation = 70.0 // by default
                    };
                    var satPayload = await Task.Run(() => JsonConvert.SerializeObject(sm));
                    HttpContent content = new StringContent(satPayload, Encoding.UTF8, "application/json");
                    var url = "YOUR AWS API URL";


                    using (HttpClient weatherClient = new HttpClient())
                    {
                        using (HttpResponseMessage weatherResponse = await weatherClient.PostAsync(url, content))
                        {
                            using (HttpContent returnedRes = weatherResponse.Content)
                            {
                                satResult = await returnedRes.ReadAsStringAsync();
                                //HttpContentHeaders headers = returnedRes.Headers;

                            }
                        }
                    }
                    numSatellites = int.Parse(satResult);
                    ss.SpeakAsync($"There are {satResult} operational GPS satellites above you");
                    break;
                case "forward":
                    txtX.Text = "Forward";
                    BtnUp.BackColor = System.Drawing.Color.Red;
                    break;
                case "backward":
                    txtX.Text = "backward";
                    BtnDown.BackColor = System.Drawing.Color.Red;
                    break;
                case "stop":
                    txtX.Text = "None";
                    BtnUp.BackColor = System.Drawing.Color.Yellow;
                    BtnDown.BackColor = System.Drawing.Color.Yellow;
                    break;
                case "launch rocket":
                    if (num_missiles != 0)
                    {
                        num_missiles -= 1;
                        ss.SpeakAsync("missle launched");
                        var tempOverlay = new GMapOverlay("temporary");
                        MainMap.Overlays.Add(tempOverlay);

                        sp.launch();
                        var missileMarker = sp.getMissileMarker();

                        // missile launch coordinates
                        Double missile_lat = sp.missileLat;
                        Double missile_lng = sp.missileLon;

                        for (Double k = 0; k < 1; k += 0.05)
                        {
                            if (k > 0.95)
                            {
                                

                                Bitmap label = Blimp_GCS.Properties.Resources.explosion;
                                Bitmap resized = new Bitmap(label, new Size(50, 50));
                                var newMarker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                                new PointLatLng(pointTarget.Lat, pointTarget.Lng),
                                resized
                                );
                                
                                tempOverlay.Markers.Add(newMarker);

                                // display image for a duration of one second
                                await Task.Delay(1000);
                                tempOverlay.Markers.Remove(newMarker);
                            } else
                            {
                                Double missileActualLat = missile_lat + k * (pointTarget.Lat - missile_lat);
                                Double missileActualLon = missile_lng + k * (pointTarget.Lng - missile_lng);
                                missileMarker = sp.updateMissileMarker(missileActualLat, missileActualLon);


                                tempOverlay.Markers.Add(missileMarker);
                                await Task.Delay(250);
                                tempOverlay.Markers.Remove(missileMarker);
                            }
                            
                        }
                        
                        ss.SpeakAsync("target hit");
                        sp.unlockTarget();
                        ss.SpeakAsync("fire extinguished");
                    }
                    else
                    {
                        ss.SpeakAsync("no missile left");
                    }
                    break;
                case "focus area":
                    drawPolygon();
                    ss.SpeakAsync("Target Changed");
                    updateMap(pointTarget);
                    ss.SpeakAsync("target locked");
                    sp.lockTarget();
                    break;
                case "update weather":


                    String weatherResult;
                    WeatherModel weatherModel = new WeatherModel
                    {
                        latitude = pointTarget.Lat,
                        longitude = pointTarget.Lng
                    };
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(weatherModel));



                    HttpContent weatherPayload = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    var urlWeather = "YOUR AWS REST API URL";



                    using (HttpClient weatherClient = new HttpClient())
                    {
                        using (HttpResponseMessage weatherResponse = await weatherClient.PostAsync(urlWeather, weatherPayload))
                        {
                            using (HttpContent returnedRes = weatherResponse.Content)
                            {
                                weatherResult = await returnedRes.ReadAsStringAsync();
                                HttpContentHeaders headers = returnedRes.Headers;

                            }
                        }
                    }
                    ss.SpeakAsync($"the target weather is {weatherResult}");
                        
                        
                    break;
            }

        }

        private void drawPolygon()
        {
            if (newMarkerOverlay.Markers != null) {
                List<PointLatLng> pointslatlang = new List<PointLatLng>();
                Double sum_lat = 0;
                Double sum_lon = 0;
                
                foreach (GMap.NET.WindowsForms.Markers.GMarkerGoogle m in newMarkerOverlay.Markers)
                {
                    sum_lat += m.Position.Lat;
                    sum_lon += m.Position.Lng;
                    pointslatlang.Add(new PointLatLng(m.Position.Lat, m.Position.Lng));
                }
                sum_lat /= newMarkerOverlay.Markers.Count;
                sum_lon /= newMarkerOverlay.Markers.Count;
                GMapPolygon polygon = new GMapPolygon(pointslatlang, "polygon");
                GMapOverlay polygons = new GMapOverlay("polygons");
                polygons.Polygons.Add(polygon);
                Bitmap label = Blimp_GCS.Properties.Resources.crosshair;
                Bitmap resized = new Bitmap(label, new Size(50, 50));
                pointTarget.Lat = sum_lat;
                pointTarget.Lng = sum_lon;
                marker_target = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                    new PointLatLng(sum_lat, sum_lon),
                    resized
                    );
                polygons.Markers.Add(marker_target);
                MainMap.Overlays.Add(polygons);
                //markers.Markers.Remove(marker_target);
                //markers.Markers.Add(marker_target);

                label.Dispose();
                // refresh the map by changing zoom
                MainMap.Zoom++;
                MainMap.Zoom--;
            }
        }

        private void updateMap(PointLatLng newTarget)
        {
            var route = GoogleMapProvider.Instance.GetRoute(pointBase, newTarget, false, false, 7);
            // remove old routes
            routes.Routes.Remove(trajectory);
            trajectory = new GMapRoute(route.Points, "trajectory")
            {
                Stroke = new Pen(System.Drawing.Color.Red, 2)
            };
            
            routes.Routes.Add(trajectory);

            distance_to_target = route.Distance;
            markers.Markers.Add(marker_base);
            MainMap.Overlays.Add(markers);

            MainMap.Overlays.Add(routes);

            distance_to_target = route.Distance;
            missile = new Blimp_GCS.Missile(pointBase.Lat, pointBase.Lng, 300000, pointTarget, false, distance_to_target, false);
        }

        int xpos4_adj, ypos4_adj, w4_adj, h4_adj;
        int xpos5_adj, ypos5_adj, w5_adj, h5_adj;

        // initialization for speech recognition
        SpeechSynthesizer ss = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        Choices clist;

        // Firebase authentication config
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "YOUR AUTH SECRET",
            BasePath = "YOUR FIREBASE BASE PATH"
        };

        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = comport.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // initialize voice recognition objects
            clist = new Choices();

            // initialization for radar map
            bitmap_radar = new Bitmap(WIDTH + 1, HEIGHT + 1);
            radar_box.BackColor = System.Drawing.Color.Black;
            cx = WIDTH / 2;
            cy = HEIGHT / 2;
            u = 0;

            t.Elapsed += OnTimedEvent;
            t.AutoReset = true;
            t.Enabled = true;


            // initialize recognized objects
            obj1 = "null";
            obj2 = "null";
            obj3 = "null";
            obj4 = "null";
            obj5 = "null";

            action_counter = 0;

            frame_counter = 0;
            audio_counter = 0;
            // create a new stopwatch
            stopWatch = new Stopwatch();
            // Create a new SerialPort object with default settings. 
            comport = new SerialPort("YOUR COM PORT", 921600, Parity.None, 8, StopBits.One);
            //comport.Open();
            comport.ReadTimeout = 500;
            comport.WriteTimeout = 500;
            _continue = true;

            isConnected = false;

            xAction = 0;
            yAction = 0;
            zAction = 0;
            rotAction = 0;


            picWidth = pbo.Size.Width;
            picHeight = pbo.Size.Height;
            LATITUDE = 47.2427655;
            LONGITUDE = -122.455138;


            // variables for keeping track of target and base positions
            pointBase = new PointLatLng(LATITUDE, LONGITUDE);
            pointTarget = new PointLatLng(33.0166666, -116.6833306);

            KeyPreview = true;
            Focus();
            BtnStop.Hide();

            // Unicode character for action buttons
            BtnUp.Text = char.ConvertFromUtf32(0x2191);
            BtnDown.Text = char.ConvertFromUtf32(0x2193);
            BtnLeft.Text = char.ConvertFromUtf32(0x2190);
            BtnRight.Text = char.ConvertFromUtf32(0x2192);
            BtnLeftTurn.Text = char.ConvertFromUtf32(0x21B6);
            BtnRightTurn.Text = char.ConvertFromUtf32(0x21B7);
            BtnAscend.Text = char.ConvertFromUtf32(0x21EA);
            BtnDescend.Text = char.ConvertFromUtf32(0x21E9);



            #region map_setup
            mapProviders = new GMapProvider[4];
            mapProviders[2] = GMapProviders.BingHybridMap;
            mapProviders[1] = GMapProviders.GoogleHybridMap;
            mapProviders[0] = GMapProviders.GoogleMap;
            mapProviders[3] = GMapProviders.GoogleSatelliteMap;

            GMapProviders.GoogleMap.ApiKey = "YOUR GOOGLE MAP KEY";

            for (int i = 0; i < 4; i++)
            {
                cbMapProviders.Items.Add(mapProviders[i]);
            }

            MainMap.DragButton = MouseButtons.Right;
            


            MainMap.MinZoom = 1;
            MainMap.MaxZoom = 20;
            MainMap.CacheLocation = Path.GetDirectoryName(Application.ExecutablePath) + "/mapcache/";

            MainMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            MainMap.DragButton = MouseButtons.Left;

            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            MainMap.Position = new PointLatLng(LATITUDE, LONGITUDE);
            MainMap.MarkersEnabled = true;
            MainMap.ShowCenter = false;


            overlayPlane = new GMapOverlay("plane");

            MainMap.Overlays.Add(overlayPlane);


            // add plane marker to the overlay on map

            
            
            

            markers = new GMapOverlay("markers");
            Bitmap stationLabel = Blimp_GCS.Properties.Resources.spacestation;
            Bitmap baseLabel = new Bitmap(stationLabel, new Size(70, 70));

            marker_base =
                new GMarkerGoogle(
                    pointBase,
                    baseLabel);



            marker_target =
                new GMarkerGoogle(
                    pointTarget,
                    GMarkerGoogleType.red_pushpin);

            var route = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(pointBase, pointTarget, false, false, 7);
            

            trajectory = new GMapRoute(route.Points, "trajectory")
            {
                Stroke = new Pen(System.Drawing.Color.Red, 2)
            };
            
            routes = new GMapOverlay("routes");
            routes.Routes.Add(trajectory);


            markers.Markers.Add(marker_target);
            markers.Markers.Add(marker_base);
            MainMap.Overlays.Add(markers);

            MainMap.Overlays.Add(routes);

            MainMap.Zoom = 4;



            // plane will take off from base


            sp = new SpacePlane(pointBase.Lat, pointBase.Lng, 10, 0);
            sp.unlockTarget();

            plane_marker = sp.getMarker();
            overlayPlane.Markers.Add(plane_marker);


            Timer t_plane = new Timer();
            t_plane.Interval = 200; // update the plane position every 200 ms
            t_plane.Elapsed += (sender_plane, e_plane) => plane_update(sender_plane, e_plane);

            t_plane.AutoReset = true;
            t_plane.Enabled = true;

            #endregion

            try
            {
                captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                // no camera detected
                if (captureDevices.Count == 0)
                    throw new ApplicationException();

                cbo.Items.Clear();


                foreach (FilterInfo device in captureDevices)
                {
                    cbo.Items.Add(device.Name);
                }


                cbo.SelectedIndex = 0;
                cbo.Enabled = true;
            }
            catch (ApplicationException)
            {
                cbo.Enabled = false;
            }
        }

        private void plane_update(object sender, ElapsedEventArgs e)
        {
            try
            {
                overlayPlane.Markers.Remove(plane_marker);
            } catch (Exception)
            {

            }
            sp.updatePath(pointTarget.Lat, pointTarget.Lng);
            plane_marker = sp.getMarker();
            overlayPlane.Markers.Add(plane_marker);

        }

        

        // drawing method for radar interface
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //pen
            if (sp != null && sp.target_locked)
            {
                p_radar = new Pen(System.Drawing.Color.Red, 1f);
            } else
            {
                p_radar = new Pen(System.Drawing.Color.Green, 1f);
            }
            
            
            p_missile = new Pen(System.Drawing.Color.BlueViolet, 1f);

            //graphics
            try
            {
                g_radar = Graphics.FromImage(bitmap_radar);
            } catch
            {

            }
            
            //calculate x, y coordinate of HAND
            int tu = (u - lim) % 360;
            if (u >= 0 && u <= 180)
            {
                //right half
                //u in degree is converted into radian.

                x = cx + (int)(HAND * Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int)(HAND * -Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }

            if (tu >= 0 && tu <= 180)
            {
                //right half
                //tu in degree is converted into radian.

                tx = cx + (int)(HAND * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(HAND * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }

            try
            {
                //draw circle
                g_radar.DrawEllipse(p_radar, 0, 0, WIDTH, HEIGHT);
                g_radar.DrawEllipse(p_radar, 80, 80, WIDTH - 160, HEIGHT - 160);

                //draw perpendicular line
                g_radar.DrawLine(p_radar, new Point(cx, 0), new Point(cx, HEIGHT)); // UP-DOWN
                g_radar.DrawLine(p_radar, new Point(0, cy), new Point(WIDTH, cy)); //LEFT-RIGHT

                //draw HAND
                g_radar.DrawLine(new Pen(System.Drawing.Color.Black, 1f), new Point(cx, cy), new Point(tx, ty));
                g_radar.DrawLine(p_radar, new Point(cx, cy), new Point(x, y));
            } catch(Exception)
            {

            }
            

            // draw satellites
            if (numSatellites != 0) {
                for (int i = 0; i < numSatellites; i++)
                {
                    Bitmap satelliteImg = Blimp_GCS.Properties.Resources.satellite;
                    Bitmap resizedSat = new Bitmap(satelliteImg, new Size(20, 20));
                    g_radar.DrawImage(resizedSat, 20 * i, 10);
                }
            }

            Font missile_font = new System.Drawing.Font("Arial", 6, FontStyle.Regular, GraphicsUnit.Point);
            RectangleF rect_missile = new RectangleF(40, 150, 60, 40);

            // draw missile label
            if (num_missiles != 0)
            {
                try
                {
                    g_radar.DrawLine(p_missile, 20, 150, 25, 145);
                    g_radar.DrawLine(p_missile, 25, 145, 30, 150);
                    g_radar.DrawLine(p_missile, 20, 150, 30, 150);

                    g_radar.DrawLine(p_missile, 20, 150, 20, 180);
                    g_radar.DrawLine(p_missile, 20, 180, 30, 180);
                    g_radar.DrawLine(p_missile, 30, 180, 30, 150);

                    g_radar.DrawString($"Rocket {num_missiles}", missile_font, Brushes.MediumPurple, rect_missile);
                } catch(Exception)
                {
                    
                }
            }


            radar_box.Image = bitmap_radar;
            //p_radar.Dispose();
            //g_radar.Dispose();

            //update
            u++;
            if (u == 360)
            {
                u = 0;
            }
        }



        #region Bluetooth private methods


        #endregion
        private void BtnStart_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(captureDevices[cbo.SelectedIndex].MonikerString);
            cam.NewFrame -= Handle_New_Frame;
            cam.NewFrame += new NewFrameEventHandler(Handle_New_Frame);
            cam.Start();
            BtnStart.Hide();
            BtnStop.Show();
        }

        private void Handle_New_Frame(object sender, NewFrameEventArgs eventArgs)
        {
            if (bitmap != null)
                bitmap.Dispose();
            // upload to Firebase
            bitmap = eventArgs.Frame.Clone() as Bitmap;
            upload_firebase(bitmap);

            get_res_firebase();

            if (pbo.Image != null)
                this.Invoke(new MethodInvoker(delegate () { pbo.Image.Dispose(); }));
            if (bitmap != null)
            {

                bitmap2 = new Bitmap(bitmap, pbo.Size);

                bitmap.Dispose();
                Graphics g = Graphics.FromImage(bitmap2);

                Pen pen = new Pen(System.Drawing.Color.FromArgb(255, 0, 255, 0));
                Pen redPen = new Pen(System.Drawing.Color.FromArgb(255, 0, 0, 255));
                g.DrawLine(pen, 50, (int)(picHeight / 2), picWidth - 50, (int)(picHeight / 2));
                g.DrawLine(pen, 50, (int)(picHeight / 2) + 10, picWidth / 2 - 10, (int)(picHeight / 2) + 10);
                g.DrawLine(pen, picWidth / 2 - 10, (int)(picHeight / 2) + 10, picWidth / 2, (int)(picHeight / 2));
                g.DrawLine(pen, picWidth / 2, (int)(picHeight / 2), picWidth / 2 + 10, (int)(picHeight / 2) + 10);
                g.DrawLine(pen, picWidth / 2 + 10, (int)(picHeight / 2) + 10, picWidth - 50, (int)(picHeight / 2) + 10);
                
                g.DrawLine(pen, 50, 20, 50, 170);
                g.DrawLine(pen, picWidth - 50, 20, picWidth - 50, 170);
                g.DrawLine(pen, 50, 30, 60, 30);
                g.DrawLine(pen, 50, 40, 60, 40);

                g.DrawLine(pen, picWidth - 60, 30, picWidth - 50, 30);
                g.DrawLine(pen, picWidth - 60, 40, picWidth - 50, 40);

                g.DrawLine(pen, 50, 50, 60, 50);
                g.DrawLine(pen, 50, 60, 60, 60);

                g.DrawLine(pen, picWidth - 60, 50, picWidth - 50, 50);
                g.DrawLine(pen, picWidth - 60, 60, picWidth - 50, 60);

                g.DrawLine(pen, 50, 70, 60, 70);
                g.DrawLine(pen, 50, 80, 60, 80);

                g.DrawLine(pen, picWidth - 60, 70, picWidth - 50, 70);
                g.DrawLine(pen, picWidth - 60, 80, picWidth - 50, 80);


                g.DrawLine(pen, 50, 110, 60, 110);
                g.DrawLine(pen, 50, 140, 60, 140);

                g.DrawLine(pen, picWidth - 60, 110, picWidth - 50, 110);
                g.DrawLine(pen, picWidth - 60, 140, picWidth - 50, 140);

                g.DrawLine(pen, 50, 130, 60, 130);
                g.DrawLine(pen, 50, 120, 60, 120);

                g.DrawLine(pen, picWidth - 60, 130, picWidth - 50, 130);
                g.DrawLine(pen, picWidth - 60, 120, picWidth - 50, 120);

                g.DrawLine(pen, 50, 150, 60, 150);
                g.DrawLine(pen, 50, 160, 60, 160);

                g.DrawLine(pen, picWidth - 60, 150, picWidth - 50, 150);
                g.DrawLine(pen, picWidth - 60, 160, picWidth - 50, 160);

                g.DrawLine(pen, 50, 170, 60, 170);
                g.DrawLine(pen, 50, 180, 60, 180);

                g.DrawLine(pen, picWidth - 60, 170, picWidth - 50, 170);
                

                g.DrawLine(pen, 50, 20, 60, 20);

                g.DrawLine(pen, picWidth - 60, 20, picWidth - 50, 20);
                g.DrawLine(pen, (int)(picWidth / 2), 40, (int)(picWidth / 2), picHeight - 20);
                
                
                for (int i = 0; i < 360; i += 20)
                {
                    a = i * (Math.PI) / 180;

                    g.DrawLine(pen, (int)(picWidth / 2 + 25 * Math.Cos(a)), 
                        (int)(40 + (25 * Math.Sin(a))), (int)(picWidth / 2 + 30*Math.Cos(a)), (int) (40 + 30*Math.Sin(a)));
                }

                g.DrawLine(pen, (int)(picWidth / 2), 40, (int)(picWidth / 2) - 5, 50);
                g.DrawLine(pen, (int)(picWidth / 2), 40, (int)(picWidth / 2) + 5, 50);
                g.DrawLine(pen, (int)(picWidth / 2) - 5, 50, (int)(picWidth / 2) + 5, 50);

                // draw the bounding boxes of recognized objects as well as the labels
                Pen objPen = new Pen(System.Drawing.Color.FromArgb(255, 255, 0, 0));
                Rectangle rect_obj;
                if (obj1 != "null")
                {
                    using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(10, 10, 60, 10);
                        g.DrawString(obj1, font1, Brushes.Red, rectF1);
                        xpos1_adj = (int) (xpos1 / 640 * picWidth);
                        ypos1_adj = (int) (ypos1 / 480 * picHeight);
                        w1_adj = (int) (w1 / 480 * picWidth);
                        h1_adj = (int) (h1 / 640 * picHeight);
                        rect_obj = new Rectangle(xpos1_adj, ypos1_adj, w1_adj, h1_adj);
                        g.DrawRectangle(objPen, rect_obj);
                    }
                }

                if (obj2 != "null")
                {
                    using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(10, 20, 60, 10);
                        g.DrawString(obj2, font1, Brushes.Red, rectF1);
                        xpos2_adj = (int)(xpos2 / 640 * picWidth);
                        ypos2_adj = (int)(ypos2 / 480 * picHeight);
                        w2_adj = (int)(w2 / 480 * picWidth);
                        h2_adj = (int)(h2 / 640 * picHeight);
                        rect_obj = new Rectangle(xpos2_adj, ypos2_adj, w2_adj, h2_adj);
                        g.DrawRectangle(objPen, rect_obj);
                    }
                }

                if (obj3 != "null")
                {
                    using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(10, 30, 60, 10);
                        g.DrawString(obj3, font1, Brushes.Red, rectF1);
                        xpos3_adj = (int)(xpos3 / 640 * picWidth);
                        ypos3_adj = (int)(ypos3 / 480 * picHeight);
                        w3_adj = (int)(w3 / 480 * picWidth);
                        h3_adj = (int)(h3 / 640 * picHeight);
                        rect_obj = new Rectangle(xpos3_adj, ypos3_adj, w3_adj, h3_adj);
                        g.DrawRectangle(objPen, rect_obj);
                    }
                }

                if (obj4 != "null")
                {
                    using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(10, 40, 60, 10);
                        g.DrawString(obj4, font1, Brushes.Red, rectF1);
                        xpos4_adj = (int)(xpos2 / 640 * picWidth);
                        ypos4_adj = (int)(ypos2 / 480 * picHeight);
                        w4_adj = (int)(w4 / 480 * picWidth);
                        h4_adj = (int)(h4 / 640 * picHeight);
                        rect_obj = new Rectangle(xpos4_adj, ypos4_adj, w4_adj, h4_adj);
                        g.DrawRectangle(objPen, rect_obj);
                    }
                }

                if (obj5 != "null")
                {
                    using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(10, 50, 60, 10);
                        g.DrawString(obj5, font1, Brushes.Red, rectF1);
                        xpos5_adj = (int)(xpos5 / 640 * picWidth);
                        ypos5_adj = (int)(ypos5 / 480 * picHeight);
                        w5_adj = (int)(w5 / 480 * picWidth);
                        h5_adj = (int)(h5 / 640 * picHeight);
                        rect_obj = new Rectangle(xpos5_adj, ypos5_adj, w5_adj, h5_adj);
                        g.DrawRectangle(objPen, rect_obj);
                    }
                }

                string text1 = "Altitude";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(70, 10, 40, 10);
                    g.DrawString(text1, font1, Brushes.SpringGreen, rectF1);
                }

                string text2 = "Battery";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(picWidth - 50, 10, 40, 10);
                    g.DrawString(text2, font1, Brushes.SpringGreen, rectF1);
                }

                string text3 = "0.0 m ";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(70, 25, 40, 10);
                    g.DrawString(text3, font1, Brushes.SpringGreen, rectF1);
                }

                string text4 = "100 % ";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(picWidth - 50, 25, 40, 10);
                    g.DrawString(text4, font1, Brushes.SpringGreen, rectF1);
                }

                string text5 = "Speed";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(70, 50, 35, 10);
                    g.DrawString(text5, font1, Brushes.SpringGreen, rectF1);
                    g.DrawRectangle(pen, Rectangle.Round(rectF1));
                }

                string text6 = "0.0 m/s";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(70, 65, 40, 10);
                    g.DrawString(text6, font1, Brushes.SpringGreen, rectF1);
                }

                string text7 = "Action";
                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(10, picHeight - 70, 40, 10);
                    g.DrawString(text7, font1, Brushes.SpringGreen, rectF1);
                }

                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point))
                {
                    
                    RectangleF rectF1 = new RectangleF(100, picHeight - 60, 100, 20);
                    if (!isConnected)
                    {
                        frame_counter += 1;
                        audio_counter += 1;
                        if (frame_counter > 25)
                        {
                            g.DrawString("Low Battery !", font1, Brushes.Red, rectF1);
                        }
                        if (frame_counter == 50)
                            frame_counter = 0;
                    } else
                    {
                        g.DrawString("Armed", font1, Brushes.Orange, rectF1);
                    }
                    

                }
                
                string textF = char.ConvertFromUtf32(0x21D1);
                string textB = char.ConvertFromUtf32(0x21D3);
                string textL = char.ConvertFromUtf32(0x21D0);
                string textR = char.ConvertFromUtf32(0x21D2);
                string textRotL = char.ConvertFromUtf32(0x21B6);
                string textRotR = char.ConvertFromUtf32(0x21B7);
                string textU = char.ConvertFromUtf32(0x21A5);
                string textD = char.ConvertFromUtf32(0x21A7);

                using (System.Drawing.Font font1 = new System.Drawing.Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point))
                {
                    RectangleF rectF1 = new RectangleF(10, picHeight - 60, 10, 15);
                    RectangleF rectF2 = new RectangleF(25, picHeight - 60, 30, 15);
                    RectangleF rectF3 = new RectangleF(40, picHeight - 60, 30, 15);
                    RectangleF rectF4 = new RectangleF(55, picHeight - 60, 20, 15);
                    if (xAction == 1)
                        g.DrawString(textF, font1, Brushes.SpringGreen, rectF1);
                    else if (xAction == -1)
                        g.DrawString(textB, font1, Brushes.SpringGreen, rectF1);

                    if (yAction == 1)
                        g.DrawString(textR, font1, Brushes.SpringGreen, rectF2);
                    else if (yAction == -1)
                        g.DrawString(textL, font1, Brushes.SpringGreen, rectF2);

                    if (zAction == 1)
                        g.DrawString(textU, font1, Brushes.SpringGreen, rectF3);
                    else if (zAction == -1)
                        g.DrawString(textD, font1, Brushes.SpringGreen, rectF3);

                    if (rotAction == 1)
                        g.DrawString(textRotL, font1, Brushes.SpringGreen, rectF4);
                    else if (rotAction == -1)
                        g.DrawString(textRotR, font1, Brushes.SpringGreen, rectF4);
                }

                
                //g.DrawCurve(pen, curvePoints);
                g.Dispose();
                
                pbo.Image = bitmap2;
                
                //bitmap2.Dispose();
                pen.Dispose();
                redPen.Dispose();
            }
        }

        private async void upload_firebase(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            byte[] a = ms.GetBuffer();

            string output = Convert.ToBase64String(a);

            var data = new Image_Model
            {
                Img = output
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Image/", data);
        }

        private async void get_res_firebase()
        {
            FirebaseResponse response = await client.GetTaskAsync("Label/img0/category0");
            Category res = response.ResultAs<Category>();
            obj1 = res.label;
            xpos1 = Math.Abs(res.x);
            ypos1 = Math.Abs(res.y);
            w1 = Math.Abs(res.w);
            h1 = Math.Abs(res.h);

            FirebaseResponse response2 = await client.GetTaskAsync("Label/img0/category1");
            Category res2 = response.ResultAs<Category>();
            obj2 = res2.label;
            xpos2 = Math.Abs(res2.x);
            ypos2 = Math.Abs(res2.y);
            w2 = Math.Abs(res2.w);
            h2 = Math.Abs(res2.h);

            FirebaseResponse response3 = await client.GetTaskAsync("Label/img0/category2");
            Category res3 = response.ResultAs<Category>();
            obj3 = res3.label;
            xpos3 = Math.Abs(res3.x);
            ypos3 = Math.Abs(res3.y);
            w3 = Math.Abs(res3.w);
            h3 = Math.Abs(res3.h);

            FirebaseResponse response4 = await client.GetTaskAsync("Label/img0/category3");
            Category res4 = response.ResultAs<Category>();
            obj4 = res4.label;
            xpos4 = Math.Abs(res4.x);
            ypos4 = Math.Abs(res4.y);
            w4 = Math.Abs(res4.w);
            h4 = Math.Abs(res4.h);

            FirebaseResponse response5 = await client.GetTaskAsync("Label/img0/category4");
            Category res5 = response.ResultAs<Category>();
            obj5 = res5.label;
            xpos5 = Math.Abs(res5.x);
            ypos5 = Math.Abs(res5.y);
            w5 = Math.Abs(res5.w);
            h5 = Math.Abs(res5.h);
        }

        private String UnicodeString(String text)
        {
            return text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bitmap != null)
                bitmap.Dispose();
            if (pbo.Image != null)
                this.Invoke(new MethodInvoker(delegate () { pbo.Image.Dispose(); }));
            if (cam != null && cam.IsRunning == true)
                cam.Stop();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (cam.IsRunning == true)
            {
                cam.NewFrame -= Handle_New_Frame;
                cam.SignalToStop();
                cam = null;
            }
            BtnStart.Show();
            BtnStop.Hide();
        }


        private void cbo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.W)
                BtnUp.BackColor = System.Drawing.Color.Red;
            else if (e.KeyData == Keys.S)
                BtnDown.BackColor = System.Drawing.Color.Red;

            if (e.KeyData == Keys.A)
                BtnLeft.BackColor = System.Drawing.Color.Red;
            else if (e.KeyData == Keys.D)
                BtnRight.BackColor = System.Drawing.Color.Red;

            if (e.KeyData == Keys.Q)
                BtnLeftTurn.BackColor = System.Drawing.Color.Red;
            else if (e.KeyData == Keys.E)
                BtnRightTurn.BackColor = System.Drawing.Color.Red;

            if (e.KeyData == Keys.U)
                BtnAscend.BackColor = System.Drawing.Color.Red;
            else if (e.KeyData == Keys.I)
                BtnDescend.BackColor = System.Drawing.Color.Red;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.W)
            {
                BtnUp.BackColor = System.Drawing.Color.Yellow;
                txtX.Text = "None";
                txtSig.Text = "None";
                xAction = 0;
            } else if (e.KeyData == Keys.S)
            {
                BtnDown.BackColor = System.Drawing.Color.Yellow;
                txtX.Text = "None";
                txtSig.Text = "None";
                xAction = 0;
            }

            if (e.KeyData == Keys.A)
            {
                BtnLeft.BackColor = System.Drawing.Color.Yellow;
                txtY.Text = "None";
                txtSig.Text = "None";
                yAction = 0;
            } else if (e.KeyData == Keys.D)
            {
                BtnRight.BackColor = System.Drawing.Color.Yellow;
                txtY.Text = "None";
                txtSig.Text = "None";
                yAction = 0;
            }

            if (e.KeyData == Keys.Q)
            {
                BtnLeftTurn.BackColor = System.Drawing.Color.Yellow;
                txtRot.Text = "None";
                txtSig.Text = "None";
                rotAction = 0;
            } else if (e.KeyData == Keys.E)
            {
                BtnRightTurn.BackColor = System.Drawing.Color.Yellow;
                txtRot.Text = "None";
                txtSig.Text = "None";
                rotAction = 0;
            }

            if (e.KeyData == Keys.U)
            {
                BtnAscend.BackColor = System.Drawing.Color.Yellow;
                txtZ.Text = "None";
                txtSig.Text = "None";
                zAction = 0;
            } else if (e.KeyData == Keys.I)
            {
                BtnDescend.BackColor = System.Drawing.Color.Yellow;
                txtZ.Text = "None";
                txtSig.Text = "None";
                zAction = 0;
            }
        }

        private void cbMapProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            MainMap.MapProvider = (GMapProvider)cbMapProviders.SelectedItem;
            MainMap.MaxZoom = 19;
            MainMap.Invalidate();

            Cursor = Cursors.Default;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int received;
            byte[] motor_general;
            TimeSpan ts;
            int elapsedTime;
            if (e.KeyChar == 'w')
            {
                txtX.Text = "Forward";
                motor_general = new Byte[] { 255, 127,127,0x00,0x00, 0x00, 0x00, 0b00110000, 253 };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }
                    
                    
                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    
                    txtTimeDelay.Text = elapsedTime.ToString();
                    // write to xlsx logs
                    action_counter += 1;


                    xAction = 1;
                }
                
            }
            else if (e.KeyChar == 's')
            {
                txtX.Text = "Backward";

                motor_general = new byte[] { 255, 127, 127, 0x00, 0x00, 0x00, 0x00, 0b00000000, 253 };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);
                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();

                    txtTimeDelay.Text = elapsedTime.ToString();
                    // write to xlsx logs
                    action_counter += 1;


                    xAction = 1;
                }
                
            }

            if (e.KeyChar == 'a')
            {
                txtY.Text = "Left";

                motor_general = new byte[] { 0xFF, 0x00, 0x00, 240, 240, 0x00, 0x00, 0b00001000, 251 };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    yAction = -1;
                }
                
            }
            else if (e.KeyChar == 'd')
            {
                txtY.Text = "Right";

                motor_general = new byte[] { 0xFF, 0x00, 0x00, 240, 240, 0x00, 0x00, 0b00001000, 251 };

                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }


                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    yAction = 1;
                }
                
            }


            if (e.KeyChar == 'q')
            {
                txtRot.Text = "Turn Left";
                motor_general = new byte[] { 0x80, 0x80, 0x00, 0x00, 0x00, 0x00, 0x40 };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 7);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    rotAction = 1;
                }
                
            }
            else if (e.KeyChar == 'e')
            {
                txtRot.Text = "Turn Right";
                motor_general = new byte[] { 0x80, 0x80, 0x00, 0x00, 0x00, 0x00, 0x80 };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    rotAction = -1;
                }
                
            }


            if (e.KeyChar == 'u')
            {
                txtZ.Text = "Ascend";
                motor_general = new byte[] { 0xFF, 0x00, 0x00, 240, 240, 0x00, 0x00, 0b00001100, 0xFE };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);
                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    zAction = 1;
                }
                
            }
            else if (e.KeyChar == 'i')
            {
                txtZ.Text = "Descend";
                motor_general = new byte[] { 0xFF, 0x00, 0x00, 240, 240, 0x00, 0x00, 0b00000000, 0xFE };
                if (isConnected)
                {
                    stopWatch.Start();
                    comport.Write(motor_general, 0, 9);

                    try
                    {
                        received = comport.ReadByte();
                        txtSig.Text = received.ToString();
                    }
                    catch (TimeoutException)
                    {
                        txtSig.Text = "Timeout";
                    }

                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    elapsedTime = ts.Milliseconds;
                    stopWatch.Reset();
                    txtTimeDelay.Text = elapsedTime.ToString();
                    action_counter += 1;
                    zAction = -1;
                }
                
            }
        }

        private void BtnConnect_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isConnected)
            {
                isConnected = true;
                comport.Open();
                BtnConnect.Text = "Disconnect";
                BtnConnect.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                isConnected = false;
                comport.Close();
                BtnConnect.Text = "Connect to PC";
                BtnConnect.BackColor = System.Drawing.Color.Silver;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            button3.BackColor = System.Drawing.Color.LightGreen;
        }

    }
}
