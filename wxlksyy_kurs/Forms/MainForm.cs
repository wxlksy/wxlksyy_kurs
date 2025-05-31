using System;
using System.Windows.Forms;

namespace wxlksyy_kurs.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenForm<T>() where T : Form, new()
        {
            var form = new T();
            form.ShowDialog();
        }

        private void tsmiStudents_Click(object sender, EventArgs e)
        {
            OpenForm<StudentsForm>();
        }

        private void tsmiDebts_Click(object sender, EventArgs e)
        {
            OpenForm<DebtsForm>();
        }

        private void tsmiRetakes_Click(object sender, EventArgs e)
        {
            OpenForm<RetakesForm>();
        }

        private void tsmiSubjects_Click(object sender, EventArgs e)
        {
            OpenForm<SubjectsForm>();
        }

        private void tsmiTeachers_Click(object sender, EventArgs e)
        {
            OpenForm<TeachersForm>();
        }

        private void tsmiClassrooms_Click(object sender, EventArgs e)
        {
            OpenForm<ClassroomsForm>();
        }

        private void tsmiReports_Click(object sender, EventArgs e)
        {
            OpenForm<ReportsForm>();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            OpenForm<SettingsForm>();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tsslUser.Text = $"Пользователь: wxlksyoff";
            tsslStatus.Text = "Готово";
        }
    }
}