using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Build.Tasks;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests;

[XamlCompilation(XamlCompilationOptions.Skip)]
[XamlProcessing(XamlInflator.Runtime, true)]
public partial class Gh3082 : ContentPage
{
	public Gh3082() => InitializeComponent();

	static async Task OnClicked(object sender, EventArgs e) => await Task.Delay(1000);

	[TestFixture]
	class Tests
	{
		[Test]
		public void ThrowsOnWrongEventHandlerSignature([Values] XamlInflator inflator)
		{
			if (inflator == XamlInflator.XamlC)
				Assert.Throws<BuildException>(() => MockCompiler.Compile(typeof(Gh3082)));
			else if (inflator == XamlInflator.Runtime)
				Assert.Throws<XamlParseException>(() => new Gh3082(inflator));
			else if (inflator == XamlInflator.SourceGen)
			{
				var result = MockSourceGenerator.RunMauiSourceGenerator(MockSourceGenerator.CreateMauiCompilation(), typeof(Gh3082));
				Assert.AreEqual(1, result.Diagnostics.Length);
			}
			else
				Assert.Ignore($"Test not supported for {inflator}");
				
		}
	}
}
