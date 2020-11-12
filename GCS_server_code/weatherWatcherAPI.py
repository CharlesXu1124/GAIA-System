import requests
from datetime import datetime


class weatherWatcherAPI:

    def __init__(self):
        pass

    def get_weather(self, latitude: float, longitude: float):
        """
        Returns an array of sattalite visibilities
        :param latitude:
        :param longitude:
        :return:
        """
        url = "https://api.openweathermap.org/data/2.5/weather?"

        payload = url + "lat=" + str(latitude) + "&lon=" + str(longitude) + "&appid=YOUR APPID"
        response = requests.get(payload)
        return response.json()

# sample usage:
# ww = weatherWatcherAPI()
# res = ww.get_weather(33.0166666, -116.6833306)
