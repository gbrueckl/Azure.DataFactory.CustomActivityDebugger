# Azure.DataFactory.CustomActivityDebugger
C# Project which allows you to locally debug Azure Data Factory Custom .Net Activities

This repository contains:
* ADF Sample Project
* Sample ADF Custom .Net Activity
* *Custom Activity Debugger*

The *Custom Activity Debugger* can be copied to and integrate with any other ADF Custom .Net Activity by simply following the steps below:

# Setup
1. Copy the CustomActivityDebugger Folder to your Solution-Folder (already done in this sample)
2. Add the Custom ActivityDebugger Project to the Solution
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Debugger_Project_to_ADF_Solution_1.jpg "Add Debugger-Project to ADF-Solution (1)")
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Debugger_Project_to_ADF_Solution_2.jpg "Add Debugger-Project to ADF-Solution (2)")
3. Add a Reference to the existing C# Project which includes the Custom .Net Activity to debug
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Project_Reference_to_Debugger.jpg "Add Project Reference to Debugger-Project")
4. Add the Namespace of your ADF Custom Activity to this file
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Namespace_to_Debugger.jpg "Add Project Reference to Debugger-Project")
5. Add a Reference to the ADF Custom Activity C# Project which must reside in same Solution
![Alt text](http://files.gbrueckl.at/github/Azure.DataFactory.CustomActivityDebugger/Add_Debugger_Project_to_ADF_Solution_1.jpg "Add Debugger-Project to ADF-Solution (1)")
6. Change the static values in the Program-Class below to match your environment
7. Set Breakpoints in your Custom Activity Code
8. Happy Debugging!
