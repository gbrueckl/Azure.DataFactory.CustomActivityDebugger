using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Namespace of ADF Custom Activity
using DataDownloaderActivityNS;

namespace Azure.DataFactory
{
    /*
    This Class should help you to debug your ADF Custom Activities. Here are the steps to make this work:
    1) Create a new C# Console Project within the same solution as your ADF Custom Activities C# Project
    2) Add a Reference to the ADF Custom Activity C# Project 
    3) Add "using <your namespace>;" (=the namespace of your ADF Custuom Activity)
    4) Change the fixed values in the Main() function below to match your environment
    */
    public class Program
    {
        static void Main(string[] args)
        {
            ADFCustomActivityDebugger debugger = new ADFCustomActivityDebugger(
                                                        @"d:\Work\SourceControl\GitHub\Azure.DataFactory.CustomActivityDebugger\MyADFProject\", // directory of the ADF solution
                                                        "DataDownloaderSamplePipeline", // name of the pipeline which contains the Activity to test
                                                        "DownloadData", // name of the Activity to test
                                                        null); // optional name of a config-file (must reside in the same folder as all other files!)
            debugger.SliceStart = new DateTime(2016, 10, 07); // set SliceStart if required
            debugger.SliceEnd = new DateTime(2016, 10, 08); // set SliceEnd if required

            debugger.LoadADFEnvironment(); // Load the Environment

            debugger.ExecuteActivity(new DataDownloaderActivity()); // Execute your Activity
        }
    }
}
