using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace join_table_v6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///////****** function 1******//////
            dataGridView1.Rows.Clear();
            string t = "table";
            string by = "by";
            string field = "";
            List<string> tables_name = new List<string>();
            string statement = textBox1.Text;
            string[] spilt_statment = statement.Split();
            for (int i = 0; i < spilt_statment.Count(); i++)
            {
                if (spilt_statment[i] == t)
                {
                    tables_name.Add(spilt_statment[i + 1]);
                    i++;
                    // MessageBox.Show(spilt_statment[i]);
                }
                else if (spilt_statment[i] == by)
                {
                    for (int j = i + 1; j < spilt_statment.Count(); j++)
                    {
                        field += spilt_statment[j];
                        // if (j != spilt_statment.Count() - 1)
                        //  field += " ";
                    }
                    MessageBox.Show(field);


                    ///////////////// fe space zeyada m7taga 24elo ///////////////////////
                }
            }

            //dataGridView1.DataSource = tables_name;


            ///////****** function 2****/////
            List<List<string>> fname_lst = new List<List<string>>();
            List<List<string>> fvalues_lst = new List<List<string>>();
            List<List<string>> fl_lst = new List<List<string>>();
            data d = new data();
            List<string> lst = new List<string>();
            dataGridView1.Rows.Clear();
            int counter = 0;
            for (int i = 0; i < tables_name.Count(); i++)
            {

                string header = tables_name[i];
                string path = tables_name[i] + ".xml";
                MessageBox.Show(path);
                MessageBox.Show(header);
                if (!File.Exists(path))
                {
                    MessageBox.Show(" file not exist");
                }
                else
                {

                    counter++;

                    // d.add_data(path, header, fname_lst, fvalues_lst);
                    //d.add_data2(path, header, fl_lst,field ); 
                    d.load(path, header, field, counter);
                    d.clear();


                }
             

                lst = d.display();
                // dataGridView1.Rows.Add(lst);
                for (int j = 0; j < lst.Count; j++)
                    textBox2.Text += lst[j] + Environment.NewLine;
                //    MessageBox.Show(" left join");

                lst = d.ldisplay();

                for (int j = 0; j < lst.Count; j++)
                    textBox3.Text += lst[j] + Environment.NewLine;
                //    MessageBox.Show(" right join");


                lst = d.rdisplay();
                // dataGridView1.Rows.Add(lst);
                for (int j = 0; j < lst.Count; j++)
                    textBox4.Text += lst[j] + Environment.NewLine;
                //    MessageBox.Show(" left join");

                //    // dataGridView1.DataSource = lst;
            }
            //}

        }
    }
}
