namespace Tron
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer timer; // Asegúrate de que solo haya una definición

        // En Form1.Designer.cs
        private void InitializeComponent()
        {
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 100; // Intervalo en milisegundos
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            this.timer.Start();

            // Otros componentes del formulario
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 600); // Ajusta el tamaño del formulario según sea necesario
            this.Name = "Form1";
            this.ResumeLayout(false);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
