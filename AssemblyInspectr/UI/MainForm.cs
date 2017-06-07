#region Copyright (c) 2017 G. Gagnaux, https://github.com/ggagnaux/AssemblyInspectr
/*
AssemblyInspectr - A Winforms based application to display .NET assembly metadata

Copyright (c) 2017 G. Gagnaux, https://github.com/ggagnaux/AssemblyInspectr

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AssemblyInspectr.Utility;
using AssemblyInspectr.Utility.UI;
using KohdAndArt.Toolkit.Sys;
using MetroFramework.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;
using static AssemblyInspectr.Utility.UI.OpenFileDialogEx;
using System.Web;
using System.Linq;

namespace AssemblyInspectr.UI
{
    public partial class MainForm : MetroForm
    {
        #region Constants
        private const string TestFile = @"D:\Temp\InternalSource\AssemblyInspectr\" +
                                        @"AssemblyInspectr\bin\Debug\MetroFramework.dll";
        private const MetroFramework.MetroThemeStyle _theme = MetroFramework.MetroThemeStyle.Light;
        private const int PanelCount = 3;
        private const string Panel1BackgroundColor = "#001f33";
        private const string Panel1ForegroundColor = "#ddd";
        private const string Panel2BackgroundColor = "#001f33";
        private const string Panel2ForegroundColor = "#ddd";
        private const string Panel3BackgroundColor = "#001f33";
        private const string Panel3ForegroundColor = "#ddd";
        #endregion

        private enum PanelIdEnum { Details = 0, Classes, References }
        private HtmlPanel[] _htmlPanels = null;
        private static AssemblyUtilities assemblyUtilities = new AssemblyUtilities(Assembly.GetExecutingAssembly());

        public MainForm()
        {
            InitializeComponent();

            SetTheme(_theme);
            SetTitle($"{assemblyUtilities.AssemblyTitle} V{assemblyUtilities.AssemblyVersion}");
            CreateHtmlPanels();
            textBoxAssemblyName.Text = TestFile;
            ProcessFile();
        }

        private void SetTheme(MetroFramework.MetroThemeStyle t)
        {
            this.Theme = t;
            this.metroLabel1.Theme = t;
            this.textBoxAssemblyName.Theme = t;
            this.tabControl.Theme = t;
        }

        private void SetTitle(string t) => this.Text = t;

        private void CreateHtmlPanels()
        {
            _htmlPanels = new HtmlPanel[PanelCount];
            for (var x = 0; x<_htmlPanels.Length; x++)
            {
                _htmlPanels[x] = new HtmlPanel();
            }

            CreatePanel(ref _htmlPanels[(int)PanelIdEnum.Details], ref panelAssemblyDetails);
            CreatePanel(ref _htmlPanels[(int)PanelIdEnum.Classes], ref panelClasses);
            CreatePanel(ref _htmlPanels[(int)PanelIdEnum.References], ref panelReferencedAssemblies);
        }

        private void CreatePanel(ref HtmlPanel htmlPanel, ref Panel containerPanel)
        {
            htmlPanel.Dock = DockStyle.Fill;
            htmlPanel.BackColor = Color.Transparent;

            // Add HtmlPanel to container
            containerPanel.Controls.Add(htmlPanel);
            containerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            containerPanel.BackColor = Color.FromArgb(10, 10, 10);
        }

        private void textBoxAssemblyName_ButtonClick(object sender, EventArgs e)
        {
            List<FilterDetails> filters = new List<FilterDetails>();
            filters.Add(new FilterDetails() {
                FileTypeTitle = "Assembly Files",
                Extensions = new List<String>() { "EXE", "DLL" }
            });
            var dlg = new OpenFileDialogEx("Select Assembly for analysis...", filters);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filename = dlg.Filename;
                if (filename.Length > 0 && File.Exists(filename))
                {
                    textBoxAssemblyName.Text = filename;
                    ProcessFile();
                }
            }
        }

        private void ProcessFile()
        {
            string errorMessage = string.Empty;
            string[] results = new string[_htmlPanels.Length];
            try
            {
                string filename = textBoxAssemblyName.Text;
                if (filename.Length == 0)
                {
                    throw new FileNotFoundException();
                }

                Assembly a = Assembly.LoadFile(filename);
                AssemblyUtilities au = new AssemblyUtilities(a);
                results[(int)PanelIdEnum.Details] = GenerateAssemblyDetailsHtml(au);
                results[(int)PanelIdEnum.Classes] = GenerateAssemblyClassesHtml(au);
                results[(int)PanelIdEnum.References] = GenerateAssemblyReferencesHtml(au);
            }
            catch (BadImageFormatException ex)
            {
                string msg = @"Error:<br/><br/>File doesn't appear to be a valid .NET Assembly.<br/>" +
                            "Please select a different file.";
                errorMessage = GenerateErrorHtml(msg);
            }
            catch (FileNotFoundException ex)
            {
                string msg = @"Error:<br/><br/>File doesn't appear to exist.<br/>" +
                            "Please select a different file.";
                errorMessage = GenerateErrorHtml(msg);
            }

            if (errorMessage.Length == 0)
            {
                this._htmlPanels[(int)PanelIdEnum.Details].Text = results[(int)PanelIdEnum.Details];
                this._htmlPanels[(int)PanelIdEnum.Classes].Text = results[(int)PanelIdEnum.Classes];
                this._htmlPanels[(int)PanelIdEnum.References].Text = results[(int)PanelIdEnum.References];
            }
            else
            {
                this._htmlPanels[(int)PanelIdEnum.Details].Text = errorMessage;
                this._htmlPanels[(int)PanelIdEnum.Classes].Text = errorMessage;
                this._htmlPanels[(int)PanelIdEnum.References].Text = errorMessage;
            }
        }

        private string GenerateErrorHtml(string text)
        {
            var sb = new StringBuilder();
            sb.AppendLine(HtmlUtilities.HtmlStart());

            // Head 
            sb.AppendLine(HtmlUtilities.HtmlHeadStart());
            sb.AppendLine("<style type='text/css'>");
            sb.AppendLine("html, body {");
            sb.AppendLine("font-family: arial;");
            sb.AppendLine("background-color: #200;");
            sb.AppendLine("color: #ddd;");
            sb.AppendLine("}");
            sb.AppendLine("table {");
            sb.AppendLine("width: 100%;");
            sb.AppendLine("border-collapse: collapse;");
            sb.AppendLine("}");
            sb.AppendLine("table thead td {");
            sb.AppendLine("font-weight: bold;");
            sb.AppendLine("height: 35px;");
            sb.AppendLine("}");
            sb.AppendLine("td {");
            sb.AppendLine("padding-left: 5px;");
            sb.AppendLine("border: 1px dotted #400;");
            sb.AppendLine("}");
            sb.AppendLine("</style>");
            sb.AppendLine(HtmlUtilities.HtmlHeadEnd());

            // Body
            sb.AppendLine(HtmlUtilities.HtmlBodyStart());
            sb.AppendLine(text);
            sb.AppendLine(HtmlUtilities.HtmlBodyEnd());
            sb.AppendLine(HtmlUtilities.HtmlEnd());

            var temp = sb.ToString();
            return temp;
        }

        private string GenerateAssemblyDetailsHtml(AssemblyUtilities au)
        {
            if (null == au)
                throw new ArgumentNullException();

            var sb = new StringBuilder();
            sb.AppendLine(HtmlUtilities.HtmlStart());

            // Head 
            GenerateHtmlHeadBoilerplate(ref sb, Panel1BackgroundColor, Panel1ForegroundColor);

            // Body
            sb.AppendLine(HtmlUtilities.HtmlBodyStart());

            sb.AppendLine($"<h2>Assembly Details</h2>");
            sb.AppendLine($"<div class='table-container'>");
            sb.AppendLine("<table>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th>Item</th><th>Value</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            sb.AppendLine(HtmlUtilities.GenerateTableRow("CodeBase", au.RootAssembly.CodeBase));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("FullName", au.RootAssembly.FullName));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("GlobalAssemblyCache", au.RootAssembly.GlobalAssemblyCache.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("HostContext", au.RootAssembly.HostContext.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("ImageRuntimeVersion", au.RootAssembly.ImageRuntimeVersion));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("Location", au.RootAssembly.Location));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("EscapedCodeBase", au.RootAssembly.EscapedCodeBase));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("HostContext", au.RootAssembly.HostContext.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("ImageRuntimeVersion", au.RootAssembly.ImageRuntimeVersion));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("IsDynamic", au.RootAssembly.IsDynamic.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("IsFullyTrusted", au.RootAssembly.IsFullyTrusted.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("ReflectionOnly", au.RootAssembly.ReflectionOnly.ToString()));
            sb.AppendLine(HtmlUtilities.GenerateTableRow("SecurityRuleSet", au.RootAssembly.SecurityRuleSet.ToString()));
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private string GenerateAssemblyClassesHtml(AssemblyUtilities au)
        {
            if (null == au)
                throw new ArgumentNullException();

            int itemCount = 0;
            string sectionTitle = string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(HtmlUtilities.HtmlStart());

            // Head
            GenerateHtmlHeadBoilerplate(ref sb, Panel2BackgroundColor, Panel2ForegroundColor);

            // Body
            sb.AppendLine(HtmlUtilities.HtmlBodyStart());

            try
            {
                foreach (var classDetail in au.ClassDetailList)
                {
                    sb.AppendLine($"<h2>{classDetail.Name}</h2>");

                    // 
                    // Methods Table
                    //
                    itemCount = classDetail.MethodDetails.Count;
                    sectionTitle = $"Methods [{itemCount}]";

                    sb.AppendLine($"<div class='table-container'>");
                    sb.AppendLine($"<h3>{sectionTitle}</h3>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<thead>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<th>Scope</th><th>Virtual</th><th>Return Value</th><th>Name</th><th>Parameters</th>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</thead>");
                    sb.AppendLine("<tbody>");


                    // Sort by Scope (Public, Private, Protected), then by Name
                    var sorted = classDetail.MethodDetails.OrderByDescending(p => p.IsPublic)
                                                     .ThenByDescending(pr => pr.IsPrivate)
                                                     .ThenBy(n => n.Name);

                    foreach (var x in sorted)
                    {
                        //var methodOrProperty = x.IsProperty ? "Property" : "Method";
                        var protectionLevel = HttpUtility.HtmlEncode(x.ProtectionLevel);
                        var _static = x.IsStatic ? "static" : "";
                        var returnType = HttpUtility.HtmlEncode(x.ReturnType);
                        var name = HttpUtility.HtmlEncode(x.Name);
                        var parameterList = HttpUtility.HtmlEncode(x.GetParameterList());
                        string  _virtual = (x.IsVirtual == true) ? "virtual" : string.Empty;

                        string protectionlevelClass = string.Empty;
                        if (x.IsPublic)
                        {
                            protectionlevelClass = "class=\"public\"";
                        }
                        else if (x.IsPrivate)
                        {
                            protectionlevelClass = "class=\"private\"";
                        } 
                        else
                        {
                            protectionlevelClass = "class=\"protected\"";
                        }
                        sb.AppendLine($"<tr><td {protectionlevelClass}>{protectionLevel} {_static}</td><td>{_virtual}</td><td>{returnType}</td><td>{name}</td><td>{parameterList}</td></tr>");
                    }
                    sb.AppendLine("</tbody>");
                    sb.AppendLine("</table>");
                    sb.AppendLine("</div>");

                    // 
                    // Properties Table
                    //

                    // Extract just the name property
                    var data = classDetail.PropertyDetails.Select(i => i.Name).ToList();

                    BuildSingleColumnDivAndTable(sb:ref sb, 
                                                 tableTitle:"Properties", 
                                                 columnTitle:"Name", 
                                                 data:data);

                    //
                    // Fields Table
                    //
                    BuildSingleColumnDivAndTable(sb: ref sb,
                                                 tableTitle: "Fields",
                                                 columnTitle: "Name",
                                                 data: classDetail.Fields);

                    //
                    // Nested Types Table
                    //
                    BuildSingleColumnDivAndTable(sb: ref sb,
                                                 tableTitle: "Nested Types",
                                                 columnTitle: "Name",
                                                 data: classDetail.NestedTypes);

                    //
                    // Constructors Table
                    //
                    BuildSingleColumnDivAndTable(sb: ref sb,
                                                 tableTitle: "Constructors",
                                                 columnTitle: "Name",
                                                 data: classDetail.Constructors);

                    //
                    // Events Table
                    //
                    BuildSingleColumnDivAndTable(sb: ref sb,
                                                 tableTitle: "Events",
                                                 columnTitle: "Name",
                                                 data: classDetail.Events);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                sb.AppendLine($"<tr><td colspan='2' style='color: #f00;'>{ex.Message}</tr>");
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private void BuildSingleColumnDivAndTable(ref StringBuilder sb, 
                                                  string tableTitle, 
                                                  string columnTitle, 
                                                  IEnumerable<string> data)
        {
            int itemCount = data.Count();
            string sectionTitle = $"{tableTitle} [{itemCount}]";

            sb.AppendLine($"<div class='table-container'>");
            sb.AppendLine($"<h3>{sectionTitle}</h3>");

            if (itemCount > 0)
            {
                sb.AppendLine("<table>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine($"<th>{columnTitle}</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");

                foreach (var item in data)
                {
                    var name = HttpUtility.HtmlEncode(item);
                    sb.AppendLine($"<tr><td>{name}</td></tr>");
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

            }
            sb.AppendLine("</div>");
        }

        private string GenerateAssemblyReferencesHtml(AssemblyUtilities au)
        {
            if (null == au)
                throw new ArgumentNullException();

            var sb = new StringBuilder();
            sb.AppendLine(HtmlUtilities.HtmlStart());

            // Head 
            GenerateHtmlHeadBoilerplate(ref sb, Panel3BackgroundColor, Panel3ForegroundColor);

            // Body
            sb.AppendLine(HtmlUtilities.HtmlBodyStart());

            sb.AppendLine($"<h2>Referenced Assemblies</h2>");
            sb.AppendLine($"<div class='table-container'>");
            sb.AppendLine("<table>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th>Assembly Name</th><th>Version</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            try
            {
                foreach (var s in au.ReferencedAssemblies)
                {
                    sb.AppendLine(HtmlUtilities.GenerateTableRow(s.Name, s.Version));
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                sb.AppendLine($"<tr><td colspan='2' style='color: #f00;'>{ex.Message}</tr>");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"<tr><td colspan='2' style='color: #f00;'>{ex.Message}</tr>");
            }

            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private void GenerateHtmlHeadBoilerplate(ref StringBuilder sb, 
                                                 string backgroundColor, 
                                                 string foregroundColor)
        {
            sb.AppendLine(HtmlUtilities.HtmlHeadStart());
            sb.AppendLine("<style type='text/css'>" + Environment.NewLine);

            sb.Append("html, body {" +
                    "font-family: arial;" +
                    "font-weight: 200;" +
                    "font-size: 14px;");
            sb.Append($"background-color: {backgroundColor};" + Environment.NewLine);
            sb.Append($"color: {foregroundColor}; " + Environment.NewLine);
            sb.Append("}");
            sb.AppendLine("h2 {" +
                "    width: 100%;" + 
                "    color:  #1ac6ff;" +
                "    font-size: 1.5em;" +
                "    margin-bottom: 30px;" +
                "    padding-bottom: 10px;" +
                "    border-bottom: 2px solid #1ac6ff;" +
                "}" +
                "h3 {" +
                "   color: #e68a00;" +
                "   font-size: 1.2em;" +
                "}" +
                ".table-container {" +
                "    width: 100%;" +
                "    margin-left: 40px;" +
                "    margin-bottom: 40px;" +
                "}" +
                ".public {" +
                "    color: #0f0;" +
                "}" +
                ".private {" +
                "    color: #f00;" +
                "}" +
                ".protected {" +
                "    color: #ff0;" +
                "}" +
                "table {" +
                "    width: auto;"+
                "    border-collapse: collapse;" +
                "    border: 0px solid #999;" +
                "    font-size: 14px;" +
                "}" +

                "table thead tr {" +
                "    border-bottom: 1px dotted #fff;" +
                "    background-color: #f00;" +
                "}" +
                "table thead tr th {" +
                //"    border-left: 1px dotted #fff;" +
                "    padding: 5px;" +
                "    text-align: left; background-color: #400;" +
                "}" +
                "table tbody tr td {" +
                "    border: 1px dotted #444;" +
                "    padding: 5px;" +
                "    color: #ccc;" +
                "    min-width: 200px;" +
                "}");
            sb.AppendLine("</style>");

            string t = sb.ToString();
            sb.AppendLine(HtmlUtilities.HtmlHeadEnd());
        }

        private void GenerateHtmlBodyStartBoilerplate(ref StringBuilder sb, 
                                                      string col1Label, 
                                                      string col2Label)
        {
            sb.AppendLine(HtmlUtilities.HtmlBodyStart());
            sb.AppendLine(HtmlUtilities.HtmlTableStart());
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{col1Label}</td><td>{col2Label}</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
        }

        private void GenerateHtmlBodyEndBoilerplate(ref StringBuilder sb)
        {
            sb.AppendLine("</tbody>");
            sb.AppendLine(HtmlUtilities.HtmlTableEnd());
            sb.AppendLine(HtmlUtilities.HtmlBodyEnd());
            sb.AppendLine(HtmlUtilities.HtmlEnd());
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            using (var dlg = new AboutBox(_theme))
            {
                dlg.ShowDialog();
            }
        }
    }
}
