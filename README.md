# Niner Monitor API

This project provides the backend API for the Niner Monitor site, delivering real-time and historical occupancy data for various locations at UNC Charlotte. The API serves as a key part of the overall dashboard, enabling clients to fetch and display relevant information.

## ℹ️ About the API

The API is currently hosted at [https://uncc-occupancy-tracker-backend.onrender.com](https://uncc-occupancy-tracker-backend.onrender.com). Please note that this URL is temporary and is likely to change in the near future to reflect the current project name. The API is built with ASP.NET and supports CORS to allow requests from other websites. However, depending on your usage, you might still need to configure CORS headers in your GET requests.

### 📝 API Documentation

Currently, the API endpoints are not final and prone to change. However, here are the currently implemented endpoints:

**/api/OccupancyData/CurrentOccupancyData**

Returns the live occupancy counts of all locations.

Parameters: none

Return Schema:

```json
{
  "time": "2024-10-23T18:11:15.164Z",
  "data": {
    "dining": {
      "currentSoVi": 0,
      "maxSoVi": 0,
      "current704": 0,
      "max704": 0,
      "percentSoVi": 0,
      "percent704": 0
    },
    "adkins": {
      "currentOccupants": 0,
      "isOpen": true,
      "todayOpenTimes": "string",
      "lastUpdated": "2024-10-23T18:11:15.164Z"
    },
    "parking": [
      {
        "lotCode": "string",
        "name": "string",
        "mapLink": "string",
        "percentAvailable": 0
      }
    ]
  }
}
```

> [!NOTE]  
> More information is likely to be added to this endpoint in the near future.


**/api/OccupancyData/HistoricalOccupancyData**

Returns a list of objects with a string `date` of the time and a `data` object of the data associated with that item at that time.

Parameters:

- **item (string)**: the item to query information on
  - can be "dining", "atkins", or "parking"
 
**Sample output (item: "atkins"):**

```json
[
  {
    "time": "2024-10-23T14:11:48.0680409-04:00",
    "data": {
      "currentOccupants": 1170,
      "isOpen": true,
      "todayOpenTimes": "7:00 AM–11:59 PM",
      "lastUpdated": "2024-10-23T14:10:49.6459084-04:00"
    }
  },
  {
    "time": "2024-10-23T14:12:48.0820789-04:00",
    "data": {
      "currentOccupants": 1170,
      "isOpen": true,
      "todayOpenTimes": "7:00 AM–11:59 PM",
      "lastUpdated": "2024-10-23T14:10:49.6459084-04:00"
    }
  }
]
```

> [!WARNING]  
> This endpoint is not final and is likely to have breaking changes in the near future. Use with caution.
   

## 🛠️ Build

### Prerequisites

This project is built with .NET 8.0 and requires the .NET SDK. Ensure this is installed before proceeding.

### Installation

1. Clone the repository
2. Restore the dependencies:

   ```sh
   dotnet restore
   ```

3. Run the application:

   ```sh
   dotnet run
   ```

   By default, the server will be hosted at `http://localhost:5096`. You should be able to access the Swagger API documentation page at this URL if everything is set up correctly.

4. Optionally, you can also build a production-ready version of the application (note this will run on port 5096—you can change this if needed in [appsettings.json](https://github.com/DUDEbehindDUDE/Niner-Monitor-API/blob/main/OccupancyTracker/appsettings.json)):

   ```sh
   dotnet publish -c Release
   ```

   Or you can use Docker to build and run the application:

   ```sh
   docker build -t occupancy-tracker .
   docker run -p 80:5096 occupancy-tracker
   ```
   
## ❗ Issues and Suggestions

If you encounter any issues or have suggestions for improvements, feel free to open an issue on this repository and provide details about the problem or feature request.

## ⚖️ License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/dudebehinddude/niner-monitor-api/blob/main/LICENSE) file for more information.
