namespace Odzywianie_Alez_To_Proste
{
    /// <summary>
    /// Menu z opcjami wy³¹czenia i uruchomienia gry
    /// </summary>
    public partial class Menu : Form
    {
        /// <summary>
        /// Nowy w¹tek wykorzystywany do uruhomienia nowego okna
        /// </summary>
        Thread newWindow;
        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        public Menu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Startuje grê 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameStart(object sender, EventArgs e)  
        {
            this.Close();
            newWindow = new Thread(OpenNewWindow);
            newWindow.SetApartmentState(ApartmentState.STA);
            newWindow.Start();   
        }

        /// <summary>
        /// Otwiera nowy ekran gry
        /// </summary>
        /// <param name="obj"></param>
        private void OpenNewWindow(object obj)  
        {
            Application.Run(new GameScreen());
        }
        /// <summary>
        /// Zamkniêcie gry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void End_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}