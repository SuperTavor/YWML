#ifndef LOADWINDOW_H
#define LOADWINDOW_H

#include <QMainWindow>

namespace Ui {
class loadWindow;
}

class loadWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit loadWindow(QWidget *parent = nullptr);
    ~loadWindow();
    void applyDelta(const std::string oldFile, const std::string patchFile, const std::string newFile);

    void HandleError(const std::exception &ex);

    void print(std::string content);
private:
    Ui::loadWindow *ui;
};

#endif // LOADWINDOW_H
