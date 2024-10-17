Bonjour, 

1 - Pourrais-tu me créer une classe C# appelée MobileFingerPrint qui merge les propriété des Class1, Class2 et  Class3 :

 public class Class1
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
     public Batteryinfo BatteryInfo { get; set; }
     public string ScreenSize { get; set; }
     public string RSA_ApplicationKey { get; set; }
     public string MCC { get; set; }
     public string MNC { get; set; }
     public string OS_ID { get; set; }
     public string SDK_VERSION { get; set; }
     public int Compromised { get; set; }
     public int Emulator { get; set; }
 }

 public class Class2
 {
     public string DeviceSystemVersion { get; set; }
     public string HardwareID { get; set; }
     public string ScreenSize { get; set; }
     public string Languages { get; set; }
     public bool MultitaskingSupported { get; set; }
     public string DeviceModel { get; set; }
     public string RSA_ApplicationKey { get; set; }
     public string TimeZone { get; set; }
     public Batteryinfo BatteryInfo { get; set; }
     public DateTime TIMESTAMP { get; set; }
     public Geolocationinfo[] GeoLocationInfo { get; set; }
     public int Emulator { get; set; }
     public string OS_ID { get; set; }
     public int Compromised { get; set; }
     public string DeviceSystemName { get; set; }
     public string DeviceName { get; set; }
     public string SDK_VERSION { get; set; }
 }

 public class Class3
 {
     public DateTime TIMESTAMP { get; set; }
     public string TimeZone { get; set; }
     public string HardwareID { get; set; }
     public string AdvertiserId { get; set; }
     public Geolocationinfo[] GeoLocationInfo { get; set; }
     public string DeviceModel { get; set; }
     public bool MultitaskingSupported { get; set; }
     public string DeviceName { get; set; }
     public string DeviceSystemName { get; set; }
     public string DeviceSystemVersion { get; set; }
     public string Languages { get; set; }
     public string WiFiMacAddress { get; set; }
     public Wifinetworksdata WiFiNetworksData { get; set; }
     public string CellTowerId { get; set; }
     public string LocationAreaCode { get; set; }
     public string ScreenSize { get; set; }
     public string RSA_ApplicationKey { get; set; }
     public string MCC { get; set; }
     public string MNC { get; set; }
     public string OS_ID { get; set; }
     public string SDK_VERSION { get; set; }
     public int Compromised { get; set; }
     public int Emulator { get; set; }
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

 2- Pourrais-tu me m'crire la methode ExtractFingerPrint qui prend en parametre une variable de type string appelée fingerPrint et qui retourne un object de type Task<MobileFingerPrint> en utilisant la méthode de deserialization de l'assemble Sytem.Text.Json
