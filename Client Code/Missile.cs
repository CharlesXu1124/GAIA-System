using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace Blimp_GCS
{
    class Missile
    {
        Double lat { get; set; }
        Double lon { get; set; }
        Double speed { get; set; }
        PointLatLng targetLocked { get; set; }
        bool missionAbort { get; set; }
        Double fuelRange { get; set; }
        bool targetReached { get; set; }

        public Missile(Double lat, Double lon, Double speed, PointLatLng targetLocked,
            bool missionAbort, Double fuelRange, bool targetReached)
        {
            this.lat = lat;
            this.lon = lon;
            this.speed = speed;
            this.targetLocked = targetLocked;
            this.missionAbort = missionAbort;
            this.fuelRange = fuelRange;
            this.targetReached = targetReached;
        }

    }
}
