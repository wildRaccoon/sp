COUNT=$(dotnet user-secrets list --id Development -p ./sp.sln | grep sp.auth:hash | wc -l)

if [ "$COUNT" = "0" ]
then 
    dotnet user-secrets set "sp.auth:hash:salt" "9aa3500b-dd8b-41f0-a51a-ac351f1f29df" --id Development -p ./sp.sln
    dotnet user-secrets set "sp.auth:hash:secret" "b6767801-5e19-4af8-b889-d09d07c560d2" --id Development -p ./sp.sln
else
    dotnet user-secrets list --id Development -p ./sp.sln
fi