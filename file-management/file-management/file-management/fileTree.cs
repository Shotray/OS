using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_management
{
    public class fileTree
    {
        public const string TXT = "文本文档";
        public const string FOLDER = "文件夹";

        public class Node
        {
            public string _filename;
            public DateTime _create_time;
            public DateTime _last_modify;
            public int _size;
            public string _type;
            public int _start;
            public string _path;
            public List<Node> _childnode;
            public Node _parent;
            
            public Node() { }

            public Node(string filename, DateTime create_time, string type,Node parent)
            {
                _start = -1;
                if (type == TXT)
                    _filename = filename + ".txt";
                else
                    _filename = filename;
                _create_time = create_time;
                _last_modify = create_time;
                _size = 0;
                _type = type;
                _childnode = new List<Node>();
                _parent = parent;
                _path = parent._path + filename + "/";
            }
        }

        public Node _curnode=new Node();
        public Node root=new Node();

        public fileTree()
        {
            root._path = "./";
            root._filename = "./";
            root._type = FOLDER;
            root._childnode=new List<Node>();
            _curnode = root;
        }

        public void update()
        {
            _curnode._size = 0;
            foreach(Node childnode in _curnode._childnode)
            {
                _curnode._size += childnode._size;
            }
        }

        public void clear(fileTree.Node node)
        {
            for(int i = node._childnode.Count - 1; i >= 0; i--)
            {
                if (node._childnode[i]._childnode.Count > 0)
                    clear(node._childnode[i]);
                node._childnode.Remove(node._childnode[i]);
            }
        }

        public bool haveSameName(string filename,string type)
        {
            foreach(Node node in _curnode._childnode)
                if (node._filename == filename && node._type == type) return true;
            return false ;
        }

        public fileTree.Node searchNode(string filename,string type,fileTree.Node begin)
        {
            fileTree.Node node = null;
            foreach (fileTree.Node childnode in begin._childnode)
                if (childnode._filename == filename && childnode._type == type)
                    node = childnode;
            if (node == null) return root;
            return node;
        }

        public int getCurSize(Node node)
        {
            int size = 0;
            foreach(Node childnode in node._childnode)
            {
                size += childnode._size;
                if (childnode._childnode.Count > 0)
                    size += getCurSize(childnode);
            }
            return size;
        }
    }
}
