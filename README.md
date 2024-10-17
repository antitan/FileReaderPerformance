bonjour,

1 - Peux tu me generer le script de creation de table sqlserver equivalente a la classe c# : 

 public class FingerPrintDetail
 {
     public Guid Id { get; set; }
     public string FingerPayload { get; set; }

     public string Version { get; set; }
     //MobileFingerPrint.TimeZone = WebFingerPrint.PmFptz
     public string TimeZone { get; set; }
     //MobileFingerPrint.Languages = Extracted from WebFingerPrint.PmFpln
     public string Languages { get; set; }
     //MobileFingerPrint.DeviceSystemName = WebFingerPrint.PmOs
     public string DeviceSystemName { get; set; }
     //MobileFingerPrint.ScreenSize = Constructed from WebFingerPrint.PmFpsc
     public string ScreenSize { get; set; }


     //MobileFingerPrint properties 
     public string HardwareID { get; set; }
     
     //GeoLocationInfo
     public string GeoLocationInfo_Longitude { get; set; }
     public string GeoLocationInfo_Latitude { get; set; }
     public string GeoLocationInfo_HorizontalAccuracy { get; set; }
     public string GeoLocationInfo_Timestamp { get; set; }
     public string GeoLocationInfo_Status { get; set; }


     public string DeviceModel { get; set; }
     public bool MultitaskingSupported { get; set; }
     public string DeviceName { get; set; } 
     public string DeviceSystemVersion { get; set; }

     //Wifinetworksdata
     public string WiFiNetworksData_BBSID { get; set; }
     public string WiFiNetworksData_SignalStrength { get; set; }
     public string WiFiNetworksData_SSID { get; set; }



     public string RSA_ApplicationKey { get; set; }
     public string MCC { get; set; }
     public string MNC { get; set; }
     public string OS_ID { get; set; } 
     public int Compromised { get; set; }
     public int Emulator { get; set; }

     //Batteryinfo
     public string BatteryInfo_Status { get; set; }
     public int? BatteryInfo_Plugged { get; set; }
     public int? BatteryInfo_Level { get; set; }
     public int? BatteryInfo_Voltage { get; set; }
     public string BatteryInfo_Technology { get; set; }
     public string BatteryInfo_Health { get; set; }

     public DateTime TIMESTAMP { get; set; }
     public string AdvertiserId { get; set; }
     public string WiFiMacAddress { get; set; }
     public string CellTowerId { get; set; }
     public string LocationAreaCode { get; set; }


     //WebFingerPrint properties
     public string PmFpua { get; set; } 
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
     public string PmBrmjv { get; set; }
     public string PmBr { get; set; }
     public string PmInpt { get; set; }
     public string PmExpt { get; set; }

 }
 
 2- Peux tu m'implementer la classe FingerPrintDetailRepository de cetter façon :
 
  public partial interface IFingerPrintDetailRepository
 { 
     Task<FingerPrintDetail?> GetByIdAsync(System.Guid id);
 
     Task<Guid> InsertAsync(FingerPrintDetail fingerPrintDetail);
 
     Task DeleteByIdAsync(System.Guid id);
 }

 /// <summary>
 /// Defines the <see cref="FingerPrintDetailRepository" />
 /// </summary>
 public class FingerPrintDetailRepository : IFingerPrintDetailRepository
 {
 
     private readonly string? connectionString;
 
     public FingerPrintDetailRepository(IConfiguration configuration)
     {
         connectionString = configuration.GetConnectionString("DefaultConnection");
     }
 
     public async Task<FingerPrintDetail?> GetByIdAsync(System.Guid id)
     {
         using (var connection = new SqlConnection(connectionString))
         {
             var p = new DynamicParameters();
             p.Add("@Id", id);

             var entity = await connection.QuerySingleOrDefaultAsync<FingerPrintDetail>("SELECT  /*file the field list*/  FROM [dbo].[FingerPrintDetail]  WHERE [Id] = @Id ", p);
             return entity;

         }
     }
 
     public async Task<Guid> InsertAsync(FingerPrintDetail fingerPrintDetail)
     {
         using (var connection = new SqlConnection(connectionString))
         {
             var p = new DynamicParameters();
             p.Add("@Id", fingerPrintDetail.Id); 
			 //add the rest

             var result = await connection.QuerySingleAsync<Guid>("INSERT INTO [dbo].[FingerPrintDetail] (/*file the fields list */)  OUTPUT INSERTED.Id VALUES ( /*file the properties */)", p);

             return result;
         }
     }
 
     public async Task DeleteByIdAsync(System.Guid id)
     {
         using (var connection = new SqlConnection(connectionString))
         {
             var p = new DynamicParameters();
             p.Add("@Id", id);

             await connection.ExecuteScalarAsync<int>("DELETE FROM [dbo].[FingerPrintDetail] WHERE [Id] = @Id", p);
         }
     }
 }

 
