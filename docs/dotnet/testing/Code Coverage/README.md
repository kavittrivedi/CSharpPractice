## What code coverage report we can add in CICD pipeline for check the unit test code coverage? what we have to do, give me the list

To check unit test code coverage in a CI/CD pipeline, you can integrate tools that generate code coverage reports and fail builds if coverage thresholds are not met. Here's how to do it:

### Popular Code Coverage Tools:

- **coverlet** (for .NET Core projects)
- **ReportGenerator** (to generate human-readable reports)
- **SonarQube** (for detailed analysis and reporting)
- **Azure DevOps Test Coverage** (built-in for Azure Pipelines)

### Steps to Add Code Coverage in CI/CD Pipeline:

1. **Install Coverage Tools**

   Add the following NuGet packages to your test projects:
   ```
   dotnet add package coverlet.msbuild
   dotnet add package coverlet.collector
   ```

2. **Run Tests with Coverage**

   Use `dotnet test` with the `--collect` option to gather code coverage:
   ```
   dotnet test /p:CollectCoverage=true /p:CoverletOutput=./coverage/ /p:CoverletOutputFormat=json
   ```

3. **Generate Coverage Reports**

   Install ReportGenerator globally to convert coverage data into readable reports:
   ```
   dotnet tool install -g dotnet-reportgenerator-globaltool
   ```
   Generate HTML reports:
   ```
   reportgenerator -reports:./coverage/coverage.json -targetdir:./coverage-reports
   ```

4. **Integrate with CI/CD**

   Modify your CI/CD pipeline (e.g., Azure DevOps, GitHub Actions, Jenkins, etc.) to:

   - Run tests and collect coverage.
   - Publish coverage results or fail builds if coverage is below a threshold.

5. **Example YAML Pipeline for Azure DevOps**
   ```yaml
   trigger:
   - main

   pool:
     vmImage: 'windows-latest'

   steps:
   - task: UseDotNet@2
     inputs:
       packageType: sdk
       version: '6.x'
       
   - script: dotnet restore
     displayName: 'Restore NuGet packages'

   - script: dotnet build --no-restore
     displayName: 'Build the solution'

   - script: dotnet test --no-build --collect:"XPlat Code Coverage" /p:CoverletOutputFormat=cobertura
     displayName: 'Run Tests with Code Coverage'

   - task: PublishCodeCoverageResults@1
     inputs:
       codeCoverageTool: 'Cobertura'
       summaryFileLocation: '$(Agent.TempDirectory)/**/coverage.cobertura.xml'
   ```

6. **Set Coverage Thresholds**

   You can fail builds if coverage is below a certain percentage by using:
   ```
   /p:Threshold=80
   ```
   This will fail the build if coverage is below 80%.

7. **(Optional) Integrate with SonarQube**

   Add SonarQube analysis to your pipeline for advanced reporting:

   - Install SonarScanner for MSBuild.
   - Analyze code and upload coverage results to SonarQube.

### Checklist for Code Coverage in CI/CD:

- Install coverlet.msbuild and reportgenerator.
- Configure dotnet test with coverage options.
- Add a step in the pipeline to generate coverage reports.
- Publish the coverage results in the pipeline (e.g., Cobertura format).
- Set coverage thresholds to fail builds if required.
- (Optional) Integrate with tools like SonarQube for advanced analysis.

By following these steps, you can ensure your CI/CD pipeline monitors code coverage effectively.