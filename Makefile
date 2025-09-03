start-all:
	(cd frontend; npm i; npm start) & (cd backend; dotnet run)

start-fe:
	(cd frontend; npm i; npm start)

start-be:
	(cd backend; dotnet run)