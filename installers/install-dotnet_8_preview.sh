#  From : https://kontext.tech/article/1277/install-net-8-sdk-on-ubuntu

DOTNET_FILE=~/Downloads/aspnetcore-runtime-8.0.0-preview.4.23260.4-linux-x64.tar.gz
tar zxf "$DOTNET_FILE" -C "$DOTNET_ROOT"

dotnet --list-sdks
