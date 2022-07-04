using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Task_Manager
{
    public partial class Programm_Hinzufügen : Form
    {
        public Programm_Hinzufügen()
        {

            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filepath = "db.json";

            // wenn Datei exisitiert
            if (File.Exists(filepath))
            {
                string inFile = System.IO.File.ReadAllText(filepath);
                bool valid = Hauptprogramm.IsValidJson(inFile);

                // Datei exisitert, aber leer
                if (new FileInfo(filepath).Length == 0)
                {
                    var firstList = new JArray();

                    var firstElement = new JObject();
                    firstElement["Name"] = textBox1.Text;
                    firstElement["Link"] = textBox2.Text;

                    firstList.Add(firstElement);

                    var listSer = JsonConvert.SerializeObject(firstList, Formatting.Indented);

                    File.WriteAllText(@"db.json", listSer.ToString());
                    TreeView tv = Application.OpenForms.OfType<TreeView>().FirstOrDefault();
                    if (tv != null)
                    {
                        tv.button1_Click(sender, e);
                    }
                }
                // Datei exisitiert und gefüllt
                else
                {
                    // Datei ist eine valide JSON
                    if (valid == true)
                    {
                        var array = JArray.Parse(inFile);

                        var itemtoadd = new JObject();
                        itemtoadd["Name"] = textBox1.Text;
                        itemtoadd["Link"] = textBox2.Text;
                        array.Add(itemtoadd);

                        var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);

                        File.WriteAllText(@"db.json", jsonToOutput.ToString());
                        this.Close();

                        TreeView tv = Application.OpenForms.OfType<TreeView>().FirstOrDefault();
                        if (tv != null)
                        {
                            tv.button1_Click(sender, e);
                        }
                        /*
                        // Startet Eingabe-Fenster
                        Programm_Hinzufügen table = new();
                        table.ShowDialog();
                        */
                    }
                    // etwas anderes
                    else
                    {
                        textBox1.Text = "Etwas ist drin";
                        System.IO.File.Delete(filepath);
                    }
                }
            }
            // wenn Datei nicht exisitiert
            else
            {
                StreamWriter sw = new StreamWriter(filepath);
                var firstList = new JArray();

                var firstElement = new JObject();
                firstElement["Name"] = textBox1.Text;
                firstElement["Link"] = textBox2.Text;

                firstList.Add(firstElement);

                var listSer = JsonConvert.SerializeObject(firstList, Formatting.Indented);

                sw.WriteLine(listSer.ToString());
                sw.Close();
                TreeView tv = Application.OpenForms.OfType<TreeView>().FirstOrDefault();
                if (tv != null)
                {
                    tv.button1_Click(sender, e);
                }
            }
        }
    }
}
