# !/bin/bash
docker image build -t nickpreston17/tpot-api_slim:1.0.0 -f Dockerfile  .
dotnet publish --runtime alpine-x64 -c Release --self-contained true /p:PublishTrimmed=true -o ./publish
docker images -a