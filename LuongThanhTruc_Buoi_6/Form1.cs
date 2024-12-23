using LuongThanhTruc_Buoi_6.Model2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuongThanhTruc_Buoi_6
{
    public partial class QuanLySinhVien : Form
    {
        private StudentContextDB context;
        private Student selectedStudent;
        public QuanLySinhVien()
        {
            InitializeComponent();
        }

        private void FillFalcutyComboBox(List<Faculty> listFalcutys)
        {
            this.cboKhoa.DataSource = listFalcutys;
            this.cboKhoa.DisplayMember = "FacultyName";
            this.cboKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells[1].Value = item.FullName;
                dgvSinhVien.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvSinhVien.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void ClearForm()
        {
            txt_MSSV.Clear();
            txtHo_Ten.Clear();
            cboKhoa.SelectedIndex = -1;
            txtDiemTB.Clear();
        }

        private void LoadData()
        {
            try
            {
                context = new StudentContextDB();
                List<Faculty> listFalcutys = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
                FillFalcutyComboBox(listFalcutys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QuanLySinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                string studentID = row.Cells[0].Value.ToString();

                selectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                if (selectedStudent != null)
                {

                    txt_MSSV.Text = selectedStudent.StudentID.ToString();
                    txtHo_Ten.Text = selectedStudent.FullName;
                    cboKhoa.SelectedValue = selectedStudent.FacultyID;
                    txtDiemTB.Text = selectedStudent.AverageScore.ToString();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Student newStudent = new Student()
                {
                    StudentID = txt_MSSV.Text,
                    FullName = txtHo_Ten.Text,
                    FacultyID = Convert.ToInt32(cboKhoa.SelectedValue),
                    AverageScore = float.Parse(txtDiemTB.Text)
                };

                context.Students.Add(newStudent);
                context.SaveChanges();

                MessageBox.Show("Thêm sinh viên thành công.");
                ClearForm();
                LoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã số " + selectedStudent.StudentID + " ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(selectedStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công.");
                        ClearForm();
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    selectedStudent.StudentID = txt_MSSV.Text;
                    selectedStudent.FullName = txtHo_Ten.Text;
                    selectedStudent.FacultyID = Convert.ToInt32(cboKhoa.SelectedValue);
                    selectedStudent.AverageScore = float.Parse(txtDiemTB.Text);

                    context.SaveChanges();

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công.");
                    ClearForm();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
