using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace file_management
{
    public partial class Form1 : Form
    {
        const int TXT = 0;
        const int FOLDER_EMPTY = 1;
        const int FOLDER_FULL = 2;
        public string cur_path = "./";
        public int cur_size = 0;
        public fileTree filetree = new fileTree();
        public virtualDisk v_disk = new virtualDisk();
        

        public Form1()
        {
            InitializeComponent();

            read_bitmap();
            read_catalog();
            read_memory();
            updateView();
            init_treeview();

            this.contextMenuStrip2.Closed += new ToolStripDropDownClosedEventHandler(contextMenuStrip2_Closed);
            fileList.ContextMenuStrip = this.contextMenuStrip1;
        }

        public void init_treeview()
        {
            this.treeView1.Nodes.Clear();
            this.treeView1.BeginUpdate();
            TreeNode tn = new TreeNode();
            tn.Name = filetree.root._filename;
            tn.Text = filetree.root._filename;
            tn.Tag = FOLDER_EMPTY;
            tn.ImageIndex = FOLDER_EMPTY;
            tn.SelectedImageIndex = FOLDER_EMPTY;
            treeView1.Nodes.Add(tn);
            this.initTreeNode(tn,filetree.root);
            this.treeView1.ExpandAll();
            this.treeView1.EndUpdate();
        }

        public void initTreeNode(TreeNode parentTn,fileTree.Node node)
        {
            if (node._childnode.Count == 0)
                return;
            
            foreach(fileTree.Node childnode in node._childnode)
            {
                TreeNode tn = new TreeNode();
                if (childnode._type == fileTree.FOLDER)
                {
                    tn.Name = childnode._filename;
                    tn.Text = childnode._filename;
                    tn.Tag = FOLDER_EMPTY;
                    tn.ImageIndex = FOLDER_EMPTY;
                    tn.SelectedImageIndex = FOLDER_EMPTY;
                }
                else
                {
                    tn.Name = childnode._filename;
                    tn.Text = childnode._filename;
                    tn.Tag = TXT;
                    tn.ImageIndex = TXT;
                    tn.SelectedImageIndex = TXT;
                }
                parentTn.Nodes.Add(tn);
                initTreeNode(tn,childnode);
            }
        }

        private void 新建文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            string str = Interaction.InputBox("请输入文件的名称", "新建文件夹", "", 100, 100);
            if (filetree.haveSameName(str, fileTree.FOLDER))
                MessageBox.Show("此位置已包含同名文件");
            else if (str == "")
                return;
            else
            {
                DateTime time = DateTime.Now.ToLocalTime();
                fileTree.Node node = new fileTree.Node(str, time, fileTree.FOLDER, filetree._curnode);
                addItem(node);
                filetree._curnode._childnode.Add(node);
                updateView();
            }
        }

        private void 新建文本文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("请输入文件的名称", "新建文本文档", "", 100, 100);
            if (filetree.haveSameName(str+".txt", fileTree.TXT))
                MessageBox.Show("此位置已包含同名文件");
            else if (str == "")
                return;
            else
            {
                DateTime time = DateTime.Now.ToLocalTime();
                fileTree.Node node = new fileTree.Node(str, time, fileTree.TXT, filetree._curnode);
                addItem(node);
                filetree._curnode._childnode.Add(node);
                updateView();
            }
        }

        public void addItem(fileTree.Node node)
        {
            int type;
            if (node._type == fileTree.FOLDER)
                type = FOLDER_EMPTY;
            else
                type = TXT;
            ListViewItem item = new ListViewItem(node._filename, type);
            string[] items = { node._create_time.ToString(), node._last_modify.ToString(), node._type, node._size.ToString()+"B" };
            item.SubItems.AddRange(items);
            fileList.Items.Add(item);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            catalog.Text = cur_path;
            catalog.ReadOnly = true;
        }

        private void fileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (fileList.SelectedItems.Count > 0)
            {
                string node = fileList.SelectedItems[0].SubItems[0].Text;
                string type = fileList.SelectedItems[0].SubItems[3].Text;
                fileTree.Node next_node = filetree.searchNode(node, type,filetree._curnode);
                if (type == fileTree.FOLDER)
                {
                    filetree._curnode = next_node;
                    cur_path = next_node._path;
                    updateView();
                }
                else if (type == fileTree.TXT)
                {
                    TXTFile txtfile = new TXTFile(next_node,this);
                    txtfile.Show();
                }
            }
        }

        private void fileList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fileList.ContextMenuStrip = contextMenuStrip2;
            }
        }

        private void contextMenuStrip2_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            fileList.ContextMenuStrip = contextMenuStrip1;
        }

        private void 格式化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filetree.clear(filetree.root);
            v_disk.clear();
            cur_path = filetree.root._path;
            updateView();
            filetree._curnode = filetree.root;
            MessageBox.Show("已完成格式化！");
        }

        public void updateView()
        {
            fileList.Items.Clear();
            foreach(fileTree.Node node in filetree._curnode._childnode)
                addItem(node);
            catalog.Text = cur_path;
            cur_size = filetree.getCurSize(filetree.root);
            this.curSize.Text = cur_size.ToString() + "B";
            init_treeview();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count > 0)
            {
                string node = fileList.SelectedItems[0].SubItems[0].Text;
                string type = fileList.SelectedItems[0].SubItems[3].Text;
                fileTree.Node del_node = filetree.searchNode(node, type,filetree._curnode);
                filetree._curnode._childnode.Remove(del_node);
                v_disk.deleteFile(del_node);
                updateView();
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count > 0)
            {
                string node = fileList.SelectedItems[0].SubItems[0].Text;
                string type = fileList.SelectedItems[0].SubItems[3].Text;
                fileTree.Node next_node = filetree.searchNode(node, type,filetree._curnode);
                if (type == fileTree.FOLDER)
                {
                    filetree._curnode = next_node;
                    cur_path = next_node._path;
                    updateView();
                }
                else if (type == fileTree.TXT)
                {
                    TXTFile txtfile = new TXTFile(next_node,this);
                    txtfile.Show();
                }
                
            }
        }

        private void formatButton_Click(object sender, EventArgs e)
        {
            filetree.clear(filetree.root);
            v_disk.clear();
            cur_path = filetree.root._path ;
            updateView();
            filetree._curnode = filetree.root;
            MessageBox.Show("已完成格式化！");
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            if (cur_path == filetree.root._path)
                return;
            filetree._curnode = filetree._curnode._parent;
            cur_path = filetree._curnode._path;
            updateView();
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string node = fileList.SelectedItems[0].SubItems[0].Text;
            string type = fileList.SelectedItems[0].SubItems[3].Text;
            fileTree.Node rename_node = filetree.searchNode(node, type,filetree._curnode);
            
            if (type == fileTree.FOLDER)
            {
                string str = Interaction.InputBox("请输入文件的名称", "新建文件夹", "", 100, 100);
                if (filetree.haveSameName(str, fileTree.FOLDER)&&str!=rename_node._filename)
                    MessageBox.Show("此位置已包含同名文件");
                else if (str == "")
                    return;
                else
                    rename_node._filename = str;
            }
            else if (type == fileTree.TXT)
            {
                string str = Interaction.InputBox("请输入文件的名称", "新建文本文档", "", 100, 100);
                if (filetree.haveSameName(str + ".txt", fileTree.TXT) && (str+".txt") != rename_node._filename)
                    MessageBox.Show("此位置已包含同名文件");
                else if (str == "")
                    return;
                else
                    rename_node._filename = str + ".txt";
            }

            DateTime time = DateTime.Now.ToLocalTime();
            rename_node._last_modify = time;
            updateView();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateLog();
        }

        public void updateLog()
        {
            if (File.Exists(Application.StartupPath + "\\CategoryInfo.txt"))
                File.Delete(Application.StartupPath + "\\CategoryInfo.txt");
            recursive_write(filetree.root);

            write_bitmap();
            write_memory();
        }

        public void read_catalog()
        {
            string path = Application.StartupPath + "\\CategoryInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string str = reader.ReadLine();
                while(str!=null)
                {
                    fileTree.Node node = new fileTree.Node();
                    node._start = int.Parse(str);
                    str = reader.ReadLine();
                    node._filename = str;
                    str = reader.ReadLine();
                    node._create_time = Convert.ToDateTime(str);
                    str = reader.ReadLine();
                    node._last_modify = Convert.ToDateTime(str);
                    str = reader.ReadLine();
                    node._size = int.Parse(str);
                    str = reader.ReadLine();
                    node._type = str;
                    str = reader.ReadLine();
                    node._parent = filetree.searchNode(str, fileTree.FOLDER,filetree._curnode);
                    str = reader.ReadLine();
                    node._path = str;
                    str = reader.ReadLine();
                    node._childnode = new List<fileTree.Node>();
                    node._parent._childnode.Add(node);
                }
                reader.Close();
            }
        }

        public void recursive_write(fileTree.Node node)
        {
            foreach (fileTree.Node childnode in node._childnode)
            {
                write_catalog(childnode);
                if (childnode._childnode.Count > 0)
                    recursive_write(childnode);
            }
        }

        public void write_catalog(fileTree.Node node)
        {
            
            StreamWriter writer = File.AppendText(Application.StartupPath + "\\CategoryInfo.txt");

            writer.WriteLine(node._start);
            writer.WriteLine(node._filename);
            writer.WriteLine(node._create_time);
            writer.WriteLine(node._last_modify);
            writer.WriteLine(node._size);
            writer.WriteLine(node._type);
            writer.WriteLine(node._parent._filename);
            writer.WriteLine(node._path);
            writer.Close();
        }

        public void read_bitmap()
        {
            string path = Application.StartupPath + "\\BitMapInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < v_disk._blockNum; i++)
                {
                    v_disk._bitMap[i] = int.Parse(reader.ReadLine());
                }
                reader.Close();
            }
        }

        public void write_bitmap()
        {
            if (File.Exists(Application.StartupPath + "\\BitMapInfo.txt"))
                File.Delete(Application.StartupPath + "\\BitMapInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\BitMapInfo.txt");
            
            foreach(int i in v_disk._bitMap)
            {
                writer.WriteLine(i);
            }
            writer.Close();
        }

        public void read_memory()
        {
            string path = Application.StartupPath + "\\MemoryInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < v_disk._blockNum; i++)
                {
                    v_disk._memory[i] = reader.ReadLine();
                }
                reader.Close();
            }
        }

        public void write_memory()
        {
            if (File.Exists(Application.StartupPath + "\\MemoryInfo.txt"))
                File.Delete(Application.StartupPath + "\\MemoryInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\MemoryInfo.txt");

            foreach(string i in v_disk._memory)
            {
                writer.WriteLine(i);
            }
            writer.Close();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string node = treeView1.SelectedNode.Text;
            int index = treeView1.SelectedNode.SelectedImageIndex;
            string type;
            if (index == FOLDER_EMPTY)
                type = fileTree.FOLDER;
            else
                type = fileTree.TXT;
            fileTree.Node next_node = filetree.searchNode(node, type,filetree.root);
            if (type == fileTree.FOLDER)
            {
                filetree._curnode = next_node;
                cur_path = next_node._path;
                updateView();
            }
            else if (type == fileTree.TXT)
            {
                TXTFile txtfile = new TXTFile(next_node, this);
                txtfile.Show();
            }
        }
    }
}
