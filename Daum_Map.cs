using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Econtact
{
    public partial class Maps : Form
    {
        public Maps()
        {
            InitializeComponent();
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
            string city = textBox1.Text;
            string state = textBox2.Text;
            string country = textBox3.Text;

            //StringBuilder 클래스의 새 인스턴스를 add로 추가한다.

            StringBuilder add = new StringBuilder("http://map.daum.net/?q="); //지도 접근
            add.Append(city);
            add.Append(state);
            add.Append(country);
            //지정된 Char 개체의 문자열 표현을 add에 추가합니다.

            webBrowser1.Navigate(add.ToString());
            //연결된 링크에서 add의 인스턴스를 통해 탐색을 합니다.
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close(); // 닫기
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
