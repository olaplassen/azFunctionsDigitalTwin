# Azure functions to update Digital Twin and Azure Maps values
This repo is based of an [End to end solution](https://docs.microsoft.com/en-us/azure/digital-twins/tutorial-end-to-end) provided by azure but slightly modified to be able to process BME280 sensor data. Two more functions are added to update the azure indoor module as well as process the datastream from Azure digital twins to Azure time series insights.

## Prerequisites
- Visual studio 2019 installed with the following NuGet packages
  - Azure DigitalTwins.Core
  - Azure Identity
  - System.net.Http
  -	Azure Core


## What does the repo contain

The repo contains four functions which are used to update DT values based on sensordata recieved in IoTHub as well as updating Azure Maps values based on DT values in addition to routing data from ADT to Time series insights. **ProcessHubToDTEvents**: processes incoming IoT Hub data and updates Azure Digital Twins accordingly. **ProcessDTRoutedData**: processes data from digital twins, and updates the parent twins in Azure Digital Twins accordingly. **ProcessDTUpdatestoMaps** processes data from digital twins, and updates the map unit accordingly. **processDTUpdatetoMaps**: processes temperature and humidity updates from digital twin to Azure maps. Updates the stateset with values to determine which colour to set on the room. 

## How to run the sample

- Open the AdtE2ESample.sln project
- Functions are located in the SampleFunctionsApp project file
- Update dependencies
  - Right-select Packages and choose "Manage NuGet Packages..." 
  - Select the update tab and check for updates
- Publish the application
  - Right click on "SampleFunctionsApp" > Publish
  - Choose Azure
  - Azure Functions App (Windows)
  - Fill in required information
  - Click the "+" sign on "Functions app" and create a new Azure functions and storage account resource.
  - Publish
  - Check the azure portal to verify that your functions were published -> functionsApp resource -> functions
- Give the function app "Azure Digital Twins Data Owner" role in the Azure Digital twins instance.
- Add the ADT_SERVICE_URL as an environment variable in your functionapp.
  - "Azure function resource" > Configuration > New Application setting > Save
- Add the two stateSetId as an environment variable in your functionapp.
  - statesetID1 = temperature stateset
  - statesetID2 = humidity stateset

## Debug/LogStream

When running the functions you can stream log events in VsCode by installing the "Azure functions" addon -> "ctrl + shift p" -> Azure Functions: Start streaming logs.