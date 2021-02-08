# NetCoreAppBoilerplate

boilerplate for .net core web app with simple example of database crud, rest api, and notification system (web socket).

### backend
- AspNetCore 2.2
- EntityFrameworkCore
- EntityFrameworkCore.UnitOfWork
- SQLite
- SignalR
- xUnit

### frontend
- React
- Redux
- Ant Design
- Axios
- SignalR

### prerequisite
- Windows 10
- Node.js 14.15.4
- Visual Studio 2017

### building ui (cmd)
```
cd NetCoreApp\Client
npm install
npm run-script build
```
### building host (Visual Studio 2017)
- Build > Build Solution

### running
```
cd NetCoreApp\bin\Debug\netcoreapp2.2
dotnet .\NetCoreApp.dll
```
- open http://localhost:8000 in browser
