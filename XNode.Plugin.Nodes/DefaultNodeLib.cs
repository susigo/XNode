using XLib.Base.Ex;
using XLib.Base.VirtualDisk;
using XLib.Node;
using XNode.Plugin.Nodes.Define.Data;
using XNode.Plugin.Nodes.Define.Drivers;
using XNode.Plugin.Nodes.Define.Events;
using XNode.Plugin.Nodes.Define.Flows;
using XNode.Plugin.Nodes.Define.Functions;

namespace XNode.Plugin.Nodes
{
    public class DefaultNodeLib : NodeLibBase
    {
        #region 单例

        private DefaultNodeLib() { }
        public static DefaultNodeLib Instance { get; } = new DefaultNodeLib();

        #endregion
        public override string Name { get; set; } = "Default";
        public override string Title { get; set; } = "Default";

        #region INodeLib 方法
        const string DataNodes = "数据节点";
        const string DriverNodes = "驱动节点";
        const string EventNodes = "事件节点";
        const string FlowNodes = "流控制节点";
        const string LogicNodes = "逻辑节点";
        const string ControlNodes = "控制节点";
        const string OtherNodes = "其他节点";
        const string FuncsNodes = "函数节点";


        public override void Init()
        {
            RegisterNode<Data_Int>(DataNodes);
            RegisterNode<Data_Double>(DataNodes);
            RegisterNode<Data_String>(DataNodes);
            RegisterNode<FrameDriver>(DriverNodes);
            RegisterNode<TimerDriver>(DriverNodes);
            RegisterNode<Event_Keyboard>(EventNodes);
            RegisterNode<Func_Compare>(FuncsNodes);
            RegisterNode<Func_RatioToInt>(FuncsNodes);
            RegisterNode<Func_NumberToRatio>(FuncsNodes);
            RegisterNode<Func_SendNetMessage>(FuncsNodes);
            RegisterNode<Func_Log>(FuncsNodes);
            RegisterNode<Func_CreateThread>(FuncsNodes);
            RegisterNode<Func_Delay>(FuncsNodes);
            RegisterNode<Func_Sleep>(FuncsNodes);
            RegisterNode<Flow_If>(FlowNodes);
            RegisterNode<Flow_LoopByCount>(FlowNodes);
            RegisterNode<Flow_While>(FlowNodes);
            RegisterNode<Flow_Switch>(FlowNodes);

        }

        #endregion
    }
}