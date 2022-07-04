using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XBCAD_Stock_Taking_application
{
    public partial class View_Stock_Finance : Form
    {
        private ArrayList brandNames = new ArrayList();
        private ArrayList catergoryNames = new ArrayList();
        private ArrayList subCatergoryNames = new ArrayList();
        private int randomAdditiveSeed = 0;
        
        public View_Stock_Finance()
        {
            Random rand = new Random();
            randomAdditiveSeed = rand.Next();
            InitializeComponent();
            setUpAltInfo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindGridView();
            GridViewDimensions();
            populateInsert();
            
        }

        public void Totals()
        {
            decimal val = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[7] != null && item.Cells[7].Value != DBNull.Value) {

                    if (item.Cells[7].Value != null && item.Cells[7].Value.ToString().Length>2) {
                        val += Convert.ToDecimal(item.Cells[7].Value.ToString().Replace("R ", ""));
                    }
                    //val += Convert.ToDecimal(item.Cells[7].Value);
                }


            }
            txtTotal.Text = "R " + val.ToString();
        }
        private Boolean findDuplicate(String title,String date,String price,int index)
        {
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != index && dataGridView1.Rows[i].Cells[1].Value!=null && dataGridView1.Rows[i].Cells[2].Value != null && dataGridView1.Rows[i].Cells[4].Value != null)
                {
                    String currTitle = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    String currDate = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    String currPrice = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    //if(title.Equals(currTitle) && date.Equals(currDate) && price.Equals(currPrice))
                    if (title.Equals(currTitle)) 
                    {
                        return true;
                    }
                }
                else
                {
                    continue;
                }
            }
            
            return false;
        }
        private void colorDuplicates()
        {
            Random random = new Random(randomAdditiveSeed);
            int randomSeedAdditive = random.Next();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[2].Value != null && dataGridView1.Rows[i].Cells[4].Value != null)
                {
                    String currTitle = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    String currDate = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    String currPrice = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    if (findDuplicate(currTitle, currDate, currPrice, i))
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = genColor(i*randomSeedAdditive);
                    }
                }
            }
        }
        
        private Color genColor(int seed)
        {
            Random random = new Random(seed);
            int r = random.Next(60, 150);
            random = new Random(r*seed);
            int g = random.Next(60, 150);
            random = new Random(g*seed);
            int b = random.Next(60, 150);

            return Color.FromArgb(r, g, b);
        }

        public void BindGridView()
        {           
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total] " +
                           "FROM [Stock_Main] AS SM " +
                           "FULL JOIN [Scales] AS S ON S.S_Stock_ID = SM.Stock_ID " +
                           "FULL JOIN [Loadcells] AS L ON L.L_Stock_ID = SM.Stock_ID " +
                           "FULL JOIN [Weights] AS W ON W.W_Stock_ID = SM.Stock_ID " +
                           "FULL JOIN [Weight_Sets] AS WS ON WS.WS_Stock_ID = SM.Stock_ID " +
                           "FULL JOIN [Misc] AS M ON M.M_Stock_ID = SM.Stock_ID " +
                           "WHERE SM.Stock_ID > 0 " +
                           "ORDER BY Catergory, Sub_Catergory, " +
                           "CASE WHEN [Limit_Unit] = 'mg' THEN 1 " +
                                "WHEN [Limit_Unit] = 'g' THEN 2 " +
                                "WHEN [Limit_Unit] = 'kg' THEN 3 " +
                                "WHEN [Limit_Unit] = 't' THEN 4 " +
                                "ELSE [Limit_Unit] END DESC, " +
                                "S.Limit DESC, " +
                           "CASE WHEN [L_Mass_Unit] = 'mg' THEN 1 " +
                                "WHEN [L_Mass_Unit] = 'g' THEN 2 " +
                                "WHEN [L_Mass_Unit] = 'kg' THEN 3 " +
                                "WHEN [L_Mass_Unit] = 't' THEN 4 " +
                                "ELSE [L_Mass_Unit] END DESC, " +
                                "L.L_Mass DESC, " +
                           "CASE WHEN [W_Mass_Unit] = 'mg' THEN 1 " +
                                "WHEN [W_Mass_Unit] = 'g' THEN 2 " +
                                "WHEN [W_Mass_Unit] = 'kg' THEN 3 " +
                                "WHEN [W_Mass_Unit] = 't' THEN 4 " +
                                "ELSE [W_Mass_Unit] END DESC, " +
                                "W.W_Mass DESC, " +
                           "CASE WHEN [WS_Min_Mass_Unit] = 'mg' THEN 1 " +
                                "WHEN [WS_Min_Mass_Unit] = 'g' THEN 2 " +
                                "WHEN [WS_Min_Mass_Unit] = 'kg' THEN 3 " +
                                "WHEN [WS_Min_Mass_Unit] = 't' THEN 4 " +
                                "ELSE [WS_Min_Mass_Unit] END DESC, " +
                                "WS.MinMass DESC, " +
                           "CASE WHEN [WS_Max_Mass_Unit] = 'mg' THEN 1 " +
                                "WHEN [WS_Max_Mass_Unit] = 'g' THEN 2 " +
                                "WHEN [WS_Max_Mass_Unit] = 'kg' THEN 3 " +
                                "WHEN [WS_Max_Mass_Unit] = 't' THEN 4 " +
                                "ELSE [WS_Max_Mass_Unit] END DESC, " +
                                "WS.MaxMass DESC";


            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();        
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
            colorDuplicates();

        }
        
        public void GridViewDimensions()
        {
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[2].Width = 75;
            dataGridView1.Columns[3].Width = 75;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;
        }

        private void setUpAltInfo()
        {
            gbLoadCellData.Visible = false;
            gbLoadCellData.Left = 10;
            gbLoadCellData.Top = 233;
            gbLoadCellData.Height = 173;

            gbMiscData.Visible = false;
            gbMiscData.Left = 10;
            gbMiscData.Top = 233;
            gbMiscData.Height = 173;

            gbScaleData.Visible = false;
            gbScaleData.Left = 10;
            gbScaleData.Top = 233;
            gbScaleData.Height = 173;

            gbWeightData.Visible = false;
            gbWeightData.Left = 10;
            gbWeightData.Top = 233;
            gbWeightData.Height = 173;

            gbWeightSetData.Visible = false;
            gbWeightSetData.Left = 10;
            gbWeightSetData.Top = 233;
            gbWeightSetData.Height = 173;
        }

        private void cbGenTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            String typeID = cbGenTypeID.Text;

            gbLoadCellData.Visible = false;
            gbMiscData.Visible = false;
            gbScaleData.Visible = false;
            gbWeightData.Visible = false;
            gbWeightSetData.Visible = false;

            if (typeID.Equals("Scale"))
            {
                gbScaleData.Visible = true;
            }
            else if (typeID.Equals("Loadcell"))
            {
                gbLoadCellData.Visible = true;
            }
            else if (typeID.Equals("Weight"))
            {
                gbWeightData.Visible = true;
            }
            else if (typeID.Equals("Weight_Set"))
            {
                gbWeightSetData.Visible = true;
            }
            else if (typeID.Equals("Misc"))
            {
                gbMiscData.Visible = true;
            }
        }

        private void populateInsert()
        {
            String query = "Select Brand, Catergory, Sub_Catergory from [Stock_Main]";//Catergory, Sub_Catergory

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            //ArrayList brandNames = new ArrayList();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        String temp = reader.GetString(0);
                        if (!brandNames.Contains(temp))
                        {
                            brandNames.Add(temp);
                        }
                    }
                    catch (Exception)
                    {}

                    try
                    {
                        String temp = reader.GetString(1);
                        if (!catergoryNames.Contains(temp))
                        {
                            catergoryNames.Add(temp);
                        }
                    }
                    catch (Exception)
                    { }

                    try
                    {
                        String temp = reader.GetString(2);
                        if (!subCatergoryNames.Contains(temp))
                        {
                            subCatergoryNames.Add(temp);
                        }
                    }
                    catch (Exception)
                    { }

                }
            }
            foreach (String item in brandNames)
            {
                cbGenBrand.Items.Add(item);
                cbBrand.Items.Add(item);
            }

            foreach (String item in catergoryNames)
            {
                cbGenCatagory.Items.Add(item);
                cbCatergories.Items.Add(item);
            }

            foreach (String item in subCatergoryNames)
            {
                cbGenSubCat.Items.Add(item);
                cbSubCatergories.Items.Add(item);
            }

            con.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtGenTitle.Text.Length == 0)
            {
                MessageBox.Show("Please fill out all required fields");
            }
            else if (cbGenCatagory.Text.Length == 0)
            {
                MessageBox.Show("Please fill out all required fields");
            }
            else if (cbGenTypeID.Text.Length == 0)
            {
                MessageBox.Show("Please fill out all required fields");
            }
            else
            {
                if (cbGenTypeID.SelectedIndex == 0)
                {
                    int id = insertGen("Scale");
                    insertScale(id);
                }
                else if (cbGenTypeID.SelectedIndex == 1)
                {
                    int id = insertGen("Loadcell");
                    insertLoadCell(id);
                }
                else if(cbGenTypeID.SelectedIndex == 2)
                {
                    int id = insertGen("Weight");
                    insertWeight(id);
                }
                else if(cbGenTypeID.SelectedIndex == 3)
                {
                    if (txtWSMaxMass.Value>txtWSMinMass.Value)
                    {
                        int id = insertGen("Weight_Sets");
                        insertWeightSet(id);
                    }
                    else
                    {
                        MessageBox.Show("Minimum Value cannot be more than Maximum");
                    }
                }
                else if(cbGenTypeID.SelectedIndex == 4)
                {
                    int id = insertGen("Misc");
                    insertMisc(id);
                }
             }
            BindGridView();
        }

        private int insertGen(String typeID) {

            string insertStockMain = "Insert into [Stock_Main](Brand, Title, Date, Unit, Price_Each, Type_ID, Catergory, Sub_Catergory)" +
                                          "Values(@valBrand, @valTitle, @valDate, @valUnit, @valPrice_Each, @valType_ID, @valCatergory, @valSub_Catergory)";

            decimal priceEach = txtGenPrice.Value;
            decimal unit = txtGenUnit.Value;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(insertStockMain, con);
            con.Open();

            cmd.Parameters.AddWithValue("valBrand", cbGenBrand.Text);
            cmd.Parameters.AddWithValue("valTitle", txtGenTitle.Text);
            if (cbNoDate.Checked == true)
            {
                cmd.Parameters.AddWithValue("valDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("valDate", dtGenDate.Value);
            }                     
            cmd.Parameters.AddWithValue("valUnit", unit);            
            if (priceEach == 0)
            {
                cmd.Parameters.AddWithValue("valPrice_Each", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("valPrice_Each", priceEach);
            }            
            cmd.Parameters.AddWithValue("valType_ID", typeID);
            cmd.Parameters.AddWithValue("valCatergory", cbGenCatagory.Text);
            cmd.Parameters.AddWithValue("valSub_Catergory", cbGenSubCat.Text);

            int result1 = cmd.ExecuteNonQuery();
            con.Close();
           
            int id = 0;

            string highestID = "SELECT MAX(Stock_ID) FROM [Stock_Main]";
            SqlCommand cmd1 = new SqlCommand(highestID, con);
            con.Open();
            SqlDataReader rd = cmd1.ExecuteReader();
            while (rd.Read())
            {
                id = rd.GetInt32(0);
                break;
            }
            con.Close();
           
            return id;
        }

        private void insertScale(int id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            bool isWaterProof = cbWaterproof.Checked;

            Decimal isDimensions = txtScaleDimL.Value;
            Decimal DimLenght = txtScaleDimL.Value;
            Decimal DimWidth = txtScaleDimW.Value;

            string insertScale = "Insert into [Scales](S_Stock_ID,Limit, Limit_Unit, Dimension_Length, Dimension_Opperator, Dimension_Width, Is_Water_Proof)" +
                                 "Values(@valID, @valLimit, @valLimit_Unit, @valDimension_Length, @valDimension_Opperator, @valDimension_Width, @valIs_Water_Proof)"; //Dimension unit has not been included.

            SqlCommand cmd = new SqlCommand(insertScale, con);
            con.Open();

            cmd.Parameters.AddWithValue("valID", id);
            cmd.Parameters.AddWithValue("valLimit", txtScaleLimit.Value);
            cmd.Parameters.AddWithValue("valLimit_Unit", cbScaleLimitUnit.Text);
            if (DimLenght == 0)
            {
                cmd.Parameters.AddWithValue("valDimension_Length", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("valDimension_Length", DimLenght);
            }
            if (isDimensions > 0)
            {
                cmd.Parameters.AddWithValue("valDimension_Opperator", " x ");
            }
            else
            {
                cmd.Parameters.AddWithValue("valDimension_Opperator", DBNull.Value);
            }
            if (DimWidth == 0)
            {
                cmd.Parameters.AddWithValue("valDimension_Width", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("valDimension_Width", DimWidth);
            }
            
            cmd.Parameters.AddWithValue("valIs_Water_Proof", isWaterProof);

            int result2 = cmd.ExecuteNonQuery();

            if ( result2 > 0)
            {
                MessageBox.Show("Item has been added.");
            }
            con.Close();
        }
        private void insertLoadCell(int id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            string insertLoadcell = "Insert into [Loadcells](L_Stock_ID, L_Mass, L_Mass_Unit)" +
                                 "Values(@valID, @valL_Mass, @valL_Mass_Unit)"; 

            SqlCommand cmd = new SqlCommand(insertLoadcell, con);
            con.Open();

            cmd.Parameters.AddWithValue("valID", id);
            cmd.Parameters.AddWithValue("valL_Mass", txtLoadMass.Value);
            cmd.Parameters.AddWithValue("valL_Mass_Unit", cbLoadMU.Text);

            int result2 = cmd.ExecuteNonQuery();

            if (result2 > 0)
            {
                MessageBox.Show("Item has been added.");
            }
            con.Close();
        }
        private void insertWeight(int id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            string insertWeights = "Insert into [Weights](W_Stock_ID, W_Mass, W_Mass_Unit)" +
                                 "Values(@valID, @valW_Mass, @valW_Mass_Unit)";

            SqlCommand cmd = new SqlCommand(insertWeights, con);
            con.Open();

            cmd.Parameters.AddWithValue("valID", id);
            cmd.Parameters.AddWithValue("valW_Mass", txtWeightMass.Value);
            cmd.Parameters.AddWithValue("valW_Mass_Unit", cbWeightMU.Text);

            int result2 = cmd.ExecuteNonQuery();

            if (result2 > 0)
            {
                MessageBox.Show("Item has been added.");
            }
            con.Close();
        }
        private void insertWeightSet(int id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            Decimal WSMin = txtWSMinMass.Value;
            Decimal WSMax = txtWSMaxMass.Value;

            string insertWeightSet = "Insert into [Weight_Sets](WS_Stock_ID, MinMass, WS_Opperator, MaxMass, WS_Min_Mass_Unit, WS_Max_Mass_Unit)" +
                                 "Values(@valID, @valMin_Mass, @valWS_Opperator, @valMax_Mass, @valMin_Mass_Unit, @valMax_Mass_Unit)";

            SqlCommand cmd = new SqlCommand(insertWeightSet, con);
            con.Open();

            cmd.Parameters.AddWithValue("valID", id);
            cmd.Parameters.AddWithValue("valMin_Mass", WSMin);
            if (WSMin > 0)
            {
                cmd.Parameters.AddWithValue("valWS_Opperator", " - ");
            }
            else
            {
                cmd.Parameters.AddWithValue("valWS_Opperator", DBNull.Value);
            }            
            cmd.Parameters.AddWithValue("valMax_Mass", WSMax);           
            cmd.Parameters.AddWithValue("valMin_Mass_Unit", cbWSMinMassU.Text);
            cmd.Parameters.AddWithValue("valMax_Mass_Unit", cbWSMaxMassU.Text);

            int result2 = cmd.ExecuteNonQuery();

            if (result2 > 0)
            {
                MessageBox.Show("Item has been added.");
            }
            con.Close();
        }
        private void insertMisc(int id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            string insertWeightSet = "Insert into [Misc](M_Stock_ID, Added_Info)" +
                                 "Values(@valID, @Added_Info)";

            SqlCommand cmd = new SqlCommand(insertWeightSet, con);
            con.Open();

            cmd.Parameters.AddWithValue("valID", id);
            cmd.Parameters.AddWithValue("Added_Info", txtMiscAddedInfo.Text);
           
            int result2 = cmd.ExecuteNonQuery();

            if (result2 > 0)
            {
                MessageBox.Show("Item has been added.");
            }
            con.Close();
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            BindGridView();
            cbTypeID.SelectedIndex = -1;
            cbBrand.SelectedIndex = -1;
            cbCatergories.SelectedIndex = -1;
            cbSubCatergories.SelectedIndex = -1;
            txtSearch.Text = "";
        }

        private void cbTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedType = cbTypeID.SelectedIndex;
            if (selectedType > -1)
            {
                cbBrand.SelectedIndex = -1;
                cbCatergories.SelectedIndex = -1;
                cbSubCatergories.SelectedIndex = -1;
                txtSearch.Text = "";
            }
            if (selectedType==0)
            {
                getScales();
                colorDuplicates();
            }
            else if (selectedType == 1)
            {
                getLoadCell();
                colorDuplicates();
            }
            else if (selectedType == 2)
            {
                getWeight();
                colorDuplicates();
            }
            else if (selectedType == 3)
            {
                getWeightSet();
                colorDuplicates();
            }
            else if (selectedType == 4)
            {
                getMisc();
                colorDuplicates();
            }
        }

        private void getScales() {

            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total], CONCAT(Scales.Limit, Scales.Limit_Unit) AS [Weight Limit], CONCAT(Scales.Dimension_Length, Scales.Dimension_Opperator, Scales.Dimension_Width) AS [Dimensions], is_Water_Proof " +
                           "FROM [Stock_Main], [Scales] " +
                           "WHERE Stock_Main.Stock_ID = Scales.S_Stock_ID";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
           
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }
        private void getLoadCell() 
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total], CONCAT(Loadcells.L_Mass, Loadcells.L_Mass_Unit) AS [Loadcell Limit]" +
                           "FROM [Stock_Main],[Loadcells] " +
                           "WHERE Stock_Main.Stock_ID = Loadcells.L_Stock_ID";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }
        private void getWeight() 
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total], CONCAT(Weights.W_Mass, Weights.W_Mass_Unit) AS [Weight] " +
                           "FROM [Stock_Main],[Weights] " +
                           "WHERE Stock_Main.Stock_ID = Weights.W_Stock_ID";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }
        private void getWeightSet() 
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total], CONCAT(Weight_Sets.MinMass, Weight_Sets.WS_Min_Mass_Unit, Weight_Sets.WS_Opperator, Weight_Sets.MinMass, Weight_Sets.WS_Max_Mass_Unit) AS [Set Range] " +
                           "FROM [Stock_Main],[Weight_Sets] " +
                           "WHERE Stock_Main.Stock_ID = Weight_Sets.WS_Stock_ID";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }
        private void getMisc() 
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total], Misc.Added_Info " +
                           "FROM [Stock_Main],[Misc] " +
                           "WHERE Stock_Main.Stock_ID = Misc.M_Stock_ID";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }

        private void cbCatergories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCatergories.SelectedIndex>-1)
            {
                cbTypeID.SelectedIndex = -1;
                cbBrand.SelectedIndex = -1;
                cbSubCatergories.SelectedIndex = -1;
                txtSearch.Text = "";

                string selectedCat = cbCatergories.SelectedItem.ToString();
                getSelectedCatergory(selectedCat);
                colorDuplicates();
            }
        }
        private void getSelectedCatergory(string selectedCat)
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total] " +
                           "FROM [Stock_Main] " +
                           "WHERE Stock_Main.Catergory LIKE @valCatergories";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("valCatergories", selectedCat);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }

        
        private void cbSubCatergories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubCatergories.SelectedIndex>-1)
            {
                cbTypeID.SelectedIndex = -1;
                cbBrand.SelectedIndex = -1;
                cbCatergories.SelectedIndex = -1;
                txtSearch.Text = "";

                string selectedSubCat = cbSubCatergories.SelectedItem.ToString();
                getSelectedSubCatergory(selectedSubCat);
                colorDuplicates();
            }
        }
        private void getSelectedSubCatergory(string selectedSubCat)
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each AS [Price],'R #'), Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total] " +
                           "FROM [Stock_Main] " +
                           "WHERE Stock_Main.Sub_Catergory LIKE @valSubCatergory";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("valSubCatergory", selectedSubCat);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }


        private void cbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBrand.SelectedIndex>-1)
            {
                cbTypeID.SelectedIndex = -1;
                cbCatergories.SelectedIndex = -1;
                cbSubCatergories.SelectedIndex = -1;
                txtSearch.Text = "";

                string selectedBrand = cbBrand.SelectedItem.ToString();
                getSelectedBrand(selectedBrand);
                colorDuplicates();
            }
        }
        private void getSelectedBrand(string selectedBrand)
        {
            String query = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total] " +
                           "FROM [Stock_Main] " +
                           "WHERE Stock_Main.Brand LIKE @valBrand";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("valBrand", selectedBrand);
            con.Open();

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            Totals();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            cbTypeID.SelectedIndex = -1;
            cbBrand.SelectedIndex = -1;
            cbCatergories.SelectedIndex = -1;
            cbSubCatergories.SelectedIndex = -1;
            string title = string.Empty;

            string titleQuery = "SELECT Stock_ID AS [ID], Title, Date, Unit, FORMAT(Price_Each,'R #') AS [Price], Catergory, Sub_Catergory, FORMAT(Unit*Price_Each,'R #') AS [Price Total] " +
                                "FROM [Stock_Main] WHERE Stock_Main.Title LIKE '%' + @valTitle + '%' OR Stock_Main.Brand LIKE '%' + @valBrand + '%'";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
            SqlCommand cmd = new SqlCommand(titleQuery, con);
            con.Open();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("valTitle", txtSearch.Text);
            cmd.Parameters.AddWithValue("valBrand", txtSearch.Text);
            SqlDataReader rd = cmd.ExecuteReader();
           
            con.Close();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            Totals();
            colorDuplicates();
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            string deleteQuery = "DELETE FROM [Scales] WHERE S_Stock_ID = @valStock_ID; " +
                                 "DELETE FROM [Loadcells] WHERE L_Stock_ID = @valStock_ID; " +
                                 "DELETE FROM [Weights] WHERE W_Stock_ID = @valStock_ID; " +
                                 "DELETE FROM [Weight_Sets] WHERE WS_Stock_ID = @valStock_ID; " +
                                 "DELETE FROM [Misc] WHERE M_Stock_ID = @valStock_ID; " +
                                 "DELETE FROM [Stock_Main] WHERE Stock_ID = @valStock_ID ";

            if (dataGridView1.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                if (MessageBox.Show("Are you sure, you would like to delete this record? \n" +
                                   "This opperation can not be reversed once confirmed.", "DataGridView", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {                    
                    SqlCommand cmd = new SqlCommand(deleteQuery, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("valStock_ID", dataGridView1.CurrentRow.Cells[0].Value);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    int id = 0;

                    string highestID = "SELECT MAX(Stock_ID) FROM [Stock_Main]";
                    SqlCommand cmd1 = new SqlCommand(highestID, con);
                    con.Open();
                    SqlDataReader rd = cmd1.ExecuteReader();
                    while (rd.Read())
                    {
                        id = rd.GetInt32(0);
                        break;
                    }
                    con.Close();

                    string reseedQuery = "DBCC CHECKIDENT (Stock_Main, RESEED, @valReseedValue)";

                    SqlCommand cmd2 = new SqlCommand(reseedQuery, con);
                    con.Open();

                    cmd2.Parameters.AddWithValue("valReseedValue", id);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    e.Cancel = true;
                }                
            }
            else
            {
                e.Cancel = true;
            }

            Totals();
        }

        //Investigate this btnCheck_Click, i think its just the reseed button
        private void btnCheck_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            randomAdditiveSeed = rand.Next();
            colorDuplicates();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            int id = 0;

            string highestID = "SELECT MAX(Stock_ID) FROM [Stock_Main]";
            SqlCommand cmd1 = new SqlCommand(highestID, con);
            con.Open();
            SqlDataReader rd = cmd1.ExecuteReader();
            while (rd.Read())
            {
                id = rd.GetInt32(0);
                break;
            }
            con.Close();

            string reseedQuery = "DBCC CHECKIDENT (Stock_Main, RESEED, @valReseedValue)";

            SqlCommand cmd2 = new SqlCommand(reseedQuery, con);
            con.Open();

            cmd2.Parameters.AddWithValue("valReseedValue", id);
            cmd2.ExecuteNonQuery();

            MessageBox.Show("This is the reseed value: " + id);
            con.Close();

        }
       
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtIDDisplay.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            DateTime selectedDate = DateTime.Today;

            string dateQuery = "SELECT Date " +
                               "FROM [Stock_main] " +
                               "WHERE Stock_ID = @valStock_ID";

            SqlCommand cmd = new SqlCommand(dateQuery, con);
            con.Open();
            cmd.Parameters.AddWithValue("valStock_ID", txtIDDisplay.Text);

            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string DateNull = string.Empty;
                DateNull = rd["Date"].ToString();

                if (DateNull == "")
                {
                    selectedDate = DateTime.Today;
                    checkNoDate.Checked = true;
                }
                else
                {
                    selectedDate = (DateTime)rd["Date"];
                    break;
                }               
            }

            dtpUpdate.Value = selectedDate;
            con.Close();

            txtUnitUpdate.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPriceUpdate.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();            
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE [Stock_Main] " +
                                 "SET Date = @valDate, Unit = @valUnit, Price_Each = @valPrice_Each " +
                                 "WHERE Stock_ID = @valStock_ID";
                                

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

            SqlCommand cmd = new SqlCommand(updateQuery, con);
            con.Open();
            cmd.Parameters.AddWithValue("valStock_ID", txtIDDisplay.Text);
            if (checkNoDate.Checked == true)
            {
                cmd.Parameters.AddWithValue("valDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("valDate", dtpUpdate.Value);
            }
            cmd.Parameters.AddWithValue("valUnit", txtUnitUpdate.Value);
            cmd.Parameters.AddWithValue("valPrice_Each", txtPriceUpdate.Value);
            

            int result = cmd.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Data has been updated");
            }
            con.Close();

            BindGridView();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            colorDuplicates();
        }
    }
}
