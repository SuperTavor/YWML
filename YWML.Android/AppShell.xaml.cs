using YWML.Android.Services;

namespace YWML.Android;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Navigating += OnNavigating;
	}

	private void OnNavigating(object? sender, ShellNavigatingEventArgs e)
	{
		if (CAppState.Current.ExtensionInstall.IsBusy && e.CanCancel)
		{
			e.Cancel();
		}
	}
}
