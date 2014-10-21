﻿using System.IO;
using L10NSharp.UI;
using NUnit.Framework;

namespace L10NSharp.Tests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// These tests need to create a "real" Localization Manager, but with the capability of
	/// removing all trace of it after the tests.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	class ILocalizableComponentTests
	{
		private LocalizationManager m_manager;
		private L10NSharpExtender m_extender;
		private string m_tmxPath;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Setup for each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			var installedTmxDir = "../../src/L10NSharpTests/TestTmx";
			m_manager = LocalizationManager.Create("en", "Test", "Test", "1.0", installedTmxDir, "", null, "");
			m_tmxPath = m_manager.GetTmxPathForLanguage("en", true);
			m_extender = new L10NSharpExtender();
			m_extender.LocalizationManagerId = "Test";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Teardown for each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTeardown()
		{
			m_extender = null;
			m_manager = null;
			var localAppDataDir = Directory.GetParent(Path.GetDirectoryName(m_tmxPath));
			Directory.Delete(localAppDataDir.FullName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that we can localize an ILocalizableComponent object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TestLocalizingALocalizableComponent()
		{
			// Setup test
			m_extender.BeginInit(); // Doesn't currently do anything, but for completeness...
			var locComponent = new MockLocalizableComponent();
			m_extender.SetLocalizingId(locComponent, "TestLocalizableComponent");

			// SUT
			m_extender.EndInit();

			// Verify English
			Assert.AreEqual("It's a crow", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Crow"));
			Assert.AreEqual("It's not a crow", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Raven"));
			Assert.AreEqual("It's a chicken", locComponent.GetLocalizedStringFromMock(locComponent.ChickenButton, "TestItem.Chicken.Rooster"));
			Assert.AreEqual("Fish-eating bird", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Eagle"));

			// SUT2
			LocalizationManager.SetUILanguage("fr", true);

			// Verify French
			Assert.AreEqual("C'est un corbeau", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Crow"));
			Assert.AreEqual("Ce n'est pas un corbeau", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Raven"));
			Assert.AreEqual("C'est un poulet", locComponent.GetLocalizedStringFromMock(locComponent.ChickenButton, "TestItem.Chicken.Rooster"));
			Assert.AreEqual("Un oiseau qui mange des poissons", locComponent.GetLocalizedStringFromMock(locComponent.BirdButton, "TestItem.Bird.Eagle"));
		}
	}
}
