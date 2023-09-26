.PHONY: all clean test

all: test benchmark clean

clean:
	dotnet clean tests/
	dotnet clean BenchMarkCuckoo/
	dotnet clean BachelorProject.csproj

test:
	dotnet clean tests/
	dotnet test tests/

benchmark:
	dotnet clean BenchMarkCuckoo/
	dotnet clean tests/
	dotnet build BenchMarkCuckoo/BenchMarkCuckoo.csproj
	dotnet build BachelorProject.csproj
	dotnet run --project BachelorProject.csproj