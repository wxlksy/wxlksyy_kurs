using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using wxlksyy_kurs.Data;
using wxlksyy_kurs.Data.Models;
using wxlksyy_kurs.Data.Repositories;

namespace wxlksyy_kurs.Forms
{
    public partial class DebtsForm : Form
    {
        private readonly DebtRepository _debtRepository;
        private readonly StudentRepository _studentRepository;
        private readonly SubjectRepository _subjectRepository;

        public DebtsForm()
        {
            InitializeComponent();
            _debtRepository = new DebtRepository(new DatabaseHelper());
            _studentRepository = new StudentRepository(new DatabaseHelper());
            _subjectRepository = new SubjectRepository(new DatabaseHelper());

            ConfigureDataGridView();
            LoadStudents();
            LoadSubjects();
            LoadDebts();
        }

        private void ConfigureDataGridView()
        {
            // Базовые настройки
            dgvDebts.AutoGenerateColumns = false;
            dgvDebts.AllowUserToAddRows = false;
            dgvDebts.ReadOnly = true;
            dgvDebts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDebts.MultiSelect = false;
            dgvDebts.RowHeadersVisible = false;

            // Визуальные улучшения
            dgvDebts.BackgroundColor = SystemColors.Window;
            dgvDebts.BorderStyle = BorderStyle.FixedSingle;
            dgvDebts.GridColor = SystemColors.ControlLight;

            // Стиль заголовков столбцов
            dgvDebts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Padding = new Padding(5),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };

            // Стиль строк
            dgvDebts.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(3),
                SelectionBackColor = Color.LightSteelBlue,
                SelectionForeColor = Color.Black
            };

            // Альтернативные строки
            dgvDebts.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.AliceBlue
            };

            // Очистка существующих столбцов
            dgvDebts.Columns.Clear();

            // Скрытый столбец ID
            dgvDebts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Visible = false
            });

            // Столбец Студент
            dgvDebts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "StudentName",
                HeaderText = "Студент",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }
            });

            // Столбец Предмет
            dgvDebts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SubjectName",
                HeaderText = "Предмет",
                Width = 180,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }
            });

            // Столбец Описание
            dgvDebts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Описание задолженности",
                Width = 350,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }
            });

            // Настройка высоты строк
            dgvDebts.RowTemplate.Height = 28;

            // Включение двойной буферизации для плавной прокрутки
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                dgvDebts,
                new object[] { true });
        }

        private void LoadStudents()
        {
            try
            {
                cmbStudent.DataSource = _studentRepository.GetAll();
                cmbStudent.DisplayMember = "FullName";
                cmbStudent.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки студентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSubjects()
        {
            try
            {
                cmbSubject.DataSource = _subjectRepository.GetAll();
                cmbSubject.DisplayMember = "Name";
                cmbSubject.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDebts()
        {
            try
            {
                var debts = _debtRepository.GetAll();
                var students = _studentRepository.GetAll();
                var subjects = _subjectRepository.GetAll();

                // Создаем список для отображения с именами вместо ID
                var displayDebts = debts.Select(d => new
                {
                    d.Id,
                    StudentName = students.FirstOrDefault(s => s.Id == d.StudentId)?.FullName ?? "Неизвестно",
                    SubjectName = subjects.FirstOrDefault(s => s.Id == d.SubjectId)?.Name ?? "Неизвестно",
                    d.Description
                }).ToList();

                dgvDebts.DataSource = displayDebts;

                // Настраиваем ширину столбца с описанием
                if (dgvDebts.Columns["Description"] != null)
                {
                    dgvDebts.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDebts.Columns["Description"].MinimumWidth = 300;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задолженностей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var debt = new Debt
                {
                    StudentId = (int)cmbStudent.SelectedValue,
                    SubjectId = (int)cmbSubject.SelectedValue,
                    Description = txtDescription.Text
                };

                _debtRepository.Add(debt);
                LoadDebts();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления задолженности: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDebts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите задолженность для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedRow = dgvDebts.SelectedRows[0];
                int debtId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

                _debtRepository.Delete(debtId);
                LoadDebts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления задолженности: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDebts();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ClearInputs()
        {
            txtDescription.Text = "";
        }

        private void DebtsForm_Load(object sender, EventArgs e)
        { }
    }
}