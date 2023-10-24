#ifndef CREATEWINDOW_H
#define CREATEWINDOW_H
#include <filesystem>
#include <QMainWindow>
namespace fs = std::filesystem;
namespace Ui {
class CreateWindow;
}

class CreateWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit CreateWindow(QWidget *parent = nullptr);
    ~CreateWindow();

private slots:
    void on_createBtn_clicked();

    void on_browseBtn_clicked();

    void CreatePatch(QString game,QString name,QString version,QString author,QString moddedRomfsPath);

    int patchFa(std::string vanillaFA,std::string moddedFA,bool isfa);

    void createDelta(std::string oldFile,std::string newFile, std::string patchFile);

    void print(std::string content);

    int Compress(bool isMov,bool isSnd,bool isFa,std::string moddedRomfs);

    int CreateMetadata();

    void HandleError(const std::exception &ex);

private:
    Ui::CreateWindow *ui;
};

#endif // CREATEWINDOW_H
