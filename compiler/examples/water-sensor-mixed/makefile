send-msgs.dll: main.cs water-sensor.cell
	make -s clean
	mkdir tmp/
	dotnet ../../bin/cellc-cs.dll project.txt tmp/
	cp main.cs water-sensor.csproj tmp/
	dotnet build -c Debug tmp/
	@echo
	@echo '********************************************************************************'
	@echo '********************************************************************************'
	@echo
	@echo 'To run the program type:'
	@echo '  dotnet run --project tmp/'

clean:
	@rm -rf tmp/
