using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;

namespace Task_Manager
{
    public partial class Auswahl : Form
    {
        public Auswahl()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader(@"db.json");
            List<string> list = new List<string>();

            var srOutput = sr.ReadToEnd();
            var listDeser = JArray.Parse(srOutput);

            sr.Close();
            foreach (var item in listDeser)
            {
                // list.Add(item["Name"].ToString());
                checkedListBox1.Items.Add(item.Value<string>("Name"));
            }

            string[] programme = list.ToArray();

            checkedListBox1.CheckOnClick = true;
        }

        public List<string> GetCheckedBox()
        {
            var selectedLangs = new List<string>();

            foreach (var lang in checkedListBox1.CheckedItems)
            {
                selectedLangs.Add(lang.ToString());
            }
            return selectedLangs;
        }

        // "Installieren"-Knopf
        private void button1_Click(object sender, EventArgs e)
        {
            // Bekommen der markierten Programme 
            var selection = GetCheckedBox();
            var selectionMessage = selection.Aggregate((a, b) => a + ", " + b);

            // Reader für Datenbank initialisiert
            StreamReader sr = new StreamReader(@"db.json");

            // Datenbank als String-Datentyp lesen
            var programme = sr.ReadToEnd();

            // Parsen der Datenbank als JSON
            var jprogramme = JArray.Parse(programme);

            // Initialisieren der Variablen
            var selectedProgramm = new JObject();
            var selectedProgrammLinks = new List<string>();
            var selectedProgrammLinksAgg = "";

            // für jedes Element, welches markiert wurde, soll der Link aus der JSON entnommen werden und in eine Liste gepackt werden
            foreach (var item in selection)
            {
                var selectedProgrammIndex = jprogramme.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == item)
                                         .Index;
                selectedProgramm = (JObject)jprogramme[selectedProgrammIndex];

                selectedProgrammLinks.Add(selectedProgramm["Link"].ToString());
            }
            //selectedProgrammLinksAgg = selectedProgrammLinks.Aggregate((a, b) => a + ", " + b);
            // System.Windows.Forms.MessageBox.Show(selectedProgrammLinksAgg + " = " + selectionMessage);

            using (var client = new WebClient())
            {
                foreach (var link in selectedProgrammLinks)
                {
                    client.DownloadFile(link, selectedProgramm["Name"].ToString() + ".exe");
                }
            }
        }
    }
}
