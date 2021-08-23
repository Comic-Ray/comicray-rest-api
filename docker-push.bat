@echo off

dotnet publish -c Release
docker rmi comic-api
rem docker rmi registry.heroku.com/test-asp-api/web

docker build -t comic-api .

heroku container:push -a test-asp-api web
timeout 5 > null
heroku container:release -a test-asp-api web
