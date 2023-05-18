
function nurelease(){
dotnet clean;
rm -rf obj/ bin/;
dotnet build;
dotnet pack -p:PackageVersion="$1" -c Release;

}

function nupush()
{
    ## Copy your api key to your .bashrc
    nuget push "$1" {{api-key}} -Source https://www.myget.org/F/code-mechanic/api/v2/package
}


function numechanic() {

## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex

}

function nurazor() {

dotnet new classlib


## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex

# And RazorHAT
dotnet add package CodeMechanic.RazorHAT

}


function nuapi() {

dotnet new webapi


## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex


}



function nuclasslib() {

dotnet new classlib


## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex


}