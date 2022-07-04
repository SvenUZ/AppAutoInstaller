using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Task_Manager
{
    public partial class Programm_bearbeiten : Form
    {
        public Programm_bearbeiten()
        {
            InitializeComponent();

            // Laden der JSON und Hinzufügen der Elemente in die ComboBox
            string filepath = "db.json";
            string inFile = System.IO.File.ReadAllText(filepath);

            var programme = JArray.Parse(inFile);
            foreach (var currentP in programme)
            {
                comboBox1.Items.Add(currentP.Value<string>("Name"));
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {

        }

        // Ändern Button
        private void button1_Click(object sender, EventArgs e)
        {
            string inFile = System.IO.File.ReadAllText("db.json");

            string valueName = comboBox1.SelectedItem.ToString();

            // Ganze Liste (JArray)
            var jarray = JArray.Parse(inFile);

            // wenn "neuer Name" gewählt ist, aber kein neuer Link
            if (textBox1.Text != "neuer Name" && textBox2.Text == "neuer Link")
            {
                var selectedIndex = jarray.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == valueName)
                                         .Index;
                var jobject = (JObject)jarray[selectedIndex];

                jobject["Name"] = textBox1.Text;
            }

            // wenn "neuer Link" gewählt ist, aber kein neuer Name
            if (textBox1.Text == "neuer Name" && textBox2.Text != "neuer Link")
            {
                var selectedIndex = jarray.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == valueName)
                                         .Index;
                var jobject = (JObject)jarray[selectedIndex];

                jobject["Link"] = textBox2.Text;
            }

            // wenn ein neuer Link und Name gewählt wurde
            if (textBox1.Text != "neuer Name" && textBox2.Text != "neuer Link")
            {
                var selectedIndex = jarray.Select((x, index) => new { Code = x.Value<string>("Name"), Node = x, Index = index })
                                         .Single(x => x.Code == valueName)
                                         .Index;
                var jobject = (JObject)jarray[selectedIndex];

                jobject["Name"] = textBox1.Text;
                jobject["Link"] = textBox2.Text;
            }

            // Änderungen in "db.json" schreiben und TreeView aktuallisieren
            var jsonToOutput = JsonConvert.SerializeObject(jarray, Formatting.Indented);

            File.WriteAllText(@"db.json", jsonToOutput.ToString());
            this.Close();

            TreeView tv = Application.OpenForms.OfType<TreeView>().FirstOrDefault();
            if (tv != null)
            {
                tv.button1_Click(sender, e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Programm_bearbeiten_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
