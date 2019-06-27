using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();

            //Conectamos nuestra base de datos Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\JuegosDB.mdb";
            BindGrid();
        }

        //Mostramos los registros en la tabla
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            cmd.Connection = con;

            if(txtBuscador.Text != "")
            {
                cmd.CommandText = "select * from Juegos where Nombre like '%" + txtBuscador.Text + "%' order by IdJuego"; 
            }
            else
            {
                cmd.CommandText = "select  * from Juegos";
            }


            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if(dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Agregamos registros en la tabla
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            cmd.Connection = con;

            if(txtId.Text != "")
            {
                if(txtId.IsEnabled == true)
                {
                    if(ddlESBR.Text != "Seleccionar ESBR" && ddlOnline.Text != "Seleccionar Online")
                    {
                        cmd.CommandText = "insert into Juegos(IdJuego, Nombre, ESBR, Genero, ConsolaPermitida, CantidadJugadores, Online, Empresa, Precio, FechaCreacion, FechaSalida) Values (" + txtId.Text + ",'" + txtNombre.Text + "','" + ddlESBR.Text + "','" + txtGenero.Text + "','" + txtConsola.Text + "','" + txtJugadores.Text + "','" + ddlOnline.Text + "','" + txtEmpresa.Text + "','" + txtPrecio.Text + "','" + dpFC.Text + "','" + dpFS.Text + "')";
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("El registro se agrego exitosamente!");
                        ClearAll();
                    }
                    else
                    {
                        MessageBox.Show("Porfavor seleccionar el ESBR y/o Online!");
                    }
                }
                else
                {
                    cmd.CommandText = "update Juegos set Nombre='" + txtNombre.Text + "', ESBR='" + ddlESBR.Text + "', Genero='" + txtConsola.Text +
                        "', ConsolaPermitida='" + txtConsola.Text + "', CantidadJugadores='" + txtJugadores.Text + "', Online='" + ddlOnline.Text +
                        "', Empresa='" + txtEmpresa.Text + "', Precio='" + txtPrecio.Text + "', FechaCreacion='" + dpFC.Text + "', FechaSalida='" + dpFS.Text + "' where IdJuego=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Detalles del juegos actualizados satisfactoriamente.");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Porfavor agrega un juego.");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            ddlESBR.SelectedIndex = 0;
            txtGenero.Text = "";
            txtConsola.Text = "";
            txtJugadores.Text = "";
            ddlOnline.SelectedIndex = 0;
            txtEmpresa.Text = "";
            txtPrecio.Text = "";
            dpFC.Text = "";
            dpFS.Text = "";

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if(gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtId.Text = row["IdJuego"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                ddlESBR.Text = row["ESBR"].ToString();
                txtGenero.Text = row["Genero"].ToString();
                txtConsola.Text = row["ConsolaPermitida"].ToString();
                txtJugadores.Text = row["CantidadJugadores"].ToString();
                ddlOnline.Text = row["Online"].ToString();
                txtEmpresa.Text = row["Empresa"].ToString();
                txtPrecio.Text = row["Precio"].ToString();
                dpFC.Text = row["FechaCreacion"].ToString();
                dpFS.Text = row["FechaSalida"].ToString();
                txtId.IsEnabled = false;
                btnAgregar.Content = "Actualizar.";
            }
            else
            {
                MessageBox.Show("Porfavor seleccionar cualqueir juego de la lista.");
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandText = "delete from Juegos where IdJuego=" + row["IdJuego"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Juego eliminado satisfactoriamente.");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Porfavor seleccionar un juego de la lista");
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
