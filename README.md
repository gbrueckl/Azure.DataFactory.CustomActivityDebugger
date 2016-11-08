# Azure.DataFactory.CustomActivityDebugger
A C# Project which allows you to locally debug Azure Data Factory Custom .Net Activities

Azure Data Factory's Cusotm Activities are quite powerful but are also quite painful when it comes to development and debugging. This tool should help you to ease these pains by enabling you to run your Custom C# Activity in a kind of local environment. It retrieves all information and settings from your local Azure Data Factory project folder to set up this environment and allows you debug the Custom Activity like any other C# code.
The C# Console Application is supposted to be run from within Visual Studio mainly. 

This repository contains:
* *Custom Activity Debugger* (can be copied to/used with any other Custom C# Activity)
* Sample ADF Custom .Net Activity
* Sample ADF Project calling the Custom .Net Activity from above


The *Custom Activity Debugger* can be copied to and integrate with any other ADF Custom .Net Activity by simply following the steps below:

# Setup
1. Copy the CustomActivityDebugger Folder to your Solution-Folder (already done in this sample)
2. Add the Custom ActivityDebugger Project to the Solution
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Debugger_Project_to_ADF_Solution_1.jpg "Add Debugger-Project to ADF-Solution (1)")
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Debugger_Project_to_ADF_Solution_2.jpg "Add Debugger-Project to ADF-Solution (2)")
3. Add a Reference to the ADF Custom Activity C# Project which must reside in same Solution
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Project_Reference_to_Debugger.jpg "Add Project Reference to Debugger-Project")
4. Add the Namespace of your ADF Custom Activity to this file
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Namespace_to_Debugger.jpg "Add Namespace to Programm-Class")
6. Change the static values in the Program-Class below to match your environment
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Change_Debugger_Static_Variables.jpg "Change static variables in Debugger-Program")
7. Set Breakpoints in your Custom Activity Code
8. Start the Debugger (and/or set as Startup-Project)
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Start_Debugger.jpg "Start the Debugger")
9. Happy Debugging!
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Debugger_in_Action.jpg "Debugger in Action")
