set webdriver.chrome.driver=D:\OnlinerTests\OnlinerTests\bin\Debug\chromedriver.exe
echo %webdriver.chrome.driver%
java -Dwebdriver.chrome.driver=%webdriver.chrome.driver% -jar selenium-server-standalone-3.3.1.jar -role node 