using System;
using System.IO;
using Mistware.Files;
using Mistware.Utils;

namespace TicketCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Setup("App.config", Directory.GetCurrentDirectory(), null, "TicketCollector");
            
            string sourceAccount   = Config.Get("SourceAccount");
            string sourceFileShare = Config.Get("SourceFileShare");
            string sourceDirectory = Config.Get("SourceDirectory");
            string metadata        = Config.Get("Metadata");
            string attachments     = Config.Get("Attachments");
            string logs            = Config.Get("Logs");
        
            // Access the Source Directory (bootstrap also sets up the Logs container)
            IFile filesys = FileBootstrap.SetupFileSys(sourceAccount, sourceFileShare, null, logs);
            Log.Me.LogFile = "TicketCollector.log";
            filesys.ChangeDirectory(sourceDirectory);

            foreach (DirectoryEntry de in filesys.FileList())
            {
                string filename = de.Name;
                string destination = ( Path.GetExtension(filename) == ".xml" ) ? metadata : attachments;

                try
                {
                    filesys.FileDownload(filename, destination);
                    filesys.FileDelete(filename);
                    Log.Me.Info(filename + " moved from " + sourceFileShare + ":" + sourceDirectory + 
                                " to " + destination);
                } 
                catch (Exception ex)
                {
                    Log.Me.Error("Failed to move " + filename + " from " + sourceFileShare + ":" + 
                                sourceDirectory + " to " + destination + " Exception was: " + ex.Message);
                }
            }
        }
    }
}

