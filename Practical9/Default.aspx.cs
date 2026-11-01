using System;
using System.Data;
using System.Data.SqlClient;

namespace Practical9
{
    public partial class Default : System.Web.UI.Page
    {
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PlacementDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSummary();
                LoadPlacements();
            }
        }

        private void LoadSummary()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Students", con);

                lblTotal.Text = cmd.ExecuteScalar().ToString();

                // Count placed students
                cmd.CommandText =
                    "SELECT COUNT(DISTINCT StudentId) FROM Students";

                lblPlaced.Text = cmd.ExecuteScalar().ToString();

                // Unplaced students cannot be calculated
                // because IsPlaced column has been removed.
                lblUnplaced.Text = "N/A";

                // Highest Package
                cmd.CommandText =
                    "SELECT MAX(PackageLPA) FROM Students";

                object result = cmd.ExecuteScalar();

                decimal highest = result == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(result);

                lblHighestPackage.Text = highest.ToString("N2");

                int total = Convert.ToInt32(lblTotal.Text);
                int placed = Convert.ToInt32(lblPlaced.Text);

                decimal percentage = total == 0
                    ? 0
                    : (decimal)placed * 100 / total;

                lblPercentage.Text =
                    percentage.ToString("N2") + "%";
            }
        }

        private void LoadPlacements()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT PlacementId, StudentId, CompanyName, JobRole, PackageLPA FROM Students";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvPlacements.DataSource = dt;
                gvPlacements.DataBind();
            }
        }
    }
}