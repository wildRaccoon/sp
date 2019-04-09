COUNT=$(dotnet user-secrets set sp.auth.salt $(uuidgen) --id dev -p ./sp.sln | wc -l)

if [ "$COUNT" = "0" ]
then 
    dotnet user-secrets set sp.auth.salt $(uuidgen) --id dev -p ./sp.sln
else
    dotnet user-secrets list --id dev -p ./sp.sln
fi