﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capanegocio;

namespace Capapresentacion
{
    public partial class frmusuario : Form
    {
        public frmusuario()
        {
            InitializeComponent();
            this.ttmensaje.SetToolTip(this.txtnombreusuario, "Ingrese el Nombre del Cliente");
            this.ttmensaje.SetToolTip(this.txtapellidosusuario, "Ingrese Los Apellidos del Cliente");
            this.ttmensaje.SetToolTip(this.txtnumdocumento, "Ingrese el Documento del Cliente");
            this.ttmensaje.SetToolTip(this.txtdireccion, "Ingrese la Dirección del Cliente");

            
        }

        private bool IsNuevo = false;
        private bool IsEditar = false;

        private void MensajeOK(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Sistema Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //Para mostrar mensaje de error
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Sistema Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Limpia los controles del formulario
        private void Limpiar()
        {
            this.txtidusuario.Text = string.Empty;
            this.txtnombreusuario.Text = string.Empty;
            this.txtapellidosusuario.Text = string.Empty;
            this.txtnumdocumento.Text = string.Empty;
            this.txtdireccion.Text = string.Empty;
            this.txttelefono.Text = string.Empty;
            this.txtemailusuario.Text = string.Empty;
            this.txtnombredeusuario.Text = string.Empty;
            this.txtcontrasena.Text = string.Empty;

        }
        //Habilita los controles de los formularios
        private void Habilitar(bool Valor)
        {
            this.txtidusuario.ReadOnly = true;
            this.txtnombreusuario.ReadOnly = !Valor;
            this.txtdireccion.ReadOnly = !Valor;
            this.cmbsexo.Enabled = Valor;
            this.dtfechanacimiento.Enabled = Valor;
            this.txtnumdocumento.Enabled = Valor;
            this.txtdireccion.ReadOnly = !Valor;
            this.txttelefono.ReadOnly = !Valor;
            this.txtemailusuario.ReadOnly = !Valor;            
            this.txtnombredeusuario.ReadOnly = !Valor;
            this.txtcontrasena.ReadOnly = !Valor;
        }
        //Habilita los botones
        private void Botones()
        {
            if (this.IsNuevo || this.IsEditar)
            {
                this.Habilitar(true);
                this.btnnuevo.Enabled = false;
                this.btnguardar.Enabled = true;
                this.btneditar.Enabled = false;
                this.btncancelar.Enabled = true;
            }
            else
            {
                this.Habilitar(false);
                this.btnnuevo.Enabled = true;
                this.btnguardar.Enabled = false;
                this.btneditar.Enabled = true;
                this.btncancelar.Enabled = false;
            }
        }
        private void OcultarColumnas()
        {
            this.datagridusuario.Columns[0].Visible = false;
            this.datagridusuario.Columns[1].Visible = false;
        }
        private void Mostrar()
        {
            this.datagridusuario.DataSource = Nusuario.Mostrar();
            this.OcultarColumnas();
            lblTotal.Text = "Total Registros: " + Convert.ToString(datagridusuario.Rows.Count);
        }
        private void BuscarApellidos()
        {
            this.datagridusuario.DataSource = Nusuario.BuscarApellidos(this.txtbuscar.Text);
            this.OcultarColumnas();
            lblTotal.Text = "Total Registros: " + Convert.ToString(datagridusuario.Rows.Count);
        }

        private void BuscarNum_Documento()
        {
            this.datagridusuario.DataSource = Nusuario.BuscarNum_Documento(this.txtbuscar.Text);
            this.OcultarColumnas();
            lblTotal.Text = "Total Registros: " + Convert.ToString(datagridusuario.Rows.Count);
        }

        private void Btnbuscar_Click(object sender, EventArgs e)
        {
            if (Cmbbuscar.Text.Equals("Apellidos"))
            {
                this.BuscarApellidos();
            }
            else if (Cmbbuscar.Text.Equals("Documento"))
            {
                this.BuscarNum_Documento();
            }
        }

        private void Btneliminar_Click(object sender, EventArgs e)
        {
            try 
            {              
                DialogResult Opcion;
                if (chkeliminar.Checked)
                {
                    
                    Opcion = MessageBox.Show("Realmente Desea Eliminar los Registros", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (Opcion == DialogResult.OK)
                    {
                        string Codigo;
                        string Rpta = "";

                        foreach (DataGridViewRow row in datagridusuario.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells[0].Value))
                            {
                                Codigo = Convert.ToString(row.Cells[1].Value);
                                Rpta = Nusuario.Eliminar(Convert.ToInt32(Codigo));

                                if (Rpta.Equals("OK"))
                                {
                                    this.MensajeOK("Se Eliminó Correctamente el registro");
                                }
                                else
                                {
                                    this.MensajeError(Rpta);
                                }

                            }
                        }
                        this.Mostrar();
                    }
                }
                else
                {
                    MessageBox.Show("debe seleccionar una fila para eliminar");
                }
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Btnnuevo_Click(object sender, EventArgs e)
        {
            this.IsNuevo = true;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(true);
            this.txtnombreusuario.Focus();
        }

        private void Btnguardar_Click(object sender, EventArgs e)
        {
            try
            {

                //La variable que almacena si se inserto 
                //o se modifico la tabla
                string Rpta = "";
                if (this.txtnombreusuario.Text == string.Empty || this.txtapellidosusuario.Text == string.Empty || txtnumdocumento.Text == string.Empty || txtnombredeusuario.Text == string.Empty || txtcontrasena.Text == string.Empty)
                {
                    MensajeError("Falta ingresar algunos datos, serán remarcados");
                    erroricono.SetError(txtnombreusuario, "Ingrese un Valor");
                    erroricono.SetError(txtapellidosusuario, "Ingrese un Valor");
                    erroricono.SetError(txtnumdocumento, "Ingrese un Valor");
                    erroricono.SetError(txtnombredeusuario, "Ingrese un Valor");
                    erroricono.SetError(txtcontrasena, "Ingrese un Valor");
                }
                else
                {
                    if (this.IsNuevo)
                    {
                        //Vamos a Insertar un Trabajador 
                        Rpta = Nusuario.Insertar(this.txtnombreusuario.Text.Trim().ToUpper(),
                        this.txtapellidosusuario.Text.Trim().ToUpper(), cmbsexo.Text,
                        dtfechanacimiento.Value,
                        txtnumdocumento.Text, txtdireccion.Text,
                        txttelefono.Text, txtemailusuario.Text, txtnombredeusuario.Text,txtcontrasena.Text);

                    }
                    else
                    {
                        //Vamos a modificar un Trabajador
                        Rpta = Nusuario.Editar(Convert.ToInt32(this.txtidusuario.Text), this.txtnombreusuario.Text.Trim().ToUpper(),
                        this.txtapellidosusuario.Text.Trim().ToUpper(), cmbsexo.Text,
                        dtfechanacimiento.Value,
                        txtnumdocumento.Text, txtdireccion.Text,
                        txttelefono.Text, txtemailusuario.Text, txtnombredeusuario.Text, txtcontrasena.Text);
                    }
                    //Si la respuesta fue OK, fue porque se modifico 
                    //o inserto el Cliente
                    //de forma correcta
                    if (Rpta.Equals("OK"))
                    {
                        if (this.IsNuevo)
                        {
                            this.MensajeOK("Se insertó de forma correcta el registro");
                        }
                        else
                        {
                            this.MensajeOK("Se actualizó de forma correcta el registro");
                        }

                    }
                    else
                    {
                        //Mostramos el mensaje de error
                        this.MensajeError(Rpta);
                    }
                    this.IsNuevo = false;
                    this.IsEditar = false;
                    this.Botones();
                    this.Limpiar();
                    this.Mostrar();
                    this.txtidusuario.Text = "";

                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Btneditar_Click(object sender, EventArgs e)
        {
            if (!this.txtidusuario.Text.Equals(""))
            {
                this.IsEditar = true;
                this.Botones();
            }
            else
            {
                this.MensajeError("Debe de buscar un registro para Modificar");
            }
        }

        private void Btncancelar_Click(object sender, EventArgs e)
        {

            this.IsNuevo = false;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.txtidusuario.Text = string.Empty;
        }

        private void Datagridusuario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == datagridusuario.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell ChkEliminar =
                    (DataGridViewCheckBoxCell)datagridusuario.Rows[e.RowIndex].Cells["Eliminar"];
                ChkEliminar.Value = !Convert.ToBoolean(ChkEliminar.Value);
            }
        }

        private void Datagridusuario_DoubleClick(object sender, EventArgs e)
        {
            this.txtidusuario.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Idusuario"].Value);
            this.txtnombreusuario.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Nombre"].Value);
            this.txtapellidosusuario.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Apellido"].Value);
            this.cmbsexo.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Sexo"].Value);
            this.dtfechanacimiento.Value = Convert.ToDateTime(this.datagridusuario.CurrentRow.Cells["fecha_nac"].Value);
            this.txtnumdocumento.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["num_doc"].Value);
            this.txtdireccion.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Direccion"].Value);
            this.txttelefono.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Telefono"].Value);
            this.txtemailusuario.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Email"].Value);
            this.txtnombredeusuario.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["Usuario"].Value);
            this.txtcontrasena.Text = Convert.ToString(this.datagridusuario.CurrentRow.Cells["claveusu"].Value);


            this.tabControl1.SelectedIndex = 1;
        }

        private void Chkeliminar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkeliminar.Checked)
            {
                this.datagridusuario.Columns[0].Visible = true;
            }
            else
            {
                this.datagridusuario.Columns[0].Visible = false;
            }
        }

        private void Frmusuario_Load(object sender, EventArgs e)
        {
            this.Mostrar();
            this.Habilitar(false);
            this.Botones();
        }
    }
}
