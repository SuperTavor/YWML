#ifndef CREATEWINDOW_H
#define CREATEWINDOW_H

#include <QMainWindow>

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

private:
    Ui::CreateWindow *ui;
};

#endif // CREATEWINDOW_H
