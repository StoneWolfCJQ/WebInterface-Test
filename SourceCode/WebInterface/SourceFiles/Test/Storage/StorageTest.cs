﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WebInterface
{
    public partial class StorageTest : Form
    {
        public StorageTest()
        {
            InitializeComponent();
            String cp = Directory.GetCurrentDirectory();
            defaultPath = cp + defaultDir + defaultFileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestAction t = new TestAction(defaultPath);
        }

        private String defaultFileName = "TestStorage.json";
        private String defaultDir = @"\json\";
        private String defaultPath;
    }
}
