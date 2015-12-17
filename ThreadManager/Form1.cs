using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ThreadManager
{
    public partial class Form1 : Form
    {
        internal Random mRandom = new Random();
        List<Thread> threads = new List<Thread>();
        PictureBox[] pictures;
        public Form1()
        {
            InitializeComponent();
            pictures = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            for (int i = 0; i < 6; i++)
            {
                Thread thread = new Thread(someTask);
                thread.IsBackground = true;
                thread.Start(i);
                threads.Add(thread);
            };
            Thread manager = new Thread(manageTask);
            manager.IsBackground = true;
            manager.Start();
        }

        private void manageTask()
        {
            while (true)
            {
                for (int i = 0; i < threads.Count; i++)
                    if (!threads[i].IsAlive)
                    {
                        changePicture(this, new object[] { pictures[(int)i], global::ThreadManager.Properties.Resources.suspended });
                        Thread.Sleep(100);
                        threads[i] = new Thread(someTask);
                        
                        threads[i].Start(i);
                    }
                Thread.Sleep(300);
            }
        }

        private void someTask(object id)
        {
            changePicture(this, new object[] { pictures[(int)id], global::ThreadManager.Properties.Resources.working });
            Thread.Sleep(mRandom.Next(500, 3000));
            changePicture(this, new object[] { pictures[(int)id], global::ThreadManager.Properties.Resources.stopped });
        }

        public void changePicture(object sender, object[] objects)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, object[]>(changePicture), sender, objects);
                return;
            }

            ((PictureBox)objects[0]).Image = (Bitmap)objects[1];
        }
    }
}
