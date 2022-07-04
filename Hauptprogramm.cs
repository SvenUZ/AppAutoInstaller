using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;



namespace Task_Manager
{
    public partial class Hauptprogramm : Form
    {
        public Hauptprogramm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void Download(string name, string link)
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            wc.DownloadFileAsync(new Uri(link), name);
        }
        public void wc_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        // "Install Signal"-Button
        void button1_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("db.json");
            var array = JArray.Parse(sr.ReadToEnd());

            var selectedIndex = array.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == "Signal")
                                         .Index;

            var jprogramm = (JObject)array[selectedIndex];
            Download(jprogramm["Name"].ToString() + ".exe", jprogramm["Link"].ToString());
        }

        // Funktion "IsValidJson"
        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TreeView table = new();

            table.ShowDialog();
        }
        // "zum Katalog hinzufügen / initiallisieren"-Button
        private void button2_Click_1(object sender, EventArgs e)
        {
            string filepath = "db.json";
            string inFile = "";

            bool dbExist = File.Exists(filepath);

            // wenn keine "db.json" exisitiert
            if (dbExist == false)
            {
                StreamWriter sw = new StreamWriter(filepath);

                var firstList = new JArray();

                var firstElement = new JObject();
                firstElement["Name"] = "Platzhalter";
                firstElement["Link"] = "Platzhalter";

                firstList.Add(firstElement);

                var listSer = JsonConvert.SerializeObject(firstList, Formatting.Indented);

                // File.WriteAllText(@"db.json", listSer.ToString());
                sw.WriteLine(listSer.ToString());
                sw.Close();

                System.Windows.Forms.MessageBox.Show("Datenbank erstellt und Initialisiert!");

                sw.Close();
            }
            // wenn "db.json" exisitert
            else
            {
                // wenn "db.json" leer ist
                if (new FileInfo(filepath).Length == 0)
                {
                    var firstList = new JArray();

                    var firstElement = new JObject();
                    firstElement["Name"] = "Platzhalter";
                    firstElement["Link"] = "Platzhalter";

                    firstList.Add(firstElement);

                    var listSer = JsonConvert.SerializeObject(firstList, Formatting.Indented);

                    // File.WriteAllText(@"db.json", listSer.ToString());
                    StreamWriter sw = new StreamWriter(filepath);
                    sw.WriteLine(listSer.ToString());
                    sw.Close();

                    System.Windows.Forms.MessageBox.Show("Initialisiert!");
                }
                // wenn "db.json" gefüllt ist
                else
                {
                    StreamReader sr = new StreamReader(filepath);
                    bool valid = IsValidJson(sr.ReadToEnd());
                    sr.Close();
                    // richtige JSON
                    if (valid == true)
                    {
                        // Startet Eingabe-Fenster
                        Programm_Hinzufügen table = new();
                        table.ShowDialog();
                    }
                    // etwas anderes
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Die Datenbank enthält andere Informationen, Datei wird gelöscht");
                        // Löscht Datei
                        System.IO.File.Delete(filepath);
                    }
                }
            }
        }

        // Auswahl-Menü
        private void button4_Click_1(object sender, EventArgs e)
        {
            Auswahl table1 = new();
            table1.ShowDialog();
        }
    }
}