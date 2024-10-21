bonjour , 
j'ai une classe nommée FingerPrint qui est enregistrée en base de données avec le data model c# suivant :

 public class FingerPrint
 {
     public Guid Id { get; set; }
     public string FingerPayload { get; set; }
     public byte FingerPrintTypeId { get; set; }
 
     public string? Version { get; set; } 
     public string? TimeZone { get; set; } 
     public string? Languages { get; set; } 
     public string? DeviceSystemName { get; set; } 
     public string? ScreenSize { get; set; }


     //MobileFingerPrint properties 
     public string? HardwareID { get; set; }

     public IEnumerable<GeoLocationInfo>? GeoLocationInfos;

     public string? DeviceModel { get; set; }
     public bool MultitaskingSupported { get; set; }
     public string? DeviceName { get; set; } 
     public string? DeviceSystemVersion { get; set; }

     //Wifinetworksdata
     public string? WiFiNetworksData_BBSID { get; set; }
     public string? WiFiNetworksData_SignalStrength { get; set; }
     public string? WiFiNetworksData_SSID { get; set; }



     public string? RSA_ApplicationKey { get; set; }
     public string? MCC { get; set; }
     public string? MNC { get; set; }
     public string? OS_ID { get; set; } 
     public int? Compromised { get; set; }
     public int? Emulator { get; set; }

     //Batteryinfo
     public string? BatteryInfo_Status { get; set; }
     public int? BatteryInfo_Plugged { get; set; }
     public int? BatteryInfo_Level { get; set; }
     public int? BatteryInfo_Voltage { get; set; }
     public string? BatteryInfo_Technology { get; set; }
     public string? BatteryInfo_Health { get; set; }

     public string? Timestamp { get; set; }
     public string? AdvertiserId { get; set; }
     public string? WiFiMacAddress { get; set; }
     public string? CellTowerId { get; set; }
     public string? LocationAreaCode { get; set; }


     //WebFingerPrint properties
     public string? PmFpua { get; set; } 
     public string? PmFpjv { get; set; }
     public string? PmFpco { get; set; }
     public string? PmFpasw { get; set; }
     public string? PmFpan { get; set; }
     public string? PmFpacn { get; set; }
     public string? PmFpol { get; set; }
     public string? PmFposp { get; set; }
     public string? PmFpup { get; set; }
     public string? PmFpsaw { get; set; }
     public string? PmFpspd { get; set; }
     public string? PmFpsbd { get; set; }
     public string? PmFpsdx { get; set; }
     public string? PmFpsdy { get; set; }
     public string? PmFpslx { get; set; }
     public string? PmFpsly { get; set; }
     public string? PmFpsfse { get; set; }
     public string? PmFpsui { get; set; } 
     public string? PmBrmjv { get; set; }
     public string? PmBr { get; set; }
     public string? PmInpt { get; set; }
     public string? PmExpt { get; set; }

 }
 
 avec :
 
  public class GeoLocationInfo
 {
     public Guid Id { get; set; }
     public Guid FingerPrintId { get; set; }
     public string? Longitude { get; set; }
     public string? Latitude { get; set; }
     public string? HorizontalAccuracy { get; set; }
     public string? Timestamp { get; set; }
     public string? Status { get; set; }
 }
 
 
 pourrais tu ecrire grace a la libraie dapper la methode GetById , qui retournera le FingerPrint en remplissant la liste IEnumerable<GeoLocationInfo>? grace a dapper ?
 La jointure doit etre LEFT JOIN car il est possible que la classe FingerPrint ne possedent pas de GeoLocationInfo.
 
 Voici le debut de la methode:
 
   public async Task<FingerPrint?> GetByIdAsync(Guid fingerPrintId)
  {
      using (var connection = new SqlConnection(connectionString))
      {
          var p = new DynamicParameters();
          p.Add("@Id", id);

          var sql = @"SELECT...";
      }
  }
