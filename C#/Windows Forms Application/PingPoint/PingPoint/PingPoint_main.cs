using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;
//using System.Threading;

public struct point
{
    public int player;
    public int value;
    public point(int p1, int p2)
    {
        player = p1;
        value = p2;
    }
};

namespace PingPoint
{
    public partial class PingPoint_main : Form
    {
        // Nowy wątek do odbierania i wysyłania sygnałów aplikacji C++.
        public static int recivedPointSide = -1; //strona po której zdobyto punkt (0 - lewo, 1 - prawo)
        // Jest to alternatywa dla nowego wątku
        Timer timer = new Timer();

        // Connection strings - przez PuTTY oraz z wydziału. 
        protected static string ConnString = "SERVER=xxxxx; Port=9999; DATABASE=xxxxx; UID=xxxxx; PASSWORD=xxxxx;"; // PuTTY.
        //protected static string ConnString = "SERVER=xxxxx; Port=9999; DATABASE=xxxxx; UID=xxxxx; PASSWORD=xxxxx;"; // Wydział.
        protected MySqlConnection conn = new MySqlConnection(ConnString);

        // Lista punktów zawodników.
        List<point> points = new List<point>();
        point p;
        
        // Reszta zmiennych. 
        bool logged1 = false; // Zawodnik 1 się zalogował.
        bool logged2 = false; // Zawodnik 2 się zalogował.
        public static bool choosed = false; // Tryb gry został wybrany.
        public static decimal set_max = 7; // Ilość setów w rozgrywce.
        public static decimal point_max = 12; // Ilość punktów w secie.
        decimal point_margin = 0; // Różnica punktów potrzebna do wygrania seta. 
        public static decimal tournament_id; // Id turnieju.
        public static decimal match_id; // Id meczu.
        public static string tournament_type; // Przechowuje tryb turnieju.
        int points_id = 0; // Id punktu.
        int point1; // Ilość punktów zawodnika1.
        int point2; // Ilość punktów zawodnika2.
        int sets_player1 = 0; // Ilość wygranych setów zawodnika1. 
        int sets_player2 = 0; // Ilość wygranych setów zawodnika2.
        public static bool rematch = false; // Zmienna określająca czy jest możliwość rewanżu.
        public static bool accept_set = false; // Zmienna określająca czy set został zaakceptowany i ma zostać przesłany do bazy.
        public static string my_login; // Zmienna przechowująca login z bazy danych.
        public static string my_password; // Zmienna przechowująca hasło z bazy danych.
        bool serve_player1 = true; // Zmienna przechowująca kto serwuje.
        int serve_player_start;
        public static decimal serve_number = 2;
        int serve_points_old = 0; // Zmienna przechowująca poprzednią ilość sumy punktów zawodników w danym secie. (zmiana serwów)
        NamedPipeServer PServer1 = new NamedPipeServer(@"\\.\pipe\myNamedPipe1", 0); // Odbieranie sygnałów.
        //NamedPipeServer PServer2 = new NamedPipeServer(@"\\.\pipe\myNamedPipe2", 1); // Wysyłanie sygnałów.
        bool free = true; // Zmienna która ogranicza wywołania funkcji przez timer

        public PingPoint_main() // Funkcja inicjalizująca.
        {
            InitializeComponent();
        }

        void startConnectCpp()
        {
            PServer1.Start();
            //PServer2.Start();

            timer.Interval = 100; // Czy tyle wystarczy?
            timer.Tick += new EventHandler(connectCpp);
        } // Otworzenie połączenia z C++

        void stopConnectCpp()
        {
            PServer1.StopServer();
            //PServer2.StopServer();
        } // Zamknięcie połączenia z C++
        
        private void connectCpp(object sender, EventArgs e)
        {
            if (free == true && recivedPointSide != -1)
            {
                free = false;
                int set = sets_player1 + sets_player2;
                //string Ms = null;
                if (recivedPointSide == 0)
                {
                    if (set % 2 == 0) //Gracz jest po stronie startowej
                    {
                        //punkt dla gracza 1
                        label_points_up1_Click(null, null);
                    }
                    else
                    {
                        //punkt dla gracza 2
                        label_points_up2_Click(null, null);
                    }

                }
                else if (recivedPointSide == 1)
                {
                    if (set % 2 == 0) //Gracz jest po stronie startowej
                    {
                        //punkt dla gracza 2
                        label_points_up2_Click(null, null);
                    }
                    else
                    {
                        //punkt dla gracza 1
                        label_points_up1_Click(null, null);
                    }
                }
                /*
                if (Ms != null)
                {
                    PServer2.SendMessage(Ms, PServer2.clientse);
                }
                */
                free = true;
                recivedPointSide = -1;
            }
        } // Zbieranie i wysyłanie sygnałów do C++

        private bool OpenConnection() // Funkcja otwierająca połączenie z bazą danych, oraz z c++.
        {
            try
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                this.label_baza.Text = "ONLINE";
                this.label_baza.ForeColor = Color.FromArgb(2, 100, 125);
                return true;
            }
            catch (MySqlException ex) //// Wypisuje najbardziej znane przyczyny wystąpienia błędu połączenia.
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    case 1042:
                        MessageBox.Show("Can't resolve the client IP address into a hostname");
                        break;
                    default:
                        MessageBox.Show("Unknown error");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection() // Funkcja zamykająca połączenie z bazą danych, oraz z c++.
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void PingPoint_main_Load(object sender, EventArgs e) // Funkcja obsługująca event załadowania PingPoint_main.
        {
            OpenConnection();
            startConnectCpp();
        }

        private void button_exit_Click(object sender, EventArgs e) // Funkcja obsługująca event kliknięcia na button_exit.
        {
            CloseConnection();
            stopConnectCpp();
            Application.Exit();
        }

        private int last_point(int player) // Funkcja wyszukuje ostatni punkt zawodnika o id 'player' w liście 'points'.
        {
            for (int i = 1; i < points.Count + 1; i++)
            {
                point k;
                k = points[points.Count - i];
                if (k.player == player)
                {
                    return k.value;
                }
            }
            return 0;
        }

        public void points_update() // Każdym razem gdy powinien się zmienić punkt wywołaj tą funkcję.
        {
            //initialize score labels
            point1 = last_point(1);
            point2 = last_point(2);
            label_points1.Text = point1.ToString();
            label_points2.Text = point2.ToString();

            // revert
            if (point2 > 0 || point1 > 0)
            {
                label_points_revert.Visible = true;
            }
            else
            {
                label_points_revert.Visible = false;
            }
            //up
            if (point1 < point_max + point_margin)
            {
                label_points_up1.Visible = true;
                label_points_up1.Text = (point1 + 1).ToString();
            }
            else
            {
                label_points_up1.Visible = false;
            }
            if (point2 < point_max + point_margin)
            {
                label_points_up2.Visible = true;
                label_points_up2.Text = (point2 + 1).ToString();
            }
            else
            {
                label_points_up2.Visible = false;
            }
            // Zmiana serwowania piłeczki - co serve_naumer punktów.
            if ((point1 + point2) % serve_number == 0 && (point1 + point2) > 0 && serve_points_old <= (point1 + point2) || serve_points_old % serve_number == 0 && serve_points_old > (point1 + point2) && (point1 + point2) > 0)
            {
                if (serve_player1 == true) // Serwował gracz 1
                {
                    serve_player1 = false; // Jak prawda to serwuje gracz 2
                    pictureBox_serve2.Visible = true;
                    pictureBox_serve1.Visible = false;
                }
                else
                {
                    serve_player1 = true; // Jak fałsz to serwuje gracz 1
                    pictureBox_serve1.Visible = true;
                    pictureBox_serve2.Visible = false;
                }
            }
            serve_points_old = point1 + point2;
        }

        public void sets_update(int win) // Każdym razem gdy powinien się zmienić SET wywołaj tą funkcję.
        {
            int set = sets_player1 + sets_player2;
            //// Zmiana zaczynającego przy serwach - zmiana stron więc gracz który przegrał będzie zaczynał
            if(serve_player_start == 0) // Gdy zaczynał gracz 1
            {
                if (set % 2 == 1)
                {
                    serve_player1 = false; // Jak prawda to serwuje gracz 2
                    pictureBox_serve2.Visible = true;
                    pictureBox_serve1.Visible = false;
                }
                else
                {
                    serve_player1 = true; // Jak fałsz to serwuje gracz 1
                    pictureBox_serve1.Visible = true;
                    pictureBox_serve2.Visible = false;
                }
            }
            else // Gdy zaczynał gracz 2
            {
                if (set % 2 == 1)
                {
                    serve_player1 = true; // Jak fałsz to serwuje gracz 1
                    pictureBox_serve1.Visible = true;
                    pictureBox_serve2.Visible = false;
                }
                else
                {
                    serve_player1 = false; // Jak prawda to serwuje gracz 2
                    pictureBox_serve2.Visible = true;
                    pictureBox_serve1.Visible = false;
                }
            }

            //// Insert danych do tabeli punkty.
            string sql_add_points = "INSERT INTO punkty(numer_setu, punkt, mecze_id, punkty_id ) VALUES ";
            for (int i = 2; i < points.Count; i++)
            {
                string player;
                points_id++;
                if (points[i].player == 1) player = label_player1.Text;
                else player = label_player2.Text;
                sql_add_points += "(" + set + ", '"+ player +"', "+ match_id +", " + points_id + ")";
                if (i + 1 < points.Count) sql_add_points += ", ";
            }
            MySqlCommand cmd = new MySqlCommand(sql_add_points, conn);
            cmd.ExecuteNonQuery();

            //// Wyświetlanie obrazków pokazujących liczbę wygranych setów.
            if (win == point1)
            {
                switch (sets_player1)
                {
                    case 1:
                        pictureBox_point1_1.Visible = true;
                        break;
                    case 2:
                        pictureBox_point1_2.Visible = true;
                        break;
                    case 3:
                        pictureBox_point1_3.Visible = true;
                        break;
                    case 4:
                        pictureBox_point1_4.Visible = true;
                        break;
                    case 5:
                        pictureBox_point1_5.Visible = true;
                        break;
                    case 6:
                        pictureBox_point1_6.Visible = true;
                        break;
                    case 7:
                        pictureBox_point1_7.Visible = true;
                        break;
                }
            }
            else
            {
                switch (sets_player2)
                {
                    case 1:
                        pictureBox_point2_1.Visible = true;
                        break;
                    case 2:
                        pictureBox_point2_2.Visible = true;
                        break;
                    case 3:
                        pictureBox_point2_3.Visible = true;
                        break;
                    case 4:
                        pictureBox_point2_4.Visible = true;
                        break;
                    case 5:
                        pictureBox_point2_5.Visible = true;
                        break;
                    case 6:
                        pictureBox_point2_6.Visible = true;
                        break;
                    case 7:
                        pictureBox_point2_7.Visible = true;
                        break;
                }
            }

            //// Sprawdzenie czy jest to koniec meczu i który zawodnik wygrał.
            if(sets_player1 == set_max)
            {
                endgame(point1);
            }
            else if(sets_player2 == set_max)
            {
                endgame(point2);
            }
            else // nie jest to koniec meczu.
            {
                label_set.Text = (set + 1).ToString();
                points.Clear();
                p = new point(1, 0);
                points.Add(p);
                p = new point(2, 0);
                points.Add(p);
                points_update();
            }           
        }

        private void cleanup(bool all) // Funkcja czyszcząca ekran aplikacji. Parametr 'all' gdy jest true wylogowuje zawodników.
        {
            //// Przywróć zmienne do domyślnych wartości.
            pictureBox_point1_1.Visible = false;
            pictureBox_point1_2.Visible = false;
            pictureBox_point1_3.Visible = false;
            pictureBox_point1_4.Visible = false;
            pictureBox_point1_5.Visible = false;
            pictureBox_point1_6.Visible = false;
            pictureBox_point1_7.Visible = false;
            pictureBox_point2_1.Visible = false;
            pictureBox_point2_2.Visible = false;
            pictureBox_point2_3.Visible = false;
            pictureBox_point2_4.Visible = false;
            pictureBox_point2_5.Visible = false;
            pictureBox_point2_6.Visible = false;
            pictureBox_point2_7.Visible = false;
            pictureBox_serve1.Visible = false;
            pictureBox_serve2.Visible = false;
            match_id = 0;
            points_id = 0;
            sets_player1 = 0;
            sets_player2 = 0;
            label_set.Text = "1";
       
            //// Przywróć atrybut Visible do domyślnych wartości.
            panel_sets1.Visible = false;
            panel_sets2.Visible = false;
            label_static_set.Visible = false;
            label_set.Visible = false;
            label_colon.Visible = false;
            label_points1.Visible = false;
            label_points2.Visible = false;
            label_points_up1.Visible = false;
            label_points_up2.Visible = false;
            label_player1.Visible = false;
            label_player2.Visible = false;

            //// Przywróć atrybut Enabled do domyślnych wartości.
            button_login1.Enabled = true;
            button_login2.Enabled = true;
            listBox_rodzaj.Enabled = true;
            button_start.Enabled = true;

            //// Jeżeli chcemy wylogować graczy po zakończeniu meczu
            if(all == true) 
            {
                //// Przywróć zmienne do domyślnych wartości.
                logged1 = false;
                logged2 = false;
                listBox_rodzaj.ClearSelected();
                button_login1.Text = "Zaloguj gracza1";
                button_login2.Text = "Zaloguj gracza2";
                label_player1.Text = "Zawodnik1";
                label_player2.Text = "Zawodnik2";
                choosed = false;
            }
        }

        public void endgame(int win) // Funkcja wywoływana przy skończeniu meczu.
        {
            timer.Stop();
            string winner;
            bool turniej = false;
            if (win == point1)
            {
                winner = label_player1.Text;
            }
            else
            {
                winner = label_player2.Text;
            }
            //// Sprawdza rodzaj meczu i postępuje następująco.

            if (listBox_rodzaj.SelectedItem.ToString() == "Turniej")
            {
                turniej = true;
                //// Robi UPDATE bazy danych date na dziś.
                DateTime thisDay = DateTime.Today;
                string sql_update_data = "UPDATE mecze SET data = '" + thisDay.ToString("d") + "' WHERE mecze_id = " + match_id;
                MySqlCommand cmd = new MySqlCommand(sql_update_data, conn);
                cmd.ExecuteNonQuery();

                if(tournament_type == "pucharowy")//// Jeżeli jest to mecz pucharowy.
                {
                    //// Ogólne założenie: Gdy mecze pucharowe danego poziomu się skończą - połącz w pary graczy którzy wygrali swój mecz.
                    //// Liczy ilość meczy które nie zostały rozegrane w pucharze
                    string sql = "SELECT COUNT(mecze_ID) from mecze where turnieje_ID = " + tournament_id + " AND data is null";
                    cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    string value = rdr[0].ToString();
                    rdr.Close();
                    if (value == "0") //// Jeżeli zwróci 0 - wszystkie mecze się odbyły.
                    {
                        //// Liczymy ilosc graczy.
                        string sql_players = "SELECT COUNT(login) from gracze where login in (select gracz1_ID from mecze where turnieje_ID = ";
                        sql_players += tournament_id + " UNION SELECT gracz2_ID from mecze where turnieje_ID = " + tournament_id + ")";
                        cmd = new MySqlCommand(sql_players, conn);
                        rdr = cmd.ExecuteReader();
                        rdr.Read();
                        int N = Int32.Parse(rdr[0].ToString());
                        rdr.Close();
                        //// Liczymy ilosc meczy.
                        string sql_match = "SELECT COUNT(mecze_ID) from mecze where turnieje_ID = " + tournament_id;
                        cmd = new MySqlCommand(sql_match, conn);
                        rdr = cmd.ExecuteReader();
                        rdr.Read();
                        int M = Int32.Parse(rdr[0].ToString());
                        rdr.Close();
                        //// Gdy puchar się nie skończył (Ilość meczy != Ilość zawodników).
                        if (M != N)
                        {
                            while (M > N / 2)
                            {
                                M -= N / 2;
                                N = N / 2;
                            }
                            //// Zauważmy że teraz M = ostatnie mecze.
                            if (M > 0)
                            {
                                //// Wyciągam wygranych z tych meczy. 
                                string sql_winners = "SELECT punkt FROM punkty p INNER JOIN (SELECT mecze_id FROM mecze where turnieje_ID = ";
                                sql_winners += tournament_id + " order by mecze_id DESC LIMIT " + M + ") as m USING(mecze_ID) where p.punkty_ID = (SELECT max(punkty_ID) from punkty WHERE mecze_ID = p.mecze_ID)";
                                cmd = new MySqlCommand(sql_winners, conn);
                                rdr = cmd.ExecuteReader();
                                List<string> nowi = new List<string>();
                                List<string> losers = new List<string>();
                                DataTable data_winners = new DataTable();
                                data_winners.Load(rdr);
                                rdr.Close();
                                //// Dodaje do listy 'nowi' wygranych.
                                if (data_winners.Rows.Count > 0)
                                {
                                    foreach (DataRow row in data_winners.Rows)
                                    {
                                        nowi.Add(row["punkt"].ToString());
                                    }
                                }
                                if (M == 2) //// Kiedy ostatnimi meczami były te z półfinału.
                                {
                                    //// Dodaje wszystkich graczy z ostatnich dwóch meczy z turnieju pucharowego.
                                    string sql_losers = "SELECT gracz1_ID, gracz2_ID FROM mecze where turnieje_ID = " + tournament_id + " order by mecze_id DESC LIMIT 2";
                                    cmd = new MySqlCommand(sql_losers, conn);
                                    rdr = cmd.ExecuteReader();
                                    DataTable data_losers = new DataTable();
                                    data_losers.Load(rdr);
                                    rdr.Close();
                                    //// Dodaje do listy 'losers' wszystkich.
                                    if (data_losers.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in data_losers.Rows)
                                        {
                                            losers.Add(row["gracz1_ID"].ToString());
                                            losers.Add(row["gracz2_ID"].ToString());
                                        }
                                    }
                                    //// Usuwa wygranych z listy.
                                    string winner1 = nowi[0];
                                    string winner2 = nowi[1];
                                    losers.Remove(winner1);
                                    losers.Remove(winner2);
                                    //// Dodaje mecze o 3 miejsce i finalowe.
                                    string sql_add_final_match = "INSERT INTO mecze(turnieje_ID, gracz1_id, gracz2_id, data) VALUES(" + tournament_id + ", '" + nowi[0] + "', '" + nowi[1] + "', null)";
                                    cmd = new MySqlCommand(sql_add_final_match, conn);
                                    cmd.ExecuteNonQuery();
                                    string sql_add_3rd_place_match = "INSERT INTO mecze(turnieje_ID, gracz1_id, gracz2_id, data) VALUES(" + tournament_id + ", '" + losers[0] + "', '" + losers[1] + "', null)";
                                    cmd = new MySqlCommand(sql_add_3rd_place_match, conn);
                                    cmd.ExecuteNonQuery();
                                }
                                else //// Kiedy ostatnimi meczami były z większą liczbą meczy do zagrania. 
                                {
                                    for (int i = 0; i < nowi.Count() / 2; i++)
                                    {
                                        string sql_add_match = "INSERT INTO mecze(turnieje_ID, gracz1_id, gracz2_id, data) VALUES(" + tournament_id + ", '" + nowi[2 * i] + "', '" + nowi[2 * i + 1] + "', null)";
                                        cmd = new MySqlCommand(sql_add_match, conn);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            } //// Gdy nie jest to mecz turniejowy.
            else
            {
                turniej = false;
            }
            //// Po zakończeniu każdego meczu następuje zmiana wartości niektórych elementów i wyświetlenie Endgame.
            Endgame end = new Endgame(winner, turniej);
            //label_points_down1.Visible = false;
            label_points_revert.Visible = false;
            label_points_up1.Visible = false;
            label_points_up2.Visible = false;
            end.ShowDialog();
            if (rematch == false)
            {
                //// Gdy w meczu towarzyskim nie chcemy rewanżu - czyścimy ekran.
                cleanup(true);
            }
            else
            {
                //// Gdy chcemy rewanż - możemy go zacząć w dowolnej chwili.
                button_start.Enabled = true;
                button_start.Text = "Rewanż";
            }
        }

        private void button_start_Click(object sender, EventArgs e) // Funkcja obsługująca event kliknięcia na button_start.
        {
            //// Sprawdzamy czy osoby się zalogowały i wybrały rodzaj rozgrywki.
            if(logged1 && logged2 && choosed)
            {
                cleanup(false);
                //// Losowanie która osoba zaczyna
                Random rand = new Random(System.DateTime.Now.Millisecond);
                serve_player_start = rand.Next(0, 2);
                if (serve_player_start == 0)
                {
                    serve_player1 = true;
                    pictureBox_serve1.Visible = true;
                }
                else
                {
                    serve_player1 = false;
                    pictureBox_serve2.Visible = true;
                }
                //// Zmień atrybut Visible na true 
                label_player1.Visible = true;
                label_player2.Visible = true;
                panel_sets1.Visible = true;
                panel_sets2.Visible = true;
                label_static_set.Visible = true;
                label_set.Visible = true;
                label_colon.Visible = true;
                label_points1.Visible = true;
                label_points2.Visible = true;
                label_points_up1.Visible = true;
                label_points_up2.Visible = true;
                
                //// Dodaj podstawowe wartości w tablicy 'points'.
                points.Clear();
                p = new point(1, 0);
                points.Add(p);
                p = new point(2, 0);
                points.Add(p);
                points_update();

                //// Zmień atrybut Enabled na false
                button_login1.Enabled = false;
                button_login2.Enabled = false;
                listBox_rodzaj.Enabled = false;
                button_start.Enabled = false;
                button_start.Text = "Start";

                //// Odczytaj z bazy danych 'match_id' bieżącego meczu (turniej), lub stwórz cały mecz (towarzyski). 
                if(listBox_rodzaj.SelectedItem.ToString() == "Turniej") //// Odczytanie 'match_id' z bazy danych
                {
                    string sql = "SELECT mecze_id FROM mecze WHERE turnieje_ID = " + tournament_id + " AND (gracz1_id = '"+label_player1.Text+ "' AND gracz2_id = '" + label_player2.Text + "') OR (gracz1_id = '" + label_player2.Text + "' AND gracz2_id = '" + label_player1.Text + "') AND data IS NULL";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read()) match_id = Int32.Parse(rdr[0].ToString());
                    else
                    {
                        MessageBox.Show("Brak meczy do zagrania!");
                        cleanup(true);
                    }
                    rdr.Close();  
                }
                else //// Dodanie meczu towarzyskiego.
                {
                    DateTime thisDay = DateTime.Today;
                    string sql_add = "INSERT INTO mecze(turnieje_ID, gracz1_id, gracz2_id, data) VALUES(null , '" + label_player1.Text + "', '" + label_player2.Text + "', '" + thisDay.ToString("d") + "')";
                    MySqlCommand cmd = new MySqlCommand(sql_add, conn);
                    cmd.ExecuteNonQuery();
                    string sql = "SELECT max(mecze_id) FROM mecze";
                    cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    match_id = Int32.Parse(rdr[0].ToString());
                    rdr.Close();
                }
                timer.Start(); // rozpoczecie sprawdzania sygnałów z C++ 
            }
            //// Wywołaj odpowiednie komunikaty w wypadku błędu zaczęcia gry.
            else if(logged1 && choosed) MessageBox.Show("Player 2 is not logged in!");
            else if(logged2 && choosed) MessageBox.Show("Player 1 is not logged in!");
            else MessageBox.Show("Set all match settings and log in!");
        }

        private void button_login1_Click(object sender, EventArgs e) // Funkcja obsługująca zalogowanie się zawodnika1.
        {
            Login log = new Login();
            log.ShowDialog();

            //// Sprawdza poprawność wprowadzonych danych
            string sql = "SELECT haslo FROM gracze WHERE login='" + my_login + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                if (label_player2.Text != my_login)
                {
                    if (rdr[0].ToString() == my_password)
                    {
                        label_player1.Text = my_login;
                        label_player1.Visible = true;
                        logged1 = true;
                        button_login1.Text = "Przeloguj " + my_login;
                    }
                    else MessageBox.Show("Bledne haslo!");
                }
                else MessageBox.Show("Osoba juz zalogowana!");
            }
            else MessageBox.Show("Bledny login!");
            rdr.Close();
        }

        private void button_login2_Click(object sender, EventArgs e) // Funkcja obsługująca zalogowanie się zawodnika2.
        {
            Login log = new Login();
            log.ShowDialog();

            //// Sprawdza poprawność wprowadzonych danych
            string sql = "SELECT haslo FROM gracze WHERE login='" + my_login + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                if(label_player1.Text != my_login)
                {
                    if (rdr[0].ToString() == my_password)
                    {
                        label_player2.Text = my_login;
                        label_player2.Visible = true;
                        logged2 = true;
                        button_login2.Text = "Przeloguj " + my_login;
                    }
                    else MessageBox.Show("Bledne haslo!");
                }
                else MessageBox.Show("Osoba juz zalogowana!");
            }
            else MessageBox.Show("Bledny login!");
            rdr.Close();
        }

        private void checkBox_auto_CheckedChanged(object sender, EventArgs e) // Funkcja obsługująca opcję automatycznego zliczania punktów dzięki programowi Michała. TODO: funkcjonalność
        {
            if(checkBox_auto.Checked == true)
            {
                //Tworzenie nowego procesu, który komunikuje się z c++
                startConnectCpp();
            }
            else
            {
                timer.Stop();
                stopConnectCpp();
            }
        }

        private void listBox_rodzaj_SelectedIndexChanged(object sender, EventArgs e) // Funkcja obsługująca event wyboru trybu gry.
        {
            if(this.listBox_rodzaj.SelectedItem != null) //// Ten if sprawdza się gdy program odznaczył wybór.
            {
                if (this.listBox_rodzaj.SelectedItem.ToString() == "Turniej")
                {
                    if(logged1 == true && logged2 == true) //// Gracze muszą być zalogowani
                    {
                        //// Sprawdzanie dostępnych nierozegranych meczy dla zalogowanych zawodników.
                        List<string> tablica = new List<string>();
                        //string tournament_id = "WHERE ((gracz1_id = '" + label_player1.Text + "' AND gracz2_id = '" + label_player2.Text;
                        //tournament_id += "') OR (gracz1_id = '" + label_player2.Text + "' AND gracz2_id = '" + label_player1.Text + "')) AND data IS NULL AND turnieje_id IS NOT NULL";
                        string tournament_id = "SELECT turnieje_id FROM mecze WHERE (((gracz1_id = '" + label_player1.Text + "' ";
                        tournament_id += "AND gracz2_id = '" + label_player2.Text + "') OR (gracz1_id = '" + label_player2.Text + "' ";
                        tournament_id += "AND gracz2_id = '" + label_player1.Text + "')) AND data IS NULL) AND turnieje_id IS NOT NULL";
                        MySqlCommand cmd_id = new MySqlCommand(tournament_id, conn);
                        MySqlDataReader rdr = cmd_id.ExecuteReader();
                        DataTable data_tournament_id = new DataTable();
                        data_tournament_id.Load(rdr);
                        rdr.Close();
                        if (data_tournament_id.Rows.Count > 0)
                        {
                            foreach(DataRow row in data_tournament_id.Rows)
                            {
                                tablica.Add(row["turnieje_id"].ToString());
                            }
                            TournamentMatchSettings settings = new TournamentMatchSettings(tablica, conn);
                            settings.ShowDialog(); //// Gdy wybierze się turniej wartość 'choosed' zmieni się na true.
                        }
                        else
                        {
                            MessageBox.Show("Brak dostępnych meczy turniejowych dla zalogowanych graczy!");
                            listBox_rodzaj.ClearSelected();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Musisz najpierw się zalogować!");
                        listBox_rodzaj.ClearSelected();
                    }
                }
                else if (this.listBox_rodzaj.SelectedItem.ToString() == "Mecz towarzyski")
                {
                    FriendlyMatchSettings settings = new FriendlyMatchSettings();
                    settings.ShowDialog(); // Gdy wybierze się tryb meczu towarzyskiego wartość 'choosed' zmieni się na true.
                }
                
            }
        }

        private void label_points_up1_Click(object sender, EventArgs e) // Funkcja obsługująca dodanie punktu zawodnika1.
        {
            point k = new point(1, point1 + 1);
            points.Add(k);
            points_update();
            if (point1 == point_max + point_margin) //// Gdy punkty osiągną liczbę punktów potrzebną do wygrania zrób:
            {
                if (point1 - point2 > 1) //// Gdy przewaga jest większa niż 1 zrób:
                {
                    Wait agree = new Wait(label_player1.Text);
                    agree.ShowDialog();
                    if (accept_set == false) //// Gdy set zostanie zaakceptowany przez obu graczy dodaje set.
                    {
                        point_margin = 0;
                        sets_player1++;
                        sets_update(point1);
                    }
                    else
                    {
                        points.Remove(k);
                        points_update();
                    }
                }
                else
                {
                    point_margin++;
                    points_update();
                }
             }
        }

        private void label_points_up2_Click(object sender, EventArgs e) // Funkcja obsługująca dodanie punktu zawodnika2.
        {
            point k = new point(2, point2 + 1);
            points.Add(k);
            points_update();
            if (point2 == point_max + point_margin)
            {
                if (point2 - point1 > 1) //// Gdy przewaga jest większa niż 1 zrób:
                {
                    Wait agree = new Wait(label_player2.Text);
                    agree.ShowDialog();
                    if (accept_set == false) //// Gdy set zostanie zaakceptowany przez obu graczy dodaje set.
                    {
                        point_margin = 0;
                        sets_player2++;
                        sets_update(point2);
                    }
                    else
                    {
                        points.Remove(k);
                        points_update();
                    }
                }
                else
                {
                    point_margin++;
                    points_update();
                }
            }
        }
        
        private void label_points_revert_Click(object sender, EventArgs e)
        {
            point k = points.Last();
            points.Remove(k);
            if (point2 - point1 > 0 && point_margin > 0)
            {
                point_margin--;
            }
            if (point1 - point2 > 0 && point_margin > 0)
            {
                point_margin--;
            }
            points_update();
            if (point1 >= point_max + point_margin)
            {
                Wait agree = new Wait(label_player1.Text);
                agree.ShowDialog();
                if (accept_set == false) //// Gdy set zostanie zaakceptowany przez obu graczy dodaje set.
                {
                    point_margin = 0;
                    sets_player1++;
                    sets_update(point1);
                }
                else
                {
                    points.Add(k);
                    point_margin++;
                    points_update();
                }
            }
            if (point2 >= point_max + point_margin)
            {
                Wait agree = new Wait(label_player2.Text);
                agree.ShowDialog();
                if (accept_set == false) //// Gdy set zostanie zaakceptowany przez obu graczy dodaje set.
                {
                    point_margin = 0;
                    sets_player2++;
                    sets_update(point2);
                }
                else
                {
                    points.Add(k);
                    point_margin++;
                    points_update();
                }
            }
        }
    }
}
