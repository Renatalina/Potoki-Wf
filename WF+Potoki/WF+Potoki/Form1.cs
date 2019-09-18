using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_Potoki
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        //private void ProcessFiles()
        //{
        //    // Загрузить все файлы *.jpg и создать новую папку для модифицированных данных, 
        //    string[] files = Directory.GetFiles("Cities", "*.jpg", SearchOption.AllDirectories);
        //    string newDir = "ModifiedPictures";
        //    Directory.CreateDirectory(newDir);

        //    // Обработка графических данных в блокирующей манере. 
        //    foreach (string currentFile in files)
        //    {
        //        string filename = Path.GetFileName(currentFile);
        //        using (Bitmap bitmap = new Bitmap(currentFile))
        //        {
        //            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            Thread.Sleep(1000);
        //            bitmap.Save(Path.Combine(newDir, filename));
        //            this.Text = $"Загрузка {filename} {Thread.CurrentThread.ManagedThreadId} потоках";
        //        }
        //    }

        //    this.Text = "Загрузка завершена!";
        //}


        //private void ProcessFiles()
        //{
        //    // Загрузить все файлы *.jpg и создать новую папку для модифицированных данных, 
        //    string[] files = Directory.GetFiles("Cities", "*.jpg", SearchOption.AllDirectories);
        //    string newDir = "ModifiedPictures";
        //    Directory.CreateDirectory(newDir);

        //    // Обработка графических данных в параллельном режиме! 
        //    Parallel.ForEach(files, currentFile =>
        //    {
        //        string filename = Path.GetFileName(currentFile);
        //        using (Bitmap bitmap = new Bitmap(currentFile))
        //        {
        //            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            Thread.Sleep(1000);
        //            bitmap.Save(Path.Combine(newDir, filename));
        //            // Error - попытка доступа к форме из другого потока
        //            this.Text = string.Format("Загрузка {0} в {1} потоке.", filename, Thread.CurrentThread.ManagedThreadId);
        //        }
        //    }
        //    );

        //    this.Text = "Загрузка завершена!";
        //}

        private CancellationTokenSource cancellation = new CancellationTokenSource();
        private void btStart_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => ProcessFiles());

            //ProcessFiles();
        }
        private void ProcessFiles()
        {
            // Использовать экземпляр ParallelOptions для сохранения CancellationToken. 
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = cancellation.Token;
            parOpts.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            // Загрузить все файлы *.jpg и создать новую папку для модифицированных данных. 
            string[] files = Directory.GetFiles("Cities", "*.jpg", SearchOption.AllDirectories);
            string newDir = "ModifledPictures";
            Directory.CreateDirectory(newDir);
            try
            {
                Parallel.ForEach(files, parOpts, currentFile =>
                {
                    parOpts.CancellationToken.ThrowIfCancellationRequested();
                    string filename = Path.GetFileName(currentFile);
                    using (Bitmap bitmap = new Bitmap(currentFile))
                    {
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        Thread.Sleep(3000);
                        bitmap.Save(Path.Combine(newDir, filename));
                        MessageBox.Show(string.Format("Загрузка {0} в {1} потоке.", filename, Thread.CurrentThread.ManagedThreadId));
                        //this.Text = string.Format("Загрузка {0} в {1} потоке.", filename, Thread.CurrentThread.ManagedThreadId);
                    }
                }
            );
                MessageBox.Show("Загрузка завершена!");
                //this.Text = "Загрузка завершена!";
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message);
                //this.Text = ex.Message;
            }
        }

        private void btEnd_Click(object sender, EventArgs e)
        {
            cancellation.Cancel();
        }
    }
}
