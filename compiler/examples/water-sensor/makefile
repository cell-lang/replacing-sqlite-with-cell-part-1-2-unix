test-water-sensor.dll: main.cell
	make -s clean
	mkdir tmp/
	dotnet ../../bin/cellc-cs.dll project.txt tmp/
	cp water-sensor.csproj tmp/
	dotnet build -c Debug tmp/
	@echo
	@echo '********************************************************************************'
	@echo '********************************************************************************'
	@echo
	@echo 'To run the program type:'
	@echo '  dotnet run --project tmp/'

clean:
	@rm -rf tmp/
