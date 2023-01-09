using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Odzywianie_Alez_To_Proste
{
    /// <summary>
    /// Główny ekran gry
    /// </summary>
    public partial class GameScreen : Form
    {
        /// <summary>
        /// Nowy wątek wykorzystywany do uruhomienia nowego okna
        /// </summary>
        Thread newMenuWindow;
        /// <summary>
        /// Tablica do przechowywania jedzenia
        /// </summary>
        PictureBox[] Food = new PictureBox[12];
        /// <summary>
        /// Picture box dla efektu kopiowania jedzenia
        /// </summary>
        private PictureBox dragEffectPicBox = new PictureBox();
        /// <summary>
        /// Kopia oryginalnego jedzenia
        /// </summary>
        private PictureBox foodBox = new PictureBox();
        /// <summary>
        /// Picture box do określenia lokalizacji
        /// </summary>
        private PictureBox boxForLocation = new PictureBox();
        /// <summary>
        /// Lista produktów klasy Foods
        /// </summary>
        List<Foods> foodType = new List<Foods>();
        /// <summary>
        /// Lista obrazków z avatarami
        /// </summary>
        List<Image> avatarImage = new List<Image>();
        /// <summary>
        /// Lista pasków ładwoania
        /// </summary>
        List<ProgressBar> foodBar = new List<ProgressBar>();
        /// <summary>
        /// Generowanie losowych liczb
        /// </summary>
        Random randomNumber = new Random();
        /// <summary>
        /// Przechowywanie chwilowej lokalizacji jedzenia przy przesuwie w lewo
        /// </summary>
        Point[] leftPoints = new Point[12];
        /// <summary>
        /// Przechowywanie chwilowej lokalizacji jedzenia przy przesuwie w prawo
        /// </summary>
        Point[] rightPoints = new Point[12];
        /// <summary>
        /// Licznik w lewo
        /// </summary>
        int counterLeft = 0;
        /// <summary>
        /// Licznik w prawo
        /// </summary>
        int counterRight = 0;
        /// <summary>
        /// Tablica ze zmiennymi służącymi do chowania pasków przy dużej ilości punktów
        /// </summary>
        int[] barHide = { 0, 1, 2, 3, 4, 5 };
        /// <summary>
        /// Punkty
        /// </summary>
        int score = 0;

        /// <summary>
        /// Ładowanie głównego ekranu gry oraz inicjalizacja komponentów w nim zawartych z użyciem list
        /// </summary>
        public GameScreen()
        {
            InitializeComponent();
            DoubleBuffered = true;

            proteinsFoodBar.Hide(); //Ukrycie pasków
            fatFoodBar.Hide();
            carbonsFoodBar.Hide();
            potassiumFoodBar.Hide();
            sodiumFoodBar.Hide();

            foodProteinsText.Hide();    //Ukrycie napisów jedzenia
            foodFatText.Hide();
            foodCarbonsText.Hide();
            foodPotassiumText.Hide();
            foodSodiumText.Hide();

            foodType.Add(new Foods() { name = "Bread", proteins = 9, fat = 3, carbons = 78, potassium = 50, sodium = 80});
            foodType.Add(new Foods() { name = "Cheese", proteins = 25, fat = 33, carbons = 1, potassium = 98, sodium = 621 });
            foodType.Add(new Foods() { name = "Hamburger", proteins = 17, fat = 14, carbons = 24, potassium = 226, sodium = 414 });
            foodType.Add(new Foods() { name = "Eggs", proteins = 13, fat = 11, carbons = 1, potassium = 126, sodium = 124 });
            foodType.Add(new Foods() { name = "Tomato", proteins = 1, fat = 0, carbons = 4, potassium = 237, sodium = 5 });
            foodType.Add(new Foods() { name = "Steak", proteins = 25, fat = 19, carbons = 0, potassium = 279, sodium = 58 });
            foodType.Add(new Foods() { name = "Fish", proteins = 22, fat = 3, carbons = 0, potassium = 384, sodium = 61 });
            foodType.Add(new Foods() { name = "Carrot", proteins = 0, fat = 0, carbons = 1, potassium = 320, sodium = 61 });
            foodType.Add(new Foods() { name = "IceCream", proteins = 3, fat = 11, carbons = 24, potassium = 200, sodium = 80 });
            foodType.Add(new Foods() { name = "Ham", proteins = 21, fat = 6, carbons = 1, potassium = 287, sodium = 1203 });
            foodType.Add(new Foods() { name = "Milk", proteins = 3, fat = 1, carbons = 5, potassium = 150, sodium = 44 });
            foodType.Add(new Foods() { name = "Sausage", proteins = 14, fat = 31, carbons = 0, potassium = 150, sodium = 200 });
          
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar1);
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar2);
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar3);
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar4);
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar5);
            avatarImage.Add(Odzywianie_Alez_To_Proste.Properties.Resources.Avatar6);

            foodBar.Add(proteinsFoodBar);
            foodBar.Add(fatFoodBar);
            foodBar.Add(carbonsFoodBar);
            foodBar.Add(potassiumFoodBar);
            foodBar.Add(sodiumFoodBar);

            Points.Text = "Punkty: " + score;
        }

        /// <summary>
        /// Pozwala na upuszczanie jedzenia i inicjalizuje tablice pictureBoxów z jedzeniem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameScreen_Load(object sender, EventArgs e)    
        {
            this.foodBox.MouseDown += new MouseEventHandler(PictureBox_MouseDown);

            this.Avatar.DragEnter += new DragEventHandler(Avatar_DragEnter);
            this.panel3.DragEnter += new DragEventHandler(panel3_DragEnter);

            this.Avatar.DragOver += new DragEventHandler(Avatar_DragOver);
            this.panel3.DragOver += new DragEventHandler(panel3_DragOver);

            Avatar.AllowDrop = true;
            panel3.AllowDrop = true;

            this.dragEffectPicBox.Visible = true;
            this.panel3.Controls.Add(this.dragEffectPicBox);

            Lottery();

            Food[0] = Bread;
            Food[1] = Cheese;
            Food[2] = Hamburger;
            Food[3] = Eggs;
            Food[4] = Tomato;
            Food[5] = Steak;
            Food[6] = Fish;
            Food[7] = Carrot;
            Food[8] = IceCream;
            Food[9] = Ham;
            Food[10] = Milk;
            Food[11] = Sausage;

            Food[6].Hide();
            Food[7].Hide();
            Food[8].Hide();
            Food[9].Hide();
            Food[10].Hide(); 
            Food[11].Hide();

        }
        /// <summary>
        /// Losowanie pacjenta i pasków
        /// </summary>
        public void Lottery()   
        {
            Avatar.Image = avatarImage[randomNumber.Next(0, 5)];    //Losowanie pacjenta

            proteinsAvatarBar.Maximum = randomNumber.Next(75, 102);
            proteinsAvatarBar.Value = randomNumber.Next(0, 50);   //Losowanie niedoborów
           
            fatAvatarBar.Maximum = randomNumber.Next(50, 76);
            fatAvatarBar.Value = randomNumber.Next(0, 35);
          
            carbonsAvatarBar.Maximum = randomNumber.Next(325, 409);
            carbonsAvatarBar.Value = randomNumber.Next(0, 200);

            potassiumAvatarBar.Maximum = randomNumber.Next(2000, 3000);
            potassiumAvatarBar.Value = randomNumber.Next(0, 1500);

            sodiumAvatarBar.Maximum = randomNumber.Next(1200, 1500);
            sodiumAvatarBar.Value = randomNumber.Next(0, 1000);
           
        }
        /// <summary>
        /// Zachowanie gry pod względem ilości punktów. Utrudnienie dla gracza
        /// </summary>
        public void Score()
        {
            if(score < 1000 && score>300)
            {
                for(int i=0; i<5; i++)
                {
                    if(i == barHide[1])
                    {
                        foodBar[i].Hide();
                    }
                    else
                    {
                        foodBar[i].Show();
                    }
                }
            }
            else if(score < 1000 && score > 300)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == barHide[0] || i == barHide[3])
                    {
                        foodBar[i].Hide();
                    }
                    else
                    {
                        foodBar[i].Show();
                    }
                }
            }
            else if(score > 1000 && score < 1500)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == barHide[0] || i == barHide[3] || i == barHide[4])
                    {
                        foodBar[i].Hide();
                    }
                    else
                    {
                        foodBar[i].Show();
                    }
                }
            }
            else if(score >1500)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == barHide[0] || i == barHide[3] || i == barHide[4] || i == barHide[1]) 
                    {
                        foodBar[i].Hide();
                    }
                    else
                    {
                        foodBar[i].Show();
                    }
                }
            }
        }
        /// <summary>
        /// Kliknięcie na picture boxa z jedzeniem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)   
        {
            foodBox = ((PictureBox)sender);
            ((PictureBox)sender).DoDragDrop(((PictureBox)sender).Image, DragDropEffects.Copy);
            
        }
        /// <summary>
        /// Ustalenie nowych lokacji pasków oraz napisów gdy najedziemy na picture boxa z jedzeniem oraz wczytanie wartości odżywczych produktu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            boxForLocation = ((PictureBox)sender);
            proteinsFoodBar.Location = new Point(boxForLocation.Location.X, boxForLocation.Location.Y - 101);   
            fatFoodBar.Location = new Point(boxForLocation.Location.X, boxForLocation.Location.Y - 81);
            carbonsFoodBar.Location = new Point(boxForLocation.Location.X, boxForLocation.Location.Y - 61);
            potassiumFoodBar.Location = new Point(boxForLocation.Location.X, boxForLocation.Location.Y - 41);
            sodiumFoodBar.Location = new Point(boxForLocation.Location.X, boxForLocation.Location.Y - 21);

            foodProteinsText.Location = new Point(boxForLocation.Location.X- 67, boxForLocation.Location.Y - 106);  
            foodFatText.Location = new Point(boxForLocation.Location.X - 82, boxForLocation.Location.Y - 86);
            foodCarbonsText.Location = new Point(boxForLocation.Location.X - 110, boxForLocation.Location.Y - 66);
            foodPotassiumText.Location = new Point(boxForLocation.Location.X - 71, boxForLocation.Location.Y - 46);
            foodSodiumText.Location = new Point(boxForLocation.Location.X - 67, boxForLocation.Location.Y - 26);
          
            if(score>300)
            {
                Score();
            }
            else
            {
                proteinsFoodBar.Show(); //Wyświetlenie pasków
                fatFoodBar.Show();
                carbonsFoodBar.Show();
                potassiumFoodBar.Show();
                sodiumFoodBar.Show();
            }
                    
            foodProteinsText.Show(); //Wyświetlenie napisów
            foodFatText.Show();
            foodCarbonsText.Show();
            foodPotassiumText.Show();
            foodSodiumText.Show();

            for(int i = 0; i<12; i++)
            {
                if (boxForLocation.Name == foodType[i].name) //Wartości składników odżywczych produktów
                {
                    proteinsFoodBar.Value = foodType[i].proteins;
                    fatFoodBar.Value = foodType[i].fat;
                    carbonsFoodBar.Value = foodType[i].carbons;
                    potassiumFoodBar.Value = foodType[i].potassium;
                    sodiumFoodBar.Value = foodType[i].sodium;
                }
            }
            
        }

        /// <summary>
        /// Po przesunięciu myszki po za jedzenie znikają napisy i paski
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseLeave(object sender, EventArgs e)  
        {
            boxForLocation = ((PictureBox)sender);

            proteinsFoodBar.Hide();
            fatFoodBar.Hide();
            carbonsFoodBar.Hide();
            potassiumFoodBar.Hide();
            sodiumFoodBar.Hide();

            foodProteinsText.Hide();
            foodFatText.Hide();
            foodCarbonsText.Hide();
            foodPotassiumText.Hide();
            foodSodiumText.Hide();
        }
        /// <summary>
        /// Kiedy najedziemy na panel chowa się oryginalny picture box jedzenia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel3_DragEnter(object sender, DragEventArgs e) 
        {
            {
                e.Effect = DragDropEffects.Copy;
            }
            foodBox.Hide();
        }
        /// <summary>
        /// Kiedy przesuwamy jedzenie nad panelem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel3_DragOver(object sender, DragEventArgs e)    
        {
            this.dragEffectPicBox.Location = this.panel3.PointToClient(new Point(e.X - dragEffectPicBox.Width / 2, e.Y - dragEffectPicBox.Height / 2));
            Image img = e.Data.GetData(e.Data.GetFormats()[0]) as Image;
            this.dragEffectPicBox.Image = img;
            this.dragEffectPicBox.SizeMode = PictureBoxSizeMode.AutoSize;
            this.dragEffectPicBox.Visible = true;
        }

        /// <summary>
        /// Zabezpieczenie przed pozostawieniem jedzenia na panelu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel3_DragDrop(object sender, DragEventArgs e) 
        {
            this.dragEffectPicBox.Visible = false;
            foodBox.Show();
        }
        /// <summary>
        /// Kiedy najedziemy na avatara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avatar_DragEnter(object sender, DragEventArgs e)   
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        /// <summary>
        /// Kiedy ruszami jedzeniem nad avatarem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avatar_DragOver(object sender, DragEventArgs e)
        {
            this.dragEffectPicBox.SizeMode = PictureBoxSizeMode.AutoSize;

            this.dragEffectPicBox.Visible = false;

        }
        /// <summary>
        /// Kiedy upuścimy jedzenie na avatarze. Dodanie punktów, zapełnienie pasków oraz powrót widoku oryginalnego picture boxa z jedzeniem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avatar_DragDrop(object sender, DragEventArgs e)    
        {
            this.dragEffectPicBox.Visible = false;
            foodBox.Show();
                
            

            for (int i = 0; i < 12; i++)
            {
                if (foodBox.Name == foodType[i].name) //Wartości składników odżywczych produktów
                {
                    if (proteinsAvatarBar.Value + foodType[i].proteins > proteinsAvatarBar.Maximum) //Warunki białek
                    {
                        proteinsAvatarBar.Value = proteinsAvatarBar.Maximum;
                        score = score - ((proteinsAvatarBar.Value + foodType[i].proteins) - proteinsAvatarBar.Maximum)/6;
                    }
                    else
                    {
                        proteinsAvatarBar.Value = proteinsAvatarBar.Value + foodType[i].proteins;
                        score = score + 15;
                    }

                    if (fatAvatarBar.Value + foodType[i].fat > fatAvatarBar.Maximum) //Warunki tłuszczy
                    {
                        fatAvatarBar.Value = fatAvatarBar.Maximum;
                        score = score - ((fatAvatarBar.Value + foodType[i].fat) - fatAvatarBar.Maximum)/6;
                    }
                    else
                    {
                        fatAvatarBar.Value = fatAvatarBar.Value + foodType[i].fat;
                        score = score + 15;
                    }

                    if (carbonsAvatarBar.Value + foodType[i].carbons > carbonsAvatarBar.Maximum)    //Warunki węglowodanów
                    {
                        carbonsAvatarBar.Value = carbonsAvatarBar.Maximum;
                        score = score - ((carbonsAvatarBar.Value + foodType[i].carbons) - carbonsAvatarBar.Maximum)/6;
                    }
                    else
                    {
                        carbonsAvatarBar.Value = carbonsAvatarBar.Value + foodType[i].carbons;
                        score = score + 15;
                    }

                    if (potassiumAvatarBar.Value + foodType[i].potassium > potassiumAvatarBar.Maximum)    //Warunki potasu
                    {
                        potassiumAvatarBar.Value = potassiumAvatarBar.Maximum;
                        score = score - ((potassiumAvatarBar.Value + foodType[i].potassium) - potassiumAvatarBar.Maximum)/6;
                    }
                    else
                    {
                        potassiumAvatarBar.Value = potassiumAvatarBar.Value + foodType[i].potassium;
                        score = score + 15;
                    }

                    if (sodiumAvatarBar.Value + foodType[i].sodium > sodiumAvatarBar.Maximum)    //Warunki potasu
                    {
                        sodiumAvatarBar.Value = sodiumAvatarBar.Maximum;
                        score = score - ((sodiumAvatarBar.Value + foodType[i].sodium) - sodiumAvatarBar.Maximum)/6;
                    }
                    else
                    {
                        sodiumAvatarBar.Value = sodiumAvatarBar.Value + foodType[i].sodium;
                        score = score + 15;
                    }
                    
                    if(score<0)
                    {
                        score = 0;
                    }

                    if(proteinsAvatarBar.Value == proteinsAvatarBar.Maximum && fatAvatarBar.Value == fatAvatarBar.Maximum && carbonsAvatarBar.Value == carbonsAvatarBar.Maximum && potassiumAvatarBar.Value == potassiumAvatarBar.Maximum && sodiumAvatarBar.Value == sodiumAvatarBar.Maximum)
                    {
                        Lottery();
                        score = score + 500;
                    }
                    
                    Points.Text = "Punkty: " + score;
                }
            }
        }

        /// <summary>
        /// Przycisk przejścia znowu do menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndGame_Click(object sender, EventArgs e)      
        {
            this.Close();
            newMenuWindow = new Thread(OpenMenuAgain);
            newMenuWindow.SetApartmentState(ApartmentState.STA);
            newMenuWindow.Start();

        }
        /// <summary>
        /// Przycisk startu gry od nowa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAgain_Click(object sender, EventArgs e)      
        {
            this.Close();
            newMenuWindow = new Thread(OpenGameScreenAgain);
            newMenuWindow.SetApartmentState(ApartmentState.STA);
            newMenuWindow.Start();
        }
        /// <summary>
        /// // Ładuje ekran gry
        /// </summary>
        /// <param name="obj"></param>
        private void OpenGameScreenAgain(object obj)    
        {
            Application.Run(new GameScreen());
        }
        /// <summary>
        /// Ładuje ekran menu
        /// </summary>
        /// <param name="obj"></param>
        private void OpenMenuAgain(object obj)  
        {
            Application.Run(new Menu());
        }
        /// <summary>
        /// // Przesunięcie jedzenia w lewo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftArrow_Click(object sender, EventArgs e) 
        {

            
            for (int i = 0; i<12; i++)          //Określa położenie jedzenia
            {
                leftPoints[i] = Food[i].Location;
            }

            Point last = Food[11].Location;
            counterLeft = 10;
            for(; counterLeft>=0; counterLeft--)        //Zamienia położenie jedzenia
            {
                Food[counterLeft+1].Location = Food[counterLeft].Location;
            }
            Food[0].Location = last;

            for (int i = 0; i < 12; i++)    //Wyświetla tylko jedzenie na taśmie
            {
                if (Food[i].Location.Y == 483 && foodType[i].name == Food[i].Name)
                {
                    Food[i].Show();
                }
                else
                {
                    Food[i].Hide();
                }
            }

        }
        /// <summary>
        /// Przesunięcie jedzenia w prawo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightArrow_Click(object sender, EventArgs e)   
        {
            for (int i = 0; i < 12; i++)
            {
                rightPoints[i] = Food[i].Location;
            }
           
            Point first = Food[0].Location;
            counterRight = 1;
            for (; counterRight < 12; counterRight++)
            {
                Food[counterRight - 1].Location = Food[counterRight].Location;
            }
            Food[counterRight - 1].Location = first;

            for (int i = 0; i < 12; i++)
            {
                if (Food[i].Location.Y == 483 && foodType[i].name == Food[i].Name)
                {
                    Food[i].Show();
                }
                else
                {
                    Food[i].Hide();
                }
            }

        }

      
    }
}
