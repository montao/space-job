EDITOR_FLAGS = -quit -projectPath $(shell pwd)
BUILD_DIR_BASE = build
PRODUCT_NAME = SpaceJob
VERSION = $(shell git describe --always)
EDITOR = tools/run_editor.sh

.PHONY: all
all: check clean lighting win linux

.PHONY: lighting
lighting: EDITOR_FLAGS += -executeMethod Util.BakeLighting
lighting:
	$(EDITOR) $(EDITOR_FLAGS)
	git checkout Assets/LightingSettings.lighting

.PHONY: linux
linux: BUILD_DIR = $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-linux
linux: EXECUTABLE = $(BUILD_DIR)/$(PRODUCT_NAME)
linux: EDITOR_FLAGS += -buildLinux64Player $(EXECUTABLE)
linux:
	rm -fr $(BUILD_DIR)
	mkdir -p $(BUILD_DIR)
	$(EDITOR) $(EDITOR_FLAGS)
	rm -fr $(BUILD_DIR)/*_DoNotShip
	zip -r $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-linux.zip $(BUILD_DIR)

.PHONY: win
win: BUILD_DIR = $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-win
win: EXECUTABLE = $(BUILD_DIR)/$(PRODUCT_NAME).exe
win: EDITOR_FLAGS += -buildWindows64Player $(EXECUTABLE)
win:
	rm -fr $(BUILD_DIR)
	mkdir -p $(BUILD_DIR)
	$(EDITOR) $(EDITOR_FLAGS)
	rm -fr $(BUILD_DIR)/*_DoNotShip
	zip -r $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-win.zip $(BUILD_DIR)

.PHONY: clean
clean:
	rm -fr $(BUILD_DIR_BASE)

.PHONY: check
check:
	@[ -z $(git status -s) ] \
		&& echo "Please ensure your working tree is clean." \
		&& echo "Check git status for any uncommitted changes and try again." \
		&& exit 1
