using FSLib.App.SimpleUpdater;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrpClient
{
    static class Program
    {
        public static EventWaitHandle ProgramStarted;  

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //创建自己命名的EventWaitHandle事件，系统中没有该事件时创建同时标记outIsCreate为true，如果已存在，则返回该事件同时标记outIsCreate为false
            bool outIsCreate;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "GZGFRPStartEvent", out outIsCreate);

            //系统中已存在该事件，则调用该事件
            if (!outIsCreate)
            {
                ProgramStarted.Set();
                return;
            }

            CheckUpdate();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void CheckUpdate()
        {
            Updater.CheckUpdateSimple("https://gitee.com/zhanggangbz/public_update/raw/master/frpclient/{0}", "update_c.xml");

            //4.通过实例对象进行手动更新
            var updater = Updater.Instance;
            //这里可以设置一些Updater的属性
            //然后手动调用检查更新
            updater.BeginCheckUpdateInProcess();

        }
    }
}
