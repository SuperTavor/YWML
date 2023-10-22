#ifndef XDELTAMANAGER_H
#define XDELTAMANAGER_H
#include <iostream>

class xdeltamanager
{
public:
    xdeltamanager();
private:
    void createPatch(std::string oldFile, std::string newFile, std::string patchFile);
    void applyPatch(std::string oldFile,std::string patchFile,std::string newFile);
};


#endif // XDELTAMANAGER_H
