using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using Timers = System.Timers;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected int id;
        protected int active;
        protected int r0 = 0;
        protected int r1 = 0;
        protected int r2 = 0;
        protected int r3 = 0;
        protected int r4 = 0;
        protected int rAdd0 = 0;
        protected int rAdd1 = 0;
        protected int rAdd2 = 0;
        protected int rAdd3 = 0;
        protected int rAdd4 = 0;
        protected Timers::Timer Env_VarClock = new Timers::Timer(60000);
        protected Timers::Timer Res_AddClock = new Timers::Timer(1000);
        protected Timers::Timer SpecialClock = new Timers::Timer(100);
        protected Timers::Timer SteelClock = new Timers::Timer(50);
        protected Timers::Timer Res_SyncClock = new Timers::Timer(3000);
        //protected static string ConnString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=5432))(CONNECT_DATA=(SID=ora2016)));User Id=xxx;Password=xxx;"; //putty
        protected static string ConnString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oraas)(PORT=1521))(CONNECT_DATA=(SID=ora2016)));User Id=xxx;Password=xxx;"; //wydzial
        protected OracleConnection conn = new OracleConnection(ConnString);


        public MainWindow()
        {
            InitializeComponent();
            conn.Open();
            SyncResource();
            Res_SyncClock.Enabled = false;
            Res_SyncClock.Start();
            Res_SyncClock.Elapsed += Clock_ElapsedSyncResource;
            Res_AddClock.Enabled = false;
            Res_AddClock.Start();
            Res_AddClock.Elapsed += Clock_ElapsedAddResource;
            Env_VarClock.Enabled = false;
            Env_VarClock.Start();
            Env_VarClock.Elapsed += Clock_ElapsedEnvironmentVariable;

        }
        /*MenuClick*/
        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            nullAdd();
            MainPageGrid.Visibility = Visibility.Visible;
            BuildingsGrid.Visibility = Visibility.Collapsed;
            TechnologyGrid.Visibility = Visibility.Collapsed;
            PlanetarySystemGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
        }
        private void Buildings_Click(object sender, RoutedEventArgs e)
        {
            nullAdd();
            MainPageGrid.Visibility = Visibility.Collapsed;
            BuildingsGrid.Visibility = Visibility.Visible;
            TechnologyGrid.Visibility = Visibility.Collapsed;
            PlanetarySystemGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;

            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = "SELECT NAZWA as \"Nazwa\" FROM BUD WHERE ID_BUD IN (SELECT BUD_ID_BUD FROM POTRZEBNA_TECH WHERE ID_TECHNOLOGII IN (SELECT TECH_ID_TECH FROM ODKRYTE_TECH WHERE ULEPSZENIE=1 GROUP BY TECH_ID_TECH))OR ID_BUD=0 OR ID_BUD=1";
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            oda.Fill(ds, "BuildingsTable");
            BuildingsDataGrid.ItemsSource = ds.Tables["BuildingsTable"].DefaultView;
        }
        private void Technology_Click(object sender, RoutedEventArgs e)
        {
            nullAdd();
            MainPageGrid.Visibility = Visibility.Collapsed;
            BuildingsGrid.Visibility = Visibility.Collapsed;
            TechnologyGrid.Visibility = Visibility.Visible;
            PlanetarySystemGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;

            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM (SELECT NAZWA as \"Nazwa\" FROM TECH WHERE ID_TECH NOT IN(SELECT TECH_ID_TECH FROM ODKRYTE_TECH "
                            +"GROUP BY TECH_ID_TECH) ORDER BY (KOSZT_SPEC+KOSZT_ENER+KOSZT_SURP+KOSZT_SURR+KOSZT_SURU)) WHERE ROWNUM <6";
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            oda.Fill(ds, "TechnologyTable");
            TechnologyDataGrid.ItemsSource = ds.Tables["TechnologyTable"].DefaultView;
        }
        private void PlanetarySystem_Click(object sender, RoutedEventArgs e)
        {
            nullAdd();
            MainPageGrid.Visibility = Visibility.Collapsed;
            BuildingsGrid.Visibility = Visibility.Collapsed;
            TechnologyGrid.Visibility = Visibility.Collapsed;
            PlanetarySystemGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Collapsed;

            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.NAZWA as \"Nazwa\",p.ODLEGLOSC as \"Dystans\", w.SUROWCE_POSPOLITE as \"Stal\", w.SUROWCE_RZADKIE as \"Graf\", w.SUROWCE_UNIKALNE as \"Antym\" FROM PLANETA p LEFT JOIN WARUNKI w ON p.ID_PLANETY=w.PLANETA_ID_PLANETY ORDER BY ID_PLANETY";
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            oda.Fill(ds,"PlanetTable");
            PlanetDataGrid.ItemsSource = ds.Tables["PlanetTable"].DefaultView;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            nullAdd();
            MainPageGrid.Visibility = Visibility.Collapsed;
            BuildingsGrid.Visibility = Visibility.Collapsed;
            TechnologyGrid.Visibility = Visibility.Collapsed;
            PlanetarySystemGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
        }

        /*SpecialistsClick*/
        private void SpecialistsClick_Click(object sender, RoutedEventArgs e)
        {
            if (r1 > 0)
            {
                this.SpecialistsClick.IsHitTestVisible = false;
                SpecialClock.Enabled = false;
                SpecialClock.Start();
                SpecialClock.Elapsed += Clock_ElapsedSpecialists;
                r1--;
                ResToString();
            }
        }
        private void SpecialistsClick_MouseEnter(object sender, MouseEventArgs e)
        {
            this.SpecReqButton.Visibility = Visibility.Visible;
        }
        private void SpecialistsClick_MouseLeave(object sender, MouseEventArgs e)
        {
            this.SpecReqButton.Visibility = Visibility.Hidden;
        }

        /*EnergyClick*/
        private void EnergyClick_Click(object sender, RoutedEventArgs e)
        {
            r1++;
            ResToString();
        }
        private void EnergyClick_MouseEnter(object sender, MouseEventArgs e)
        {
            this.EneReqButton.Visibility = Visibility.Visible;
        }
        private void EnergyClick_MouseLeave(object sender, MouseEventArgs e)
        {
            this.EneReqButton.Visibility = Visibility.Hidden;
        }

        /*SteelClick*/
        private void SteelClick_Click(object sender, RoutedEventArgs e)
        {
            if ((r0 > 0) && (r1 > 2))
            {
                this.SteelClick.IsHitTestVisible = false;
                SteelClock.Enabled = false;
                SteelClock.Start();
                SteelClock.Elapsed += Clock_ElapsedSteel;
                r0--;
                r1 -= 3;
                ResToString();
            }
        }
        private void SteelClick_MouseEnter(object sender, MouseEventArgs e)
        {
            this.SteelReqButton.Visibility = Visibility.Visible;
        }
        private void SteelClick_MouseLeave(object sender, MouseEventArgs e)
        {
            this.SteelReqButton.Visibility = Visibility.Hidden;
        }

        /*Resources - int to string*/
        private void ResToString()
        {
            Res0.Text = r0.ToString();
            Res1.Text = r1.ToString();
            Res2.Text = r2.ToString();
            Res3.Text = r3.ToString();
            Res4.Text = r4.ToString();
            Gain0.Text = rAdd0.ToString();
            Gain1.Text = rAdd1.ToString();
            Gain2.Text = rAdd2.ToString();
            Gain3.Text = rAdd3.ToString();
            Gain4.Text = rAdd4.ToString();
        }

        /*Synchronise resources what you got*/
        private void SyncResource()
        {
            int active;
            int[] prev_res = new int[5];
            int[] prev_add = new int[5];
            OracleCommand cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "SELECT ID_PLANETY FROM PLANETA WHERE AKTYWNA=1";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            active = dr.GetInt32(0);
            dr.Close();
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        cmd.CommandText = "SELECT SPECJALISCI FROM SUROWCE WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 1:
                        cmd.CommandText = "SELECT ENERGIA FROM SUROWCE WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 2:
                        cmd.CommandText = "SELECT SUROWCE_POSPOLITE FROM SUROWCE WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 3:
                        cmd.CommandText = "SELECT SUROWCE_RZADKIE FROM SUROWCE WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 4:
                        cmd.CommandText = "SELECT SUROWCE_UNIKALNE FROM SUROWCE WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                }
                OracleDataReader dr2 = cmd.ExecuteReader();
                dr2.Read();
                prev_res[i] = dr2.GetInt32(0);
                dr2.Close();
            }
            r0 = prev_res[0];
            r1 = prev_res[1];
            r2 = prev_res[2];
            r3 = prev_res[3];
            r4 = prev_res[4];


            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        cmd.CommandText = "SELECT SPECJALISCI FROM SUROWCE_PRZYROST WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 1:
                        cmd.CommandText = "SELECT ENERGIA FROM SUROWCE_PRZYROST WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 2:
                        cmd.CommandText = "SELECT SUROWCE_POSPOLITE FROM SUROWCE_PRZYROST WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 3:
                        cmd.CommandText = "SELECT SUROWCE_RZADKIE FROM SUROWCE_PRZYROST WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                    case 4:
                        cmd.CommandText = "SELECT SUROWCE_UNIKALNE FROM SUROWCE_PRZYROST WHERE PLANETA_ID_PLANETY = " + active.ToString(); break;
                }
                OracleDataReader dr2 = cmd.ExecuteReader();
                dr2.Read();
                prev_add[i] = dr2.GetInt32(0);
                dr2.Close();
            }
            rAdd0 = prev_add[0];
            rAdd1 = prev_add[1];
            rAdd2 = prev_add[2];
            rAdd3 = prev_add[3];
            rAdd4 = prev_add[4];
            ResToString();
        }

        /*ClockElapsedSpecialists*/
        private void Clock_ElapsedSpecialists(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.TrainProgressBar.Value += 10;
            }
            ));
        }

        /*ClockElapsedSteel*/
        private void Clock_ElapsedSteel(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.SteelProgressBar.Value += 10;
            }
            ));
        }

        /*Clock_ElapsedSyncResource*/
        private void Clock_ElapsedSyncResource(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                int[] add_res = new int[5];
                int[] add_gain = new int[5];
                int active;
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT ID_PLANETY FROM PLANETA WHERE AKTYWNA=1";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                active = dr.GetInt32(0);
                dr.Close();

                add_res[0] = Int32.Parse(Res0.Text);
                add_res[1] = Int32.Parse(Res1.Text);
                add_res[2] = Int32.Parse(Res2.Text);
                add_res[3] = Int32.Parse(Res3.Text);
                add_res[4] = Int32.Parse(Res4.Text);

                cmd.CommandText = "UPDATE SUROWCE SET SPECJALISCI = " + add_res[0].ToString() 
                                + ",ENERGIA = " + add_res[1].ToString() 
                                + ",SUROWCE_POSPOLITE = " + add_res[2].ToString() 
                                + ",SUROWCE_RZADKIE = " + add_res[3].ToString() 
                                + ",SUROWCE_UNIKALNE = " + add_res[4].ToString() 
                                + " WHERE PLANETA_ID_PLANETY = " + active.ToString();
                cmd.ExecuteNonQuery();//PAMIĘTAJ O COMMIT W SQLDEVELOPER!

                add_gain[0] = Int32.Parse(Gain0.Text);
                add_gain[1] = Int32.Parse(Gain1.Text);
                add_gain[2] = Int32.Parse(Gain2.Text);
                add_gain[3] = Int32.Parse(Gain3.Text);
                add_gain[4] = Int32.Parse(Gain4.Text);
                cmd.CommandText = "UPDATE SUROWCE_PRZYROST SET SPECJALISCI = " + add_gain[0].ToString()
                                + ",ENERGIA = " + add_gain[1].ToString()
                                + ",SUROWCE_POSPOLITE = " + add_gain[2].ToString()
                                + ",SUROWCE_RZADKIE = " + add_gain[3].ToString()
                                + ",SUROWCE_UNIKALNE = " + add_gain[4].ToString()
                                + " WHERE PLANETA_ID_PLANETY = " + active.ToString();
                cmd.ExecuteNonQuery();//PAMIĘTAJ O COMMIT W SQLDEVELOPER!
            }
            ));
        }

        /*Clock_ElapsedAddResource*/
        private void Clock_ElapsedAddResource(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                int a;
                r0 += Int32.Parse(Gain0.Text);
                r1 += Int32.Parse(Gain1.Text);
                r2 += Int32.Parse(Gain2.Text);
                r3 += Int32.Parse(Gain3.Text);
                r4 += Int32.Parse(Gain4.Text);
                a = Int32.Parse(Seconds.Content.ToString());
                a--;
                Seconds.Content = a.ToString();
                ResToString();
            }));
         }

        /*Clock_ElapsedEnvironmentVariable*/
        private void Clock_ElapsedEnvironmentVariable(object sender, System.Timers.ElapsedEventArgs e)
        {
            int[] en = new int[3];
            Random r = new Random();
            int i = r.Next(0, 300);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT SUROWCE_POSPOLITE FROM WARUNKI LEFT JOIN PLANETA ON WARUNKI.PLANETA_ID_PLANETY=PLANETA.ID_PLANETY WHERE PLANETA_ID_PLANETY = " + active.ToString();
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            en[0] = dr.GetInt32(0);
            cmd.CommandText = "SELECT SUROWCE_RZADKIE FROM WARUNKI LEFT JOIN PLANETA ON WARUNKI.PLANETA_ID_PLANETY=PLANETA.ID_PLANETY WHERE PLANETA_ID_PLANETY = " + active.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            en[1] = dr.GetInt32(0);
            cmd.CommandText = "SELECT SUROWCE_UNIKALNE FROM WARUNKI LEFT JOIN PLANETA ON WARUNKI.PLANETA_ID_PLANETY=PLANETA.ID_PLANETY WHERE PLANETA_ID_PLANETY = " + active.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            en[2] = dr.GetInt32(0);
            if (i < en[0])
            {
                r2++;
            }
            else if (i < en[1])
            {
                r3++;
            }
            else r4++;
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                Seconds.Content = "60";
            }));
        }

        /*TrainProgressBar*/
        private void TrainProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (this.TrainProgressBar.Value == 100)
            {
                r0++;
                Res0.Text = r0.ToString();
                this.SpecialistsClick.IsHitTestVisible = true;
                this.SpecialClock.Close();
                this.TrainProgressBar.Value = 0;
                SpecialClock.Elapsed -= Clock_ElapsedSpecialists;
            }
        }

        /*SteelProgressBar*/
        private void SteelProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SteelProgressBar.Value == 100)
            {
                r2++;
                Res2.Text = r2.ToString();
                this.SteelClick.IsHitTestVisible = true;
                this.SteelClock.Close();
                this.SteelProgressBar.Value = 0;
                SteelClock.Elapsed -= Clock_ElapsedSteel;
            }

        }

        /*Set null values in Add*/
        private void nullAdd()
        {
            Add0.Content = "";
            Add0.Foreground = new SolidColorBrush(Colors.GreenYellow);
            Add1.Content = "";
            Add1.Foreground = new SolidColorBrush(Colors.GreenYellow);
            Add2.Content = "";
            Add2.Foreground = new SolidColorBrush(Colors.GreenYellow);
            Add3.Content = "";
            Add3.Foreground = new SolidColorBrush(Colors.GreenYellow);
            Add4.Content = "";
            Add4.Foreground = new SolidColorBrush(Colors.GreenYellow);
        }

        /*Selected Building in Grid*/
        private void BuildingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = (DataRowView)BuildingsDataGrid.SelectedItem;
            OracleCommand cmd = new OracleCommand();
            string result;
            if (drv != null)
            {
                result = (drv["Nazwa"]).ToString();
                cmd.CommandText = "SELECT ID_BUD FROM BUD WHERE NAZWA = '" + result + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                id = dr.GetInt32(0);

                nullAdd();
                showBuilding();
            }
            else
            {
                buildingsName.Text = "";
                buildReqSpecText.Text = "";
                buildReqEnergyText.Text = "";
                buildReqSteelText.Text = "";
                buildReqGrapheneText.Text = "";
                buildReqAntymatterText.Text = "";
                buildLevel.Text = "";
                buildingsDescription.Text = "Wybierz budynek";
            }

        }
        private void showBuilding()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            int lvl;

            cmd.CommandText = "SELECT ID_PLANETY FROM PLANETA WHERE AKTYWNA = 1";
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            active = dr.GetInt32(0);
            cmd.CommandText = "SELECT ULEPSZENIE FROM ZBUDOWANE_B WHERE BUD_ID_BUD = "+id.ToString()+" AND PLANETA_ID_PLANETY = "+active.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            if(dr.HasRows == false)
            {
                lvl = 0;
            }
            else
            {
                lvl = dr.GetInt32(0);
            }
            //zczytanie wartości req
            cmd.CommandText = "SELECT ku.KOSZT_SPEC as \"Specjalisci\", ku.KOSZT_ENER as \"Energia\", ku.KOSZT_SURP as \"Stal\", ku.KOSZT_SURR as \"Grafen\", ku.KOSZT_SURU as \"Antymateria\" FROM KOSZT_ULB ku "
                            +"LEFT JOIN ZBUDOWANE_B zb ON ku.BUD_ID_BUD=zb.BUD_ID_BUD WHERE ku.BUD_ID_BUD=" + id.ToString()+" AND ku.ULEPSZENIE="+lvl.ToString()+"+1";
            OracleDataAdapter odaReq = new OracleDataAdapter();
            odaReq.SelectCommand = cmd;

            DataSet dsReq = new DataSet();
            odaReq.Fill(dsReq, "ReqTable");

            DataGrid myReq = new DataGrid();
            myReq.ItemsSource = dsReq.Tables["ReqTable"].DefaultView;
            DataRowView drvReq = (DataRowView)myReq.Items[0];

            buildReqSpecText.Text = (drvReq["Specjalisci"]).ToString();
            buildReqEnergyText.Text = (drvReq["Energia"]).ToString();
            buildReqSteelText.Text = (drvReq["Stal"]).ToString();
            buildReqGrapheneText.Text = (drvReq["Grafen"]).ToString();
            buildReqAntymatterText.Text = (drvReq["Antymateria"]).ToString();

            //zczytanie wartosci add
            cmd.CommandText = "SELECT prz.DODAJ_SPEC as \"Specjalisci\", prz.DODAJ_ENER as \"Energia\", prz.DODAJ_SURP as \"Stal\", prz.DODAJ_SURR as \"Grafen\",prz.DODAJ_SURU as \"Antymateria\" FROM PRZYROST_ZA_ULB prz "
                            +"LEFT JOIN ZBUDOWANE_B zb ON prz.BUD_ID_BUD=zb.BUD_ID_BUD WHERE prz.BUD_ID_BUD="+id.ToString()+" AND prz.NR_ULEPSZENIA="+lvl.ToString()+"+1";
            OracleDataAdapter odaAdd = new OracleDataAdapter();
            odaAdd.SelectCommand = cmd;

            DataSet dsAdd = new DataSet();
            odaAdd.Fill(dsAdd, "AddTable");

            DataGrid myAdd = new DataGrid();
            myAdd.ItemsSource = dsAdd.Tables["AddTable"].DefaultView;
            DataRowView drvAdd = (DataRowView)myAdd.Items[0];

            int[] a = new int[5];
            a[0] = Int32.Parse((drvAdd["Specjalisci"]).ToString());
            a[1] = Int32.Parse((drvAdd["Energia"]).ToString());
            a[2] = Int32.Parse((drvAdd["Stal"]).ToString());
            a[3] = Int32.Parse((drvAdd["Grafen"]).ToString());
            a[4] = Int32.Parse((drvAdd["Antymateria"]).ToString());

            if (a[0] < 0) Add0.Foreground = new SolidColorBrush(Colors.Red);
            if (a[1] < 0) Add1.Foreground = new SolidColorBrush(Colors.Red);
            if (a[2] < 0) Add2.Foreground = new SolidColorBrush(Colors.Red);
            if (a[3] < 0) Add3.Foreground = new SolidColorBrush(Colors.Red);
            if (a[4] < 0) Add4.Foreground = new SolidColorBrush(Colors.Red);
            if (a[0] != 0) Add0.Content = a[0].ToString();
            if (a[1] != 0) Add1.Content = a[1].ToString();
            if (a[2] != 0) Add2.Content = a[2].ToString();
            if (a[3] != 0) Add3.Content = a[3].ToString();
            if (a[4] != 0) Add4.Content = a[4].ToString();


            //zczytanie wartosci nazwy i lvl
            cmd.CommandText = "SELECT NAZWA FROM BUD WHERE ID_BUD=" + id.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            buildingsName.Text = dr.GetString(0);

            buildLevel.Text = lvl.ToString();

            //zczytanie opisu 
            BuildDescription();
        }
        private void BuildDescription()
        {
            buildingsDescription.Text = "";//do zrobienia w przyszlosci
        }
        private void buildingsUpgrade_Click(object sender, RoutedEventArgs e)
        {
            int req0, req1, req2, req3, req4;
            int lvl;
            bool x = true;
            if (buildLevel.Text == "")
            {
                return;
            }
            lvl = Int32.Parse(buildLevel.Text);
            req0 = Int32.Parse(buildReqSpecText.Text);
            req1 = Int32.Parse(buildReqEnergyText.Text);
            req2 = Int32.Parse(buildReqSteelText.Text);
            req3 = Int32.Parse(buildReqGrapheneText.Text);
            req4 = Int32.Parse(buildReqAntymatterText.Text);

            if (req0 <= r0 && req1 <= r1 && req2 <= r2 && req3 <= r3 && req4 <= r4)
            {

                if (Add0.Content.ToString() != "") { if (rAdd0 + Int32.Parse(Add0.Content.ToString()) <= 0) x = false; }
                if (Add1.Content.ToString() != "") { if (rAdd1 + Int32.Parse(Add1.Content.ToString()) <= 0) x = false; }
                if (Add2.Content.ToString() != "") { if (rAdd2 + Int32.Parse(Add2.Content.ToString()) <= 0) x = false; }
                if (Add3.Content.ToString() != "") { if (rAdd3 + Int32.Parse(Add3.Content.ToString()) <= 0) x = false; }
                if (Add4.Content.ToString() != "") { if (rAdd4 + Int32.Parse(Add4.Content.ToString()) <= 0) x = false; }

                if (x == false)
                {
                    return;
                }
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                if (lvl == 0)
                {
                    lvl++;
                    cmd.CommandText = "INSERT INTO ZBUDOWANE_B VALUES(" + active.ToString() + ", " + id.ToString() + ", " + lvl.ToString() + ")";
                }
                else
                {
                    lvl++;
                    cmd.CommandText = "UPDATE ZBUDOWANE_B SET ULEPSZENIE = " + lvl.ToString() + " WHERE PLANETA_ID_PLANETY = " + active.ToString() + " AND BUD_ID_BUD = " + id.ToString();
                }
                cmd.ExecuteNonQuery();
                if (Add0.Content.ToString() != "") { rAdd0 += Int32.Parse(Add0.Content.ToString()); }
                if (Add1.Content.ToString() != "") { rAdd1 += Int32.Parse(Add1.Content.ToString()); }
                if (Add2.Content.ToString() != "") { rAdd2 += Int32.Parse(Add2.Content.ToString()); }
                if (Add3.Content.ToString() != "") { rAdd3 += Int32.Parse(Add3.Content.ToString()); }
                if (Add4.Content.ToString() != "") { rAdd4 += Int32.Parse(Add4.Content.ToString()); }
                r0 -= req0;
                r1 -= req1;
                r2 -= req2;
                r3 -= req3;
                r4 -= req4;
                ResToString();

                //update
                nullAdd();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.CommandText = "SELECT NAZWA as \"Nazwa\" FROM BUD WHERE ID_BUD IN (SELECT BUD_ID_BUD FROM POTRZEBNA_TECH WHERE ID_TECHNOLOGII IN (SELECT TECH_ID_TECH FROM ODKRYTE_TECH WHERE ULEPSZENIE=1 GROUP BY TECH_ID_TECH))OR ID_BUD=0 OR ID_BUD=1";
                OracleDataAdapter oda = new OracleDataAdapter();
                oda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oda.Fill(ds, "BuildingsTable");
                BuildingsDataGrid.ItemsSource = ds.Tables["BuildingsTable"].DefaultView;
            }
            else
            {
                return;
            }
        }

        /*Selected Technology in Grid*/
        private void TechnologyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = (DataRowView)TechnologyDataGrid.SelectedItem;
            OracleCommand cmd = new OracleCommand();
            string result;
            if (drv != null)
            {
                result = (drv["Nazwa"]).ToString();
                cmd.CommandText = "SELECT ID_TECH FROM TECH WHERE NAZWA = '" + result + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                id = dr.GetInt32(0);

                showTechnology();
            }
            else
            {
                currentTechnologyName.Text = "";
                techReqSpecText.Text = "";
                techReqEnergyText.Text = "";
                techReqSteelText.Text = "";
                techReqGrapheneText.Text = "";
                techReqAntymatterText.Text = "";
                currentTechnologyDescription.Text = "Wybierz technologie";
            }
        }
        private void showTechnology()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            int lvl;

            cmd.CommandText = "SELECT ID_PLANETY FROM PLANETA WHERE AKTYWNA = 1";
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            active = dr.GetInt32(0);
            cmd.CommandText = "SELECT ULEPSZENIE FROM ZBUDOWANE_B WHERE BUD_ID_BUD = " + id.ToString() + " AND PLANETA_ID_PLANETY = " + active.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == false)
            {
                lvl = 0;
            }
            else
            {
                lvl = dr.GetInt32(0);
            }
            //zczytanie wartości req
            cmd.CommandText = "SELECT KOSZT_SPEC as \"Specjalisci\", KOSZT_ENER as \"Energia\", KOSZT_SURP as \"Stal\", KOSZT_SURR as \"Grafen\", KOSZT_SURU as \"Antymateria\" FROM TECH WHERE ID_TECH=" + id.ToString();
            OracleDataAdapter odaReq = new OracleDataAdapter();
            odaReq.SelectCommand = cmd;

            DataSet dsReq = new DataSet();
            odaReq.Fill(dsReq, "ReqTable");

            DataGrid myReq = new DataGrid();
            myReq.ItemsSource = dsReq.Tables["ReqTable"].DefaultView;
            DataRowView drvReq = (DataRowView)myReq.Items[0];

            techReqSpecText.Text = (drvReq["Specjalisci"]).ToString();
            techReqEnergyText.Text = (drvReq["Energia"]).ToString();
            techReqSteelText.Text = (drvReq["Stal"]).ToString();
            techReqGrapheneText.Text = (drvReq["Grafen"]).ToString();
            techReqAntymatterText.Text = (drvReq["Antymateria"]).ToString();

            //zczytanie wartosci nazwy i lvl
            cmd.CommandText = "SELECT NAZWA FROM TECH WHERE ID_TECH=" + id.ToString();
            dr = cmd.ExecuteReader();
            dr.Read();
            currentTechnologyName.Text = dr.GetString(0);

            //zczytanie opisu 
            TechnologyDescription();
        }
        private void TechnologyDescription()
        {
            currentTechnologyDescription.Text = "";//do zrobienia w przyszlosci
        }
        private void currentTechnologyUpgrade_Click(object sender, RoutedEventArgs e)
        {
            int req0, req1, req2, req3, req4;
            if (currentTechnologyName.Text == "")
            {
                return;
            }
            req0 = Int32.Parse(techReqSpecText.Text);
            req1 = Int32.Parse(techReqEnergyText.Text);
            req2 = Int32.Parse(techReqSteelText.Text);
            req3 = Int32.Parse(techReqGrapheneText.Text);
            req4 = Int32.Parse(techReqAntymatterText.Text);
            if (req0 <= r0 && req1 <= r1 && req2 <= r2 && req3 <= r3 && req4 <= r4)
            {
                r0 -= req0;
                r1 -= req1;
                r2 -= req2;
                r3 -= req3;
                r4 -= req4;

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO ODKRYTE_TECH VALUES (" + id.ToString() + ", 0)";
                cmd.ExecuteNonQuery();
                ResToString();

                //update
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM (SELECT NAZWA as \"Nazwa\" FROM TECH WHERE ID_TECH NOT IN(SELECT TECH_ID_TECH FROM ODKRYTE_TECH "
                                + "GROUP BY TECH_ID_TECH) ORDER BY (KOSZT_SPEC+KOSZT_ENER+KOSZT_SURP+KOSZT_SURR+KOSZT_SURU)) WHERE ROWNUM <6";
                OracleDataAdapter oda = new OracleDataAdapter();
                oda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oda.Fill(ds, "TechnologyTable");
                TechnologyDataGrid.ItemsSource = ds.Tables["TechnologyTable"].DefaultView;

            }
            else
            {
                currentTechnologyUpgrade.Content = "Nie można";
            }
        }

        /*Options*/
        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            r0 += 50;
            r1 += 2000;
            r2 += 100;
            r3 += 5;
            r4 += 1;
            ResToString();
            helpButton.IsEnabled = false;
            helpButton.Content = "Wspomogłeś";
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(sure.IsChecked == true)
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM ZBUDOWANE_B";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM ODKRYTE_TECH";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM PLANETA";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Merkury', 92, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Wenus', 41, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Ziemia', 0, 1, 1)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Mars', 78, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Jowisz', 630, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Saturn', 1277, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Uran', 2721, 0, 0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO PLANETA(NAZWA, ODLEGLOSC, POSIADANIE, AKTYWNA) VALUES('Neptun', 4349, 0, 0)";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Zresetowano cały postęp!");
                ResToString();
                SyncResource();
                sure.IsChecked = false;
                helpButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Jesteś świadomy konsekwencji?");
            }
        }

        /*Do zrobienia w przyszlosci*/
        private void Colonize_Click(object sender, RoutedEventArgs e)
        {
            //do zrobienia w przyszlosci
        }
    }
}
