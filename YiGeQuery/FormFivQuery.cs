﻿using FastReport;
using Mulaolao.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YiGeQuery
{
    public partial class FormFivQuery : Form
    {
        public FormFivQuery ( )
        {
            InitializeComponent( );
        }

        YiGeBll.Bll.VerBll _bll = new YiGeBll.Bll.VerBll( );
        YiGeEntiry.VersionEntity _model = new YiGeEntiry.VersionEntity( );
        DataTable tableOnly = new DataTable( ); DataTable tableOnlys = new DataTable( );
        DataTable print, prints;
        Report report = new Report( );
        DataSet RDataSet = new DataSet( ); EcanRMB rmb = new EcanRMB( );
        string strWhere = "", file = "", signOf = "";
        public string sign = "";
        private void FormFivQuery_Load ( object sender ,EventArgs e )
        {
            lookUpEdit1.EditValue = lookUpEdit2.EditValue = lookUpEdit3.EditValue = null;
            lookUpEdit1.ItemIndex = lookUpEdit2.ItemIndex = lookUpEdit3.ItemIndex = -1;

            tableOnly = tableOnlys = null;
            textBox1.Text = DateTime.Now.ToString( "yyyy年MM月dd日" );
            dateTimePicker1.Value = DateTime.Now;

            DataTable tableOne = new DataTable( );
            if ( tableOne != null && tableOne.Rows.Count > 0 )
                return;
            tableOne = new DataTable( "data" );
            tableOne.Columns.Add( "SH" ,Type.GetType( "System.String" ) );
            tableOne.Rows.Add( "" );
            tableOne.Rows.Add( "Y" );
            tableOne.Rows.Add( "N" );
            lookUpEdit3.Properties.DataSource = tableOne;
            lookUpEdit3.Properties.DisplayMember = "SH";
        }

        void createRDataSet ( string signOfName ,string strWhere )
        {
            RDataSet = new DataSet( );
            print = _bll.GetDataTablePrint( strWhere ,signOfName );
            if ( print == null )
            {
                MessageBox.Show( "" + signOfName + "模板数据源为空" );
                signOf = "1";
                return;
            }
            /*
            prints = _bll.GetDataTableTotal( strWhere ,signOfName );
            if ( prints != null && prints.Rows.Count > 0 )
            {
                report.SetParameterValue( "小写合计" ,prints.Rows[0]["小写合计"].ToString( ) );
                if ( !string.IsNullOrEmpty( prints.Rows[0]["小写合计"].ToString( ) ) )
                    report.SetParameterValue( "小写合计大写" ,rmb.CmycurD( Convert.ToDecimal( prints.Rows[0]["小写合计"].ToString( ) ) ) );
                prints.TableName = "COPTC";
                RDataSet.Tables.Add( prints );
            }
            */
            print.TableName = "COPMA";
            RDataSet.Tables.Add( print );
        }

        //Print
        private void button1_Click ( object sender ,EventArgs e )
        {
            if ( lookUpEdit2.EditValue == null )
            {
                MessageBox.Show( "退货单单别、单号不可为空" );
                return;
            }
            //if ( lookUpEdit3.EditValue == null )
            //{
            //    MessageBox.Show( "审核码不可为空" );
            //    return;
            //}


            strWhere = "1=1";
            if ( lookUpEdit1.Text != null && lookUpEdit1.Text != "==请选择==" )
            {
                _model.TJ016 = lookUpEdit1.Text.Split( '-' )[0].Trim( );
                _model.TJ017 = lookUpEdit1.Text.Split( '-' )[1].Trim( );
                strWhere = strWhere + "AND TJ016='" + _model.TJ016 + "' AND TJ017='" + _model.TJ017 + "'";
            }
            
            if ( string.IsNullOrEmpty( textBox1.Text ) )
                _model.TI003 = DateTime.Now.ToString( "yyyyMMdd" );
            else
                _model.TI003 = dateTimePicker1.Value.ToString( "yyyyMMdd" );
            _model.TI001 = lookUpEdit2.Text.Split( '-' )[0].Trim( );
            _model.TI002 = lookUpEdit2.Text.Split( '-' )[1].Trim( );

            strWhere = strWhere + " AND TI001='" + _model.TI001 + "' AND TI002='" + _model.TI002 + "' AND TI003='" + _model.TI003 + "'";

            if ( lookUpEdit3.EditValue != null && lookUpEdit3.Text != "" )
            {
                _model.TJ020 = lookUpEdit3.Text;
                strWhere = strWhere + " AND TJ020='" + _model.TJ020 + "'";
            }

            file = "";
            file = Application.StartupPath + "\\退货单.frx";
            report.Load( file );
            createRDataSet( "退货单" ,strWhere );
            if ( signOf == "1" )
                return;
            report.RegisterData( RDataSet );
            report.Show( );
        }
        //Cancel
        private void button2_Click ( object sender ,EventArgs e )
        {
            this.Close( );
        }
        //Clear
        private void button3_Click ( object sender ,EventArgs e )
        {
            lookUpEdit1.EditValue = lookUpEdit2.EditValue =lookUpEdit3.EditValue= null;
            lookUpEdit1.ItemIndex = lookUpEdit2.ItemIndex =lookUpEdit3.ItemIndex= -1;
            textBox1.Text = DateTime.Now.ToString( "yyyy年MM月dd日" );
            dateTimePicker1.Value = DateTime.Now;

            tableOnly = tableOnlys = null;
        }

        private void dateTimePicker1_ValueChanged ( object sender ,EventArgs e )
        {
            query( );
        }
        private void lookUpEdit3_EditValueChanged ( object sender ,EventArgs e )
        {
            query( );
        }
        void query ( )
        {
            textBox1.Text = dateTimePicker1.Value.ToString( "yyyy年MM月dd日" );
            lookUpEdit1.EditValue = lookUpEdit2.EditValue = null;
            lookUpEdit1.ItemIndex = lookUpEdit2.ItemIndex = -1;
            if ( string.IsNullOrEmpty( textBox1.Text ) )
                _model.TI003 = DateTime.Now.ToString( "yyyyMMdd" );
            else
                _model.TI003 = dateTimePicker1.Value.ToString( "yyyyMMdd" );
            tableOnly = null;

            //if ( lookUpEdit3.EditValue == null )
            //    return;

            _model.TJ020 = lookUpEdit3.Text;

            tableOnly = _bll.GetDataTableOnlyEle( _model.TI003 ,_model.TJ020 );
            lookUpEdit2.Properties.DataSource = tableOnly;
            lookUpEdit2.Properties.DisplayMember = "TC";
        }
        private void textBox1_KeyPress ( object sender ,KeyPressEventArgs e )
        {
            if ( e.KeyChar == 8 )
                textBox1.Text = "";
            else
                e.Handled = true;
        }
        private void lookUpEdit2_EditValueChanged ( object sender ,EventArgs e )
        {
            if ( lookUpEdit2.EditValue != null )
            {
                string[] str = lookUpEdit2.Text.Split( '-' );
                _model.TI001 = str[0].Trim( );
                _model.TI002 = str[1].Trim( );
                if ( string.IsNullOrEmpty( textBox1.Text ) )
                    _model.TI003 = DateTime.Now.ToString( "yyyyMMdd" );
                else
                    _model.TI003 = dateTimePicker1.Value.ToString( "yyyyMMdd" );
                tableOnlys = null;

                tableOnlys = _bll.GetDataTableOnlyEles( _model.TI001 ,_model.TI002 );
                lookUpEdit1.Properties.DataSource = tableOnlys;
                lookUpEdit1.Properties.DisplayMember = "TC";
            }
        }
    }
}
