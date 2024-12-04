using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mayin_Tarlasi
{
    public partial class Form1 : Form
    {
        private int gridSize = 30; // Izgara boyutu (10x10)
        private int mineCount = 123; // Mayın sayısı
        private Button[,] buttons; // Buton matrisi
        private int[,] mines; // Mayınlar ve komşu sayıları
        private Panel panel1; // Oyun alanını tutacak panel
        public Form1()
        {
            InitializeComponent();
            CreateControls();
            StartGame();
        }
        private void CreateControls()
        {
            // Paneli oluştur
            panel1 = new Panel
            {
                Size = new Size(900, 900), // Izgara boyutu
                Location = new Point(10, 50), // Formda panelin konumu
                BorderStyle = BorderStyle.FixedSingle // Çerçeve ekle
            };
            Controls.Add(panel1);

            // Yeni oyun başlatma butonu
            Button restartButton = new Button
            {
                Text = "Restart",
                Size = new Size(100, 30),
                Location = new Point(10, 10)
            };
            restartButton.Click += (s, e) => StartGame();
            Controls.Add(restartButton);

            // Bilgilendirici label (isteğe bağlı)
            Label infoLabel = new Label
            {
                Text = "Minesweeper Game",
                Location = new Point(120, 15),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            Controls.Add(infoLabel);
        }

        private void StartGame()
        {
            panel1.Controls.Clear();
            buttons = new Button[gridSize, gridSize];
            mines = new int[gridSize, gridSize];

            // Mayınları rastgele yerleştir
            PlaceMines();

            // Izgarayı oluştur
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Button button = new Button
                    {
                        Size = new Size(30, 30),
                        Location = new Point(x * 20, y * 20),
                        Tag = new Point(x, y),
                        BackColor = Color.LightGray
                    };
                    button.Click += Button_Click;
                    panel1.Controls.Add(button);
                    buttons[x, y] = button;
                }
            }
        }

        private void PlaceMines()
        {

            Random random = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(gridSize);
                    y = random.Next(gridSize);
                }
                while (mines[x, y] == -1); // Eğer zaten mayın varsa tekrar konum seç
                mines[x, y] = -1;

                // Komşu hücreleri güncelle
                UpdateNeighbors(x, y);
            }
        }


        private void UpdateNeighbors(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize && mines[nx, ny] != -1)
                    {
                        mines[nx, ny]++;
                    }
                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Point location = (Point)clickedButton.Tag;
            int x = location.X;
            int y = location.Y;

            if (mines[x, y] == -1)
            {
                // Mayına tıklanırsa oyun biter
                clickedButton.BackColor = Color.Red;
                MessageBox.Show("Game Over!");
                StartGame();
            }
            else
            {
                // Hücreyi aç
                OpenCell(x, y);
            }
        }

        private void OpenCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= gridSize || y >= gridSize || buttons[x, y].Enabled == false)
            {
                return; // Sınırları kontrol et veya zaten açılmış hücreyi tekrar açma
            }

            buttons[x, y].Enabled = false;
            buttons[x, y].BackColor = Color.White;

            if (mines[x, y] > 0)
            {
                buttons[x, y].Text = mines[x, y].ToString();
                return;
            }

            // Boş hücre için komşuları aç
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    OpenCell(x + dx, y + dy);
                }
            }
        }

        
    }
}
