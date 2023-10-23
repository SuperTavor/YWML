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

private:
    Ui::loadWindow *ui;
};

#endif // LOADWINDOW_H
