#!/bin/sh
dotnet build
TARGET_DIR="Tpk"
BUILD_DIR="bin/Debug/tizen10.0/"
WIDGETTEST="Tizen.NUI.WidgetTest-1.0.0.tpk"
NUITIZENGALLERY="org.tizen.example.NUITizenGallery-1.0.0.tpk"
cp Tizen.NUI.WidgetTest/$BUILD_DIR$WIDGETTEST $TARGET_DIR
echo "$WIDGETTEST is stored in $TARGET_DIR"
cp NUITizenGallery/$BUILD_DIR$NUITIZENGALLERY $TARGET_DIR
echo "$NUITIZENGALLERY is stored in $TARGET_DIR"