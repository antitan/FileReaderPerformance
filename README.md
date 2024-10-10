

J'ai modelisé un systeme de worflows et ses differentes etapes (un workflow a 1 ou n rules) : 
le schema de base de données est :  

CREATE TABLE [dbo].[Workflow](
	[Id] [uniqueidentifier] NOT NULL,
	[ActivityTypeId] [tinyint] NOT NULL,
	[ExecutionOrder] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ActiveFrom] [date] NOT NULL,
	[ActiveTo] [date] NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdateOn] [datetime] NULL,
 CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)

CREATE TABLE [dbo].[Rule](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkFlowId] [uniqueidentifier] NOT NULL,
	[ExecutionOrder] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Expression] [nvarchar](max) NOT NULL,
	[ErrorMessage] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)

le model c# associé est :  

 public class Workflow 
 {
     public Guid Id { get; set; }
     public byte ActivityTypeId { get; set; }

     public short ExecutionOrder { get; set; }

     public string Name { get; set; }

     public DateTime ActiveFrom { get; set; }

     public DateTime? ActiveTo { get; set; }

     public string CreatedBy { get; set; }

     public DateTime CreatedOn { get; set; }

     public string UpdatedBy { get; set; }

     public DateTime? UpdatedOn { get; set; }

     public IList<Rule> Rules { get; set; }

 }
 et
  public class Rule  
 {
     public Guid Id { get; set; }
     public Guid WorkFlowId { get; set; }
     public short ExecutionOrder { get; set; }
     public string Name { get; set; }
     public string Description { get; set; }
     public string Expression { get; set; }
     public string ErrorMessage { get; set; }
     public string CreatedBy { get; set; }
     public DateTime CreatedOn { get; set; }
     public string UpdatedBy { get; set; }
     public DateTime? UpdatedOn { get; set; }

 }



De la meme facon que l'exemple precedent (sans splitOn), avec dapper version .net 8, j'aimerai que tu créer une methode public async Task<IEnumerable<WorkFlow>> GetAllAsync() qui remplisse la liste des workflow si ils sont actifs 
(la date du jour doit etre entre ActiveFrom et ActiveTo, (si ActiveTo est null , on ne tiens pas compte de ce critére et toutes les dates apres ActiveFrom sont valides ).
Les liste de Workflow doit etre triées par ExecutionOrder croissants et les regles de chaque workflow doivent etre triées par  ExecutionOrder croissant.
