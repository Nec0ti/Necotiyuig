using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Necoti
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> userCredentials = new Dictionary<string, string>();
        private Dictionary<string, List<Game>> userGameCredentials = new Dictionary<string, List<Game>>(); 
        private string dosyaYolu = "kullaniciBilgileri.txt";
        private List<Game> games = new List<Game>();
        private User currentUser = new User();


        class User
        {
            public string Name { get; set; }
            public List<Game> Library { get; set; }

            public User()
            {
                Name = "";
                Library = new List<Game>();
            }
        }



        public Form1()
        {
            InitializeComponent();

            LoadGames();
            CreateGamePanels();
            LoadUserCredentials();
        }

        private void LoadUserCredentials()
        {
            if (File.Exists(dosyaYolu))
            {
                string[] satirlar = File.ReadAllLines(dosyaYolu);
                foreach (string satir in satirlar)
                {
                    string[] bilgiler = satir.Split(',');
                    if (bilgiler.Length == 2)
                    {
                        string kullaniciAdi = bilgiler[0];
                        string sifre = bilgiler[1];
                        userCredentials.Add(kullaniciAdi, sifre);
                        userGameCredentials.Add(kullaniciAdi, new List<Game>());
                    }
                }
            }
        }




        private void LoadGames()
        {
            games.Add(new Game("Project Doc", "game1", 0.00));
            games.Add(new Game("Project Doc 2", "game2", 0.00));
            games.Add(new Game("Project Doc 3", "game2", 0.00));
            games.Add(new Game("Project Doc 4", "game1", 0.00));
            games.Add(new Game("Project Doc 5", "game2", 0.00));
            games.Add(new Game("Project Doc 6", "game1", 0.00));
            games.Add(new Game("Project Doc 7", "game2", 0.00));
            games.Add(new Game("Project Doc 8", "game2", 0.00));
            games.Add(new Game("Project Doc 9", "game1", 0.00));
            games.Add(new Game("Project Doc 10", "game2", 0.00));
            games.Add(new Game("Project Doc 11", "game2", 0.00));
            games.Add(new Game("Project Doc 12", "game2", 0.00));
            games.Add(new Game("Project Doc 13", "game1", 0.00));
            games.Add(new Game("Project Doc 14", "game2", 0.00));
            games.Add(new Game("Project Doc 15", "game1", 0.00));
            games.Add(new Game("Project Doc 16", "game2", 0.00));
            games.Add(new Game("Project Doc 17", "game2", 0.00));
            games.Add(new Game("Project Doc 18", "game1", 0.00));
            games.Add(new Game("Project Doc 19", "game2", 0.00));
            games.Add(new Game("Depth", "game 3", 05.00));
        }

        private void CreateGamePanels()
        {
            int panelWidth = 100;
            int panelHeight = 150;
            int maxColumns = 5; // Her satırda en fazla 5 oyun

            int top = 10;
            int left = 10;

            foreach (var game in games)
            {
                Panel panel = new Panel();
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Width = panelWidth;
                panel.Height = panelHeight;
                panel.Top = top;
                panel.Left = left;
                panel.BackColor = Color.LightBlue;

                Label nameLabel = new Label();
                nameLabel.Text = game.Name;
                nameLabel.Dock = DockStyle.Top;

                Label descriptionLabel = new Label();
                descriptionLabel.Text = game.Description;
                descriptionLabel.Dock = DockStyle.Fill;

                Label priceLabel = new Label();
                priceLabel.Text = "Price: $" + game.Price.ToString("0.00");
                priceLabel.Dock = DockStyle.Bottom;

                Button buyButton = new Button();
                buyButton.Text = "Buy Game";
                buyButton.Dock = DockStyle.Bottom;

                buyButton.Click += (sender, e) =>
                {
                    GamePanel_BuyButtonClick(game);
                };

                panel.Controls.Add(nameLabel);
                panel.Controls.Add(descriptionLabel);
                panel.Controls.Add(priceLabel);
                panel.Controls.Add(buyButton);

                panel1.Controls.Add(panel);

                left += panelWidth + 10; // Panel arasında 10 piksel boşluk bırak

                if (left + panelWidth > panel1.Width) // Eğer panel sığmıyorsa alt satıra geç
                {
                    left = 10;
                    top += panelHeight + 10; // Yeni satıra geçtiğimizde 10 piksel boşluk bırak
                }
            }
        }

        private void GamePanel_BuyButtonClick(Game game)
        {
            if (game.IsOwned)
            {
                MessageBox.Show("You already own this game.");
            }
            else
            {
                if (game.Price == 0.00)
                {
                    AddToLibrary(game);
                }
                else
                {
                    ShowPurchaseScreen(game);
                }
            }
        }


        private void AddToLibrary(Game game)
        {
            List<Game> library = currentUser.Library;

            bool alreadyOwned = library.Any(ownedGame => ownedGame.Name == game.Name);

            if (!alreadyOwned)
            {
                library.Add(game);
                currentUser.Library = library;

                List<string> ownedGames = userGameCredentials[currentUser.Name];
                ownedGames.Add(game.Name);
                userGameCredentials[currentUser.Name] = ownedGames;
            }
            else
            {
                MessageBox.Show("This game already in your library.");
            }

            Panel libraryPanel = panel2;

            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Width = 100;
            panel.Height = 150;

            Label nameLabel = new Label();
            nameLabel.Text = game.Name;
            nameLabel.Dock = DockStyle.Top;

            Label descriptionLabel = new Label();
            descriptionLabel.Text = game.Description;
            descriptionLabel.Dock = DockStyle.Fill;

            Label priceLabel = new Label();
            priceLabel.Text = "Price: $" + game.Price.ToString("0.00");
            priceLabel.Dock = DockStyle.Bottom;

            Button buyButton = new Button();
            buyButton.Text = "Play Game";
            buyButton.Dock = DockStyle.Bottom;

            buyButton.Click += (sender, e) =>
            {
                GamePanel_PlayButtonClick(game);
            };

            panel.Controls.Add(nameLabel);
            panel.Controls.Add(descriptionLabel);
            panel.Controls.Add(priceLabel);
            panel.Controls.Add(buyButton);

            libraryPanel.Controls.Add(panel);

            game.IsOwned = true;

            int maxColumns = 5;
            int spacing = 10;
            int column = libraryPanel.Controls.Count % maxColumns;
            int row = libraryPanel.Controls.Count / maxColumns;
            int newLeft = column * (panel.Width + spacing);
            int newTop = row * (panel.Height + spacing);
            panel.Left = newLeft;
            panel.Top = newTop;
        }

        private void GamePanel_PlayButtonClick(Game game)
        {
            if (game.Price > 0.00)
            {
                MessageBox.Show("You need to purchase this game before playing.");
            }
            else
            {

            }
        }


        private void ShowPurchaseScreen(Game game)
        {

        }

        class Game
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public bool IsOwned { get; set; }

            public Game(string name, string description, double price)
            {
                Name = name;
                Description = description;
                Price = price;
                IsOwned = false;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = isim.Text;
            string sifre = this.sifre.Text;

            if (userCredentials.ContainsKey(kullaniciAdi) && userCredentials[kullaniciAdi] == sifre)
            {
                MessageBox.Show("Login Success!");
                button1.Visible = false;
                isim.Visible = false;
                this.sifre.Visible = false;


                button3.Visible = false;
                yenisifre.Visible = false;
                yeniisim.Visible = false;

                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;

                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;

                button2.Visible = true;
            }
            else
            {
                MessageBox.Show("Check your info again.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Log Out Success!");
            button2.Visible = false;

            button1.Visible = true;
            isim.Visible = true;
            sifre.Visible = true;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;

            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;

            button3.Visible = true;
            yenisifre.Visible = true;
            yeniisim.Visible = true;

            DisableAllPanels();
            Kaydet();
        }

        private void Kaydet()
        {
            using (StreamWriter writer = new StreamWriter(dosyaYolu))
            {
                foreach (var kvp in userCredentials)
                {
                    writer.WriteLine($"{kvp.Key},{kvp.Value}");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = yeniisim.Text;
            string sifre = yenisifre.Text;

            if (userCredentials.ContainsKey(kullaniciAdi))
            {
                MessageBox.Show("This name already exist.");
            }
            else
            {
                userCredentials.Add(kullaniciAdi, sifre);
                MessageBox.Show("New account created. Don't forgot your password.");


            }
        }

        private void DisableAllPanels()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
                panel1.Visible = false;
            else
                panel1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
                panel2.Visible = false;
            else
                panel2.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
            else
                panel3.Visible = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}

