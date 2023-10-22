#include "xdeltamanager.h"
#include <iostream>
#include <string>

xdeltamanager::xdeltamanager() {}

void xdeltamanager::applyPatch(std::string oldFile, std::string patchFile, std::string newFile)
{
    std::string command = "/C xdelta3.exe -e -s " + oldFile + " " + newFile + " " + patchFile;
    system(command.c_str());
}

void xdeltamanager::createPatch(std::string oldFile, std::string newFile, std::string patchFile)
{
    std::string command = "/C xdelta3.exe -d -s " + oldFile + " " + patchFile + " " + newFile;
    system(command.c_str());
}
