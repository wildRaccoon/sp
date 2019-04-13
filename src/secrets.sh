COUNT=$(dotnet user-secrets list --id Development -p ./sp.sln | grep sp.auth:hash | wc -l)

if [ "$COUNT" = "0" ]
then 
    dotnet user-secrets set "sp.auth:hash:salt" "my_prod_salt" --id Development -p ./sp.sln
    dotnet user-secrets set "sp.auth:hash:secret" "my_prod_secret" --id Development -p ./sp.sln
else
    dotnet user-secrets list --id Development -p ./sp.sln
fi