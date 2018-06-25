using Econtact.econtactClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Econtact.econtactClasses
{
    class contactClass
    {
        //게터, 세터 속성
        //어플리케이션에서 데이터 캐리어로서의 역할
        public int ContactID { get; set; }

        public string  FirstName { get; set; }

        public string  LastName { get; set; }

        public string  ContactNo { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //데이터베이스에서 데이터 선택

        public DataTable Select()

        {



            //step 1 : 데이터베이스 연결

            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //Step 2 :  SQL 쿼리 작성
                string sql = "SELECT * FROM tbl_contact";

                //sql과 conn을 사용하여 cmd 생성 
                SqlCommand cmd = new SqlCommand(sql, conn);

                //cmd를 사용하여 SQL DataAdapter 만들기
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                conn.Open();

                adapter.Fill(dt);
            }  
            catch(Exception ex)
            {
              
            }
             finally
            {
                conn.Close();
            }

            return dt;

        }



        // 데이터베이스 안에 값 넣기

        public bool Insert(contactClass c)
        {
            //기본 반환 유형 만들기 및 해당 값을 false로 설정

            bool isSuccess = false;

            //Step 1 : 데이터베이스 연결
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                // Step 2 : 데이터를 삽입하는 SQL 쿼리 만들기
                string sql = "INSERT INTO tbl_contact (FirstName, LastName, ContactNo, Address, Gender) VALUES (@FirstName, @LastName, @ContactNo, @Address, @Gender)";

                //SQL과 conn을 사용하여 SQL 커맨드 생성하기
                SqlCommand cmd = new SqlCommand(sql, conn);

                //데이터를 추가 할 매개 변수 만들기

                cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
                cmd.Parameters.AddWithValue("@LastName", c.LastName);
                cmd.Parameters.AddWithValue("@ContactNo", c.ContactNo);
                cmd.Parameters.AddWithValue("@Address", c.Address);
                cmd.Parameters.AddWithValue("@Gender", c.Gender);


                //여기에서 연결 열기
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                //쿼리가 성공적으로 실행되면 행의 값은 0보다 커야합니다. 그렇지 않으면 값은 0.

                if (rows>0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {
               
            }

            finally
            {
                conn.Close();
            }

            return isSuccess;

        }

        //응용 프로그램에서 데이터베이스의 데이터를 업데이트하는 방법
        public bool Update(contactClass c)
        {
            //기본 반환 유형을 만들고 기본값을 false로 설정합니다.
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //데이터베이스에서 데이터를 업데이트하는 SQL
                string sql = "UPDATE tbl_contact SET FirstName=@FirstName, LastName=@LastName, ContactNo=@ContactNo, Address=@Address, Gender=@Gender WHERE ContactID=@ContactID";

                //SQL 명령 작성
                SqlCommand cmd = new SqlCommand(sql, conn);
                //값을 추가 할 매개 변수 만들기

                cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
                cmd.Parameters.AddWithValue("@LastName", c.LastName);
                cmd.Parameters.AddWithValue("@ContactNo", c.ContactNo);
                cmd.Parameters.AddWithValue("@Address", c.Address);
                cmd.Parameters.AddWithValue("@Gender", c.Gender);
                cmd.Parameters.AddWithValue("ContactID", c.ContactID);


                //데이터베이스 연결 열기
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //쿼리가 성공적으로 실행되면 행의 값이 0보다 커야합니다. 그렇지 않으면 값이 0이됩니다.

                if (rows>0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }

            catch(Exception ex)
            {
               
            }

            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        //데이터베이스에서 데이터를 삭제하는 방법

        public bool Delete(contactClass c)
        {
            // 기본 반환 값을 만들고 해당 값을 false로 설정합니다.
            bool isSuccess = false;

            //SQL 연결 만들기 
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //데이터를 삭제하는 SQL
                string sql = "DELETE FROM tbl_contact WHERE ContactID=@ContactID";

                //SQL 명령 작성 
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ContactID", c.ContactID);
                //연결 열기
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                //쿼리가 성공적으로 실행되면 행의 값은 0보다 크고 그 값은 0입니다.

                if (rows>0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                //연결 닫기 
                conn.Close();
            }

            return isSuccess;
        }

    }
}
