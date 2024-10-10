J'ai modelisé un systeme qui permet de modeliser des listes restrictives qui contient des valeurs dans un model de base de données :
le schema de base de données est :
CREATE TABLE [dbo].[RestrictionList](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RestrictionList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 

CREATE TABLE [dbo].[RestrictionValue](
	[Id] [uniqueidentifier] NOT NULL,
	[RestrictionListId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_RestrictionValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)

 le model c# associé est  :
public class RestrictionList
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<RestrictionValue> RestrictionValues { get; set; }   
}

 public class RestrictionValue 
 {
     public Guid Id { get; set; }
     public Guid RestrictionListId { get; set; }
     public string Value { get; set; }
 }

 avec dapper version  .net 8, j'aimerai que tu créer une methode  public async Task<IEnumerable<RestrictionList>> GetAllAsync() 
 qui remplisse les listes et leurs valuers
