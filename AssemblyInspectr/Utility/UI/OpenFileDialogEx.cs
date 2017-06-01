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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblyInspectr.Utility.UI
{
    public class OpenFileDialogEx
    {
        public class FilterDetails
        {
            public string FileTypeTitle { get; set; }
            public List<string> Extensions { get; set; }

            public string ExtensionListAsString
            {
                get
                {
                    string result = string.Empty;
                    if (Extensions.Count > 0)
                    {
                        foreach (var ext in Extensions)
                        {
                            result += $"*.{ext};";
                        }
                    }
                    return result;
                }
            }
        }

        private OpenFileDialog _openFileDialog;
        private List<FilterDetails> _filterList; 


        public string Title { get; set; } = "Select File";
        public bool MultipleFileSelect { get; set; } = false;
        public string Filename { get; set; } = string.Empty;
        public List<string> Filenames { get; set; } = new List<string>();

        public OpenFileDialogEx(string _title = "Select File...", List<FilterDetails> filters = null)
        {
            this._filterList = filters;
            this.Title = _title;
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Title = _title;
            _openFileDialog.FileName = this.Filename;
        }

        public DialogResult ShowDialog()
        {
            _openFileDialog.Filter = BuildFilters();
            DialogResult result = _openFileDialog.ShowDialog();
            Filename = _openFileDialog.FileName;
            return result;
        }

        public void AddFilter(string title, List<string> extensionSpecs)
        {
            var details = new FilterDetails()
            {
                FileTypeTitle = title,
                Extensions = extensionSpecs
            };

            if (!this._filterList.Contains(details))
            {
                _filterList.Add(details);
            }
        }

        private string BuildFilters()
        {
            // Sample output : "Assembly Files (*.EXE;*.DLL)|*.EXE; *.DLL";
            string result = string.Empty;
            foreach (var f in _filterList)
            {
                var extensionListAsString = f.ExtensionListAsString;
                result += $"{f.FileTypeTitle} ({extensionListAsString})|{extensionListAsString}";
            }
            return result;
        }
    }
}
