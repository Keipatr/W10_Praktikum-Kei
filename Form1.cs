using MySql.Data.MySqlClient;
using System;
using System.Data;


namespace W08_Praktikum_Kei
{
    public partial class formPertandingan : System.Windows.Forms.Form
    {
        public formPertandingan()
        {
            InitializeComponent();
        }
        public static string sqlConnection = "server=localhost;uid=root;pwd=; database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        DataTable dtTeamAway = new DataTable();
        DataTable dtTeamHome = new DataTable();
        private void formPertandingan_Load(object sender, EventArgs e)
        {
            sqlQuery = "SELECT t.team_id as `ID Tim`, t.team_name as `Nama Tim`, m.manager_name as `Nama Manager`, IF(m2.manager_name IS NULL,'----',m2.manager_name) as `Nama Asisten Manager`,p.player_name as `Nama Kapten`, home_stadium as `Stadium`, capacity as `Kapasitas` FROM team t LEFT JOIN manager m2 ON  t.assmanager_id = m2.manager_id ,manager m, player p WHERE t.manager_id = m.manager_id AND t.captain_id = p.player_id ;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamAway);
            sqlAdapter.Fill(dtTeamHome);
            cBoxTimHome.DataSource = dtTeamHome;
            cBoxTimHome.DisplayMember = "Nama Tim";
            cBoxTimHome.ValueMember = "ID Tim";
            cBoxTimAway.DataSource = dtTeamAway;
            cBoxTimAway.DisplayMember = "Nama Tim";
            cBoxTimAway.ValueMember = "ID Tim";
        }

        private void cBoxTimHome_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblManagerHome.Text = dtTeamHome.Rows[cBoxTimHome.SelectedIndex]["Nama Manager"].ToString();
            lblCaptainHome.Text = dtTeamHome.Rows[cBoxTimHome.SelectedIndex]["Nama Kapten"].ToString();
            lblStadium.Text = dtTeamHome.Rows[cBoxTimHome.SelectedIndex]["Stadium"].ToString();
            lblCapacity.Text = dtTeamHome.Rows[cBoxTimHome.SelectedIndex]["Kapasitas"].ToString();
        }

        private void cBoxTimAway_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblManagerAway.Text = dtTeamAway.Rows[cBoxTimAway.SelectedIndex]["Nama Manager"].ToString();
            lblCaptainAway.Text = dtTeamAway.Rows[cBoxTimAway.SelectedIndex]["Nama Kapten"].ToString();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            DataTable dtTanggal = new DataTable();
            sqlQuery = "select date_format(m.match_date, \"%e %M %Y\") as Tanggal from `match` m, team t where t.team_id = m.team_home or t.team_id = m.team_away and m.team_home = '" + cBoxTimHome.SelectedValue.ToString() + "' and m.team_away = '" + cBoxTimAway.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTanggal);
            lbTanggal.Text = dtTanggal.Rows[cBoxTimHome.SelectedIndex]["Tanggal"].ToString();

            DataTable dtSkor = new DataTable();
            sqlQuery = "select concat(m.goal_home, ' - ', m.goal_away) as skor from `match` m, team t where t.team_id = m.team_home or t.team_id = m.team_away and m.team_home = '" + cBoxTimHome.SelectedValue.ToString() + "' and m.team_away = '" + cBoxTimAway.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);                            
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtSkor);
            lbSkor.Text = dtSkor.Rows[cBoxTimHome.SelectedIndex]["skor"].ToString();

            DataTable dtGrid = new DataTable();
            sqlQuery = "select minute,if (p.team_id = team_home, player_name,'') as 'Player Name 1', if (p.team_id = team_home,if (type = 'CY','Yellow Card',if (type = 'CR','Red Card',if (type = 'GO','Goal',if (type = 'GP','Goal Penalty',if (type = 'GW','Own Goal','Penalty Miss'))))),'') as 'Tipe 1', if (p.team_id = team_away, player_name,'') as 'Player Name 2', if (p.team_id = team_away,if (type = 'CY','Yellow Card',if (type = 'CR','Red Card',if (type = 'GO','Goal',if (type = 'GP','Goal Penalty',if (type = 'GW','Own Goal','Penalty Miss'))))),'') as 'Tipe 2' from `match` m,dmatch d, player p where p.player_id = d.player_id and d.match_id = m.match_id and m.team_away = '" + cBoxTimAway.SelectedValue.ToString() + "'and m.team_home = '" + cBoxTimHome.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtGrid);                        
            dtgMatch.DataSource = dtGrid;
        }
    }
}
