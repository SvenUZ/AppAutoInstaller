using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;

namespace Task_Manager
{
    internal class UpdateLinks
    {
        public static string UpdateSignal()
        {
            string url = "https://updates.signal.org/desktop/";
            var client = new WebClient();

            using (client)
            {
                client.DownloadFile("https://updates.signal.org/desktop/latest.yml", "latest.yml");
            }

            string yml = File.ReadAllText("latest.yml");
            Regex rx = new Regex(@"(?<=url: ).*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(yml);
            string version = "";
            for (int i = 0; i < matches.Count; i++)
            {
                version = matches[i].Value;
            }
            string latestLink = url + version;
            return latestLink;
        }

        public static string Update7Zip()
        {
            string latestLink = "";

            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load("http://7-zip.de");

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//*[@id='content']/article/div/table/tbody/tr[1]/td[1]/p/a"))
            {
                latestLink = link.OuterHtml;
            }

            Regex rx = new Regex(@"(https:).*e", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(latestLink);

            latestLink = matches[0].Value;

            return latestLink;
        }

        public static string UpdateDellUpdate()
        {
            string latestLink = "";

            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load("https://www.dell.com/support/home/de-de/drivers/DriversDetails?driverId=PWD0M");

            var nodes = doc.DocumentNode.SelectNodes("//a[@href]");
            foreach (var node in nodes)
            {
                string link = node.Attributes["href"].Value;
                if (link.Contains(".EXE"))
                {
                    //System.Windows.Forms.MessageBox.Show(node.Attributes["href"].Value);
                    latestLink = node.Attributes["href"].Value;
                }
            }

            return latestLink;
        }
    }
}
