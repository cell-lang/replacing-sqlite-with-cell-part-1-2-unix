send-msgs.dll: main.cell
	make -s clean
	mkdir tmp/
	dotnet ../../bin/cellc-cs.dll project.txt tmp/
	cp send-msgs.csproj tmp/
	dotnet build -c Debug tmp/
	@echo
	@echo '********************************************************************************'
	@echo '********************************************************************************'
	@echo
	@echo 'To run the program type:'
	@echo '  dotnet run --project tmp/ init-state.txt msg-list.txt final-state.txt'
	@echo '  cat final-state.txt'
	@echo ''
	@echo 'The program will load an initial state for the automaton from'
	@echo 'init-state.txt, a list of messages to send to it from msg-list.txt'
	@echo 'and will save its final state in the file final-state.txt'
	@echo ''

clean:
	@rm -rf final-state.txt tmp/
