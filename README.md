Bonjour,
j'ai une liste d'objet ResultRule dont voici la definition :
class ResultRule 
{
	public int Id {get;set;}
	public bool isSuccess {get;set;}
	public string LambdaExpression {get;set;}
	public ResultType ExpectedResult {get;set} 
}

public enum ResultType
{
	Challenge=1,
	Allow=2,
}

Ceci represente une liste d'objets d'etapes d'un workflow.

On itere sur la liste et voici les regles :  
Pour chaque element : si l'expression de la variable LambdaExpression s'est executée avec sucess ( que le resultat est true ), alors la valeur de isSuccess est egale à true.
Prenons le cas isSuccess == true :
si ExpectedResult == ResultType.Challenge => on arrete d'iterer sur la liste et on sort de la boucle
si ExpectedResult == ResultType.Allow => on continue a iterer sur le prochain element de la liste(si il y en a d'autres)
Pour le cas isSuccess == false :
si ExpectedResult == ResultType.Challenge => on continue a iterer sur le prochain element de la liste (si il y en a d'autres)
si ExpectedResult == ResultType.Allow =>  on arrete d'iterer sur la liste et on sort de la boucle

Pourrais-tu m'ecrire l'expression pour savoir si cette liste va iterer ssur tous ses elements sans etre interrompu par une sortie de boucle
