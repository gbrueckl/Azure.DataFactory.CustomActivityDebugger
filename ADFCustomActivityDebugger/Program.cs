using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.DataFactories.Runtime;

// Namespace of ADF Custom Activity
using DataDownloaderActivityNS;

namespace Azure.DataFactory
{
    /*
    This C# Console Project should help you to debug your ADF Custom Activities. Here are the steps to make it work:
    1) Add a Reference to the ADF Custom Activity C# Project (e.g. MyADFProject) - must reside in same Solution!
    3) Add the Namespace of your ADF Custom Activity to this file: "using <your namespace>;" (e.g. using DataDownloaderActivityNS)
    4) Change the static values in the Program-Class below to match your environment
    5) Set Breakpoints in your Custom Activity Code
    6) Start the Debugger as any other regular console application
    */
    public class Program
    {
        // change all these variables according to your ADF Custom Activity
        private static string ADFWorkingDirectory = @"..\..\..\MyADFProject\"; // directory of the ADF solution, can be relative or absolute path
        private static string ADFPipelineName = "DataDownloaderSamplePipeline"; // name of the pipeline which contains the Activity to test
        private static string ADFActivityName = "DownloadData"; // name of the Activity to test
        
        // e.g. "MyConfig.json", optional (must reside in the ADFWorkingDirectory!) and parameters to be 
        // substituted must be specified as "<config>" in the json definition files, e.g. "accessKey": "<config>"
        private static string ADFConfigurationFile = null; 
        
        private static DateTime SliceStart = new DateTime(2016, 10, 07); // set Slice-StartDate
        private static DateTime SliceEnd = new DateTime(2016, 10, 07); // set Slice-EndDate
        private static IDotNetActivity executeable = new DataDownloaderActivity(); // the IDotNetActivity that should be debugged


        static void Main(string[] args)
        {
            ADFCustomActivityDebugger debugger = new ADFCustomActivityDebugger(
                                                        ADFWorkingDirectory,
                                                        ADFPipelineName, 
                                                        ADFActivityName,
                                                        ADFConfigurationFile);
            debugger.SliceStart = SliceStart;   
            debugger.SliceEnd = SliceEnd; 

            debugger.LoadADFEnvironment(); // Load the Environment

            debugger.ExecuteActivity(executeable); // Execute your Activity
        }
    }
}
