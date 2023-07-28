using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Text;
using System.Globalization;
using better.Classes;

namespace better.Modules
{
    public class SlotsOfVegas
    {
        IWebDriver webDriver = new ChromeDriver();
        public bool isLoggedIn = false;
        public float balance = 0;
        public List<Tournament> Tournaments = new List<Tournament>();

        public SlotsOfVegas(string email, string password)
        {
            Console.WriteLine("Inializing SlotsOfVegas.com");
            webDriver.Navigate().GoToUrl("https://www.slotsofvegas.com/");
            Console.WriteLine("SlotsOfVegas.com initialized");
            LoginInclave(email, password);
            checkLoginState();
            if (isLoggedIn)
            {
                Console.WriteLine("Logged in to SlotsOfVegas.com");
            }
            else
            {
                Console.WriteLine("ERROR: Failed to log in to SlotsOfVegas.com");
                Exception e = new Exception("Failed to log in to SlotsOfVegas.com");
                throw e;
            }
            Console.WriteLine("Getting Tournaments");
            getTournaments();
            Console.WriteLine("Tournaments retrieved");

        }

        public void LoginInclave(string email, string password)
        {
            Console.WriteLine("Logging in to SlotsOfVegas.com via Inclave SSO");

            var loginButton = webDriver.FindElement(By.XPath("/html/body/header/div/div/ul/li[4]/span/a[1]"));
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            IWebElement loginModal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"lmModalOverlay\"]/div/div[1]/div/a")));

            if (loginModal.Displayed)
            {
                Console.WriteLine("Login modal is displayed");
            }
            else
            {
                Console.WriteLine("ERROR: Login modal is not displayed");
                Exception e = new Exception("Login modal is not displayed");
                throw e;
            }

            var ssoLoginButton = webDriver.FindElement(By.XPath("//*[@id=\"lmModalOverlay\"]/div/div[1]/div/a"));

            ssoLoginButton.Click();
            IWebElement ssoLoginForm = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app\"]/div")));

            if (ssoLoginForm.Displayed)
            {
                Console.WriteLine("SSO login form is displayed");
            }
            else
            {
                Console.WriteLine("ERROR: SSO login form is not displayed");
                Exception e = new Exception("SSO login form is not displayed");
                throw e;
            }

            var emailInput = webDriver.FindElement(By.CssSelector("#input-email"));
            emailInput.Click();
            emailInput.SendKeys(email);
            var passwordInput = webDriver.FindElement(By.CssSelector("#input-password"));
            passwordInput.Click();
            passwordInput.SendKeys(password);
            var loginButton2 = webDriver.FindElement(By.CssSelector("#login-wrapper > form > div.row.d-flex.justify-content-center.pt-4 > div:nth-child(1) > div > button"));
            loginButton2.Click();

            checkLoginState();

        }
        public bool checkLoginState()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            IWebElement isLoggedIn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#uph > div.fullbar > div.right > div.headerlogged.desktop > div.player")));

            if (isLoggedIn.Text.Contains("Ripley"))
            {
                Console.WriteLine("Login successful");
                this.isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("Login failed");
                this.isLoggedIn = false;
            }
            return this.isLoggedIn;
        }

        public void getTournaments()
        {
            webDriver.Navigate().GoToUrl("https://www.slotsofvegas.com/tournaments-lobby.php");
            var tableXPath = "//*[@id=\"tournaments-lobby\"]/div/div/div/div[1]";
            var selectedDetailsXPath = "//*[@id=\"tournaments-lobby\"]/div/div/div[2]";
            var tournamentPlayersXPath = "//*[@id=\"tournaments-lobby\"]/div/div/div[2]/div/div[2]/div/div[1]/div[2]/div[3]/div/text()";
            int currentRow = 0;
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            IWebElement tournamentTable = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#tournaments-lobby > div > div > div > div.v-window.v-item-group.theme--dark.v-tabs-items > div > div.v-window-item.v-window-item--active > div > div > table > tbody")));
            var tournamentRows = tournamentTable.FindElements(By.TagName("tr"));

            foreach (var row in tournamentRows)
            {
                //Select row item
                row.Click();

                //Select the details tab
                IWebElement details = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(selectedDetailsXPath)));
                if (details.Displayed)
                {
                    Tournament tournament = new Tournament();
                    var tournamentDetailsContainer = details.FindElements(By.CssSelector("#tournaments-lobby > div > div > div.col.col-5 > div > div.v-card__text.tournamentDetail.tournamentDetailTxt--text > div > div.row.align-center > div.py-0.col-md-6.col-lg-5.col-xl-4.col > div.container.pa-1"));
                    var tournamentDetails = tournamentDetailsContainer[0].FindElements(By.ClassName("row"));
                    var id = tournamentDetails[0].FindElements(By.TagName("span"))[1].Text;
                    tournament.id = int.Parse(id);
                    tournament.Mode = tournamentDetails[1].FindElements(By.TagName("span"))[1].Text;
                    tournament.Status = tournamentDetails[2].FindElements(By.TagName("span"))[1].Text;
                    //parse datetime from string in format mm/yy hh:mm
                    tournament.StartDate = DateTime.ParseExact(tournamentDetails[3].FindElements(By.TagName("span"))[1].Text, "MM/dd HH:mm", CultureInfo.InvariantCulture);

                    tournament.EndDate = DateTime.ParseExact(tournamentDetails[4].FindElements(By.TagName("span"))[1].Text, "MM/dd HH:mm", CultureInfo.InvariantCulture);
                    var players = webDriver.FindElement(By.XPath("//*[@id=\"tournaments-lobby\"]/div/div/div[2]/div/div[2]/div/div[1]/div[2]/div[3]/div")).Text;
                    var playersArray = players.Split('/');
                    tournament.Players = int.Parse(playersArray[0]);
                    tournament.MaxPlayers = int.Parse(playersArray[1]);
                    var tournamentCells = row.FindElements(By.TagName("td"));
                    tournament.Name = tournamentCells[1].Text;
                    tournament.Game = tournamentCells[1].Text;
                    tournament.PrizePool = float.Parse(webDriver.FindElement(By.XPath("//*[@id=\"tournaments-lobby\"]/div/div/div[2]/div/div[2]/div/div[1]/div[2]/div[1]/div/span")).Text.Replace("$", ""));
                    tournament.BuyIn = float.Parse(tournamentCells[4].Text.Replace("$", ""));
                    tournament.Status = tournamentCells[3].Text;
                    //add the tournament to list if it is not already in the list
                    if (!Tournaments.Any(t => t.id == tournament.id))
                    {
                        Tournaments.Add(tournament);
                        Console.WriteLine("Added tournament: " + tournament.Name);
                    }
                    else
                    {
                        continue;
                    }
                    currentRow++;
                }
                else
                {
                    Console.WriteLine("ERROR: Tournament details not displayed");
                    Exception e = new Exception("Tournament details not displayed");
                    throw e;
                }
            }
        }
    }
}

