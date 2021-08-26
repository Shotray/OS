using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_management
{
    public partial class TXTFile : Form
    {
        public bool is_changed;
        public Form1 form;
        public fileTree.Node curnode;
        string nowcontent="";

        public TXTFile(fileTree.Node node,Form1 form)
        {
            this.curnode = node;
            this.form = form;
            InitializeComponent();
        }

        private void TXTFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (is_changed)
            {
                const string message = "确认保存吗？";
                const string caption = "关闭TXT";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    this.form.v_disk.deleteFile(curnode);
                    if(!this.form.v_disk.addFile(curnode,textBox1.Text))
                    {
                        MessageBox.Show("内存空间不足，文件将清空");
                    }
                    
                    DateTime time = DateTime.Now.ToLocalTime();
                    curnode._last_modify = time;
                    this.form.updateView();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            is_changed = true;
            nowcontent = textBox1.Text;
        }

        private void TXTFile_Load(object sender, EventArgs e)
        {
            textBox1.Text = this.form.v_disk.showFile(curnode);
            is_changed = false;
        }
    }
}
