using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Codefirsy
{
    public partial class Form1 : Form
    {
        private UniversityDBEntities dbContext;
        public Form1()
        {
            InitializeComponent();
            dbContext = new UniversityDBEntities();
        }
        private void LoadStudentData()
        {
            // Lấy dữ liệu từ bảng Student và thực hiện JOIN với bảng Faculty để lấy tên khoa
            var studentData = from student in dbContext.Student
                              join faculty in dbContext.Faculty on student.FacultyID equals faculty.FacultyID
                              select new
                              {
                                  student.StudentID,
                                  student.FullName,
                                  student.AverageScore,
                                  FacultyName = faculty.FacultyName
                              };


            foreach (var item in studentData)
            {
                dataGridView1.Rows.Add(item.StudentID, item.FullName, item.FacultyName, item.AverageScore);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtMSSV.Text = row.Cells[0].Value?.ToString() ?? "";   // Cột MSSV
                txtHoTen.Text = row.Cells[1].Value?.ToString() ?? "";  // Cột HoTen
                cmbKhoa.Text = row.Cells[2].Value?.ToString() ?? "";   // Cột Khoa
                txtDiemTB.Text = row.Cells[3].Value?.ToString() ?? ""; // Cột Diem TB
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudentData();
            LoadFacultyList();
        }

        private void LoadFacultyList()
        {
            cmbKhoa.DataSource = dbContext.Faculty.ToList();
            cmbKhoa.DisplayMember = "FacultyName";
            cmbKhoa.ValueMember = "FacultyID";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Thêm mới sinh viên vào cơ sở dữ liệu
                var newStudent = new Student
                {
                    FullName = txtHoTen.Text,
                    AverageScore = decimal.Parse(txtDiemTB.Text),
                    FacultyID = (int)cmbKhoa.SelectedValue
                };

                dbContext.Student.Add(newStudent);
                dbContext.SaveChanges();

                // Sau khi thêm, thêm sinh viên vào DataGridView mà không tải lại toàn bộ dữ liệu
                dataGridView1.Rows.Add(newStudent.StudentID, newStudent.FullName, cmbKhoa.Text, newStudent.AverageScore);

                MessageBox.Show("Thêm sinh viên thành công!");

                // Clear các trường nhập liệu sau khi thêm
                txtMSSV.Clear();
                txtHoTen.Clear();
                txtDiemTB.Clear();
                cmbKhoa.SelectedIndex = -1;  // Đặt lại giá trị ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            try
            {
                // Lấy StudentID từ ô MSSV
                int studentId = int.Parse(txtMSSV.Text);

                // Tìm sinh viên trong database
                var student = dbContext.Student.Find(studentId);

                if (student != null)
                {
                    // Cập nhật thông tin sinh viên
                    student.FullName = txtHoTen.Text;
                    student.AverageScore = decimal.Parse(txtDiemTB.Text);
                    student.FacultyID = (int)cmbKhoa.SelectedValue;

                    dbContext.SaveChanges(); // Lưu thay đổi vào database

                    // Cập nhật dòng tương ứng trong DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == studentId.ToString()) // Tìm dòng đúng ID
                        {
                            row.Cells[1].Value = txtHoTen.Text;     // Cập nhật Họ tên
                            row.Cells[2].Value = cmbKhoa.Text;      // Cập nhật Khoa
                            row.Cells[3].Value = txtDiemTB.Text;    // Cập nhật Điểm TB
                            break;
                        }
                    }

                    MessageBox.Show("Cập nhật sinh viên thành công!");
                }
                else
                {
                    MessageBox.Show("Mã sinh viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho Mã sinh viên và Điểm trung bình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int studentId = int.Parse(txtMSSV.Text);
                var student = dbContext.Student.Find(studentId);

                if (student != null)
                {
                    dbContext.Student.Remove(student);
                    dbContext.SaveChanges();
                    MessageBox.Show("Xóa sinh viên thành công!");

                    // Xóa dòng đã chọn trong DataGridView
                    DataGridViewRow row = dataGridView1.SelectedRows[0];
                    dataGridView1.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
