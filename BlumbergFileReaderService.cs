Voici le fonctionnement détaillée :
Lorsqu’une nouvelle requête web est envoyée au travers de la méthode Payment :
1)	On insert cette nouvelle requete en base. (table Activity + PaymentActivity) . Activity=null et ChallengeResult=null
2)	les règles sont remontées de la base de données de la table Rule ordonnées par ExecuteOrder croissant et le moteur de worflow exécute toutes les règles dans l’ordre. 
3)	On itère sur les résultats d’exécution de chaque règle :
-	Si le résultat de l’exécution de la règle courante est IsSuccess=true :
-Si le résultat attendu de la règle est de type challenge  -> alors il y a challenge on insert un enregistrement dans ActivityWorkflow avec la valeur 0 puisque la règle n’est pas passée.
-Sinon si le resultat est de type Allow  on insert un enregistrement dans ActivityWorkflow avec la valeur 1 puisque la règle est pas passée.
              -     Sinon Si le résultat de l’exécution de la règle courante est IsSuccess=false :
	    - Si le résultat attendu de la règle est de type challenge  -> alors il y a challenge on insert un enregistrement dans ActivityWorkflow avec la valeur 0 puisque la règle n’est pas passée.
	    - Sinon si le résultat est de type Allow on insert on insert un enregistrement dans ActivityWorkflow avec la valeur 0 puisque la règle est passée.
