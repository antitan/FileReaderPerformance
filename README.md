Bonjour, 
Grace a ton aide précieuse j'ai crée 2 classes qui permettent de stocker les caractéristiques d'un telephone mobile et d'un appareil qui utilise un navigateur web.
Voici la classe MobileFingerPrint qui recupère les caractéristiques d'un telephone mobile :

public class MobileFingerPrint
{
    public string TimeZone { get; set; }
    public string HardwareID { get; set; }
    public Geolocationinfo[] GeoLocationInfo { get; set; }
    public string DeviceModel { get; set; }
    public bool MultitaskingSupported { get; set; }
    public string DeviceName { get; set; }
    public string DeviceSystemName { get; set; }
    public string DeviceSystemVersion { get; set; }
    public string Languages { get; set; }
    public Wifinetworksdata WiFiNetworksData { get; set; }
    public string ScreenSize { get; set; }
    public string RSA_ApplicationKey { get; set; }
    public string MCC { get; set; }
    public string MNC { get; set; }
    public string OS_ID { get; set; }
    public string SDK_VERSION { get; set; }
    public int Compromised { get; set; }
    public int Emulator { get; set; }
    public Batteryinfo BatteryInfo { get; set; }
    public DateTime TIMESTAMP { get; set; }
    public string AdvertiserId { get; set; }
    public string WiFiMacAddress { get; set; }
    public string CellTowerId { get; set; }
    public string LocationAreaCode { get; set; }
}

public class Wifinetworksdata
{
    public string BBSID { get; set; }
    public string SignalStrength { get; set; }
    public string SSID { get; set; }
}

public class Batteryinfo
{
    public string Status { get; set; }
    public int Plugged { get; set; }
    public int Level { get; set; }
    public int Voltage { get; set; }
    public string Technology { get; set; }
    public string Health { get; set; }
}

public class Geolocationinfo
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    public string HorizontalAccuracy { get; set; }
    public string Timestamp { get; set; }
    public string Status { get; set; }
}

et j'ai donc crée la classe MobileFingerPrint pour pouvoir extraire les informations de ce type de chaine de caractere formatté en json :

{
        "TimeZone": "2.0",
        "HardwareID": "1f002923-ef5c-4631-9904-1c1f07184a13",
        "GeoLocationInfo": [
            {
                "Longitude": "-122.083922",
                "Latitude": "37.4220936",
                "HorizontalAccuracy": "600",
                "Timestamp": "1729065070697",
                "Status": "0"
            }
        ],
        "DeviceModel": "sdk_gphone64_arm64",
        "MultitaskingSupported": true,
        "DeviceName": "emu64a",
        "DeviceSystemName": "Android",
        "DeviceSystemVersion": "34",
        "Languages": "en",
        "WiFiNetworksData": {
            "BBSID": "00:13:10:85:fe:01",
            "SignalStrength": "-50",
            "SSID": "AndroidWifi"
        },
        "BatteryInfo": {
            "Status": "BATTERY_STATUS_NOT_CHARGING",
            "Plugged": 0,
            "Level": 100,
            "Voltage": 5,
            "Technology": "Li-ion",
            "Health": "GOOD"
        },
        "ScreenSize": "1080x2209",
        "RSA_ApplicationKey": "7A2570399334778C15998F82C669EF69",
        "MCC": "310",
        "MNC": "260",
        "OS_ID": "edd3eb78e5529a55",
        "SDK_VERSION": "4.4.0",
        "Compromised": 0,
        "Emulator": 0,
      
        "TIMESTAMP": "0001-01-01T00:00:00",
        "AdvertiserId": "68c57157-f4e9-42d2-8e1c-5ab5ef5a315d",
        "WiFiMacAddress": "2:15:b2:0:0:0",
        "CellTowerId": "91",
        "LocationAreaCode": "3",
        "DeviceSystemName" : "Android",
        "DeviceName" : "Samsung galaxy s5",
      
      }

tu j'ai donc crée la methode de transfomration suivante :

public async Task<MobileFingerPrint> ExtractFingerPrint(string fingerPrint)
{
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fingerPrint));
    return await JsonSerializer.DeserializeAsync<MobileFingerPrint>(stream, options);
}




Voici la classe WebFingerPrint qui recupère les caractéristiques d'un appareil utilisant un navigateur :

public class WebFingerPrint
{
    public string Version { get; set; }
    public string PmFpua { get; set; }
    public string PmFpsc { get; set; }
    public string PmFptz { get; set; }
    public string PmFpln { get; set; }
    public string PmFpjv { get; set; }
    public string PmFpco { get; set; }
    public string PmFpasw { get; set; }
    public string PmFpan { get; set; }
    public string PmFpacn { get; set; }
    public string PmFpol { get; set; }
    public string PmFposp { get; set; }
    public string PmFpup { get; set; }
    public string PmFpsaw { get; set; }
    public string PmFpspd { get; set; }
    public string PmFpsbd { get; set; }
    public string PmFpsdx { get; set; }
    public string PmFpsdy { get; set; }
    public string PmFpslx { get; set; }
    public string PmFpsly { get; set; }
    public string PmFpsfse { get; set; }
    public string PmFpsui { get; set; }
    public string PmOs { get; set; }
    public string PmBrmjv { get; set; }
    public string PmBr { get; set; }
    public string PmInpt { get; set; }
    public string PmExpt { get; set; }
}

et j'ai donc crée la classe WebFingerPrint  pour pouvoir extraire les informations de ce type de chaine de caractere :

"version%3D3%2E7%2E1%5F1%26pm%5Ffpua%3Dmozilla%2F5%2E0%20%28iphone%3B%20cpu%20iphone%20os%2013%5F5%5F1%20like%20mac%20os%20x%29%20applewebkit%2F605%2E1%2E15%20%28khtml%2C%20like%20gecko%29%20version%2F13%2E1%2E1%20mobile%2F15e148%20safari%2F604%2E1%7C5%2E0%20%28iPhone%3B%20CPU%20iPhone%20OS%2013%5F5%5F1%20like%20Mac%20OS%20X%29%20AppleWebKit%2F605%2E1%2E15%20%28KHTML%2C%20like%20Gecko%29%20Version%2F13%2E1%2E1%20Mobile%2F15E148%20Safari%2F604%2E1%7CiPhone%26pm%5Ffpsc%3D32%7C414%7C736%7C736%26pm%5Ffpsw%3D%26pm%5Ffptz%3D1%26pm%5Ffpln%3Dlang%3Den%2Dgb%7Csyslang%3D%7Cuserlang%3D%26pm%5Ffpjv%3D0%26pm%5Ffpco%3D1%26pm%5Ffpasw%3D%26pm%5Ffpan%3DNetscape%26pm%5Ffpacn%3DMozilla%26pm%5Ffpol%3Dtrue%26pm%5Ffposp%3D%26pm%5Ffpup%3D%26pm%5Ffpsaw%3D414%26pm%5Ffpspd%3D32%26pm%5Ffpsbd%3D%26pm%5Ffpsdx%3D%26pm%5Ffpsdy%3D%26pm%5Ffpslx%3D%26pm%5Ffpsly%3D%26pm%5Ffpsfse%3D%26pm%5Ffpsui%3D%26pm%5Fos%3DiPhone%2FiPod%26pm%5Fbrmjv%3D13%26pm%5Fbr%3DSafari%26pm%5Finpt%3D%26pm%5Fexpt%3D"

et tu m'a aidé a ecrire cette methode de transformation :

public WebFingerPrint StringToWebFingerPrint(string fingerPrint)
{
    var WebFingerPrint = new WebFingerPrint();
    var parameters = System.Web.HttpUtility.UrlDecode(query).Split('&');

    foreach (var param in parameters)
    {
        var keyValue = param.Split('=');
        if (keyValue.Length == 2)
        {
            var key = keyValue[0];
            var value = keyValue[1];

            switch (key)
            {
                case "version": WebFingerPrint.Version = value; break;
                case "pm_fpua": WebFingerPrint.PmFpua = value; break;
                case "pm_fpsc": WebFingerPrint.PmFpsc = value; break;
                case "pm_fptz": WebFingerPrint.PmFptz = value; break;
                case "pm_fpln": WebFingerPrint.PmFpln = value; break;
                case "pm_fpjv": WebFingerPrint.PmFpjv = value; break;
                case "pm_fpco": WebFingerPrint.PmFpco = value; break;
                case "pm_fpasw": WebFingerPrint.PmFpasw = value; break;
                case "pm_fpan": WebFingerPrint.PmFpan = value; break;
                case "pm_fpacn": WebFingerPrint.PmFpacn = value; break;
                case "pm_fpol": WebFingerPrint.PmFpol = value; break;
                case "pm_fposp": WebFingerPrint.PmFposp = value; break;
                case "pm_fpup": WebFingerPrint.PmFpup = value; break;
                case "pm_fpsaw": WebFingerPrint.PmFpsaw = value; break;
                case "pm_fpspd": WebFingerPrint.PmFpspd = value; break;
                case "pm_fpsbd": WebFingerPrint.PmFpsbd = value; break;
                case "pm_fpsdx": WebFingerPrint.PmFpsdx = value; break;
                case "pm_fpsdy": WebFingerPrint.PmFpsdy = value; break;
                case "pm_fpslx": WebFingerPrint.PmFpslx = value; break;
                case "pm_fpsly": WebFingerPrint.PmFpsly = value; break;
                case "pm_fpsfse": WebFingerPrint.PmFpsfse = value; break;
                case "pm_fpsui": WebFingerPrint.PmFpsui = value; break;
                case "pm_os": WebFingerPrint.PmOs = value; break;
                case "pm_brmjv": WebFingerPrint.PmBrmjv = value; break;
                case "pm_br": WebFingerPrint.PmBr = value; break;
                case "pm_inpt": WebFingerPrint.PmInpt = value; break;
                case "pm_expt": WebFingerPrint.PmExpt = value; break;
                default: break;
            }
        }
    }

    return WebFingerPrint;
}

J'aurai besoin de ton aide pour une tache :
La future étape de mon developpement va etre de stocker en base de données les informations de l'objet WebFingerPrint et de l'objet MobileFingerPrint.
Je me demandais si tu arrivais a voir des similitudes de certaines propriétés entre les informations du payload WebFingerPrint :
"version%3D3%2E7%2E1%5F1%26pm%5Ffpua%3Dmozilla%2F5%2E0%20%28iphone%3B%20cpu%20iphone%20os%2013%5F5%5F1%20like%20mac%20os%20x%29%20applewebkit%2F605%2E1%2E15%20%28khtml%2C%20like%20gecko%29%20version%2F13%2E1%2E1%20mobile%2F15e148%20safari%2F604%2E1%7C5%2E0%20%28iPhone%3B%20CPU%20iPhone%20OS%2013%5F5%5F1%20like%20Mac%20OS%20X%29%20AppleWebKit%2F605%2E1%2E15%20%28KHTML%2C%20like%20Gecko%29%20Version%2F13%2E1%2E1%20Mobile%2F15E148%20Safari%2F604%2E1%7CiPhone%26pm%5Ffpsc%3D32%7C414%7C736%7C736%26pm%5Ffpsw%3D%26pm%5Ffptz%3D1%26pm%5Ffpln%3Dlang%3Den%2Dgb%7Csyslang%3D%7Cuserlang%3D%26pm%5Ffpjv%3D0%26pm%5Ffpco%3D1%26pm%5Ffpasw%3D%26pm%5Ffpan%3DNetscape%26pm%5Ffpacn%3DMozilla%26pm%5Ffpol%3Dtrue%26pm%5Ffposp%3D%26pm%5Ffpup%3D%26pm%5Ffpsaw%3D414%26pm%5Ffpspd%3D32%26pm%5Ffpsbd%3D%26pm%5Ffpsdx%3D%26pm%5Ffpsdy%3D%26pm%5Ffpslx%3D%26pm%5Ffpsly%3D%26pm%5Ffpsfse%3D%26pm%5Ffpsui%3D%26pm%5Fos%3DiPhone%2FiPod%26pm%5Fbrmjv%3D13%26pm%5Fbr%3DSafari%26pm%5Finpt%3D%26pm%5Fexpt%3D"
et du payload MobileFingerPrint:

{
        "TimeZone": "2.0",
        "HardwareID": "1f002923-ef5c-4631-9904-1c1f07184a13",
        "GeoLocationInfo": [
            {
                "Longitude": "-122.083922",
                "Latitude": "37.4220936",
                "HorizontalAccuracy": "600",
                "Timestamp": "1729065070697",
                "Status": "0"
            }
        ],
        "DeviceModel": "sdk_gphone64_arm64",
        "MultitaskingSupported": true,
        "DeviceName": "emu64a",
        "DeviceSystemName": "Android",
        "DeviceSystemVersion": "34",
        "Languages": "en",
        "WiFiNetworksData": {
            "BBSID": "00:13:10:85:fe:01",
            "SignalStrength": "-50",
            "SSID": "AndroidWifi"
        },
        "BatteryInfo": {
            "Status": "BATTERY_STATUS_NOT_CHARGING",
            "Plugged": 0,
            "Level": 100,
            "Voltage": 5,
            "Technology": "Li-ion",
            "Health": "GOOD"
        },
        "ScreenSize": "1080x2209",
        "RSA_ApplicationKey": "7A2570399334778C15998F82C669EF69",
        "MCC": "310",
        "MNC": "260",
        "OS_ID": "edd3eb78e5529a55",
        "SDK_VERSION": "4.4.0",
        "Compromised": 0,
        "Emulator": 0,
      
        "TIMESTAMP": "0001-01-01T00:00:00",
        "AdvertiserId": "68c57157-f4e9-42d2-8e1c-5ab5ef5a315d",
        "WiFiMacAddress": "2:15:b2:0:0:0",
        "CellTowerId": "91",
        "LocationAreaCode": "3",
        "DeviceSystemName" : "Android",
        "DeviceName" : "Samsung galaxy s5",
      
      }
	  
Ceci pour stocker les informations qu'une seule fois en base de données.
Si tu trouves des similarités, peux tu m'indiquer de cette facon :
par exemple : 
MobileFingerPrint.SDK_VERSION = WebFingerPrint.Version
