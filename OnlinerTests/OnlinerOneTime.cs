using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinerTests
{
    public class OnlinerOneTime
    {
        protected WebDriver _webDriver;
        protected Logger log;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            log = new Logger();
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            log.Flush();
        }
    }
}
