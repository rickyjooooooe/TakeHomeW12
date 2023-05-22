using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        MySqlConnection sqlConnection;
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqlDataAdapter;
        MySqlDataReader sqlDataReader;
        DataTable dtcombobox =new DataTable();
        DataTable dtPlayer = new DataTable();
        DataTable dtManager = new DataTable();
        DataTable dtManager0 = new DataTable();
        DataTable idManager = new DataTable();
        string Connection = "server=localhost;uid=root;pwd=titi020504;database=premier_league";
        string query = "";
        string managername = "";
        string deletepemain = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void updateDGVplayer()
        {
            dtPlayer = new DataTable();
            sqlConnection = new MySqlConnection(Connection);
            query = "select * from player;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtPlayer);
            dataGridView1.DataSource = dtPlayer;
        }
        private void manageridstatus()
        {
            dtManager = new DataTable();
            query = $"SELECT manager_name,nation,birthdate\r\nFROM manager m\r\nJOIN nationality n\r\non m.nationality_id=n.nationality_id\r\nWhere working=0;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtManager);
            dataGridView3.DataSource = dtManager;
        }
        private void updateCMB()
        {
            dtcombobox = new DataTable();
            query = "SELECT team_name as 'tname', team_id as 'tid' FROM team t;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtcombobox);
        }
        private void updateDGV2()
        {
            dtManager0 = new DataTable();
            sqlConnection = new MySqlConnection(Connection);
            query = $"SELECT m.manager_name,t.team_name,birthdate,nation\r\nFrom team t\r\nJOIN manager m\r\non t.manager_id=m.manager_id\r\njoin nationality n\r\non m.nationality_id=n.nationality_id\r\nwhere t.team_name='{cmb_team.Text}';";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtManager0);
            dataGridView2.DataSource = dtManager0;
        }
        private void updateDGV4() {
            dtPlayer = new DataTable();
            sqlConnection = new MySqlConnection(Connection);
            query = $"select p.player_name,n.nation,p.playing_pos,p.team_number,p.height,p.weight,p.birthdate from player p, team t, nationality n where p.nationality_id=n.nationality_id and p.team_id=t.team_id and p.status=1 and t.team_name='{cmb_teamdelete.Text}';";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtPlayer);
            dataGridView4.DataSource = dtPlayer;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dtcombobox = new DataTable();
                sqlConnection = new MySqlConnection(Connection);
                query = "SELECT nation N,nationality_id as 'ID' FROM nationality n;";
                sqlCommand = new MySqlCommand(query, sqlConnection);
                sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dtcombobox);
                cmb_nationality.DataSource= dtcombobox;
                cmb_nationality.DisplayMember = "N";
                cmb_nationality.ValueMember = "ID";

                updateCMB();
                cmb_teamname.DataSource = dtcombobox;
                cmb_teamname.DisplayMember = "tname";
                cmb_teamname.ValueMember = "tid";
                updateCMB();
                cmb_team.DataSource = dtcombobox;
                cmb_team.DisplayMember = "tname";
                cmb_team.ValueMember = "tid";
                updateCMB();
                cmb_teamdelete.DataSource = dtcombobox;
                cmb_teamdelete.DisplayMember = "tname";
                cmb_teamdelete.ValueMember= "tid";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            updateDGVplayer();
            manageridstatus();
        }

        private void btn_playertoteam_Click(object sender, EventArgs e)
        {
            string playerid = txtBox_playerid.Text;
            string name = txtBox_name.Text;
            string teamnumber = txtBox_teamnumber.Text;
            string nationality = cmb_nationality.SelectedValue.ToString();
            string pos = txtBox_position.Text;
            string height = txtBox_height.Text;
            string weight = txtBox_weight.Text;
            string birthdate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string teamname = cmb_teamname.SelectedValue.ToString();
            //MessageBox.Show(teamname);
            query = $"INSERT INTO PLAYER VALUES ('{playerid}',{teamnumber},'{name}','{nationality}','{pos}',{height},{weight},'{birthdate}','{teamname}',1,0);";
            try
            {
                sqlConnection.Open();
                sqlCommand= new MySqlCommand(query,sqlConnection);
                sqlDataReader=sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                updateDGVplayer();
            }
        }

        private void cmb_team_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDGV2();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string managerdgvatas=dtManager0.Rows[0][0].ToString();
            idManager = new DataTable();
            query = $"select manager_id from manager where manager_name='{managername}';";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(idManager);
            string id = idManager.Rows[0][0].ToString();

            query = $"UPDATE team t,manager mAtas,manager mBawah set t.manager_id='{id}',mBawah.working=1,mAtas.working=0 where mAtas.manager_name='{managerdgvatas}' AND mBawah.manager_id='{id}' AND t.team_name='{cmb_team.Text}';";
            
            try
            {
                sqlConnection.Open();
                sqlCommand = new MySqlCommand(query, sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                updateDGV2();
                manageridstatus();
            }

            managername = "";
            btn_update.Enabled = false;
        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            managername = dataGridView3.CurrentCell.Value.ToString();
            //MessageBox.Show(dataGridView3.CurrentCell.Value.ToString());

            if (managername != "")
                btn_update.Enabled = true;
        }

        private void cmb_teamdelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDGV4();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            dtPlayer = new DataTable();
            query = $"select count(p.player_name) from player p,team t where p.team_id = t.team_id and t.team_name ='{cmb_teamdelete.Text}' and status =1;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtPlayer);
            int jumlah= Convert.ToInt32(dtPlayer.Rows[0][0]);
            
            if(jumlah <= 11)
            {
                MessageBox.Show("Player minimal 11");
            }
            else
            {
                query = $"UPDATE player\r\nset status=0\r\nWHERE player_name='{deletepemain}';";

                try
                {
                    sqlConnection.Open();
                    sqlCommand = new MySqlCommand(query, sqlConnection);
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    updateDGV4();
                }
            }
            
            deletepemain = "";
            btn_delete.Enabled = false;
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            deletepemain = dataGridView4.CurrentCell.Value.ToString();
            if (deletepemain != "")
                btn_delete.Enabled = true;
        }
    }
    
}
