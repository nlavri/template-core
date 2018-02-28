Template-core
==================================

Template your solutions and projects easy.

Lightweight tool that allows you to create a template package from a folder with any content.
Initially was created to automate Visual Studio solutions for microservices. But you can use it with any other IDE projects, folders or arbitrary file sets.

For example: 
You create Visual Studio solution with all neccessary projects (web/logic/data).
Then you specify a token to replace (usually a root namespace) and create a package with the tool. 
Later you can easily deploy an number of specific solutions from that package.

Useful for microservices templating.

Create Package:

  `dotnet template-core pack -f "c:\TemplateSolutionFolder" -p C:\MyTemplate.pkg -t SampleNamespace`

Deploy Package:

  `dotnet template-core unpack -p C:\MyTemplate.pkg -f "c:\NewSolutionFolder" -t NewBaseNamespace`

Usage:

 * -m = mode
      * c = create
      * d = deploy
 * -p = path to package to be created or to be deployed
 * -f = folder to be packaged or deployment folder
 * -t = token "Token=\__Token\__"
 
Packages are zip archives.
 
 This is a .net core 2 version.
 .Net Framework 4.5.2 version is [available here](https://github.com/nlavri/Templifier).
