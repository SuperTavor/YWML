#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>
#include "CreateWindow.h"
MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}
MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::on_loadBtn_clicked()
{
    QMessageBox msg;
    msg.setText("Load Mod");
    msg.setWindowTitle("Placeholder Window.");
    msg.exec();
}


void MainWindow::on_createBtn_clicked()
{
    this->close();
    CreateWindow *wdg = new CreateWindow();
    wdg->setWindowFlags(Qt::Dialog | Qt::MSWindowsFixedSizeDialogHint);
    wdg->show();
    this->hide();
}

