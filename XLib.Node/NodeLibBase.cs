using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLib.Base.Ex;
using XLib.Base.VirtualDisk;

namespace XLib.Node
{
    public abstract class NodeLibBase : INodeLib
    {
        public abstract string Name { get; set; }

        public abstract string Title { get; set; }

        public Harddisk LibHarddisk { get; set; } = new Harddisk();

        /// <summary>
        /// 节点构造器，输入名字构造节点
        /// </summary>
        public Dictionary<string, Func<NodeBase>> NodeConstructors { get; } = new Dictionary<string, Func<NodeBase>>();
        public void Clear()
        {
            //clear all nodes
        }

        /// <summary>
        /// 判断节点库是否包含某个节点
        /// </summary>
        /// <param name="typeString"></param>
        /// <returns></returns>
        public virtual bool ContainsNode(string typeString)
        {
            return LibHarddisk.FindFile(typeString) != null;
        }

        public virtual NodeBase? CreateNode(string typeString)
        {
            var file = LibHarddisk.FindFile(typeString);
            if (file != null)
            {
                var node = NodeConstructors[file.Name]();
                return node;
            }
            return null;
        }

        public virtual void Init()
        {
        }

        public bool RegisterNode<T>(string group = "") where T : NodeBase, new()
        {
            Type _type = typeof(T);
            //注册节点
            var node = (NodeBase)Activator.CreateInstance(_type);
            if (node == null) return false;
            NodeConstructors.Add(_type.Name, () => node);

            //注册节点到磁盘
            Folder curfolder = null;
            if (group == "")
            {
                curfolder = LibHarddisk.Root;
            }
            else
            {
                curfolder = LibHarddisk.FindFolder(new List<string> { group });
                if (curfolder == null)
                    curfolder = LibHarddisk.CreateFolder(group.PackToList());
            }
            LibHarddisk.CreateFile(curfolder, _type.Name, "nt", new NodeType<T>());
            return true;
        }
    }
}
