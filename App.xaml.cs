using Microsoft.Extensions.DependencyInjection;

namespace Calulatrice_NGWEM_MAKENDI_ISAAC;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}