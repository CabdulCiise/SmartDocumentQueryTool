if (!$args[0]) {
	Write-Error 'Please provide the name of the migration to which you would like to update.'
	return
}

dotnet ef database update $args[0]  -p "API.Data/API.Data.csproj" -s "API.Web/API.Web.csproj" -c ApiContext