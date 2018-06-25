# Practice_C-
Practice_C#
using Econtact.econtactClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Econtact
{
    public partial class Econtact : Form
    {
        public Econtact()
        {
            InitializeComponent();
        }

        contactClass c = new contactClass();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            //입력 필드에서 값 가져 오기

            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtBoxContactNumber.Text;
            c.Address = txtBoxAddress.Text;
            c.Gender = cmbGender.Text;


            //이전 에피소드에서 만든 메소드를 사용하여 데이터베이스에 데이터 삽입
            bool success = c.Insert(c);

            if (success==true)
            {
                //성공적으로 삽입 됨

                MessageBox.Show("새 연락처가 성공적으로 삽입되었습니다.");

                //여기서 Clear 메소드를 호출 
                Clear();

            }

            else
            {
                //삽입 실패 시 
                MessageBox.Show("새 연락처를 추가하지 못했습니다. 다시 시도하십시오.");

            }


            //데이터 GRidview에 데이터로드
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;


        }

        private void Econtact_Load(object sender, EventArgs e)
        {
            //데이터 GRidview에 데이터로드
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //필드를 지우는 메소드

        public void Clear()
        {
            txtboxFirstName.Text = "";
            txtboxLastName.Text = "";
            txtBoxContactNumber.Text = "";
            txtBoxAddress.Text = "";
            cmbGender.Text = "";
            txtboxContactID.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //텍스트 상자에서 데이터 가져 오기
            c.ContactID = int.Parse(txtboxContactID.Text);
            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtBoxContactNumber.Text;
            c.Address = txtBoxAddress.Text;
            c.Gender = cmbGender.Text;

            //데이터베이스의 데이터 업데이트

            bool success = c.Update(c);

            if (success == true)
            {
                //업데이트 완료
                MessageBox.Show("성공적으로 업데이트되었습니다.");
                //데이터 GRidview에 데이터로드
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
                //Clear 메소드 호출
                Clear();
            }
            else
            {
                //업데이트 실패
                MessageBox.Show("업데이트하지 못했습니다. 다시 시도하십시오.");
            }

        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //데이터 격자보기에서 데이터 가져 오기 및 텍스트 상자에 각각로드

            //마우스가 클릭 된 행 식별

            int rowIndex = e.RowIndex;
            txtboxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            txtboxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            txtboxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxContactNumber.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            txtBoxAddress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //여기서 Clear 메소드 호출
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //응용 프로그램에서 연락처 ID 가져 오기
            c.ContactID = Convert.ToInt32(txtboxContactID.Text);
            bool success = c.Delete(c);
            if (success == true)
            {
                //성공적으로 삭제

                MessageBox.Show("성공적으로 삭제 되었습니다.");

                //데이터 GridView 새로 고침
                //데이터 GRidview에 데이터로드
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;

                //Clear 메소드 호출
                Clear();
            }
            else
            {
                //삭제 실패 
                MessageBox.Show("연락처를 삭제하지 못했습니다. 다시 시도하십시오.");
            }
        }

        static string myconnstr = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {

            //텍스트 상자에서 값 가져 오기
            string Keyword = txtboxSearch.Text;

            SqlConnection conn = new SqlConnection(myconnstr);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE FirstName Like '%" + Keyword + "%' OR LastName LIKE '%"+Keyword+"%' OR Address LIKE '%"+Keyword+"%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
