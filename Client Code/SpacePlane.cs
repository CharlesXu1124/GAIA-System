using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Automation.Peers;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using Windows.Phone.Notification.Management;
using Windows.ApplicationModel.Contacts;

namespace Blimp_GCS
{

    // class for object space plane, randomly flies over US at very high speed
    class SpacePlane
    {
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public Double speed { get; set; }
        private Random rng;
        public Double heading { get; set; }

        private Bitmap plane_label;
        public bool target_locked { get; set; }
        private Double turning_force;

        public Double vLat;
        public Double vLon;

        public Double missileLat;
        public Double missileLon;
        public Double missileHeading;
        
        public SpacePlane(Double lat, Double lon, Double s, Double h)
        {
            
            latitude = lat;
            longitude = lon;
            speed = s;
            heading = h;
            using (var plane_image = Blimp_GCS.Properties.Resources.spaceplane)
            {
                plane_label = new Bitmap(plane_image, new Size(60, 40));
            }
            target_locked = false;
            rng = new Random();
        }

        // method for randomly flying
        public void updatePath(Double targetLat, Double targetLon)
        {
            
            if (target_locked)
            {
                Double headingDiff = (Math.Atan2(latitude - targetLat, targetLon - longitude)) / Math.PI * 180 - heading;
                
                if (headingDiff * 100 > 0)
                {
                    turning_force = 50;
                } else if (headingDiff * 100 < 0)
                {
                    turning_force = -50;
                } else
                {
                    turning_force = 0;
                }
                Console.WriteLine(heading);
                Console.WriteLine(Math.Atan2(latitude - targetLat, targetLon - longitude));
                Console.WriteLine(headingDiff);
                Console.WriteLine($"turning_force is  {turning_force}");
            } else
            {
                // generate a random turning force between 20 to 50
                turning_force = rng.Next(20, 50);
            }
            

            
            Double center_lat;
            Double center_lon;
            Double angle;
            Double r = Math.Pow(speed, 2) / Math.Abs(turning_force);
            if (turning_force > 0)
            {
                // turn right
                // calculate the circle center
                center_lon = longitude - r * Math.Sin(heading * Math.PI / 180);
                center_lat = latitude - r * Math.Cos(heading * Math.PI / 180);
                angle = 0.2 * speed / r; // in radians
                Console.WriteLine($"Angle is {angle}");
                // increment the heading
                heading += angle;
                // update the coordinate of the space plane
                longitude = center_lon + r * Math.Sin(heading * Math.PI / 180);
                latitude = center_lat + r * Math.Cos(heading * Math.PI / 180);

            } else if (turning_force < 0)
            {
                // turn left
                // calculate the circle center
                center_lon = longitude + r * Math.Sin(heading * Math.PI / 180);
                center_lat = latitude + r * Math.Cos(heading * Math.PI / 180);
                angle = 0.2 * speed / r;
                // increment the heading
                heading -= angle;
                // update the coordinate of the space plane
                longitude = center_lon - r * Math.Sin(heading * Math.PI / 180);
                latitude = center_lat - r * Math.Cos(heading * Math.PI / 180);

            } else
            {
                latitude -= 0.2 / 25 * speed * Math.Sin(heading * Math.PI / 180);
                longitude += 0.2 / 25 * speed * Math.Cos(heading * Math.PI / 180);
                vLat = 0.2 / 250 * speed * Math.Sin(heading * Math.PI / 180);
                vLon = 0.2 / 250 * speed * Math.Cos(heading * Math.PI / 180);
                
            }
        }



        // stackoverflow
        // https://stackoverflow.com/questions/5172906/rotating-graphics
        private Bitmap RotateBitmap(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }


        public GMarkerGoogle getMarker()
        {
            Bitmap rotated_plane = RotateBitmap(plane_label, (float)heading);
            return new GMarkerGoogle(
                    new PointLatLng(latitude, longitude),
                    rotated_plane);
        }

        public void lockTarget()
        {
            target_locked = true;
        }

        public void unlockTarget()
        {
            target_locked = false;
        }

        public void launch()
        {
            // launch the missile and unlock the target
            missileLat = latitude;
            missileLon = longitude;
            missileHeading = heading;
        }

        public GMarkerGoogle getMissileMarker()
        {
            Bitmap missileLabel = Properties.Resources.missile;
            Bitmap resizedMissle = new Bitmap(missileLabel, new Size(50, 50));
            Bitmap rotatedMissile = RotateBitmap(resizedMissle, (float)heading + 90);
            return new GMarkerGoogle(
                    new PointLatLng(missileLat, missileLon),
                    rotatedMissile);
        }

        public GMarkerGoogle updateMissileMarker(Double updateLat, Double updateLon)
        {
            Bitmap missileLabel = Properties.Resources.missile;
            Bitmap resizedMissle = new Bitmap(missileLabel, new Size(30, 30));
            Bitmap rotatedMissile = RotateBitmap(resizedMissle, (float)heading + 90);
            return new GMarkerGoogle(
                    new PointLatLng(updateLat, updateLon),
                    rotatedMissile);
        }


    }
}
