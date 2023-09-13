using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Media;


namespace C02控制台23._9._13 {
    internal static class Program {
        static string 原神路径="";

        [STAThread]
        static void Main(string[] args) {
            //程序启动

            Console.WriteLine("欢迎使用白屏原神启动程序！");
            Console.WriteLine("制作人员：");
            Console.WriteLine("【沈伊利】");
            Console.WriteLine("【重生之为宿管查你应到】");
            Console.WriteLine("程序已启动！按下回车设置原神路径！");
            Console.ReadLine();
            原神路径 = 文件选择();
            Console.WriteLine("正在检测白屏……");

            //程序运行
            new Thread(() => {
                while (!白色像素超过阈值启动原神(原神路径)) Console.WriteLine("未检测到白屏，重新检测！");
            }).Start();

            //程序退出
            Console.WriteLine("输入exit以退出");
            if (Console.ReadLine() == "exit") Environment.Exit(0);

        }
        static bool 白色像素超过阈值启动原神(string 原神路径) {
            using (Bitmap screenShot = 截图系统.屏幕截图()) {
                if (screenShot.检测颜色比例(220, 220, 220, 0.9f)) {
                    程序启动系统.启动(原神路径);
                    播放音频();
                    Console.WriteLine("白屏检测成功！原神已启动！");
                    return true;
                }
            }
            return false;
        }
        [STAThread]
        static string 文件选择() {
            OpenFileDialog 文件窗口 = new OpenFileDialog();
            文件窗口.Title = "请选择原神的主程序";
            文件窗口.Filter = "可执行文件 (*.exe)|*.exe";
            if (文件窗口.ShowDialog() == DialogResult.OK) return 文件窗口.FileName;
            return null;
        }
        public static bool 检测颜色比例(this Bitmap 图像, int r, int g, int b, float 阈值) {
            int 所有像素 = 图像.Width * 图像.Height;
            int 白色像素 = 0;
            BitmapData imageData = null;
            try {
                imageData = 图像.LockBits(new Rectangle(0, 0, 图像.Width, 图像.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                unsafe {
                    byte* ptr = (byte*)imageData.Scan0;
                    for (int y = 0; y < 图像.Height; y++) {
                        for (int x = 0; x < 图像.Width; x++) {
                            int index = x * 4 + y * imageData.Stride;
                            int blue = ptr[index];
                            int green = ptr[index + 1];
                            int red = ptr[index + 2];
                            if (red > r && green > g && blue > b) {
                                白色像素++;
                            }
                        }
                    }
                }
            } finally {
                if (imageData != null) 图像.UnlockBits(imageData);
            }
            Console.WriteLine("白色比例" + (double)白色像素 / 所有像素);
            return (double)白色像素 / 所有像素 >= 阈值;
        }
        public static void 播放音频() {
            string namespaceName = Assembly.GetExecutingAssembly().GetName().Name.ToString();
            Assembly assembly = Assembly.GetExecutingAssembly();
            SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("C02控制台23._9._13.启动.wav"));
            sp.Play();
        }
        [DllImport("winmm.dll")]
        public static extern long PlaySound(String fileName, long a, long b);
    }

    public class 截图系统 {
        public static Bitmap 屏幕截图() {
            Rectangle 屏幕 = Screen.PrimaryScreen.Bounds;
            Bitmap 图像 = new Bitmap(屏幕.Width, 屏幕.Height);
            using (Graphics graphics = Graphics.FromImage(图像)) {
                graphics.CopyFromScreen(屏幕.Location, Point.Empty, 屏幕.Size);
            }
            return 图像;
        }
    }
    public class 程序启动系统 {
        public static void 启动(string 程序路径) {
            System.Diagnostics.Process.Start(程序路径);
        }
    }
}
