using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _0722detetion.ViewModel
{
    public class SocketConnectionViewModel:BindableBase
    {

        #region service
        private string  serviceIp;

		public string  ServiceIp
		{
			get { return serviceIp; }
			set { serviceIp = value; RaisePropertyChanged(); }
		}

		private string servicePort;

		public string ServicePort
		{
			get { return servicePort; }
			set { servicePort = value; RaisePropertyChanged(); }
		}


		private String clientIp;

		public String ClientIp
		{
			get { return clientIp; }
			set { clientIp = value;  RaisePropertyChanged(); }
		}


		private string clientPort;

		public string ClientPort
		{
			get { return clientPort; }
			set { clientPort = value; RaisePropertyChanged();}
		}



		private string  serviceReceiveMessage;

		public string  ServiceReceiveMessage
		{
			get { return serviceReceiveMessage; }
			set { serviceReceiveMessage = value; RaisePropertyChanged(); }
		}

		private string serviceSendMessage;

		public string ServiceSendMessage
		{
			get { return serviceSendMessage; }
			set { serviceSendMessage = value;  RaisePropertyChanged(); }
		}



		Socket ServiceSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        #endregion

        


        public SocketConnectionViewModel()
        {
            ServiceConnnectionCommand = new DelegateCommand(ConnectToServer);
			ServiceSendCommand = new DelegateCommand(ServiceSend);
        }

        

        public void ReceiveMessage(string message)
        {
            ServiceReceiveMessage += message;
        }

        public DelegateCommand ServiceConnnectionCommand { get; set; }

		public DelegateCommand ServiceSendCommand { get; set; }
		bool flag= false;

        void ConnectToServer()
		{
			try
			{
				IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ServiceIp), int.Parse(ServicePort));
				ServiceSocket.Connect(iPEndPoint);
				

                /*ServiceSocket.Listen(10); // 设置监听队列长度为10*/
                ReceiveMessage("连接成功！\n");
				flag = true;

                Task.Run(() => {

					LisentMessage();

				});
				


            }
			catch (SocketException ex)
			{
				Console.WriteLine($"连接失败: {ex.Message}");
			}
        }

        private void LisentMessage()
        {
			return;
        }


		private void ServiceSend()
		{
			if (flag)
			{
				try
				{
					byte[] data = Encoding.UTF8.GetBytes(ServiceSendMessage);
					ServiceSocket.Send(data);
					ReceiveMessage($"发送成功: {ServiceSendMessage}\n");
				}
				catch (Exception ex)
				{
					ReceiveMessage($"发送失败: {ex.Message}\n");
				}
			}
			else
			{
				ReceiveMessage("请先连接服务器！\n");
			}
		}
    }
}
