#Deploying using CLI
az login
az group create -l switzerlandnorth -n wordsapi_rg
#Create app service plan - F1 is the free version
az appservice plan create --name wordsapi_plan -g wordsapi_rg -sku F1

#List runtimes for web apps "DOTNET|5.0" is preferred
az webapp list-runtimes --linux

az webapp create -g wordsapi_rg -n maabrle-words-api --runtime "DOTNET|5.0" --plan wordsapi_plan_f1  --deployment-local-git

#dotnet clean/build/publish
dotnet clean
dotnet restore
dotnet build --configuration Release --no-restore
dotnet publish --no-build --nologo --configuration Release -o ./web-build
#zip the application in  ./web-build into ./web-build/web-build.zip
#deploy the app as an archive
az webapp deployment source config-zip --resource-group wordsapi_rg --name maabrle-words-api --src ./web-build/web-build.zip

# testing response times with curl
curl -X GET "https://maabrle-words-api.azurewebsites.net/Words" -H  "accept: application/json"  -H 'Accept-Encoding: gzip, deflate, sdch' -s -o /dev/null -w  "%{time_starttransfer}\n"

# testing with C#:
# namespace testapi
# {
#     class Program
#     {
#         static void Main(string[] args)
#         {
#             var wc = new WebClient();
#             DateTime start = DateTime.UtcNow;
#             for (int i = 0; i < 1000; i++)
#             {
#                 string tmp = wc.DownloadString("https://maabrle-words-api.azurewebsites.net/Words");
#             }
#             Console.WriteLine("time needed for 1000 calls:" + (DateTime.UtcNow - start).TotalSeconds + "sec");
#             Console.ReadLine();
#         }
#     }
# }
