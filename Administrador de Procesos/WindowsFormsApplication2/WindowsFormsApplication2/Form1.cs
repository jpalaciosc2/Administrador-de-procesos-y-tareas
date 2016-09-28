using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace WindowsFormsApplication2
{


    public partial class Form1 : Form
    {

        Color[] paleta = new Color[250];
        int noProcesos = 0;
        Random randonGen = new Random();
        int fila = 0, columna = 0, quantum, tiempoTotal, tiempoRes, sleepTime=1000;
        Thread hilo;

        public class listaProcesos
        {
            public int rafaga;
            public int tiempoRestante;
            public string nombeProceso;
            public listaProcesos sig;
            public Boolean terminado;
            public Color color;


            public listaProcesos(int r, int t, string n, Color c) {
                this.rafaga = r;
                this.tiempoRestante = t;
                this.nombeProceso = n;
                this.terminado = false;
                this.color = c;
            }
        }

        listaProcesos root = null;

        public Boolean buscar(string nombre) {
            listaProcesos proceso = root;
            while (proceso != null) {
                if (proceso.nombeProceso == nombre)
                    return true;
                proceso = proceso.sig;
            }
            return false;
        }

        public Boolean buscaTrabajo()
        {
            listaProcesos proceso = root;
            while (proceso != null)
            {
                if (!proceso.terminado)
                    return true;
                proceso = proceso.sig;
            }
            return false;
        }

        public void insertarProceso(listaProcesos raiz, listaProcesos nuevo) {
            if (root == null)
                root = nuevo;
            else {
                listaProcesos aux = root;
                while (aux.sig != null)
                    aux = aux.sig;
                aux.sig = nuevo;
            }
        }

        public void mostrarProcesos() {
            listaProcesos nodo = root;
            while (nodo != null) {
                MessageBox.Show(nodo.nombeProceso + "->" + nodo.rafaga + ",");
                nodo = nodo.sig;
            }
        }




        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 250; i++)
                paleta[i] = Color.FromArgb(randonGen.Next(255), randonGen.Next(255),
                randonGen.Next(255));
            for (int i = 0; i < 20; i++)
                tabla.Rows.Add("");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labelQuantum.Text = trackBar1.Value + "";
        }

        private void agregarbtn_MouseEnter(object sender, EventArgs e)
        {
            agregarbtn.BackColor = Color.FromArgb(238, 238, 238);
        }

        private void agregarbtn_MouseLeave(object sender, EventArgs e)
        {
            agregarbtn.BackColor = Color.White;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (nombreProcesoTxt.Text.Equals("Nombre de Proceso"))
                nombreProcesoTxt.Clear();
        }

        private void nombreProcesoTxt_Leave(object sender, EventArgs e)
        {
            if (nombreProcesoTxt.Text.Equals(""))
                nombreProcesoTxt.Text = "Nombre de Proceso";
        }

        private void textBox1_Enter_1(object sender, EventArgs e)
        {
            if (tiempoTxt.Text.Equals("Tiempo de Ejecución"))
                tiempoTxt.Clear();
        }

        private void tiempoTxt_Leave(object sender, EventArgs e)
        {
            if (tiempoTxt.Text.Equals(""))
                tiempoTxt.Text = "Tiempo de Ejecución";
        }

        private void agregarbtn_MouseDown(object sender, MouseEventArgs e)
        {
            agregarbtn.BackColor = Color.FromArgb(93, 81, 181);
            agregarbtn.ForeColor = Color.White;
        }

        private void agregarbtn_MouseUp(object sender, MouseEventArgs e)
        {
            agregarbtn.BackColor = Color.White;
            agregarbtn.ForeColor = Color.Black;
            


        }

        private void nombreProcesoTxt_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            TotalBar.Maximum = tiempoTotal;
            quantum = trackBar1.Value;
            hilo = new Thread(new ThreadStart(procesar));
            CheckForIllegalCrossThreadCalls = false;
            hilo.Start();
        }

        private void agregarbtn_Click(object sender, EventArgs e)
        {
            agregar();
        }

        private void procesarbtn_MouseDown_1(object sender, MouseEventArgs e)
        {
            procesarbtn.BackColor = Color.FromArgb(93, 81, 181);
            procesarbtn.ForeColor = Color.White;
        }

        private void procesarbtn_MouseUp_1(object sender, MouseEventArgs e)
        {
            procesarbtn.BackColor = Color.White;
            procesarbtn.ForeColor = Color.Black;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            hilo.Abort();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            panelAyuda.Visible = true;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            panelAyuda.Visible = false;
        }

        private void nombreProcesoTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                tiempoTxt.Focus();

        }

        private void procesarbtn_MouseEnter_1(object sender, EventArgs e)
        {
            procesarbtn.BackColor = Color.FromArgb(238, 238, 238);
        }

        private void tiempoTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                agregar();
        }

        private void timertxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                sleepTime = int.Parse(timertxt.Text);
                timertxt.Visible = false;
                timerlbl.Visible = false;
                nombreProcesoTxt.Focus();
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            
            if (!timertxt.Visible)
            {
                timertxt.Visible = true;
                timerlbl.Visible = true;
                timertxt.Focus();
            }
            else {
                timertxt.Visible = false;
                timerlbl.Visible = false;
                nombreProcesoTxt.Focus();
            }
        }

        private void procesarbtn_MouseLeave_1(object sender, EventArgs e)
        {
            procesarbtn.BackColor = Color.White;
        }
        
        public void procesar() {
            
            while (buscaTrabajo())
            {
                listaProcesos proceso = root;
                while (proceso != null)
                {
                    if (!proceso.terminado)
                        for (int i = 0; i < quantum; i++)
                        {
                            if (proceso.tiempoRestante > 0)
                            {
                                tiempoRes = tiempoRes - 1;
                                proceso.tiempoRestante = proceso.tiempoRestante - 1;
                                pintar(proceso, false); 
                            }
                            else if (proceso.tiempoRestante == 0) {
                                pintar(proceso, true);
                            }
                            if (proceso.tiempoRestante == 0) {
                                proceso.terminado = true;
                            }
                            int valor = 100 / proceso.rafaga;
                            int porcentaje = 100 - (valor * proceso.tiempoRestante);
                            progressBar.Value = porcentaje;
                            
                            TotalBar.Value = tiempoTotal - tiempoRes;

                            Thread.Sleep(sleepTime);
                        }
                    proceso = proceso.sig;
                }
            }
            tiempoTotal = 0;              
        }

        public void pintar(listaProcesos proceso, bool terminado) {
            if (!terminado)
            {
                tabla[columna, fila].Style.BackColor = proceso.color;
                tabla[columna, fila].Style.SelectionBackColor = proceso.color;
            }else
            {
                tabla[columna, fila].Style.BackColor = Color.Red;
                tabla[columna, fila].Style.SelectionBackColor = Color.Red;
            }
            procesolbl.Text = proceso.nombeProceso;
            tiempolbl.Text = proceso.tiempoRestante + "";
            columna++;
            if (columna == 31)
            {
                fila++;
                columna = 0;
            }
        }

        public void agregar() {

            Color color = paleta[noProcesos];
            insertarProceso(root,
                new listaProcesos(
                int.Parse(tiempoTxt.Text), int.Parse(tiempoTxt.Text), nombreProcesoTxt.Text, color));
            tablaProcesos.Rows.Add(nombreProcesoTxt.Text + " = " + tiempoTxt.Text);
            tablaProcesos[0, noProcesos].Style.BackColor = paleta[noProcesos];
            tablaProcesos[0, noProcesos].Style.SelectionBackColor = paleta[noProcesos];
            noProcesos++;
            tiempoTotal += int.Parse(tiempoTxt.Text);
            tiempoRes = tiempoTotal;
            tiempoTxt.Clear();
            nombreProcesoTxt.Clear();
            nombreProcesoTxt.Focus();
        }
    }

}
