#include "mainwindow.h"

#include <QApplication>
#include <filesystem>
namespace fs = std::filesystem;
int main(int argc, char *argv[])
{
    if(fs::exists("delta.bin"))
    {
        std::remove("delta.bin");
    }
    if(fs::exists("metadata.ywmd"))
    {
        std::remove("metadata.ywmd");
    }
    if(fs::exists("mod.ywm"))
    {
        std::remove("mod.ywm");
    }
    if(fs::exists("mod.zip"))
    {
        std::remove("mod.zip");
    }
    QApplication a(argc, argv);
    MainWindow w;
    w.setWindowFlags(Qt::Dialog | Qt::MSWindowsFixedSizeDialogHint);
    w.show();
    return a.exec();
}
