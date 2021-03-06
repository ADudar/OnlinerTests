﻿using NUnit.Framework;
using OnlinerCore;

namespace OnlinerTests.Tests
{
    public abstract class TestsSetup
    {
        protected WebDriver _webDriver;
        protected Logger log;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            log = new Logger();
        }

        [SetUp]
        public void Setup()
        {
            log.CreateTest(TestContext.CurrentContext.Test.Name);
            _webDriver = new WebDriver(log);
        }

        [TearDown]
        public void TearDown()
        {
            log.WriteResults(_webDriver.Driver);
            _webDriver.Quit();
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            log.Flush();
        }
    }
}
