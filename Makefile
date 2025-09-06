DB_FILE = db/mydatabase.db
TABLE_NAME = donations
SQL_CREATE_TABLE = CREATE TABLE $(TABLE_NAME) ( \
    id INTEGER PRIMARY KEY, \
    donor_name TEXT NOT NULL, \
    amount REAL NOT NULL, \
	date TEXT NOT NULL \
);

.PHONY : all frontend backend run-test

frontend:
	@echo "-----Running Frontend React App-----"
	(cd frontend; npm i; npm start &)

backend:
	@echo "-----Running Backend .NET App-----"
	(cd backend; dotnet run &)

all: frontend backend
	@echo "-----Completed running both Frontend and Backend-----"

run-test: backend
	@echo "-----Completed running Backend and starting its tests-----"
	dotnet test be-test.sln
	
init-db:
	@echo "-----Installing SQLite for .NET-----"
	(cd backend; dotnet add package Microsoft.Data.Sqlite)
	(cd test/be-test; dotnet add package Microsoft.Data.Sqlite)
	@echo "-----Creating database and table-----"
	sqlite3 $(DB_FILE) "$(SQL_CREATE_TABLE)"
	@echo "-----Table '$(TABLE_NAME)' created in '$(DB_FILE)'-----"