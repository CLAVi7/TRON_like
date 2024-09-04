namespace Tron
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer timer; // Asegúrate de que solo haya una definición

        private void InitializeComponent()
        {
            
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 100; // Intervalo en milisegundos
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);

            // Otros componentes del formulario
            this.SuspendLayout();
            
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
