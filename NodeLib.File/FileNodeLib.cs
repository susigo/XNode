using NodeLib.File.Define;
using NodeLib.File.Define.Rename;
using XLib.Base.Ex;
using XLib.Base.VirtualDisk;
using XLib.Node;

namespace NodeLib.File
{
    public class FileNodeLib : NodeLibBase
    {
        #region 单例

        private FileNodeLib() { }
        public static FileNodeLib Instance { get; } = new FileNodeLib();

        #endregion

        #region INodeLib 方法

        public override string Name { get; set; } = "文件";
        public override string Title { get; set; } = "文件";

        public override void Init()
        {
            RegisterNode<Func_Upper>("重命名");
            RegisterNode<Func_GetFileMD5>("重命名");
        }

        #endregion
    }
}