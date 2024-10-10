

J'ai modelisé un systeme de gestions d'activités, pour le moment j'ai implémenté une entité nommmée "Activity" qui represente les attributs de bases d'une activité et une entité nommmée "PaymentActivity"   qui étend ce modèle avec des caractéristiques de payments.
le schema de base de données est :  

la table Activity :
CREATE TABLE [dbo].[Activity](
	[Id] [uniqueidentifier] NOT NULL,
	[ActivityTypeId] [tinyint] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Origin] [nvarchar](50) NOT NULL,
	[Channel] [nvarchar](50) NOT NULL,
	[Ip] [nvarchar](39) NOT NULL,
	[FingerPrint] [nvarchar](max) NOT NULL,
	[Result] [bit] NOT NULL,
	[ChallengeResult] [bit] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ActivityTypeId] ASC
)

la table ActivityType, dont la collonne Id est une clé etrangere dans la colonne ActivityTypeId de la table Activity :

CREATE TABLE [dbo].[ActivityType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[Label] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ActivityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)

et la table PaymentActivity :

CREATE TABLE [dbo].[PaymentActivity](
	[Id] [uniqueidentifier] NOT NULL,
	[ActivityTypeId] [tinyint] NOT NULL,
	[ActivityId] [uniqueidentifier] NOT NULL,
	[AccountNumber] [nvarchar](500) NOT NULL,
	[Bic] [nvarchar](200) NOT NULL,
	[PaymentType] [varchar](50) NOT NULL,
	[ExecutionDate] [datetime] NOT NULL,
	[Amount] [bigint] NOT NULL,
	[Currency] [char](3) NOT NULL,
	[CountryDestination] [char](2) NOT NULL,
	[SourceAccount] [varchar](500) NOT NULL,
	[IsNewBeneficiary] [bit] NOT NULL,
 CONSTRAINT [PK_PaymentActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)

le model c# associé est :  

 public class Activity 
 {
     public Guid Id { get; set; }
     public byte ActivityTypeId { get; set; }
     public string CreatedBy { get; set; }
     public DateTime CreatedOn { get; set; }
     public string Origin { get; set; }
     public byte Channel { get; set; }
     public string Ip { get; set; }
     public string FingerPrint { get; set; }
     public bool Result { get; set; }
     public bool? ChallengeResult { get; set; }
     public string UpdatedBy { get; set; }
     public DateTime? UpdatedOn { get; set; }
 }
 et
  public class PaymentActivity
  {
      public Guid Id { get; set; }

      public Guid ActivityId { get; set; }
      public byte ActivityTypeId { get; set; }
 
      public string AccountNumber { get; set; }
 

      public string Bic { get; set; }

      public byte PaymentTypeId { get; set; } 
      public DateTime ExecutionDate { get; set; }

      public long Amount { get; set; }

      public string Currency { get; set; }

      public string CountryDestination { get; set; } 
      public bool IsNewBeneficiary { get; set; }

      public string SourceAccount { get; set; }

      public Activity Activity { get; set; }  

  }



Avec dapper version .net 8, j'aimerai que tu créer une methode public async Task<IEnumerable<PaymentActivity>> GetPaymentActivitiesByUserAsync(string createdBy) , cela fitrera la colonne CreatedBy de la table Activity  et  remplira la liste des entites PaymentActivity et  chaque entité Activity associée.
 
Les liste des PaymentActivity doit etre triées par CreatedOn decroissants (les plus recentes en premiers) .
