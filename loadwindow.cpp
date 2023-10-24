#include "loadwindow.h"
#include "ui_loadwindow.h"
#include <QMessageBox>
#include <filesystem>
namespace fs = std::filesystem;

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

void loadWindow::applyDelta(const std::string oldFile, const std::string patchFile, const std::string newFile)
{
    std::string command = "xdelta.exe -d -s " + oldFile + " " + patchFile + " " + newFile;

    FILE* pipe = popen(command.c_str(), "r");
    if (pipe == nullptr) {
        print("Error applying patch file.");
        return;
    }

    char buffer[128];
    std::string output;

    while (fgets(buffer, sizeof(buffer), pipe) != nullptr) {
        output += buffer;
    }

    int result = pclose(pipe);
    print(command);

    if (result == 0) {
        print("Applying step 2 complete.");
    } else {
        print("Error output:\n" + output);
    }
}

void loadWindow::HandleError(const std::exception &ex)
{
    QMessageBox msg;
    msg.setText(QString::fromStdString(fs::current_path().string()));
    msg.exec();
    QMessageBox msgEx;
    QString errorMessage = QString::fromStdString(ex.what());
    msgEx.setText(errorMessage);
    msgEx.exec();
}

void loadWindow::print(std::string content)
{
    QMessageBox msg;
    msg.setText(QString::fromStdString(content));
    msg.exec();
}
