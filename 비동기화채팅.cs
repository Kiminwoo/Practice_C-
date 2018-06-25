using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
// 소켓 통신을 위한 Using 선언
namespace Econtact
{
    public partial class Chat_Client_APP : Form
    {
        Socket sck;
        EndPoint epLocal, epRemote;
        //어느 주소에서 연결 요청을 대기할 것인지를 나타냅니다 , 네트워크 상의 주소입니다.

        public Chat_Client_APP()
        {
            InitializeComponent();

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //지정된 몇가지의 요소들로 Socket클래스의 새 인스턴스를 초기화합니다.

            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //Socket인스턴스의 옵션을 설정해줍니다.

            textLocalIP.Text = GetLocalIP();
            textFriendsIp.Text = GetLocalIP();
            // 밑에서 선언해준 GetLocalIP()메소드에서 ip주소를 가져와서 각각에 text박스에 띄어줍니다.
        }

        private string GetLocalIP() // IP주소를 가져오는 메소드 , IPv4 형식으로 주소를 가져오는 방법입니다.
        {
            IPHostEntry host; // 인터넷 호스트 주소 정보를 가져와 변수에 저장
            host = Dns.GetHostEntry(Dns.GetHostName());// 단순 도메인 이름 확인기능

            foreach (IPAddress ip in host.AddressList) //호스트와 연결된 ip주소 목록을 가져옵니다.
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //만약 ip의 주소와 가져온 ip의 주소가 같다면 
                {
                    return ip.ToString(); //인터넷 주소를 표준 표기법으로 변환합니다.
                }
            }

            return "127.0.0.1";
        }

        private void MessageCallBack(IAsyncResult aResult) // IAsyncResult 인터페이스는 BeginAccept 등의 비동기 메서드에서 넘겨준 추가 정보 및 작업에 대한 정보를 저장하는 인터페이스

        {
            try
            {
                int size = sck.EndReceiveFrom(aResult, ref epRemote); // 끝점에서 받아온 자료들을 size변수에 저장을 시키고  

                if (size > 0) // 받아온 자료가 0이상일 경우에 
                {
                    byte[] receivedData = new byte[1464]; //1464 바이트의 크기를 갖는 바이트 배열을 가진 receivedDat 클래스 생성



                    receivedData = (byte[])aResult.AsyncState; //byte배열인 비동기적 상태를 receiveDate클래스에 저장을 시킵니다.

                    UTF8Encoding eEncoding = new UTF8Encoding(); //ASCIIEncoding 클래스의 새 인스턴스 eEncoding를 초기화합니다
                    //오류 해결 부분
                    string receivedMessage = eEncoding.GetString(receivedData); //변수 receivedMessage 에 가져온 receivedData를 위에서 선언해준 ascill 인코딩으로 변환해줍니다.

                    listMessage.Items.Add("상대방 : "+receivedMessage); //채팅내용박스에 상대방이 보낸 내용을 추가해줍니다.

                }

                byte[] buffer = new byte[1500]; //buffer 배열을 새로 만들어줍니다. 왜냐하면 자료를 받을 배열을 선언해 주기 위해서 입니다.

                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
                //배열선언을 해주었고 , 바이트 배열 0번째 부터 받아올 것이고 , 바이트 배열의 길이만큼 수신할 것이고 , 소켓 옵션 = 0 으로 설정해주며 , 자료를 수신하면 호출되는 비 동기 대리자 MessageCallBack 메소드를 호출하고 , 추가적을 넘겨줄 buffer배열 선언
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e) // 접속 버튼 이벤트 
        {
            try
            {
                epLocal = new IPEndPoint(IPAddress.Parse(textLocalIP.Text), Convert.ToInt32(textLocalPort.Text)); //epLocal변수에 지정된 ip주소와 Port번호를 저장시킵니다. 
                sck.Bind(epLocal); //소켓에 epLocal에 저장된 ip주소와 포트번호를 넘겨줍니다.

                epRemote = new IPEndPoint(IPAddress.Parse(textFriendsIp.Text), Convert.ToInt32(textFriendsPort.Text));//epRemote변수에 지정된 ip주소와 Port번호를 저장시킵니다.
                sck.Connect(epRemote); // 소켓에 epRemote에 저장된 ip주소와 포트번호를 연결합니다.

                byte[] buffer = new byte[1500];
                //수신될 자료형의 배열을 선언해줍니다.
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
                //배열선언을 해주었고 , 바이트 배열 0번째 부터 받아올 것이고 , 바이트 배열의 길이만큼 수신할 것이고 , 소켓 옵션 = 0 으로 설정해주며 , 자료를 수신하면 호출되는 비 동기 대리자 MessageCallBack 메소드를 호출하고 , 추가적을 넘겨줄 buffer배열 선언

                button1.Text = "연결중(Connected)...";
                button1.Enabled = false;
                button2.Enabled = true;
                textMessage.Focus(); // 포커스 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) //전송 버튼 이벤트
        {
            try
            {                  //오류 해결 부분
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();// 시스템에 디코딩할 asciiEncoding을 enc변수에 저장을 시킵니다.
                byte[] msg = new byte[1500]; // msg라는 바이트 배열변수를 생성
                msg = enc.GetBytes(textMessage.Text); // msg변수에 textMessage.text에 입력된 내용을 가져와 디코딩 합니다.


                sck.Send(msg); // 연결된 소켓에 msg를 보냅니다 . 즉 textMessage에 입력된 내용을 보냅니다.

                listMessage.Items.Add("나: " + textMessage.Text);  // 채팅내용에 내가 쓴 내용을 추가해줍니다. 

                textMessage.Clear(); // 입력한 내용은 초기화가 됩니다.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
    }
}
