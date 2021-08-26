using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_management
{
    public class virtualDisk
    {
        public const int EMPTY = 0;
        public const int END = -1;
        public const int TOL_SIZE = 1000;
        public const int BLOCK_SIZE = 2;

        public int _blockNum;
        public int _remain;
        public string[] _memory;
        public int[] _bitMap;

        public virtualDisk()
        {
            _blockNum = TOL_SIZE / BLOCK_SIZE;
            _remain = _blockNum;
            _memory = new string[_blockNum];
            _bitMap = new int[_blockNum];
            
            for(int i = 0; i < _blockNum; i++)
            {
                _memory[i] = "";
                _bitMap[i] = EMPTY;
            }
        }

        public void clear()
        {
            for(int i = 0; i < _blockNum; i++)
            {
                _memory[i] = "";
                _bitMap[i] = EMPTY;
            }
        }

        public bool addFile(fileTree.Node file,string content)
        {
            int len = (content.Length+1) / BLOCK_SIZE;
            if (this._remain < len) return false;
            file._size = content.Length;
            int pre = -1;int count = 0;
            for(int i = 0; i < _blockNum; i++)
            {
                if (count == len) break;
                if (_bitMap[i] == EMPTY)
                {
                    if (content.Length < BLOCK_SIZE)
                    {
                        _memory[i] = content;
                        content = "";
                    }
                    else
                    {
                        _memory[i] = content.Substring(0, BLOCK_SIZE);
                        content = content.Substring(BLOCK_SIZE);
                    }
                    if (file._start == -1)
                    {
                        file._start = i;
                        pre = i;
                        count++;
                        continue;
                    }
                    _bitMap[pre] = i;
                    pre = i;
                    count++;
                    _remain--;
                }
            }
            _bitMap[pre] = END;
            _remain--;
            return true;
        }

        public bool deleteFile(fileTree.Node file)
        {
            if (file._start == -1) return true;
            int cur = file._start;
            while (_bitMap[cur] != END)
            {
                int temp = _bitMap[cur];
                _memory[cur] = "";
                _bitMap[cur] = EMPTY;
                cur = temp;
                _remain++;
            }
            _memory[cur] = "";
            _remain++;
            _bitMap[cur] = EMPTY;
            file._start = -1;
            file._size = 0;
            return true;
        }

        public string showFile(fileTree.Node file)
        {
            if (file._start == -1) return "";
            string ans="";
            int cur = file._start;
            while (_bitMap[cur] != -1)
            {
                ans = ans + _memory[cur];
                cur = _bitMap[cur];
            }
            ans += _memory[cur];
            return ans;
        }
    }
}
