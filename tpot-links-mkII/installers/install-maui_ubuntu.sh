# As of may 9, 2023, this fix was needed to get maui installed: https://github.com/dotnet/maui/issues/7041
# https://learn.microsoft.com/en-us/dotnet/maui/troubleshooting

# For good measure, I installed both.

dotnet workload install android #--version 7.0.49
dotnet workload install maui-android


dotnet new install Microsoft.Maui.Templates::6.0.300-rc.3.5667
dotnet new install Microsoft.Maui.Templates::6.0.312

dotnet workload install maui-ios # (debug these)
dotnet workload install maui-maccatalyst

dotnet new list maui

echo "run dotnet new maui -n my_maui_project"
