#Game Store

##Starting SQL Server docker container

```powershell
$sa_password = "SA PASSWORD HERE"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pass@word1" -p 1433:1433 -d -v sqlvolume:/var/opt/mssql --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```
