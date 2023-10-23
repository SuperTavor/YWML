#include "createwindow.h"
#include "ui_createwindow.h"
#include <string>
#include <fstream>
#include <iostream>
#include <QMessageBox>
#include <QFileDialog>
#include <filesystem>
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
    if (!gameComboBox->currentText().isEmpty() && !modNameLineEdit->text().isEmpty() && !modVersionLineEdit->text().isEmpty() && !authorLineEdit->text().isEmpty() && !moddedRomfsLineEdit->text().isEmpty())
    {
        QMessageBox msg;
        msg.setWindowTitle("All checks passed.");
        msg.setText("Initial checks passed. Press OK to start compilation of the mod file.");
        msg.exec();
        CreatePatch(gameComboBox->currentText(), modNameLineEdit->text(), modVersionLineEdit->text(), authorLineEdit->text(), moddedRomfsLineEdit->text());
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
    if(fs::exists("delta.bin"))
    {
        std::remove("delta.bin");
    }
    if(fs::exists("metadata.ywmd"))
    {
        std::remove("metadata.ywmd");
    }
    std::filesystem::path moddedRomfs = moddedRomfsPath.toStdString();
    std::filesystem::path vanillaRomfs = std::getenv("APPDATA");
    vanillaRomfs /= "Citra";
    vanillaRomfs /= "dump";
    vanillaRomfs /= "romfs";
    vanillaRomfs /= "0004000000167800";
    bool skipFaModded = false;
    bool skipMovModded = false;
    bool skipSndModded = false;
    fs::path vanillaRomfsPath = vanillaRomfs.string();
    if (!fs::exists(vanillaRomfsPath))
    {
        QMessageBox::critical(
            this,
            "Error!",
            "You need to dump the vanilla romfs of your game. To do that, right-click on the game in Citra and click Dump ROMFS. That's it! You don't have to transfer any files, YWML will automatically grab them after you dumped them."
            );
        return;
    }
    fs::path Vanillafa = vanillaRomfsPath / "yw1_a.fa";
    if (!fs::exists(Vanillafa))
    {
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
    if (skipFaModded == false)
    {
        if (!fs::exists(moddedFa))
        {
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
            }
            else {
                return;
            }
        }
    }
checkIfModdedMov:
    if (skipMovModded == false)
    {
        if (!fs::exists(moddedMov))
        {
            int choice = QMessageBox::warning(
                this,
                "Warning",
                "MOV folder not found in the provided modded romfs. This is not required for mods that don't replace video files.Continue?",
                QMessageBox::Cancel | QMessageBox::Ok,
                QMessageBox::Cancel
                );

            if (choice == QMessageBox::Ok) {
                skipMovModded = true;
                goto checkIfModdedMov;
            }
            else {
                return;
            }
        }
    }
checkIfModdedSnd:
    if (skipSndModded == false)
    {
        if (!fs::exists(moddedSnd))
        {
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
            }
            else {
                return;
            }
        }
    }
    try
    {
        patchFa(Vanillafa.string(), moddedFa.string(),skipFaModded);
    }
    catch (const std::exception &ex)
    {
        QMessageBox msg;
        msg.setText(QString::fromStdString(std::filesystem::current_path().string()));
        msg.exec();
        QMessageBox msgEx;
        QString errorMessage = QString::fromStdString(ex.what());
        msgEx.setText(errorMessage);
        msgEx.exec();
    }
    try
    {
        CreateMetadata();
    }
    catch (const std::exception &ex)
    {
        QMessageBox msg;
        msg.setText(QString::fromStdString(std::filesystem::current_path().string()));
        msg.exec();
        QMessageBox msgEx;
        QString errorMessage = QString::fromStdString(ex.what());
        msgEx.setText(errorMessage);
        msgEx.exec();
    }
}

int CreateWindow::patchFa(std::string vanillaFA, std::string moddedFA, bool isfa)
{
    if(isfa == false)
    {
        std::filesystem::path output = "delta.bin";
        std::string outputStr = output.string();
        fs::path currentDir = fs::current_path();
        QMessageBox msg2;
        try
        {
            createDelta(vanillaFA, moddedFA, outputStr);
        }
        catch (const std::exception &ex)
        {
            QMessageBox msg;
            msg.setText(QString::fromStdString(std::filesystem::current_path().string()));
            msg.exec();
            QMessageBox msgEx;
            QString errorMessage = QString::fromStdString(ex.what());
            msgEx.setText(errorMessage);
            msgEx.exec();
            return 1;
        }
    }
    else
    {
        return 0;
    }
}

void CreateWindow::on_browseBtn_clicked()
{
    QString directoryPath = QFileDialog::getExistingDirectory(this, "Open Folder", "", QFileDialog::ShowDirsOnly);
    QLineEdit *moddedRomfsPath = findChild<QLineEdit*>("moddedRomfsLineEdit");
    moddedRomfsPath->setText(directoryPath);
}

void CreateWindow::applyDelta(const std::string oldFile, const std::string patchFile, const std::string newFile)
{
    std::string command = "xdelta.exe -e -s " + oldFile + " " + newFile + " " + patchFile;
    std::system(command.c_str());
}

void CreateWindow::createDelta(std::string oldFile, std::string newFile, std::string patchFile)
{
    std::string command = "xdelta.exe -e -s " + oldFile + " " + newFile + " " + patchFile;

    FILE* pipe = popen(command.c_str(), "r");
    if (pipe == nullptr) {
        print("Errorc creating patch file.");
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
void CreateWindow::Compress(bool isMov,bool isSnd,bool isFa)
{

}
void CreateWindow::CreateMetadata()
{
        try
        {
            std::string modname = ui->modNameLineEdit->text().toStdString();
            std::string author = ui->authorLineEdit->text().toStdString();
            std::string modver = ui->versionLineEdit->text().toStdString();
            std::ofstream outfile ("metadata.ywmd");
            outfile << modname << std::endl;
            outfile << author << std::endl;
            outfile << modver << std::endl;
            outfile.close();
            print("Compilation step 2 complete.");
        }
        catch (const std::exception &ex)
        {
            QMessageBox msg;
            msg.setText(QString::fromStdString(std::filesystem::current_path().string()));
            msg.exec();
            QMessageBox msgEx;
            QString errorMessage = QString::fromStdString(ex.what());
            msgEx.setText(errorMessage);
            msgEx.exec();
            return;
        }


}
