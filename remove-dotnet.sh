dotnet --list-runtimes
dotnet --list-sdks
echo "removing sdks and runtimes..."

sudo apt remove --purge dotnet-sdk-6.0
sudo apt remove --purge dotnet-sdk-7.0
sudo apt remove --purge dotnet-runtime-7.0

echo "here's the result:"
dotnet --list-runtimes
dotnet --list-sdks