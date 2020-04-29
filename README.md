TicketCollector
===============

TicketCollector is a console application that copies documents and their associated metadata (in xml) from an Azure File Share to local folders. Typically this is to facilitate the transfer of these files from an Azure WebApp to an application running on a file server. 

The Azure File Share can be accessed by specifying a connection string, the name of the File Share and the name of the source folder that has files to be transferred. The application moves (copies and deletes from the source folder) the files either to a Metadata folder (if the file type is .xml) or to an Attachments folder (all other file types).      

Each transfer is logged in an Azure Blob container (accessed using the Azure connection string). These log files can be easily accessed by using the Microsoft Azure Storage Explorer tool [https://azure.microsoft.com/en-gb/features/storage-explorer/]. 

This application has two nuget package dependencies: Mistware.Files and Mistware.Utils. These are both open source and can be accessed from [http://nuget.org] or [https://github.com/julianpratt]. 


Settings
--------

The following configuration is stored in App.config: 

- SourceAccount - the Azure File Share connection string
- SourceFileShare - the name of the Azure File Share
- SourceDirectory - the name of the top level folder within the Azure File Share that has files to be transferred (e.g. "Outbox")
- Metadata - the full path of the local folder where metadata files (i.e. those with a file type of .xml) are transferred to
- Attachments - the full path of the local folder where attachments are transferred to
- Logs - the name of the Azure Blob container where log files are stored


Usage
--------

This application requires the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download) to be downloaded and installed (preferably on the server where it is to run).

Having installed the SDK: download the application from GitHub (either using git or by downloading and unzipping the zip file), restore its dependencies (nuget packages) and build it:

```
git clone https://github.com/julianpratt/TicketCollector.git
dotnet restore
dotnet build 
```

Then update Appsettings.config with appropriate configuration for this install (or use the config file from a previous version of TicketCollector).

Finally schedule the application to run at time slots that correspond with the import schedule of the receiving application (e.g. using the server's AT command). Note however that .NET Core does not build a .exe, but instead the application is run by making its location the current folder and issuing this command:

```
dotnet run 
```

It is possible to build a .exe with the "dotnet publish" command, but it will be necessary to change the RuntimeIdentifier in the csproj (which is currently set to "osx-x64").
