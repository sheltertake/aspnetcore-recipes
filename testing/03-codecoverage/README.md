# Testing

## Recipe 03 - Code Coverage (from 01)

 - copy 01-functionaltests and paste renaming 03-codecoverage 
 - install nuget package in FooApFunctionalTests project
   - coverlet.msbuild

 - run tests using cli options
   - configuration -> Release
   - result-directory -> .\
   - filter -> exclude tests with [Category="local"] (example)
   - logger -> specify trx format and output file
 - and [Coverlet integration with MSBuild](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/MSBuildIntegration.md)
   - /p:CollectCoverage -> true
   - /p:CoverletOutputFormat -> Cobertura
   - /p:CoverletOutput -> path to write cobertura xml

```cmd
C:\Github\aspnetcore-recipes\testing\03-codecoverage>dotnet test .\tests\FooApiFunctionalTests.csproj --configuration Release --results-directory=.\ --filter=TestCategory!=local --logger "trx;LogFileName=.\codecoverage\results\functional.trx" /p:CollectCoverage=true  /p:CoverletOutputFormat=Cobertura /p:CoverletOutput=.\..\codecoverage\coverage\functional.xml /p:Include="[FooApi]*"
```
 - advanced options I use to add
   -  /p:exclude="[FooApi]FooApi.NameSpaceToExclude.*"


```cmd
Test run for C:\Github\aspnetcore-recipes\testing\03-codecoverage\tests\bin\Release\net5.0\FooApiFunctionalTests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.8.3
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
WARNING: Overwriting results file: C:\Github\aspnetcore-recipes\testing\03-codecoverage\.\codecoverage\results\functional.trx
Results File: C:\Github\aspnetcore-recipes\testing\03-codecoverage\.\codecoverage\results\functional.trx

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 657 ms - FooApiFunctionalTests.dll (net5.0)

Calculating coverage result...
  Generating report '.\..\codecoverage\coverage\functional.xml'

+--------+--------+--------+--------+
| Module | Line   | Branch | Method |
+--------+--------+--------+--------+
| FooApi | 83,33% | 100%   | 76,92% |
+--------+--------+--------+--------+

+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 83,33% | 100%   | 76,92% |
+---------+--------+--------+--------+
| Average | 83,33% | 100%   | 76,92% |
+---------+--------+--------+--------+
```


 - install dotnet-reportgenerator-globaltool

```cmd
C:\Github\aspnetcore-recipes\testing\03-codecoverage>dotnet tool install --tool-path ./tools dotnet-reportgenerator-globaltool
You can invoke the tool using the following command: reportgenerator
Tool 'dotnet-reportgenerator-globaltool' (version '4.8.4') was successfully installed.

```


 - run reportgenerator 


```cmd
C:\Github\aspnetcore-recipes\testing\03-codecoverage>tools\reportgenerator.exe -reports:.\codecoverage\coverage\*.xml -targetdir:.\codecoverage\report -reporttypes:HtmlInline_AzurePipelines;Cobertura  
```

 - open report generated
