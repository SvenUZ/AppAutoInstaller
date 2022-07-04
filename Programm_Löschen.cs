using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Task_Manager
{
    public partial class Programm_Löschen : Form
    {
        public Programm_Löschen()
        {
            InitializeComponent();
            string inFile = System.IO.File.ReadAllText("db.json");

            var programme = JArray.Parse(inFile);
            foreach (var currentP in programme)
            {
                comboBox1.Items.Add(currentP.Value<string>("Name"));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filepath = "db.json";
            string inFile = System.IO.File.ReadAllText(filepath);

            string key = "Name";
            string value = comboBox1.SelectedItem.ToString();

            JArray ja = JArray.Parse(inFile);

            var item = ja.Children().Where(i => (string)(i as JObject).GetValue(key) == value).SingleOrDefault();

            if (item != null)
            {
                ja.Remove(item);
            }

            var jsonToOutput = JsonConvert.SerializeObject(ja, Formatting.Indented);

            File.WriteAllText(@"db.json", jsonToOutput.ToString());

            this.Close();

            TreeView tv = Application.OpenForms.OfType<TreeView>().FirstOrDefault();
            if (tv != null)
            {
                tv.button1_Click(sender, e);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
