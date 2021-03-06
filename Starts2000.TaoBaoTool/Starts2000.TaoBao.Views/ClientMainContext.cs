﻿using System;
using System.Windows.Forms;
using Ninject;
using Starts2000.TaoBao.Views.ViewResource;

namespace Starts2000.TaoBao.Views
{
    internal class ClientMainContext : ApplicationContext
    {
        readonly ViewLogin _login;
        readonly ViewClientMain _main;
        INotifyIconMagager _notifyIcon;

        [Inject]
        public ClientMainContext(ViewLogin login, 
            ViewClientMain main, INotifyIconMagager notifyIcon)
        {
            _login = login;
            _login.IsClient = true;
            _main = main;
            _notifyIcon = notifyIcon;

            Init();
        }

        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            var view = sender as ViewLogin;
            if (view != null && view.DialogResult == DialogResult.OK)
            {
                _main.Text = string.Concat(ViewResources.ClientName, " - 会员(", view.Account, ")");
                base.MainForm = _main;
                base.MainForm.Show();
                _notifyIcon.Text = string.Format("{0}{1}会员：{2}", ViewResources.ClientName,
                    Environment.NewLine, view.Account);
                return;
            }

            _main.Dispose();
            _notifyIcon.Close();
            base.OnMainFormClosed(sender, e);
        }

        void Init()
        {
            _notifyIcon.Operate = type =>
            {
                switch (type)
                {
                    case OptType.LeftClik:
                    case OptType.ShowMainForm:
                        if (base.MainForm != null && !base.MainForm.IsDisposed)
                        {
                            if (base.MainForm.WindowState == FormWindowState.Minimized)
                            {
                                base.MainForm.WindowState = FormWindowState.Normal;
                            }
                            base.MainForm.Show();
                        }
                        break;
                    case OptType.Exit:
                        if (base.MainForm != null && !base.MainForm.IsDisposed)
                        {
                            base.MainForm.Close();
                        }
                        break;
                }
            };
            base.MainForm = _login;
            base.MainForm.Show();
            _notifyIcon.Text = string.Concat(ViewResources.ClientName, "登录");
            _notifyIcon.Show();
        }
    }
}