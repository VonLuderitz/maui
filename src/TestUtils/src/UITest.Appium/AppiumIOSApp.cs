﻿using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using UITest.Core;

namespace UITest.Appium
{
	// https://appium.github.io/appium-xcuitest-driver/5.0/capabilities/
	public class AppiumIOSApp : AppiumApp, IIOSApp
	{
		public AppiumIOSApp(Uri remoteAddress, IConfig config)
			: base(new IOSDriver(remoteAddress, GetOptions(config)), config)
		{
			_commandExecutor.AddCommandGroup(new AppiumIOSMouseActions(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSTouchActions(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSSpecificActions(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSVirtualKeyboardActions(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSThemeChangeAction(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSAlertActions(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSThemeChangeAction(this));
			_commandExecutor.AddCommandGroup(new AppiumIOSStepperActions(this));
		}

		public override ApplicationState AppState
		{
			get
			{
				try
				{
					var appId = Config.GetProperty<string>("AppId") ?? throw new InvalidOperationException($"{nameof(AppState)} could not get the appid property");
					var state = _driver?.ExecuteScript("mobile: queryAppState", new Dictionary<string, object>
					{
						{ "bundleId", appId },
					});

					// https://developer.apple.com/documentation/xctest/xcuiapplicationstate?language=objc
					return Convert.ToInt32(state) switch
					{
						1 => ApplicationState.NotRunning,
						2 or
						3 or
						4 => ApplicationState.Running,
						_ => ApplicationState.Unknown,
					};
				}
				catch
				{
					return ApplicationState.Unknown;
				}
			}
		}

		private static AppiumOptions GetOptions(IConfig config)
		{
			config.SetProperty("PlatformName", "iOS");
			config.SetProperty("AutomationName", "XCUITest");

			var options = new AppiumOptions();
			SetGeneralAppiumOptions(config, options);

			var udid = config.GetProperty<string>("Udid");
			if (!string.IsNullOrWhiteSpace(udid))
			{
				options.AddAdditionalAppiumOption(MobileCapabilityType.Udid, udid);
			}

			var appId = config.GetProperty<string>("AppId");
			if (!string.IsNullOrWhiteSpace(appId))
			{
				options.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, appId);
			}

			var args = config.GetProperty<Dictionary<string, string>>("TestConfigurationArgs");
			options.AddAdditionalAppiumOption(IOSMobileCapabilityType.ProcessArguments, new Dictionary<string, object>
			{
				{ "env", args! }
			});

			// It can be faster for Appium to deal with the app hierarchy internally as JSON, rather than XML.
			config.SetProperty("useJSONSource", "true");
			
			var headless = config.GetProperty<bool>("Headless");
			
			if (headless)
			{
				// Appium has the ability to start iOS simulators in a "headless" mode.
				// This means that the devices won't have any graphical user interface; but they will still be running silently, testing the app.
				options.AddAdditionalAppiumOption("isHeadless", true);
			}

			return options;
		}
	}
}
