var publishdotnetTask = Task("Publish-DotNet")
.Does<BuildParameters>((context,parameters) =>  {

       foreach(var project in parameters.SolutionProjects){  
        
        var isABoolean = bool.TryParse(project.GetProjectProperty("CreateMSI"),out var canCreateMSI);
        if(!canCreateMSI) 
        continue;

           foreach(var targetFramework in project.TargetFrameworkVersions){


                  
                    foreach (var (os, cpuArchitectures) in parameters.NativeRuntimes){
    
                            foreach(var cpuArchitecture in cpuArchitectures){
                   //  Information($"{project.AssemblyName} --> {project.OutputType}");

                    var msbuildSettings = new DotNetCoreMSBuildSettings {
                          MaxCpuCount = 0,
                    };
                    var settings = new DotNetCorePublishSettings
                    {
                        ArgumentCustomization = args => args
                               .Append("/p:IncludeNativeLibrariesInSingleFile=true")
                               .Append($"--runtime {cpuArchitecture}"),
                        Framework = targetFramework,
                        Configuration = parameters.Configuration,
                        NoBuild = true,
                        NoRestore = true,
                        PublishReadyToRun = true,
                        PublishSingleFile = true,
                        PublishTrimmed = false,
                        SelfContained = true,
                        OutputDirectory = parameters.Paths.Directories.Artifacts.Combine("publish").Combine(targetFramework)
                    };
                
                        DotNetCorePublish(project.ProjectFilePath.FullPath, settings);
                            }
                    
                    }
                
                }
       }


})
.Finally(() =>
{
    if (publishingError)
    {
        throw new Exception("An error occurred during dotnet publish.");
    }
});