#include "createwindow.h"
#include "ui_createwindow.h"
#include <string>
#include <fstream>
#include <iostream>
#include <QMessageBox>
#include <QFileDialog>
#include <filesystem>
#include "xdeltamanager.h"
namespace fs = std::filesystem;
CreateWindow::CreateWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::CreateWindow)
{
    ui->setupUi(this);
}

CreateWindow::~CreateWindow()
{
    delete ui;
}

void CreateWindow::on_createBtn_clicked()
{
    QComboBox *gameComboBox = findChild<QComboBox*>("gameComboBox");
    QLineEdit *modNameLineEdit = findChild<QLineEdit*>("modNameLineEdit");
    QLineEdit *modVersionLineEdit = findChild<QLineEdit*>("versionLineEdit");
    QLineEdit *authorLineEdit = findChild<QLineEdit*>("authorLineEdit");
    QLineEdit *moddedRomfsLineEdit = findChild<QLineEdit*>("moddedRomfsLineEdit");
    if(!gameComboBox->currentText().isEmpty() && !modNameLineEdit->text().isEmpty() && !modVersionLineEdit->text().isEmpty() && !authorLineEdit->text().isEmpty() && !moddedRomfsLineEdit->text().isEmpty())
    {
        QMessageBox msg;
        msg.setWindowTitle("All checks passed.");
        msg.setText("Initial checks passed. Press OK to start compilation of the mod file.");
        msg.exec();
        CreatePatch(gameComboBox->currentText(),modNameLineEdit->text(),modVersionLineEdit->text(),authorLineEdit->text(),moddedRomfsLineEdit->text());
    }
    else
    {
        QMessageBox::critical(
            this,
            "Error!",
            "Please fill out all the fields before creating the patch."
            );
    }
}

void CreateWindow::CreatePatch(QString game,QString name,QString version,QString author,QString moddedRomfsPath)
{
    std::filesystem::path moddedRomfs = moddedRomfsPath.toStdString();
    std::filesystem::path vanillaRomfs = std::getenv("APPDATA");
    vanillaRomfs /= "Citra";
    vanillaRomfs /= "dump";
    vanillaRomfs /= "romfs";
    vanillaRomfs /= "0004000000167800";
    bool skipFaModded;
    fs::path vanillaRomfsPath = vanillaRomfs.string();
    if(!fs::exists(vanillaRomfsPath))
    {
        QMessageBox::critical(
            this,
            "Error!",
            "You need to dump the vanilla romfs of your game. to do that, right click on the game in Citra and click Dump ROMFS. That's it! You don't have to transfer any files, YWML will automatically grab them after you dumped them."
            );
        return;
    }
    fs::path Vanillafa = vanillaRomfsPath / "yw1_a.fa";
    if(!fs::exists(Vanillafa))
    {
        QMessageBox::critical(
            this,
            "Error!",
            "Invalid or corrupt vanilla romfs. FA not found. Please erase your dumped romfs and redump it."
            );
        return;
    }
    fs::path moddedFa = moddedRomfs / "yw1_a.fa";
    checkIfModdedFA:
    if(skipFaModded == false)
    {
        if(!fs::exists(moddedRomfs))
        {
            int choice = QMessageBox::warning(
                this,
                "Warning",
                "FA file not found in provided modded romfs. Continue?",
                QMessageBox::Cancel | QMessageBox::Ok,
                QMessageBox::Cancel
                );

            if (choice == QMessageBox::Ok) {
                skipFaModded = true;
                goto checkIfModdedFA;
            } else {
                return;
            }
        }
    }
    QMessageBox msg;
    msg.setText("test");
    msg.exec();


}

void CreateWindow::on_browseBtn_clicked()
{
    QString directoryPath = QFileDialog::getExistingDirectory(this, "Open Folder", "", QFileDialog::ShowDirsOnly);
    QLineEdit *moddedRomfsPath = findChild<QLineEdit*>("moddedRomfsLineEdit");
    moddedRomfsPath->setText(directoryPath);
}

