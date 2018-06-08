using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace join_table_v6
{
    class data
    {
        /// <summary>
        /// join statement on test case
        /// join table Employees and table Departments by DepartmentId
        /// </summary>

        List<string> f_lst = new List<string>();
        List<string> joinlst = new List<string>();
        List<string> join_list2 = new List<string>();
        List<string> combined_file = new List<string>();
        List<List<string>> files = new List<List<string>>();
        string attribute = "";

        public void load(string path, string header, string field, int counter)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList parent_list = doc.GetElementsByTagName(header);
            add_data(field, parent_list, counter);

        }
        public void add_data(string field, XmlNodeList parent_list, int counter)
        {
            for (int i = 0; i < parent_list.Count; i++)
            {


                XmlNodeList child_list = parent_list[i].ChildNodes;
                if (child_list.Count > 1) /// nested 
                {
                    ///// insert ll word
                    if (counter == 1)/////// 2wl file
                    {
                        if (field == parent_list[i].Name || field.Contains(parent_list[i].Name))
                            file_nested(parent_list, joinlst); 
                    }
                    else  // nested w not first file
                    {
                        if (field == parent_list[i].Name || field.Contains(parent_list[i].Name))
                        {
                            List<string> joinlst2 = new List<string>();
                            joinlst2.Add(parent_list[i].Name);
                            file_nested(child_list, joinlst2);
                            joinlst2.Add(parent_list[i].Name);

                            edit_common_data_nested_2ndfile(joinlst2);
                            join_list2.AddRange(join_list2); 
                        }
                    }



                    f_lst.Add(parent_list[i].Name);
                    add_data(field, child_list, counter);
                    f_lst.Add(parent_list[i].Name);
                }
                else
                {

                    attribute = parent_list[i].Name;
                    attribute += " = ";
                    attribute += parent_list[i].InnerText;
                    f_lst.Add(attribute);
                    if (counter == 1)/////// 2wl file
                    {
                        if (field == parent_list[i].Name) 
                            joinlst.Add(attribute);
                    }
                    else
                    {
                        if (field == parent_list[i].Name)
                            combined_file = files[0];
                            

                    }

                }



            }

        }
        public void clear()
        {
            List<string> temp = new List<string>(f_lst);
            files.Add(temp);
            f_lst.Clear();
            if (combined_file.Count != 0)
            {
                files[0] = new List<string>(combined_file);
                joinlst = new List<string>(join_list2);
                join_list2.Clear();
                
            }
        }
        public void edit_common_data_nested_2ndfile(List<string> lst)
        {
            string g = "";
            string[] s;
            string s2 = "";
            string key = "";
            List<string> after_spiltf1 = new List<string>();
            List<string> common_data_lst = new List<string>();
            int index_key =0;
            bool exist = true;

            bool is_nested = true;

            for (int i = 0; i < joinlst.Count; i++)
            {
                if (joinlst[i].Contains(" = ")) // m3naha 2n el data el fe el joinlst "2wl file" m4 nested
                {
                    is_nested = false;
                    s = joinlst[i].Split();
                    after_spiltf1.Add(s[2]); // kda m3aya el vaues bt3t el id
                    key = s[0];
                    s2 = s[0]; /// 4ayl deparmentid
                }
                else
                {
                    break;
                }
               
            }
            g = lst[0].ToLower(); //// 4ayla 
            s2 = s2.ToLower();
            
            if (is_nested == false)
            {
                for (int i = 1; i < lst.Count; i++)
                {
                    s = lst[i].Split();
                    s[0] = s[0].ToLower();
                    if (s2 == g + s[0] && after_spiltf1.Contains(s[2]))
                    {
                        // jba kda de el list ele 27taha
                        key += " = ";
                        key += s[2];
                        join_list_file(key, lst);
                        // return joinlst[i];

                    }

                }
            }

            else
            {
                for (int i = 0; i < joinlst.Count; i++)
                {
                    if (joinlst[i] == lst[0])
                    {
                        for (int j = 1; i < lst.Count; i++)
                        {
                            index_key = j + i;
                            if (joinlst[i+j] != lst[i])
                            {

                                s = joinlst[i].Split();
                                string[] n = lst[i].Split();
                                if (s[0] != n[0])
                                {
                                    common_data_lst.Add(lst[i]);
                                    exist = true;
                                }
                                else if (s[0] == n[0] && s[2] != n[2])
                                {
                                    i = i + j;
                                    common_data_lst.Clear();
                                    exist = false;
                                    break;
                                }
                            }

                        }
                    }
                }
                if (exist== true && index_key!=0)
                {
                    joinlst.InsertRange(index_key,common_data_lst);
                    combined_file = joinlst;
                }
            }
        }
         
       
        public List<string> display()
        {
            return combined_file;
        }
        public List<string> ldisplay()
        {
            return joinlst;
        }
        public List<string> rdisplay()
        {
            return join_list2;
        }
        public void write()
        {
            //string table_name = tables_name[0];
            //string path = tables_name + ".xml";
            //if (!File.Exists(path))
            //{
            //    /// create  file

            //}
            //else
            //{
            XmlDocument doc = new XmlDocument();
            int counter = 1;
            //doc.Load(path);
            XmlElement header = doc.DocumentElement;
            XmlElement element = doc.DocumentElement;
            XmlWriter writer = XmlWriter.Create("table.xml");
            string[] spilt_element;
            for (int i = 0; i < combined_file.Count; i++)
            {
                if (!combined_file[i].Contains("=") && counter == 1)
                {
                    writer.WriteStartElement(combined_file[i]);
                    counter = 2;
                }
                else if (combined_file[i].Contains("="))
                {
                    spilt_element = combined_file[i].Split();
                    writer.WriteStartElement(spilt_element[0]);
                    writer.WriteString(spilt_element[2]);
                    writer.WriteEndElement();
                }
                else if (!combined_file[i].Contains("=") && counter == 2)
                {
                    counter = 1;
                }
            }



        }

        public void join_list_file(string key, List<string> lst)
        {

            List<string> first_file = new List<string>(files[0]);
            List<string> join_object = new List<string>();

            bool b = false;
            string first_tag = first_file[0];
            string second_tag = first_file[1];
            if (!second_tag.Contains("="))
            {

                join_object.Add(second_tag);
                for (int i = 2; i < first_file.Count; i++)
                {
                    if (first_file[i] == second_tag)
                    {
                        if (b == true)
                        {
                            join_object.Add(second_tag);
                            combined_file.AddRange(join_object);
                        }
                        join_object.Clear();
                        b = false;

                    }
                    else
                    {
                        if (first_file[i] == key)
                        {
                            join_object.AddRange(lst); ///*************
                            b = true;
                            continue;
                        }
                        join_object.Add(first_file[i]);
                    }
                }

            }
            else if (second_tag.Contains("="))
            {
                join_object.Add(first_tag);
                for (int i = 1; i < first_file.Count; i++)
                {
                    if (first_file[i] == first_tag)
                    {
                        if (b == true)
                        {
                            join_object.Add(first_tag);
                            combined_file.AddRange(join_object);
                        }
                        join_object.Clear();
                        b = false;

                    }
                    else
                    {
                        if (first_file[i] == key)
                        {
                            join_object.AddRange(lst); ///*************
                            b = true;
                            continue;
                        }
                        join_object.Add(first_file[i]);
                    }
                }


            }

        }

        public void file_nested(XmlNodeList parent_list, List<string> lst)
        {

            for (int i = 0; i < parent_list.Count; i++)
            {
                XmlNodeList child_list = parent_list[i].ChildNodes;
                if (child_list.Count > 1) /// nested 
                {
                    lst.Add(parent_list[i].Name);
                    file_nested(child_list, lst);
                    lst.Add(parent_list[i].Name);
                }
                else
                {

                    attribute = parent_list[i].Name;
                    attribute += " = ";
                    attribute += parent_list[i].InnerText;
                    lst.Add(attribute);
                }
            }
        }
    }
}
