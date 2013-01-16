using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiceAlumni.Theme.Helpers;

namespace RiceAlumni.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var events = PciEventCalendarHelper.GetEvents();
		}
	}
}
