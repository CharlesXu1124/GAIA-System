using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blimp_GCS
{
    class WeatherModel
    {
        [JsonProperty("latitude")]
        public Double latitude { get; set; }
        [JsonProperty("longitude")]
        public Double longitude { get; set; }


    }
}
