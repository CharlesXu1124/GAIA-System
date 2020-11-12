import requests
from datetime import datetime


class SatWatcherAPI:

    def __init__(self):
        pass

    def get_sat_visibility_data_on_coordinates(self, latitude: float, longitude: float,
                                               elevation: float = 50.0, # default elevation: 50
                                               glonass: bool = True, gps: bool = True):
        """
        Returns an array of sattalite visibilities
        :param latitude:
        :param longitude:
        :param elevation:
        :param glonass:
        :param gps:
        :return:
        """

        url = "https://api.n2yo.com/rest/v1/satellite"
        
        if glonass:
            glonass = "YES"
        else:
            glonass = "NO"

        if gps:
            gps = "YES"
        else:
            gps = "NO"

        payload = url + "/above/" + str(latitude) + "/" + str(longitude) + "/0/" + str(elevation) + "/20/&apiKey=YOURAPIKEY"
        response = requests.get(payload)
        return response.json()

# Sample usage:
# ssp = SatWatcherAPI()
#
# ssp.get_sat_visibility_data_on_coordinates(1,2)
