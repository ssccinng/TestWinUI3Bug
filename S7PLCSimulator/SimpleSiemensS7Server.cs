using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Snap7;
using static Snap7.S7Server;

namespace S7PLCSimulator
{

    public enum AreaType
    {
        SrvAreaPe = 0,
        SrvAreaPa = 1,
        SrvAreaMk = 2,
        SrvAreaCt = 3,
        SrvAreaTm = 4,
        SrvAreaDb = 5,
    }

    /// <summary>
    /// 简单版本的s7服务器，不提供CPU版本
    /// </summary>
    public class SimpleSiemensS7Server: S7Server
    {
        /// <summary>
        /// 通信socket
        /// </summary>
        // private Socket _s7Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        // private S7Server _server = new S7Server();
        public Action<string> LogAction;

        private Dictionary<int, byte[]> [] _areas = new Dictionary<int, byte[]>[7];
        public Dictionary<int, byte[]> DataBlocks => _areas[5];
        public SimpleSiemensS7Server()
        {
            for (var i = 0; i < _areas.Length; i++)
            {
                _areas[i] = new();
            }
            SetEventsCallBack(EventCallback, IntPtr.Zero);
            Console.WriteLine("sadasd");
        }
        
        /// <summary>
        /// 事件回调函数
        /// </summary>
        /// <param name="usrPtr"></param>
        /// <param name="Event"></param>
        /// <param name="Size"></param>
        public void EventCallback(IntPtr usrPtr, ref USrvEvent Event, int Size)
        {
            //Debug.WriteLine(this);
            var msg = EventText(ref Event);
            Console.WriteLine(msg);
            LogAction?.Invoke(msg);
            //RegisterArea()
        }

        public SimpleSiemensS7Server LogTo(Action<string> action)
        {
            LogAction = action;
            return this;
        }


        public bool RegisterArea(AreaType areaType, int idx, int length = 9000)
        {
            if (ServerStatus != 1) return false; ;
            if (_areas[(int)areaType].ContainsKey(idx))
            {
                LogAction?.Invoke("此区域已经被注册过了");
                return false;
            }
            _areas[(int)areaType].Add(idx, new byte[length]);
            var aa = _areas[(int)areaType].GetValueOrDefault(idx);
            
            ;
            // TODO:
            // 如果没成功要清除
            var aa1 = RegisterArea((int)areaType, idx, ref aa, length);
            return aa1 == 0;
        }

        public bool UnregisterArea(AreaType areaType, int idx)
        {
            if (_areas[(int)areaType][idx] == null)
            {
                LogAction?.Invoke("没有可以取消注册的");
                return false;
            }
            //_areas[(int)areaType][idx] = null;
            _areas[(int)areaType].Remove(idx);
            base.UnregisterArea((int)areaType, idx);
            return true;
        }

        public int StopServer()
        {
            foreach (var key in DataBlocks.Keys)
            {
                UnregisterArea(AreaType.SrvAreaDb, key);
            }
            for (int i = 0; i < _areas.Length; i++)
            {
                
                _areas[i] = new();
            }

            
            return Stop();
        }

        //public int StartServer()
        //{
        //    sta
        //    return Stop();
        //}

    }

    public static class gg
    {
        public static void EventCallback(IntPtr usrPtr, ref USrvEvent Event, int Size)
        {
            //Debug.WriteLine(this);
            //var msg = EventText(ref Event);
            //Console.WriteLine(msg);
            //LogAction?.Invoke(msg);
            // RegisterArea()
        }
    }
}
