using System.IO;
using System.Reflection;
using System.Windows.Controls;
using XLib.Base;
using XLib.Base.Ex;
using XLib.Base.VirtualDisk;
using XLib.Node;
using XNode.SubSystem.OptionSystem;

namespace XNode.SubSystem.NodeLibSystem
{
    public class NodeLibManager : IManager
    {
        #region 单例

        private NodeLibManager() { }
        public static NodeLibManager Instance { get; } = new NodeLibManager();

        #endregion

        #region 属性

        /// <summary>根文件夹</summary>
        public Folder Root => _nodeLibRoot.Root;

        /// <summary>节点库字典</summary>
        public Dictionary<string, INodeLib> NodeLibDict { get; set; } = new Dictionary<string, INodeLib>();

        #endregion

        #region IManager 方法

        public void Init()
        {
            LoadOutsideNodeLib();
        }

        public void Reset() { }

        public void Clear() { }

        #endregion

        #region 注册型

        public NodeBase? CreateNode(string typeString)
        {
            //查找包含这个type的nodelib
            var ttlib = NodeLibDict.Values.FirstOrDefault(x => x.ContainsNode(typeString));
            if (ttlib == null) return null;
            //创建节点
            NodeBase? node = ttlib.CreateNode(typeString);
            return node;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载外部节点库
        /// </summary>
        private void LoadOutsideNodeLib()
        {
            // 遍历节点库文件
            foreach (var dllPath in GetAllNodeLibDll())
            {
                // 加载动态库
                Assembly dll = Assembly.LoadFrom(dllPath);
                // 遍历全部类
                foreach (var type in dll.GetTypes())
                {
                    if (typeof(INodeLib).IsAssignableFrom(type))
                    {
                        // 获取单例
                        PropertyInfo? propertyInfo = type.GetProperty("Instance");
                        if (propertyInfo == null) continue;
                        if (propertyInfo.GetValue(null) is not INodeLib instance) continue;
                        // 初始化单例
                        instance.Init();
                        // 保存引用
                        NodeLibDict.Add(instance.Name, instance);
                    }
                }
            }
            // 遍历节点库
            foreach (var libPair in NodeLibDict)
            {
                // 创建根文件夹
                Folder root = _nodeLibRoot.CreateFolder(libPair.Value.Title.PackToList());
                // 加载文件夹
                LoadFolder(root, libPair.Value.LibHarddisk.Root);
            }
        }

        /// <summary>
        /// 获取全部节点库文件
        /// </summary>
        private List<string> GetAllNodeLibDll()
        {
            if (!Directory.Exists(OptionManager.Instance.NodeLibPath)) return new List<string>();

            DirectoryInfo directoryInfo = new DirectoryInfo(OptionManager.Instance.NodeLibPath);
            List<string> result = new List<string>();
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".dll") result.Add(fileInfo.FullName);
            }
            return result;
        }

        /// <summary>
        /// 加载文件夹至目标文件夹
        /// </summary>
        private void LoadFolder(Folder target, Folder oldFolder)
        {
            // 加载文件夹
            foreach (var oldChild in oldFolder.FolderList)
            {
                // 创建子文件夹
                Folder childFolder = new Folder(oldChild.Name, target);
                // 添加子文件夹
                target.FolderList.Add(childFolder);
                // 递归加载
                LoadFolder(childFolder, oldChild);
            }
            // 加载文件
            foreach (var oldFile in oldFolder.FileList)
            {
                // 创建文件
                _nodeLibRoot.CreateFile(target, oldFile.Name, oldFile.Extension, oldFile.Instance);
            }
        }

        #endregion

        #region 字段

        /// <summary>节点库磁盘</summary>
        private readonly Harddisk _nodeLibRoot = new Harddisk();

        #endregion
    }
}