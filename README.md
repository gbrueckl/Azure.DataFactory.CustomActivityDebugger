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
3. Add a Reference to the existing C# Project which includes the Custom .Net Activity to debug
3. Add a Reference to the ADF Custom Activity C# Project which must reside in same Solution!
4. Add the Namespace of your ADF Custom Activity to this file
5. Change the static values in the Program-Class below to match your environment
6. Set Breakpoints in your Custom Activity Code
7. Happy Debugging!



