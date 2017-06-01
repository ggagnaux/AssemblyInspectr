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
using System.Reflection;
using KohdAndArt.Toolkit.Extensions;
using KohdAndArt.Toolkit.Sys;

namespace AssemblyInspectr.UI
{
    partial class AboutBox : MetroFramework.Forms.MetroForm
    {
        // Need to tell the utility class that this is the assembly 
        private static AssemblyUtilities assemblyUtilities = new AssemblyUtilities(Assembly.GetExecutingAssembly());

        public struct SoftwareDetails
        {
            public string Description;
            public string Copyright;
            public string Url;
        }

        private List<SoftwareDetails> _softwareDetails = new List<SoftwareDetails>()
        {
            new SoftwareDetails()
            {
                Description = assemblyUtilities.AssemblyDescription,
                Copyright = assemblyUtilities.AssemblyCopyright,
                Url = @"https://github.com/ggagnaux/AssemblyInspectr"
            },
        };

        private List<SoftwareDetails> _thirdPartyDetails = new List<SoftwareDetails>()
        {
            new SoftwareDetails()
            {
                Description = "MetroFramework - Modern UI for WinForms",
                Copyright = "Copyright (c) 2013 - Jens Thiel",
                Url = @"https://github.com/thielj/MetroFramework"
            },
            new SoftwareDetails()
            {
                Description = "HTML Renderer",
                Copyright = @"Copyright (c) 2009, José Manuel Menéndez Poo" +
                            " Copyright (c) 2013, Arthur Teplitzki" +
                            " All rights reserved.",
                Url = @"https://github.com/ArthurHub/HTML-Renderer/"
            },
        };

        public AboutBox(MetroFramework.MetroThemeStyle theme)
        {
            InitializeComponent();

            this.Text = $"About {assemblyUtilities.AssemblyTitle} {assemblyUtilities.AssemblyVersion}";
            this.textBoxDescription.Text = BuildSoftwareDetails(_softwareDetails);
            this.textBoxThirdpartyComponents.Text = BuildSoftwareDetails(_thirdPartyDetails);

            SetTheme((int)theme);
        }

        private string BuildSoftwareDetails(List<SoftwareDetails> _details)
        {
            var output = string.Empty;
            foreach (var detail in _details)
            {
                output += detail.Description + Environment.NewLine;
                output += detail.Copyright + Environment.NewLine;
                output += detail.Url + Environment.NewLine + Environment.NewLine;
            }
            return output;
        }

        // TODO - Move to library
        private void OpenLink(string linkUrl)
        {
            System.Diagnostics.Process.Start(linkUrl);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetTheme(int theme)
        {
            var propertyName = "Theme";
            var t = (MetroFramework.MetroThemeStyle)theme;

            // Base Page
            if (this.HasProperty(propertyName))
            {
                this.Theme = t;
            }

            // All Children
            var children = this.GetAll();
            foreach (var child in children)
            {
                if (child.HasProperty(propertyName))
                {
                    child.SetPropertyValue(propertyName, t);
                }
            }
        }
    }
}
