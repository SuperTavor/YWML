#include "mainwindow.h"

#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;
    w.setWindowFlags(Qt::Dialog | Qt::MSWindowsFixedSizeDialogHint);
    w.show();
    return a.exec();
}
