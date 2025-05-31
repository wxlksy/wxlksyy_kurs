using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using wxlksyy_kurs.Data;
using wxlksyy_kurs.Data.Models;
using wxlksyy_kurs.Data.Repositories;

namespace wxlksyy_kurs.Forms
{
    public partial class StudentsForm : Form
    {
        private readonly StudentRepository _studentRepository;

        public StudentsForm()
        {
            InitializeComponent();
            _studentRepository = new StudentRepository(new DatabaseHelper());

            ConfigureDataGridView();
            LoadStudents();
        }

        private void ConfigureDataGridView()
        {
            // Базовые настройки
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.MultiSelect = false;
            dgvStudents.RowHeadersVisible = false;

            // Визуальные улучшения
            dgvStudents.BackgroundColor = SystemColors.Window;
            dgvStudents.BorderStyle = BorderStyle.FixedSingle;
            dgvStudents.GridColor = SystemColors.ControlLight;

            // Стиль заголовков столбцов
            dgvStudents.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Padding = new Padding(5),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };

            // Стиль строк
            dgvStudents.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(3),
                SelectionBackColor = Color.LightSteelBlue,
                SelectionForeColor = Color.Black
            };

            // Альтернативные строки
            dgvStudents.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.AliceBlue
            };

            // Очистка существующих столбцов
            dgvStudents.Columns.Clear();

            // Скрытый столбец ID
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Visible = false
            });

            // Столбец ФИО
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FullName",
                HeaderText = "ФИО студента",
                Width = 300,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }
            });

            // Столбец Группа
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "GroupName",
                HeaderText = "Группа",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }
            });

            // Столбец Номер билета
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "StudentNumber",
                HeaderText = "Номер студ. билета",
                Width = 180,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Consolas", 9, FontStyle.Regular)
                }
            });

            // Настройка высоты строк
            dgvStudents.RowTemplate.Height = 28;

            // Включение двойной буферизации для плавной прокрутки
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                dgvStudents,
                new object[] { true });
        }

        private void LoadStudents()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                dgvStudents.DataSource = _studentRepository.GetAll();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки студентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО студента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGroup.Text))
            {
                MessageBox.Show("Введите название группы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGroup.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("Введите номер студенческого билета", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumber.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var student = new Student
                {
                    FullName = txtFullName.Text.Trim(),
                    GroupName = txtGroup.Text.Trim(),
                    StudentNumber = txtNumber.Text.Trim()
                };

                int newStudentId = _studentRepository.Add(student);
                LoadStudents();
                ClearInputs();

                MessageBox.Show($"Студент успешно добавлен (ID: {newStudentId})", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Студент с таким номером билета уже существует", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (!ValidateInput()) return;

            try
            {
                var selectedStudent = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                var student = new Student
                {
                    Id = selectedStudent.Id,
                    FullName = txtFullName.Text.Trim(),
                    GroupName = txtGroup.Text.Trim(),
                    StudentNumber = txtNumber.Text.Trim()
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

            if (MessageBox.Show("Вы действительно хотите удалить выбранного студента?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
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
            txtFullName.Text = string.Empty;
            txtGroup.Text = string.Empty;
            txtNumber.Text = string.Empty;
            txtFullName.Focus();
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
        {
            // Инициализация формы
            txtFullName.Focus();
        }
    }
}