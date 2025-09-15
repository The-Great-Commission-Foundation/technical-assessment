# The Great Commission Foundation Technical Assessment
Take home technical assessment for developer interviews @thegc.org

## Pre-requisites

1. Install .Net 9.0  
https://dotnet.microsoft.com/en-us/download

2. Install latest Node.JS (used v23)  
https://nodejs.org/en/download

3. Install SQLite  
https://www.tutorialspoint.com/sqlite/sqlite_installation.htm

4. Recommended: Use Visual Studio Code  
https://code.visualstudio.com/Download  
_Install the C# Dev Kit extension on VSCode_  
https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit

## Useful make commands for the repo

`make all`  
_builds both frontend react app, and backend dotnet app, recommended to run before starting to code_  
  
`make frontend`  
_builds frontend react app (npm start)_  

`make backend`  
_builds backend dotnet app_  

`make run-test`  
_builds backend and runs the NUnit tests in the test/be-test directory, recommended to run before starting to code_  

`make init_db`  
_sets up the SQLite DB for the dotnet project and creates the following table_

```
PRAGMA table_info(donations);  
0|id|INTEGER|0||1  
1|donor_name|TEXT|1||0  
2|amount|REAL|1||0  
3|date|TEXT|1||0
```

`make terminate`  
_kills all processes for port 5050 (backend), and 3000 (frontend)_

`make list-pid-unix`   
_lists PIDs for port 5050 and 3000_  

`make list-pid-windows`  
_lists PIDs for port 5050 and 3000_
