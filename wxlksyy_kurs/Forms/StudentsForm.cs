using System;
using System.Windows.Forms;
using wxlksyy_kurs.Data;
using wxlksyy_kurs.Data.Models;
using wxlksyy_kurs.Data.Repositories;

namespace wxlksyy_kurs.Forms
{
    /// <summary>
    /// Форма управления студентами
    /// </summary>
    public partial class StudentsForm : Form
    {
        private readonly StudentRepository _studentRepository;

        public StudentsForm()
        {
            InitializeComponent();
            _studentRepository = new StudentRepository(new DatabaseHelper());
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                dgvStudents.DataSource = _studentRepository.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки студентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var student = new Student
                {
                    FullName = txtFullName.Text,
                    GroupName = txtGroup.Text,
                    StudentNumber = txtNumber.Text
                };

                _studentRepository.Add(student);
                LoadStudents();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления студента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedStudent = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                var student = new Student
                {
                    Id = selectedStudent.Id,
                    FullName = txtFullName.Text,
                    GroupName = txtGroup.Text,
                    StudentNumber = txtNumber.Text
                };

                _studentRepository.Update(student);
                LoadStudents();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления студента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedStudent = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                _studentRepository.Delete(selectedStudent.Id);
                LoadStudents();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления студента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var student = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                txtFullName.Text = student.FullName;
                txtGroup.Text = student.GroupName;
                txtNumber.Text = student.StudentNumber;
            }
        }

        private void ClearInputs()
        {
            txtFullName.Text = "";
            txtGroup.Text = "";
            txtNumber.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {// TODO: данная строка кода позволяет загрузить данные в таблицу "wxlksyy_bdDataSet.Students". При необходимости она может быть перемещена или удалена.
            this.studentsTableAdapter.Fill(this.wxlksyy_bdDataSet.Students);
        }
    }
}

