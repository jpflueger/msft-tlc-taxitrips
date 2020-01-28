# msft-tlc-taxitrips

![CI Build](https://github.com/jpflueger/msft-tlc-taxitrips/workflows/CI%20Build/badge.svg)
[![codecov.io](https://codecov.io/github/jpflueger/msft-tlc-taxitrips/coverage.svg?branch=master)](https://codecov.io/github/jpflueger/msft-tlc-taxitrips?branch=master)

Take home engineering challenge for Microsoft's CSE team

## Requirements
* [dotnet core v3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Git LFS](https://git-lfs.github.com/)

## Build Instructions
1. Open up Terminal
2. Clone repository (must have Git LFS installed and enabled) `git clone https://github.com/jpflueger/msft-tlc-taxitrips`
3. `cd ./src/TLC.Taxi.Data.Console`
4. `dotnet restore`
5. `dotnet build`

## Run Instuctions
After you have successfully followed the build instructions. Find the identifiers of the boroughs that you wish to travel from and to. Run the predict command with those identifiers to predict time and cost. If I had more time, this could have been a website or any kind of application.
- `dotnet run -- --help` - to see available commands
- `dotnet run -- boroughs [starts-with]` - to search for the identifier
  - `[starts-with]` - an optional argument to search from the start of the borough name
- `dotnet run -- predict <from-borough-id> <to-borough-id> [-v Yellow|Green|ForHire]` - to predict time and cost of a trip
  - `<from-borough-id>` - the identifier of the borough from which you are traveling
  - `<to-borough-id>` - the identifier of the borough to which you are traveling
  - `-v Yellow|Green|ForHire` - option to filter by vehicle type

Additional notes can be found in the [Notes.md](Notes.md) file.
