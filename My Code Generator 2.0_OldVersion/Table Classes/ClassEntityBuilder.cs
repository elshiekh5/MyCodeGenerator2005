using System;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace SPGen
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ClassEntityBuilder : Generator
    {
        public ClassEntityBuilder()
        {
            ClassName = global.TableEntityClass;
        }
        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            return Usingblock;
        }
        //
        private string GenerateClassBody()
        {
            return CreateEntityPropreties();
        }
        //
        private string GenerateClass(string classBody)
        {
            string xmlDocumentation = "/// <summary>\n";
            xmlDocumentation += "/// The class entity of " + SqlProvider.obj.TableName + ".\n";
            xmlDocumentation += "/// </summary>\n";
            string _class = "";
            _class += GeneateUsingBlock();
            _class += xmlDocumentation;
            _class += "public class " + ClassName;
            _class += "\n{\n" + classBody + "\n}";
            return _class;
        }
        //
        public static void Create()
        {
            ClassEntityBuilder dp = new ClassEntityBuilder();
            dp.CreateClassFile();
        }
        //
        public void CreateClassFile()
        {
            try
            {
                string _class = GenerateClass(GenerateClassBody());
                DirectoryInfo dInfo = Directory.CreateDirectory(Globals.ClassesDirectory + global.TableProgramatlyName);
                string path = dInfo.FullName + "\\" + ClassName + ".cs";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(_class);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //
        #region ClassMember
        public string CreateEntityPropreties()
        {
            try
            {
                string xmlDocumentation = "";
                //
                StringBuilder EntityPropreties = new StringBuilder();
                //
                string dataType;
                foreach (Column colCurrent in Fields)
                {
                    dataType = Globals.GetAliasDataType(colCurrent.Datatype);
                    if (dataType == "string")
                        EntityPropreties.Append("\n\tprivate " + Globals.GetAliasDataType(colCurrent.Datatype) + " _" + Globals.GetProgramatlyName(colCurrent.Name) + "= \"\";");
                    else if (dataType == "Guid")
                        EntityPropreties.Append("\n\tprivate " + Globals.GetAliasDataType(colCurrent.Datatype) + " _" + Globals.GetProgramatlyName(colCurrent.Name) + "= Guid.NewGuid();");
                    else
                        EntityPropreties.Append("\n\tprivate " + Globals.GetAliasDataType(colCurrent.Datatype) + " _" + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    //XML Documentaion
                    xmlDocumentation = "\n";
                    xmlDocumentation += "\t/// <summary>\n";
                    xmlDocumentation += "\t/// Gets or sets "+ SqlProvider.obj.TableName +" " + Globals.GetProgramatlyName(colCurrent.Name)+". \n";
                    xmlDocumentation += "\t/// </summary>";
                    EntityPropreties.Append(xmlDocumentation);
                    //Propretie body
                    EntityPropreties.Append("\n\tpublic " + Globals.GetAliasDataType(colCurrent.Datatype) + " " + Globals.GetProgramatlyName(colCurrent.Name));
                    EntityPropreties.Append("\n\t{");
                    EntityPropreties.Append("\n\t\tget { return _" + Globals.GetProgramatlyName(colCurrent.Name) + " ; }");
                    EntityPropreties.Append("\n\t\tset { _" + Globals.GetProgramatlyName(colCurrent.Name) + "= value ; }");
                    EntityPropreties.Append("\n\t}");
                    EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
                }
                //

                EntityPropreties.Append("\n");
                return EntityPropreties.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }
        #endregion





    }
}

