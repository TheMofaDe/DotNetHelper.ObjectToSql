//////////////////////////////////////////////////////////////////////
// Package MSI Tasks
//////////////////////////////////////////////////////////////////////

Task("Heat")
  .IsDependentOn("Build")
   .Does<BuildParameters>((context, parameters) =>{

// https://cakebuild.net/api/Cake.Incubator.Project/CustomProjectParserResult/
    foreach(var project in parameters.SolutionProjects){  
        
        var isABoolean = bool.TryParse(project.GetProjectProperty("CreateMSI"),out var canCreateMSI);
        //  Information($"{project.AssemblyName} --> {project.OutputType}");
        if(!canCreateMSI) 
        continue;

        foreach(var targetFramework in project.TargetFrameworkVersions){


    foreach (var (os, cpuArchitectures) in parameters.NativeRuntimes){
    
                            foreach(var cpuArchitecture in cpuArchitectures){

// LIGHT 
           // DirectoryPath harvestDirectory = parameters.Paths.Directories.ArtifactsBin.Combine(targetFramework);
              DirectoryPath harvestDirectory = parameters.Paths.Directories.Artifacts.Combine("publish").Combine(targetFramework);
              var outputMetadataFile = parameters.Paths.Directories.Installer.Combine(targetFramework).CombineWithFilePath("Components.wxs"); 
     

	        var heatSettings = new HeatSettings {
			ArgumentCustomization = args => args.Append("-var var.HeatSourceFilesDir")
												.Append("-dr AppDir")
                 ,ComponentGroupName = "NetBinComponents"
	             ,GenerateGuid = true
		         ,SuppressCom = true
		         ,SuppressRegistry = true
		         ,SuppressFragments = true
		         ,SuppressRootDirectory = true
		         ,OutputFile = outputMetadataFile.ToString()  
		         ,PreprocessorVariable = "var.HeatSourceFilesDir" 
            };

             WiXHeat(harvestDirectory, outputMetadataFile,WiXHarvestType.Dir,heatSettings);







// CANDLE

        var files = GetFiles($"{parameters.Paths.Directories.Installer.Combine(targetFramework).ToString()}/*.wxs");

        Information($"FOUND  {files.Count} files");
        var settings = new CandleSettings {
		  	    ArgumentCustomization = args => args.Append("-dHeatSourceFilesDir=" + $"{harvestDirectory.ToString()}"),
                Verbose = true,
                NoLogo = true,
                OutputDirectory = parameters.Paths.Directories.Installer.Combine(targetFramework).ToString(),
		        Defines = new Dictionary<string, string>(){ 
                    {"SampleVariable",  $"Test" }
			    }            
        };

        WiXCandle(files, settings);





  LightSettings lightSettings = new LightSettings {
	  	  ArgumentCustomization = args => args
                .Append("-dHeatSourceFilesDir=" + $"{harvestDirectory.ToString()}"),
        Defines = new Dictionary<string, string>(){ },
        OutputFile = parameters.Paths.Directories.Installer.CombineWithFilePath($"{project.AssemblyName}-{parameters.Version.PackageVersion}-fx-{targetFramework}-{os}-{cpuArchitecture}.msi").ToString(),
        NoLogo = true,
     
        };
     WiXLight(parameters.Paths.Directories.Installer.Combine(targetFramework).ToString() + "/*.wixobj", lightSettings);



                            }

    }

        }

    }

  
  });

