Task("Publish-Coverage")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsRunningOnWindows,       "Publish-Coverage works only on Windows agents.")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsRunningOnAppVeyor, "Publish-Coverage works only on AppVeyor.")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsStableRelease() || parameters.IsPreRelease(), "Publish-Coverage works only for releases.")
    .IsDependentOn("Test")
    .Does<BuildParameters>((parameters) =>
{
    
     if (FileExists(parameters.Paths.Files.TestCoverageOutputFilePath)) {
      var token = parameters.Credentials.CodeCov.Token;
     if(string.IsNullOrEmpty(token)) {
         throw new InvalidOperationException("Could not resolve CodeCov token.");
     }

   
         // Upload a coverage report using the CodecovSettings.
         Codecov(new CodecovSettings {
                Files = new [] { parameters.Paths.Files.TestCoverageOutputFilePath.ToString() }
                ,Token = token
	    	    ,Required = true
            });
	    	Information("Uploading Coverage File --> " + parameters.Paths.Files.TestCoverageOutputFilePath);
     }
    
});