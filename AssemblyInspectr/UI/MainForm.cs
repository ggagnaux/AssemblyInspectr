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

namespace AssemblyInspectr.UI
{
    public partial class MainForm : MetroForm
    {
        #region Constants
        private const string TestFile = @"D:\Temp\InternalSource\KohdAndArt.Toolkit\" + 
                                        @"KohdAndArt.Toolkit\bin\Release\KohdAndArt.Toolkit.dll";
        private const MetroFramework.MetroThemeStyle _theme = MetroFramework.MetroThemeStyle.Dark;
        private const int PanelCount = 3;
        private const string Panel1BackgroundColor = "#200";
        private const string Panel1ForegroundColor = "#ddd";
        private const string Panel2BackgroundColor = "#020";
        private const string Panel2ForegroundColor = "#ddd";
        private const string Panel3BackgroundColor = "#002";
        private const string Panel3ForegroundColor = "#ddd";
        #endregion

        public enum TestEnum
        {
            First = 0,
            Second,
            Third
        }

        private enum PanelIdEnum
        {
            Details = 0,
            Classes,
            References
        }

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

        private void SetTitle(string t) => this.Text = $"{assemblyUtilities.AssemblyTitle} V{assemblyUtilities.AssemblyVersion}";

        private void CreateHtmlPanels()
        {
            _htmlPanels = new HtmlPanel[PanelCount];
            for (var x = 0; x<_htmlPanels.Length; x++)
            {
                _htmlPanels[x] = new HtmlPanel();
            }

            HtmlPanel p = null;
            p = _htmlPanels[(int)PanelIdEnum.Details];
            p.Dock = DockStyle.Fill;
            p.BackColor = Color.Transparent;
            panelAssemblyDetails.Controls.Add(p);
            panelAssemblyDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelAssemblyDetails.BackColor = Color.FromArgb(10, 10, 10);

            p = _htmlPanels[(int)PanelIdEnum.Classes];
            p.Dock = DockStyle.Fill;
            p.BackColor = Color.Transparent;
            panelClasses.Controls.Add(p);
            panelClasses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelClasses.BackColor = Color.FromArgb(10, 10, 10);

            p = _htmlPanels[(int)PanelIdEnum.References];
            p.Dock = DockStyle.Fill;
            p.BackColor = Color.Transparent;
            panelReferencedAssembiles.Controls.Add(p);
            panelReferencedAssembiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelReferencedAssembiles.BackColor = Color.FromArgb(10, 10, 10);
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
                var au = new AssemblyUtilities(Assembly.LoadFile(textBoxAssemblyName.Text));
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

            if (errorMessage.Length == 0)
            {
                var index = PanelIdEnum.Details;
                this._htmlPanels[(int)index].Text = results[(int)index];

                index = PanelIdEnum.Classes;
                this._htmlPanels[(int)index].Text = results[(int)index];

                index = PanelIdEnum.References;
                this._htmlPanels[(int)index].Text = results[(int)index];
            }
            else
            {
                this._htmlPanels[(int)PanelIdEnum.Details].Text = errorMessage;
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
            GenerateHtmlBodyStartBoilerplate(ref sb, "Item Name", "Item Value");

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

            GenerateHtmlBodyEndBoilerplate(ref sb);

            return sb.ToString();
        }

        private string GenerateAssemblyClassesHtml(AssemblyUtilities au)
        {
            if (null == au)
                throw new ArgumentNullException();

            var sb = new StringBuilder();
            sb.AppendLine(HtmlUtilities.HtmlStart());

            // Head
            GenerateHtmlHeadBoilerplate(ref sb, Panel2BackgroundColor, Panel2ForegroundColor);

            // Body
            GenerateHtmlBodyStartBoilerplate(ref sb, "Item Name", "Item Value");

            try
            {
                foreach (var classDetails in au.ClassDetailList)
                {
                    sb.AppendLine(HtmlUtilities.GenerateTableRow("Class", classDetails.Name));
                    foreach (var x in classDetails.MethodOrPropertyDetailsList)
                    {
                        var methodOrProperty = x.IsProperty ? "Property" : "Method";
                        var protectionLevel = x.ProtectionLevel;
                        var _static = x.IsStatic ? "static" : "";
                        var returnType = x.ReturnType;
                        var name = x.Name;
                        var parameterList = x.GetParameterList();
                        var methodDetails = $"{methodOrProperty}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{protectionLevel} {_static} {returnType} {name}({parameterList})";

                        sb.AppendLine(HtmlUtilities.GenerateTableRowWithPadding(string.Empty, methodDetails));
                    }

                    sb.AppendLine(HtmlUtilities.GenerateTableRowWithPadding(string.Empty, string.Empty));
                }

            }
            catch (ReflectionTypeLoadException ex)
            {
                sb.AppendLine($"<tr><td colspan='2' style='color: #f00;'>{ex.Message}</tr>");
            }

            GenerateHtmlBodyEndBoilerplate(ref sb);

            return sb.ToString();
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
            GenerateHtmlBodyStartBoilerplate(ref sb, "Assembly Name", "Version");

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

            GenerateHtmlBodyEndBoilerplate(ref sb);

            return sb.ToString();
        }

        private void GenerateHtmlHeadBoilerplate(ref StringBuilder sb, string backgroundColor, string foregroundColor)
        {
            sb.AppendLine(HtmlUtilities.HtmlHeadStart());
            sb.AppendLine("<style type='text/css'>");
            sb.AppendLine("html, body {");
            sb.AppendLine("font-family: arial;");
            sb.AppendLine($"background-color: {backgroundColor};");
            sb.AppendLine($"color: {foregroundColor};");
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
        }

        private void GenerateHtmlBodyStartBoilerplate(ref StringBuilder sb, string col1Label, string col2Label)
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
