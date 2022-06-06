EDITOR_FLAGS = -quit -projectPath $(shell pwd)
BUILD_DIR_BASE = build
PRODUCT_NAME = SpaceJob
VERSION = $(shell git describe --always)
EDITOR = tools/run_editor.sh

.PHONY: all
all: clean lighting linux win

.PHONY: lighting
lighting: EDITOR_FLAGS += -executeMethod Util.BakeLighting
lighting:
	$(EDITOR) $(EDITOR_FLAGS)

.PHONY: linux
linux: BUILD_DIR = $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-linux
linux: EXECUTABLE = $(BUILD_DIR)/$(PRODUCT_NAME)
linux: EDITOR_FLAGS += -buildLinux64Player $(EXECUTABLE)
linux:
	rm -fr $(BUILD_DIR)
	mkdir -p $(BUILD_DIR)
	$(EDITOR) $(EDITOR_FLAGS)
	zip -r $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-linux.zip $(BUILD_DIR)

.PHONY: win
win: BUILD_DIR = $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-win
win: EXECUTABLE = $(BUILD_DIR)/$(PRODUCT_NAME).exe
win: EDITOR_FLAGS += -buildWindows64Player $(EXECUTABLE)
win:
	rm -fr $(BUILD_DIR)
	mkdir -p $(BUILD_DIR)
	$(EDITOR) $(EDITOR_FLAGS)
	zip -r $(BUILD_DIR_BASE)/$(PRODUCT_NAME)-$(VERSION)-win.zip $(BUILD_DIR)

.PHONY: clean
clean:
	rm -fr $(BUILD_DIR_BASE)
