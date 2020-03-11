.PHONY: purge clean default install uninstall setup
.DEFAULT_GOAL := default

ARTIFACTS 		:= $(shell pwd)/artifacts
BUILD			:= $(shell pwd)/.build
CONFIGURATION	:= Release
CLI_PROJECT		:= src/GitBrowse/GitBrowse.csproj
CLI_TOOL		:= git-browse
RUNTIME 		:= linux-x64

purge: clean
	rm -rf .build
	rm -rf artifacts

clean:
	dotnet clean -c Debug
	dotnet clean -c Release

run:
	dotnet run --project $(CLI_PROJECT)

restore:
	dotnet restore
	dotnet tool restore

default: clean restore
	$(MAKE) package

package:
	dotnet build $(CLI_PROJECT) -c $(CONFIGURATION)
	dotnet pack $(CLI_PROJECT) --configuration $(CONFIGURATION) \
		--no-build \
		--output $(ARTIFACTS) \
		--include-symbols

package-native:
	dotnet publish $(CLI_PROJECT) -c $(CONFIGURATION) \
		--output $(BUILD)/publish/$(RUNTIME) \
		--runtime $(RUNTIME) \
		/p:Mode=CoreRT-High

	@mkdir -p $(ARTIFACTS)
	@cp $(BUILD)/publish/$(RUNTIME)/$(CLI_TOOL) $(ARTIFACTS)/$(CLI_TOOL).$(RUNTIME)

install:
	dotnet tool install --global --add-source $(ARTIFACTS) \
		--version $$(dotnet minver -t v -a minor -v e) \
		$(CLI_TOOL)

uninstall:
	dotnet tool uninstall --global $(CLI_TOOL)

checksum:
	@rm -f $(ARTIFACTS)/SHA256SUMS.txt
	@cd $(ARTIFACTS) && find . -type f -print0 | xargs -0 sha256sum | tee SHA256SUMS.txt

validate:
	@cd $(ARTIFACTS) && cat SHA256SUMS.txt | sha256sum -c -
