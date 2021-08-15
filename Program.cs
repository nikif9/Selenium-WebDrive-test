using System;

namespace ConsoleApp1{
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;
    using CommandLine; 
   
    class Program{
        public class Options{
            [Option('d', "Department", Required = false, HelpText = "select department")]
            public string Department { get; set; }
            [Option('l', "Language", Required = false, HelpText = "select langugae")]
            public string Language { get; set; }
            [Option('e', "Experience", Required = false, HelpText = "select experience")]
            public string Experience { get; set; }
            [Option('r', "Region", Required = false, HelpText = "select region")]
            public string Region { get; set; }
            [Option('c', "CountVacancy", Required = false, HelpText = "expected result")]
            public int CountVacancy { get; set; }
        }
        static void Main(string[] args){
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://careers.veeam.ru/vacancies");
            driver.Manage().Window.Maximize();

            IWebElement emptySpace = driver.FindElement(By.XPath("//html"));

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>{
                if (o.Department != null){
                    driver.FindElement(By.XPath("//button[.='Все отделы']")).Click();
                    try{
                        driver.FindElement(By.LinkText(o.Department)).Click();
                    }
                    catch (Exception e){
                        System.Console.WriteLine($"не найден отдел с названием: {o.Department}");
                        emptySpace.Click();
                    }
                }else{
                    driver.FindElement(By.XPath("//button[.='Все отделы']")).Click();
                    driver.FindElement(By.LinkText("Разработка продуктов")).Click();
                }
                if (o.Language != null){ 
                    driver.FindElement(By.XPath("//button[.='Все языки']")).Click();
                    try{
                        driver.FindElement(By.XPath($"//label[.='{o.Language}']")).Click();
                    }
                    catch (Exception e){
                        System.Console.WriteLine($"не найден язык: {o.Language}");
                    }

                    emptySpace.Click();
                }else{
                    driver.FindElement(By.XPath("//button[.='Все языки']")).Click();
                    driver.FindElement(By.XPath("//label[.='Английский']")).Click();
                    emptySpace.Click();
                }
                if (o.Experience != null){
                    driver.FindElement(By.XPath("//button[.='Любой опыт']")).Click();
                    try{
                        driver.FindElement(By.LinkText(o.Experience)).Click();
                    }
                    catch (Exception e){
                        System.Console.WriteLine($"не найден опыт работы: {o.Experience}");
                        emptySpace.Click();
                    }
                    
                }
                if (o.Region != null){
                    driver.FindElement(By.XPath("//button[.='Любой регион']")).Click();
                    try{
                        driver.FindElement(By.LinkText(o.Region)).Click();
                    }
                    catch (Exception e){
                        System.Console.WriteLine($"не найден регион: {o.Experience}");
                        emptySpace.Click();
                    }
                    
                }
                int CountVacancy = driver.FindElements(By.ClassName("card-no-hover")).Count;
                if (o.CountVacancy > 0){
                    if (CountVacancy == o.CountVacancy){
                        System.Console.WriteLine($"ожидаемое число вакансий равна числу вакансий на сайте {o.CountVacancy} / {CountVacancy}");
                    }else if (CountVacancy > o.CountVacancy){
                        System.Console.WriteLine($"ожидаемое число вакансий меньше чем число вакансий на сайте {o.CountVacancy} / {CountVacancy}");
                    }else{
                        System.Console.WriteLine($"ожидаемое число вакансий больше чем число вакансий на сайте {o.CountVacancy} / {CountVacancy}");
                    }
                }else{
                    System.Console.WriteLine($"число вакансий на сайте равна: {CountVacancy}");
                }
            });
        }
    }
}
    