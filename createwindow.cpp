#include "createwindow.h"
#include "ui_createwindow.h"
#include <string>
#include <fstream>
#include <iostream>
#include <QMessageBox>
#include <QFileDialog>
#include <filesystem>
#include <vector>
#include <cstdio>

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

    if (gameComboBox && modNameLineEdit && modVersionLineEdit && authorLineEdit && moddedRomfsLineEdit &&
        !gameComboBox->currentText().isEmpty() && !modNameLineEdit->text().isEmpty() &&
        !modVersionLineEdit->text().isEmpty() && !authorLineEdit->text().isEmpty() &&
        !moddedRomfsLineEdit->text().isEmpty())
    {
        try {
            QMessageBox msg;
            msg.setWindowTitle("All checks passed.");
            msg.setText("Initial checks passed. Press OK to start compilation of the mod file.");
            msg.exec();
            CreatePatch(gameComboBox->currentText(), modNameLineEdit->text(), modVersionLineEdit->text(), authorLineEdit->text(), moddedRomfsLineEdit->text());
        } catch (const std::exception &ex) {
            HandleError(ex);
        }
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

void CreateWindow::CreatePatch(QString game, QString name, QString version, QString author, QString moddedRomfsPath)
{
    try {
        fs::path moddedRomfs = moddedRomfsPath.toStdString();
        fs::path vanillaRomfs = std::getenv("APPDATA");
        vanillaRomfs /= "Citra";
        vanillaRomfs /= "dump";
        vanillaRomfs /= "romfs";
        vanillaRomfs /= "0004000000167800";
        bool skipFaModded = false;
        bool skipMovModded = false;
        bool skipSndModded = false;
        fs::path vanillaRomfsPath = vanillaRomfs.string();

        if (!fs::exists(vanillaRomfsPath)) {
            QMessageBox::critical(
                this,
                "Error!",
                "You need to dump the vanilla romfs of your game. To do that, right-click on the game in Citra and click Dump ROMFS. That's it! You don't have to transfer any files, YWML will automatically grab them after you dumped them."
                );
            return;
        }

        fs::path Vanillafa = vanillaRomfsPath / "yw1_a.fa";

        if (!fs::exists(Vanillafa)) {
            QMessageBox::critical(
                this,
                "Error!",
                "Invalid or corrupt vanilla romfs. FA not found. Please erase your dumped romfs and redump it."
                );
            return;
        }

        fs::path moddedFa = moddedRomfs / "yw1_a.fa";
        fs::path moddedMov = moddedRomfs / "mov";
        fs::path moddedSnd = moddedRomfs / "snd";

    checkIfModdedFA:
        if (skipFaModded == false) {
            if (!fs::exists(moddedFa)) {
                int choice = QMessageBox::warning(
                    this,
                    "Warning",
                    "FA file not found in the provided modded romfs. Continue?",
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

    checkIfModdedMov:
        if (skipMovModded == false) {
            if (!fs::exists(moddedMov)) {
                int choice = QMessageBox::warning(
                    this,
                    "Warning",
                    "MOV folder not found in the provided modded romfs. This is not required for mods that don't replace video files. Continue?",
                    QMessageBox::Cancel | QMessageBox::Ok,
                    QMessageBox::Cancel
                    );

                if (choice == QMessageBox::Ok) {
                    skipMovModded = true;
                    goto checkIfModdedMov;
                } else {
                    return;
                }
            }
        }

    checkIfModdedSnd:
        if (skipSndModded == false) {
            if (!fs::exists(moddedSnd)) {
                int choice = QMessageBox::warning(
                    this,
                    "Warning",
                    "SND folder not found in the provided modded romfs. This is not required for mods that don't replace audio files. Continue?",
                    QMessageBox::Cancel | QMessageBox::Ok,
                    QMessageBox::Cancel
                    );

                if (choice == QMessageBox::Ok) {
                    skipSndModded = true;
                    goto checkIfModdedSnd;
                } else {
                    return;
                }
            }
        }

        patchFa(Vanillafa.string(), moddedFa.string(), skipFaModded);
        CreateMetadata();
        Compress(skipMovModded, skipSndModded, skipFaModded, moddedRomfsPath.toStdString());
    } catch (const std::exception &ex) {
        HandleError(ex);
    }
}

int CreateWindow::patchFa(std::string vanillaFA, std::string moddedFA, bool isfa)
{
    if (isfa == false)
    {
        fs::path output = "delta.bin";
        std::string outputStr = output.string();
        fs::path currentDir = fs::current_path();
        QMessageBox msg2;

        try {
            createDelta(vanillaFA, moddedFA, outputStr);
        } catch (const std::exception &ex) {
            HandleError(ex);
            return 1;
        }
    }
    return 0;
}

void CreateWindow::on_browseBtn_clicked()
{
    QString directoryPath = QFileDialog::getExistingDirectory(this, "Open Folder", "", QFileDialog::ShowDirsOnly);
    QLineEdit *moddedRomfsPath = findChild<QLineEdit*>("moddedRomfsLineEdit");
    moddedRomfsPath->setText(directoryPath);
}


void CreateWindow::createDelta(std::string oldFile, std::string newFile, std::string patchFile)
{
    std::string command = "xdelta.exe -e -s " + oldFile + " " + newFile + " " + patchFile;

    FILE* pipe = popen(command.c_str(), "r");
    if (pipe == nullptr) {
        print("Error creating patch file.");
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
        print("Compilation step 1 complete.");
    } else {
        print("Error output:\n" + output);
    }
}

void CreateWindow::print(std::string content)
{
    QMessageBox msg;
    msg.setText(QString::fromStdString(content));
    msg.exec();
}

int CreateWindow::Compress(bool isMov, bool isSnd, bool isFa, std::string moddedRomfs)
{
    std::string command = "powershell -Command Compress-Archive -Path \"metadata.ywmd\"";

    if (!isMov)
    {
        command += ", \"/mov\"";
    }

    if (!isSnd)
    {
        command += ", \"snd\"";
    }

    if (!isFa)
    {
        command += ", \"delta.bin\"";
    }

    command += " -DestinationPath \"mod.zip\" -Force";

    int rs = system(command.c_str());

    if(rs != 0)
    {
        return 1;
    }

    std::rename("mod.zip", "mod.ywm");
    QString targetFilePath = QFileDialog::getSaveFileName(nullptr, "Save Mod File As", "", "Yo-kai Watch Mod Files (*.ywm)");

    QString sourceFilePath = "mod.ywm";

    if (QFile::copy(sourceFilePath, targetFilePath)) {
        QFile::remove(sourceFilePath);
        print("File saved! Enjoy :)");
        return 0;
    } else {
        print("Couldn't move the file. Please try again.");
        return 1;
    }
}

int CreateWindow::CreateMetadata()
{
    try
    {
        std::string modname = ui->modNameLineEdit->text().toStdString();
        std::string author = ui->authorLineEdit->text().toStdString();
        std::string modver = ui->versionLineEdit->text().toStdString();
        std::ofstream outfile("metadata.ywmd");
        outfile << modname << std::endl;
        outfile << author << std::endl;
        outfile << modver << std::endl;
        outfile.close();
        print("Compilation step 2 complete.");
        return 0;
    }
    catch (const std::exception &ex)
    {
        HandleError(ex);
        return 1;
    }
}

void CreateWindow::HandleError(const std::exception &ex)
{
    QMessageBox msg;
    msg.setText(QString::fromStdString(fs::current_path().string()));
    msg.exec();
    QMessageBox msgEx;
    QString errorMessage = QString::fromStdString(ex.what());
    msgEx.setText(errorMessage);
    msgEx.exec();
}
