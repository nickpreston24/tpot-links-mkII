# !/bin/bash
dotnet publish --runtime alpine-x64 -c Release --self-contained true /p:PublishTrimmed=true -o ./publish