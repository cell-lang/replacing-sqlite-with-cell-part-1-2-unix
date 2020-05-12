standalone:
	@make -s clean
	@mkdir tmp/ debug/
	@cp project/run-northwind-queries.csproj tmp/
	dotnet compiler/bin/cellc-cs.dll -d project/standalone.txt tmp/
	dotnet build tmp/
	ln -s tmp/bin/Debug/netcoreapp3.1/run-northwind-queries .
	@echo ''
	@echo ''
	@echo 'Now type:'
	@echo '  ./run-northwind-queries dataset.txt'
	@echo ''

embedded:
	@make -s clean
	@mkdir tmp/
	@cp src/main.cs project/run-northwind-queries.csproj tmp/
	dotnet compiler/bin/cellc-cs.dll -g project/types.txt project/embedded.txt tmp/
	dotnet build tmp/
	ln -s tmp/bin/Debug/netcoreapp3.1/run-northwind-queries .
	@echo ''
	@echo ''
	@echo 'Now type:'
	@echo '  ./run-northwind-queries dataset.txt'
	@echo ''

clean:
	@rm -rf run-northwind-queries tmp/ debug/
