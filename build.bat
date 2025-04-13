@echo off
mkdir Build

xmake f -m release
xmake -a

xmake f -m debug
xmake -a

REM Create a zip file containing both Release and Debug folders
cd Build
tar -a -c -f Crush-Crush-Cheat.zip Release Debug && move Crush-Crush-Cheat.zip ..
cd ..