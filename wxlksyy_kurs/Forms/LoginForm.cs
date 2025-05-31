using System;
using System.Windows.Forms;
using wxlksyy_kurs.Services;

namespace wxlksyy_kurs.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_authService.Authenticate(txtUsername.Text, txtPassword.Text))
                {
                    // Скрываем текущую форму входа
                    this.Hide();

                    // Создаем и показываем главную форму
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += (s, args) => this.Close(); // Закрываем LoginForm при закрытии MainForm
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Неверные учетные данные", "Ошибка авторизации",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Полностью закрываем приложение
        }

        // Обработчик нажатия клавиш на форме
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); // Автоматический клик по кнопке входа при нажатии Enter
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick(); // Автоматический клик по кнопке отмены при нажатии Escape
            }
        }

        // Обработчик загрузки формы
        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtUsername; // Устанавливаем фокус на поле ввода логина
        }
    }
}