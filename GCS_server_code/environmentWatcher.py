from weatherWatcherAPI import weatherWatcherAPI
from SatWatcherAPI import SatWatcherAPI
from flask import Flask
from flask import request
import json


app = Flask(__name__)


@app.route('/')
def index():
    return 'invalid call'


@app.route('/getWeather', methods=['GET', 'POST'])
def getWeather():
    ww = weatherWatcherAPI()
    data = request.data
    loaded_json = json.loads(data)
    body = loaded_json
    print(body, "--weather query received")
    
    lat = float(body['latitude'])
    lon = float(body['longitude'])
    res = ww.get_weather(lat, lon)
    weather = res['weather'][0]['description']
    return weather

@app.route('/getSatellite', methods=['GET', 'POST'])
def getSatCount():
    sc = SatWatcherAPI()
    data = request.data
    loaded_json = json.loads(data)
    body = loaded_json
    print(body, "--satellite query received")

    latitude = float(body['latitude'])
    longitude = float(body['longitude'])
    elevation = float(body['elevation'])

    res = sc.get_sat_visibility_data_on_coordinates(latitude,longitude, elevation)
    satCount = len(res['above'])
    return str(satCount)

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
