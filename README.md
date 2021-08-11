# Azure functions to update Digital Twin and Azure Maps values
This repo is based of an (https://docs.microsoft.com/en-us/azure/digital-twins/tutorial-end-to-end)[End to end solution] provided by azure but slightly modified to be able to process BME280 sensor data and update Azure maps based on Digital twin values. 

## Prerequisites
- Visual studio installed
- 

## What does the repo contain

The repo contains three functions which are used to update DT values based on sensordata recieved in IoTHub as well as updating Azure Maps values based on DT values. **ProcessHubToDTEvents**: processes incoming IoT Hub data and updates Azure Digital Twins accordingly. **ProcessDTRoutedData**: processes data from digital twins, and updates the parent twins in Azure Digital Twins accordingly. **ProcessDTUpdatestoMaps** processes data from digital twins, and updates the map unit accordingly. 

## How to run the sample

- Open the AdtE2ESample.sln project
- Functions are located in the SampleFunctionsApp project file
- Update dependencies
  - Before publishsing to Azure
  - expand SampleFunctionsapp
  - Dependencies
  - Right-select Packages and choose "Manage NuGet Packages..." 
  - Select the update tab and check for updates
- Publish the application
  - Right click on "SampleFunctionsApp" -> Publish
  - Choose Azure
  - Azure Functions App (Windows)
  - Fill in required information
  - Click the "+" sign on "Functions app" and create a new Azure functions and storage account resource.
  - Publish
  - Check the azure portal to verify that your functions were published -> functionsApp resource -> functions
- Give the function app "Azure Digital Twins Data Owner" role in the Azure Digital twins instance.
- Add the ADT_SERVICE_URL as an environment variable in your functionapp.

## Debug/LogStream

When running the functions you can stream log events in VsCode by installing the "Azure functions" addon -> "ctrl + shift p" -> Azure Functions: Start streaming logs.