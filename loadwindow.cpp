#include "loadwindow.h"
#include "ui_loadwindow.h"

loadWindow::loadWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::loadWindow)
{
    ui->setupUi(this);
}

loadWindow::~loadWindow()
{
    delete ui;
}
