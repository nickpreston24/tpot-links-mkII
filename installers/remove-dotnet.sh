dotnet --list-runtimes
dotnet --list-sdks
echo "removing sdks and runtimes..."

sudo apt remove --purge dotnet-sdk-6.0
sudo apt remove --purge -y dotnet-sdk-6.0.408
sudo apt remove --purge -y dotnet-runtime-6.0

sudo apt-get remove - purge -y dotnet-host 
sudo apt-get remove - purge -y dotnet-hostfxr-5.0 
sudo apt-get remove - purge -y dotnet-runtime-5.0 
sudo apt-get remove - purge -y dotnet-runtime-deps-5.0 
sudo apt-get remove - purge -y dotnet-sdk-5.0
sudo apt-get remove - purge -y dotnet-targeting-pack-5.0 
sudo apt-get remove - purge -y dotnet-apphost-pack-5.0 
sudo apt-get remove - purge -y dotnet-runtime-6.0 
sudo apt-get remove - purge -y dotnet-hostfxr-6.0
sudo apt-get remove - purge -y dotnet-runtime-deps-6.0 
sudo apt-get remove - purge -y dotnet-targeting-pack-6.0
sudo apt-get remove --purge -y dotnet-apphost-pack-6.0
sudo apt-get remove --purge -y dotnet-host/now 6.0.0
sudo apt-get remove - purge -y dotnet-apphost-pack-6.0

sudo snap remove dotnet-sdk
sudo snap remove dotnet-runtime-60

sudo snap remove dotnet-sdk
sudo snap remove dotnet-runtime-70

sudo apt remove --purge dotnet-sdk-7.0
sudo apt remove --purge dotnet-runtime-7.0


sudo apt remove 'dotnet*' 'aspnet*' 'netstandard*'
sudo rm /etc/apt/sources.list.d/microsoft-prod.list
sudo apt update


sudo apt autoremove -y && sudo apt autoclean -y && sudo apt update && sudo apt upgrade

echo "here's the result:"

dotnet --list-runtimes
dotnet --list-sdks
