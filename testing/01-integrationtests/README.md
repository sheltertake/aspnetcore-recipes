# Testing

## Recipe 01 - Integration tests

```cmd
dotnet new webapi -n FooApi -o src/
dotnet new nunit -n FooApiTests -o tests/
```

 - install nuget packages
   - FluentAssertions
   - Microsoft.AspNetCore.TestHost
 - write first test

```cmd
PS C:\Github\aspnetcore-recipes\testing\01-integrationtests> dotnet test
  Determining projects to restore...
  Restored C:\Github\aspnetcore-recipes\testing\01-integrationtests\src\FooApi.csproj (in 309 ms).
  Restored C:\Github\aspnetcore-recipes\testing\01-integrationtests\tests\FooApiTests.csproj (in 795 ms).
  FooApi -> C:\Github\aspnetcore-recipes\testing\01-integrationtests\src\bin\Debug\net5.0\FooApi.dll
  FooApiTests -> C:\Github\aspnetcore-recipes\testing\01-integrationtests\tests\bin\Debug\net5.0\FooApiTests.dll
Test run for C:\Github\aspnetcore-recipes\testing\01-integrationtests\tests\bin\Debug\net5.0\FooApiTests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.8.3
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 628 ms - FooApiTests.dll (net5.0)

```

