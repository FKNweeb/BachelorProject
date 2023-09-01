all:
	dotnet build

clean:
	dotnet clean
	sudo rm -rf bin obj
	sudo rm -rf tests/bin tests/obj

test:
	dotnet test tests/