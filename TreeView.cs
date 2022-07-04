using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Task_Manager
{
    public partial class TreeView : Form
    {
        public TreeView()
        {
            InitializeComponent();
            button1_Click(new object(), new EventArgs());


        }
        // Datenbank aktuallisieren
        public void button1_Click(object sender, EventArgs e)
        {
            // Prüft ob DB vorhanden ist
            if (File.Exists(@"db.json") == true)
            {
                using (var reader = new StreamReader("db.json"))
                using (var jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
                {
                    var root = JToken.Load(jsonReader);
                    DisplayTreeView(root, Path.GetFileNameWithoutExtension("db.json"));
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter("db.json");

                var firstList = new JArray();

                var firstElement = new JObject();
                firstElement["Name"] = "Platzhalter";
                firstElement["Link"] = "Platzhalter";

                firstList.Add(firstElement);

                var listSer = JsonConvert.SerializeObject(firstList, Formatting.Indented);

                // File.WriteAllText(@"db.json", listSer.ToString());
                sw.WriteLine(listSer.ToString());
                sw.Close();

                System.Windows.Forms.MessageBox.Show("Es wurde keine Datenbank gefunden, daher wurde eine erstellt");

                sw.Close();

            }

        }
        private void DisplayTreeView(JToken root, string rootName)
        {
            treeView1.BeginUpdate();
            try
            {
                treeView1.Nodes.Clear();
                var tNode = treeView1.Nodes[treeView1.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                treeView1.ExpandAll();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                //TextBox1.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }
        // "Eintrag hinzufügen"-Button
        private void button2_Click(object sender, EventArgs e)
        {
            Programm_bearbeiten frm = new Programm_bearbeiten();
            frm.ShowDialog();
        }
        //"Eintrag löschen"-Button
        private void button3_Click(object sender, EventArgs e)
        {
            Programm_Löschen table = new();
            table.ShowDialog();

        }
        //"Eintrag hinzufügen"-Button
        private void button4_Click(object sender, EventArgs e)
        {
            Programm_Hinzufügen table = new();
            // TreeView aktuallisieren
            table.ShowDialog();
        }

        //"Links aktuallisieren"-Button
        private async void button5_Click(object sender, EventArgs e)
        {
            // Laden der Datenbank
            string file = System.IO.File.ReadAllText(@"db.json");
            JArray jlinks = JArray.Parse(file);

            // Bekommen der aktuellen Links (Signal und 7-Zip)
            string SignalNewlink = UpdateLinks.UpdateSignal();
            string ZipNewLink = UpdateLinks.Update7Zip();
            string DellUpdateNewLink = UpdateLinks.UpdateDellUpdate();

            // Bekommen der Links aus der Datenbank
            foreach (JObject programm in jlinks)
            {
                string name = programm.GetValue("Name").ToString();
                string link = programm.GetValue("Link").ToString();

                // Ist der Name "Signal" in der Datenbank -> dann aktualisier den Link
                if (name.Contains("signal") || name.Contains("Signal"))
                {
                    var selectedProgrammIndex = jlinks.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == name)
                                         .Index;
                    var Jsignal = (JObject)jlinks[selectedProgrammIndex];
                    Jsignal["Link"] = SignalNewlink;
                }

                // Ist der Name "7-Zip" oder "7Zip" in der Datenbank -> dann aktualisier den Link
                if (name.Contains("7-zip") || name.Contains("7zip"))
                {
                    var selectedProgrammIndex = jlinks.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == name)
                                         .Index;
                    var J7zip = (JObject)jlinks[selectedProgrammIndex];
                    J7zip["Link"] = ZipNewLink;
                }
                // Ist der Name "Dell Updater" in der Datenbank -> dann aktualisier den Link
                if (name.Contains("dell updater"))
                {
                    var selectedProgrammIndex = jlinks.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == name)
                                         .Index;
                    var Jdellupdate = (JObject)jlinks[selectedProgrammIndex];
                    Jdellupdate["Link"] = ZipNewLink;
                }
            }

            // neue Links in Datenbank schreiben            
            var jsonToOutput = JsonConvert.SerializeObject(jlinks, Formatting.Indented);
            File.WriteAllText(@"db.json", jsonToOutput.ToString());
            button1_Click(new object(), new EventArgs());
        }
    }
}
