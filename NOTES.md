# Take Home Engineering Challenge for the Microsoft CSE team.
This is my submission for the take home engineering challenge. In this repository I will attempt to outline my decision making process in designing the solution while working inside the guidelines of the challenge. I have taken the 3 hour guideline to mean 3 hours of focused implementation work and will not count time spent planning. 

## Approach
1. Understand the needs of the customer
2. Understand the needs of the user
3. Understand the data required to solve the problem
4. Create a plan of execution using user stories
5. Implement 
6. Document future improvements

## The Customer
I am regarding the customer in this context to be the Boulder CSE team at Microsoft. I know from the guidelines that the customer cares deeply about planning, design, testing, deployment and documentation so we must always keep these at the top of mind througout the project. 
- Planning - this will be done in a GitHub Project board
- Design
  - Given the constraints on time, developer productivity is important for this project. I am most productive in C# while using dotnet core tooling.
  - I will also use Python for scripting because it is the fastest way for me to reshape the dataset into more a more efficient structure. I would use it for the entirety of the project but I am not well-versed in the testing frameworks for Python.
  - TODO: System Design Diagram
- Testing
  - Unit testing will be done with xUnit
  - Integration testing will be done with xUnit
  - If time allows, automated UI tests with selenium would be ideal
- Deployment
  - CI will be configured with GitHub actions
  - CD will not be implemented here because I have no environment to deploy to as this will be run on local computers but in a production environment this would be required
- Documentation
  - This readme
  - Wiki
  - GitHub Project board

## The User
For this challenge the user will be someone in NYC that is trying to go from one Borough to another. This is what we know about the user:
- Wants to know how long a trip might take
- Wants to know how much a trip might cost
- Will provide a start and end borough for the trip
- Wants to filter based on yellow cab, green cab and for-hire vehicles
- Wants to filter based on date and time
- Wants to see interesting metrics

## The Data
The dataset in use for this problem is NYC's TLC Trip Record Data for Yellow, Green and For-Hire vehicles. We will also need a lookup table for the names of the boroughs and locations. There is likely malformed or incorrect data in the dataset that could skew our recommendations but for the purposes of this challenge we'll ignore data cleanliness.

### Source Data Dictionary
 These are the fields from each dataset that we will need to help the user:
- Taxi Zone Lookup:
  - `LocationID`: used to relate a trip to a taxi zone
  - `Borough`: the name of the borough this location is in (each borough can have many locations)
  - `Zone`: the name of the taxi zone (more precise than a borough)
- Yellow Trips
  - `tpep_pickup_datetime`: date and time of passenger pickup
  - `tpep_dropoff_datetime`: date and time of passenger dropoff
  - `PULocationID`: identifier of pickup location, will be used to relate back to borough
  - `DOLocationID`: identifier of dropoff location, will be used to relate back to borough
  - `Total_amount`: amount the passenger was charged for the trip
- Green Trips
  - `lpep_pickup_datetime`: date and time of passenger pickup
  - `lpep_dropoff_datetime`: date and time of passenger dropoff
  - `PULocationID`: identifier of pickup location, will be used to relate back to borough
  - `DOLocationID`: identifier of dropoff location, will be used to relate back to borough
  - `Total_amount`: amount the passenger was charged for the trip
- FHV Trips
  - `Pickup_datetime`: date and time of passenger pickup
  - `DropOff_datetime`: date and time of passenger pickup
  - `PULocationID`: identifier of pickup location, will be used to relate back to borough
  - `DOLocationID`: identifier of pickup location, will be used to relate back to borough
  - *NOTE*: the FHV dataset lacks Total_amount so we will not be able to predict this for FHV trips

### Table Schemas
Given that the datasets are rather large and we only need a subset of columns, we will merge the three trip datasets into a single table so that we can compute metrics for all types of trips instead of running individual queries for each table. 
- TaxiTrip
  - `PickUpDateTime`: datetime
  - `DropOffDateTime`: datetime
  - `PickUpLocationId`: short
  - `DropOffLocationId`: short
  - `VehicleType`: byte (1 - Yellow, 2 - Green, 3 - FHV)
  - `Duration`: long (computed column based on DropOffDateTime - PickUpDateTime)
  - `Cost`: float? (nullable because FHV trips do not have this)
- TaxiZone
  - `LocationId`: int
  - `Borough`: string
  - `Name`: string

### Metrics
We need to communicate to the user a prediction for duration and cost for the trip. We can distill this down to a single number like an average. If we have the time, we could add in some visualizations like a cost histogram, pie chart for vehicle type or average duration windowed by hour of the day. We can use the Math.NET project to easily calculate the descriptive statistics.

## Planning
The planning is documented in the GitHub Project board in more depth. The general approach is to iteratively implement "narrow" features of the application.

## Future Improvements
TODO
