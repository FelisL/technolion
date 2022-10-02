using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
using Path = System.IO.Path;

namespace ETones
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      

        ImageBrush imageBrush = new ImageBrush();
        ImageBrush imageBrush2 = new ImageBrush();
        public MainWindow()
        {
            InitializeComponent();
            imageBrush = new ImageBrush();
            /*
             imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/technolion-transparent.png"));
             imageBrush.Stretch = Stretch.UniformToFill;
             picture_technolion.Fill = imageBrush;*/

            BitmapImage bitmapImage = new BitmapImage();

            
            //Rectangle_Before.Fill = imageBrush;

            Initial_PitchParameter();

            Savepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Export_Gif_Media_Mozart.Source = new Uri("pack://application:,,,/Resources/Mozart2.gif");

            /* show_animated = true;

             Loading_Thread = new Thread(Loading_Animation);
             Loading_Thread.Start();*/
            // LoadBeforeImage("https://eoimages.gsfc.nasa.gov/images/imagerecords/150000/150267/greenland_ms1_1973245_lrg.jpg");




            //LoadAfterImage("https://eoimages.gsfc.nasa.gov/images/imagerecords/147000/147780/fuji_oli_2021001_lrg.jpg");
            //LoadBeforeImage("https://eoimages.gsfc.nasa.gov/images/imagerecords/147000/147780/fuji_oli_2013363_lrg.jpg");

            //White_Music("Test","C",0);

        }
        Thread Loading_Thread = null;

        private void LoadBeforeImage(string Before)
        {
            Uri uri = new Uri(Before);
            HttpClient f = new HttpClient();
            var g = f.GetStreamAsync(uri);
            
            Image f1 = Image.FromStream(g.Result);
            LoadBitmap_Before = (Bitmap)f1;

           
            BitmapImage bitmapImage = new BitmapImage(uri);
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = bitmapImage;
            imageBrush.Stretch = Stretch.UniformToFill;
            //picture_technolion.Fill = imageBrush;
            BitmapSource bmp = (BitmapSource)imageBrush.ImageSource;
            
            //LoadBitmap_Before = GetBitmap(bitmapImage);
            
            Rectangle_Before.Fill = imageBrush;
        }
        private void LoadAfterImage(string After)
        {
            Uri uri = new Uri(After);
            HttpClient f = new HttpClient();
            var g = f.GetStreamAsync(uri);

            Image f1 = Image.FromStream(g.Result);
            LoadBitmap_After = (Bitmap)f1;
            

            BitmapImage bitmapImage = new BitmapImage(uri);
            imageBrush2 = new ImageBrush();
            imageBrush2.ImageSource = bitmapImage;
            imageBrush2.Stretch = Stretch.UniformToFill;
            //picture_technolion.Fill = imageBrush;
            BitmapSource bmp = (BitmapSource)imageBrush2.ImageSource;

            //LoadBitmap_After = GetBitmap(bitmapImage);

            Rectangle_After.Fill = imageBrush2;
        }




        /// <summary>
        /// storage loation
        /// </summary>
        string Savepath = "";

        /* music's parameter */
        class Musician
        {
            public string original_content = "";
            public string trans_content = "";
            public string[] original_content_parts=new string[0];


            public float[] trans_RedData_Before = new float[0];
            public float[] trans_GreenData_Before = new float[0];
            public float[] trans_BlueData_Before = new float[0];

            public float[] trans_RedData_After = new float[0];
            public float[] trans_GreenData_After = new float[0];
            public float[] trans_BlueData_After = new float[0];



            public int[] index_vertical = new int[0];
          

            public int[] index_horizontal = new int[0];
         

            public int[] index_clockwise = new int[0];
          

            public int[] index_counterclockwise = new int[0];


            public Dictionary<int,string> Pitch_List_Step=new Dictionary<int, string>();
            public Dictionary<int, string> Pitch_List_Alter = new Dictionary<int, string>();
            public Dictionary<int, string> Pitch_List_Octave = new Dictionary<int, string>();
            public Dictionary<int, string> Pitch_List_Accidental = new Dictionary<int, string>();

            public double[] deltaRGB = new double[0];
        }


        Musician White_ = new Musician();
        Musician Asian = new Musician();
        Musician Africa = new Musician();

      

        Bitmap LoadBitmap_Before=null;
        Bitmap LoadBitmap_After = null;

        private Bitmap TransBitmapFromBbitmapSource(BitmapSource bc)
        {
            int width = bc.PixelHeight;
            int height = bc.PixelWidth;
            int stride = width * ((bc.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                bc.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using(var btm = new Bitmap(width, height,stride,System.Drawing.Imaging.PixelFormat.Format1bppIndexed,ptr))
                {
                    return new Bitmap(btm); 
                }

            }
            finally
            {
                if(ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }



        }

        /// <summary>
        /// Load Music Prototype , Pich List , Image Process type
        /// </summary>
        private void Initial_PitchParameter()
        {
            /** Load music **/
           
            string[] split1 = new string[] { "</note>" };

            //Taiwan.original_content =Encoding.UTF8.GetString( Properties.Resources.ClapSong_final);
            //Taiwan.original_content_parts = Taiwan.original_content.Split(split1, StringSplitOptions.RemoveEmptyEntries);


            //White_.original_content = Encoding.UTF8.GetString(Properties.Resources.Sakura_final);
            // White_.original_content_parts = White_.original_content.Split(split1, StringSplitOptions.RemoveEmptyEntries);

            string[] split5 = new string[] { "<part id=\"P4\">", "<part id=\"P2\">", "<part id=\"P3\">", "<part id=\"P1\">", "<part id=\"P5\">", "<part id=\"P7\">", "<part id=\"P6\">" };
            White_.original_content = Encoding.UTF8.GetString(Properties.Resources.Mack_the_Knife_SATB);
            White_.original_content_parts = White_.original_content.Split(split5, StringSplitOptions.RemoveEmptyEntries);

            White_.Pitch_List_Step[0] = "D"; White_.Pitch_List_Alter[0] = ""; White_.Pitch_List_Octave[0] = "4"; White_.Pitch_List_Accidental[0] = "";
            White_.Pitch_List_Step[1] = "E"; White_.Pitch_List_Alter[1] = ""; White_.Pitch_List_Octave[1] = "4"; White_.Pitch_List_Accidental[1] = "";
            White_.Pitch_List_Step[2] = "G"; White_.Pitch_List_Alter[2] = ""; White_.Pitch_List_Octave[2] = "4"; White_.Pitch_List_Accidental[2] = "";
            White_.Pitch_List_Step[3] = "A"; White_.Pitch_List_Alter[3] = ""; White_.Pitch_List_Octave[3] = "4"; White_.Pitch_List_Accidental[3] = "";
            White_.Pitch_List_Step[4] = "B"; White_.Pitch_List_Alter[4] = ""; White_.Pitch_List_Octave[4] = "4"; White_.Pitch_List_Accidental[4] = "";
            White_.Pitch_List_Step[5] = "C"; White_.Pitch_List_Alter[5] = ""; White_.Pitch_List_Octave[5] = "5"; White_.Pitch_List_Accidental[5] = "";
            White_.Pitch_List_Step[6] = "D"; White_.Pitch_List_Alter[6] = ""; White_.Pitch_List_Octave[6] = "5"; White_.Pitch_List_Accidental[6] = "";
            White_.Pitch_List_Step[7] = "E"; White_.Pitch_List_Alter[7] = ""; White_.Pitch_List_Octave[7] = "5"; White_.Pitch_List_Accidental[7] = "";
            White_.Pitch_List_Step[8] = "G"; White_.Pitch_List_Alter[8] = ""; White_.Pitch_List_Octave[8] = "5"; White_.Pitch_List_Accidental[8] = "";
            White_.Pitch_List_Step[9] = "A"; White_.Pitch_List_Alter[9] = ""; White_.Pitch_List_Octave[9] = "5"; White_.Pitch_List_Accidental[9] = "";
            White_.Pitch_List_Step[10] = "B"; White_.Pitch_List_Alter[10] = ""; White_.Pitch_List_Octave[10] = "5"; White_.Pitch_List_Accidental[10] = "";
            White_.Pitch_List_Step[11] = "F"; White_.Pitch_List_Alter[11] = "1"; White_.Pitch_List_Octave[11] = "4"; White_.Pitch_List_Accidental[11] = "sharp";
            White_.Pitch_List_Step[12] = "C"; White_.Pitch_List_Alter[12] = "1"; White_.Pitch_List_Octave[12] = "5"; White_.Pitch_List_Accidental[12] = "sharp";
            White_.Pitch_List_Step[13] = "F"; White_.Pitch_List_Alter[13] = "1"; White_.Pitch_List_Octave[13] = "5"; White_.Pitch_List_Accidental[13] = "sharp";



            Asian.original_content = Encoding.UTF8.GetString(Properties.Resources.clapping_song_actual_final);
            Asian.original_content_parts = White_.original_content.Split(split5, StringSplitOptions.RemoveEmptyEntries);

            Asian.Pitch_List_Step[0] = "E"; Asian.Pitch_List_Alter[0] = ""; Asian.Pitch_List_Octave[0] = "4"; Asian.Pitch_List_Accidental[0] = "";
            Asian.Pitch_List_Step[1] = "F"; Asian.Pitch_List_Alter[1] = ""; Asian.Pitch_List_Octave[1] = "4"; Asian.Pitch_List_Accidental[1] = "";
            Asian.Pitch_List_Step[2] = "G"; Asian.Pitch_List_Alter[2] = ""; Asian.Pitch_List_Octave[2] = "4"; Asian.Pitch_List_Accidental[2] = "";
            Asian.Pitch_List_Step[3] = "A"; Asian.Pitch_List_Alter[3] = ""; Asian.Pitch_List_Octave[3] = "4"; Asian.Pitch_List_Accidental[3] = "";
            Asian.Pitch_List_Step[4] = "B"; Asian.Pitch_List_Alter[4] = ""; Asian.Pitch_List_Octave[4] = "4"; Asian.Pitch_List_Accidental[4] = "";
            Asian.Pitch_List_Step[5] = "C"; Asian.Pitch_List_Alter[5] = ""; Asian.Pitch_List_Octave[5] = "5"; Asian.Pitch_List_Accidental[5] = "";
            Asian.Pitch_List_Step[6] = "D"; Asian.Pitch_List_Alter[6] = ""; Asian.Pitch_List_Octave[6] = "5"; Asian.Pitch_List_Accidental[6] = "";
            Asian.Pitch_List_Step[7] = "E"; Asian.Pitch_List_Alter[7] = ""; Asian.Pitch_List_Octave[7] = "5"; Asian.Pitch_List_Accidental[7] = "";
            Asian.Pitch_List_Step[8] = "F"; Asian.Pitch_List_Alter[8] = ""; Asian.Pitch_List_Octave[8] = "5"; Asian.Pitch_List_Accidental[8] = "";
            Asian.Pitch_List_Step[9] = "G"; Asian.Pitch_List_Alter[9] = ""; Asian.Pitch_List_Octave[9] = "5"; Asian.Pitch_List_Accidental[9] = "";
            Asian.Pitch_List_Step[10] = "A"; Asian.Pitch_List_Alter[10] = ""; Asian.Pitch_List_Octave[10] = "5"; Asian.Pitch_List_Accidental[10] = "";


            Africa.original_content = Encoding.UTF8.GetString(Properties.Resources.Mack_the_Knife_SATB);
            Africa.original_content_parts = White_.original_content.Split(split5, StringSplitOptions.RemoveEmptyEntries);


            Africa.Pitch_List_Step[0] = "D"; Africa.Pitch_List_Alter[0] = ""; Africa.Pitch_List_Octave[0] = "4"; Africa.Pitch_List_Accidental[0] = "";
            Africa.Pitch_List_Step[1] = "E"; Africa.Pitch_List_Alter[1] = ""; Africa.Pitch_List_Octave[1] = "4"; Africa.Pitch_List_Accidental[1] = "";
            Africa.Pitch_List_Step[2] = "G"; Africa.Pitch_List_Alter[2] = ""; Africa.Pitch_List_Octave[2] = "4"; Africa.Pitch_List_Accidental[2] = "";
            Africa.Pitch_List_Step[3] = "A"; Africa.Pitch_List_Alter[3] = ""; Africa.Pitch_List_Octave[3] = "4"; Africa.Pitch_List_Accidental[3] = "";
            Africa.Pitch_List_Step[4] = "B"; Africa.Pitch_List_Alter[4] = ""; Africa.Pitch_List_Octave[4] = "4"; Africa.Pitch_List_Accidental[4] = "";
            Africa.Pitch_List_Step[5] = "C"; Africa.Pitch_List_Alter[5] = ""; Africa.Pitch_List_Octave[5] = "5"; Africa.Pitch_List_Accidental[5] = "";
            Africa.Pitch_List_Step[6] = "D"; Africa.Pitch_List_Alter[6] = ""; Africa.Pitch_List_Octave[6] = "5"; Africa.Pitch_List_Accidental[6] = "";
            Africa.Pitch_List_Step[7] = "E"; Africa.Pitch_List_Alter[7] = ""; Africa.Pitch_List_Octave[7] = "5"; Africa.Pitch_List_Accidental[7] = "";
            Africa.Pitch_List_Step[8] = "G"; Africa.Pitch_List_Alter[8] = ""; Africa.Pitch_List_Octave[8] = "5"; Africa.Pitch_List_Accidental[8] = "";
            Africa.Pitch_List_Step[9] = "A"; Africa.Pitch_List_Alter[9] = ""; Africa.Pitch_List_Octave[9] = "5"; Africa.Pitch_List_Accidental[9] = "";
            Africa.Pitch_List_Step[10] = "B"; Africa.Pitch_List_Alter[10] = ""; Africa.Pitch_List_Octave[10] = "5"; Africa.Pitch_List_Accidental[10] = "";
            Africa.Pitch_List_Step[11] = "F"; Africa.Pitch_List_Alter[11] = "1"; Africa.Pitch_List_Octave[11] = "4"; Africa.Pitch_List_Accidental[11] = "sharp";
            Africa.Pitch_List_Step[12] = "C"; Africa.Pitch_List_Alter[12] = "1"; Africa.Pitch_List_Octave[12] = "5"; Africa.Pitch_List_Accidental[12] = "sharp";
            Africa.Pitch_List_Step[13] = "F"; Africa.Pitch_List_Alter[13] = "1"; Africa.Pitch_List_Octave[13] = "5"; Africa.Pitch_List_Accidental[13] = "sharp";


            string[] split2 = new string[] { "\r\n" };
            string[] split3 = new string[] { "," };
            string[] contents;
            string[] contents_part;
            string[] split4 = new string[] { "\n" };

            ////contents = pitch_Bach.Split(split2, StringSplitOptions.RemoveEmptyEntries);
            //string[] temp_string; string[] temp_string2;
            //int index_ = 0;
            //int index_instrument = 0;
            //for (int i = 0; i < White_.original_content_parts.Length; i++)
            //{
            //    index_ = 0;
            //        temp_string= White_.original_content_parts[i].Split(split1,StringSplitOptions.RemoveEmptyEntries);
            //        for(int j=0;j< temp_string.Length; j++)
            //        {
            //        if (j == 0)
            //        {
            //            //White_.trans_content += split5[index_instrument]+"\n";
            //            index_instrument++;
            //        }
            //            if (temp_string[j].Contains("<pitch>"))
            //            {
            //            temp_string2 = temp_string[j].Split(split4, StringSplitOptions.RemoveEmptyEntries);
            //          for(int k = 0; k < temp_string2.Length; k++)
            //            {
            //                if (temp_string2[k].Contains("<note default"))
            //                {
            //                    //White_.trans_content += temp_string2[k].Substring(0,temp_string2[k].Length-1) + $" print-object = \"no\" dynamics=\"{}\"";
            //                }
            //                else
            //                {
            //                    if(k== temp_string2.Length - 1)
            //                    {
            //                        White_.trans_content += temp_string2[k] + "\n";
            //                    }
            //                    else
            //                    {
            //                        White_.trans_content += temp_string2[k] + "<\note>";
            //                    }
                               
            //                }
            //            }
            //            index_++;
            //            }
            //        }
                
                
            //}


           





            /** Load Image Process Rule **/
  
            string imageprocess_White_ = Properties.Resources._11x12;// Properties.Resources.ClapSong_Processorder;
            string imageprocess_Asian = Properties.Resources._26x26;// Properties.Resources.ClapSong_Processorder;
            string imageprocess_Africa = Properties.Resources._30X30;// Properties.Resources.Sakura_Processorder;




            White_.index_vertical = new int[11*12];
            White_.index_horizontal = new int[11 * 12];
            White_.index_clockwise = new int[11 * 12];
            White_.index_counterclockwise = new int[11 * 12];

            contents = imageprocess_White_.Split(split2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < contents.Length; i++)
            {

                contents_part = contents[i].Split(split3, StringSplitOptions.None);
                if (contents_part[0] != "")
                {
                    White_.index_vertical[i ] = Convert.ToInt32(contents_part[0]) - 1;
                    White_.index_horizontal[i ] = Convert.ToInt32(contents_part[1]) - 1;
                    White_.index_clockwise[i ] = Convert.ToInt32(contents_part[2]) - 1;
                    White_.index_counterclockwise[i ] = Convert.ToInt32(contents_part[3]) - 1;
                }
            }


            Asian.index_vertical = new int[26 * 26];
            Asian.index_horizontal = new int[26 * 26];
            Asian.index_clockwise = new int[26 * 26];
            Asian.index_counterclockwise = new int[26 * 26];

            contents = imageprocess_Asian.Split(split2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < contents.Length; i++)
            {

                contents_part = contents[i].Split(split3, StringSplitOptions.None);
                if (contents_part[0] != "")
                {
                    Asian.index_vertical[i] = Convert.ToInt32(contents_part[0]) - 1;
                    Asian.index_horizontal[i] = Convert.ToInt32(contents_part[1]) - 1;
                    Asian.index_clockwise[i] = Convert.ToInt32(contents_part[2]) - 1;
                    Asian.index_counterclockwise[i] = Convert.ToInt32(contents_part[3]) - 1;
                }
            }
            Africa.index_vertical = new int[30 * 30];
            Africa.index_horizontal = new int[30 * 30];
            Africa.index_clockwise = new int[30 * 30];
            Africa.index_counterclockwise = new int[30 * 30];

            contents = imageprocess_Africa.Split(split2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < contents.Length; i++)
            {

                contents_part = contents[i].Split(split3, StringSplitOptions.None);
                if (contents_part[0] != "")
                {
                    Africa.index_vertical[i] = Convert.ToInt32(contents_part[0]) - 1;
                    Africa.index_horizontal[i] = Convert.ToInt32(contents_part[1]) - 1;
                    Africa.index_clockwise[i] = Convert.ToInt32(contents_part[2]) - 1;
                    Africa.index_counterclockwise[i] = Convert.ToInt32(contents_part[3]) - 1;
                }
            }



        }


        private void White_Music(string FileName,string Composer,int ImageProcess)
        {

            White_.trans_RedData_Before = new float[0];
            White_.trans_GreenData_Before = new float[0];
            White_.trans_BlueData_Before = new float[0];

            White_.trans_RedData_After = new float[0];
            White_.trans_GreenData_After = new float[0];
            White_.trans_BlueData_After = new float[0];
            White_.trans_content = "";


            


            int Image_Width_Before = LoadBitmap_Before.Width;
            int Image_Height_Before = LoadBitmap_Before.Height;


            int Image_Width_After = LoadBitmap_After.Width;
            int Image_Height_After = LoadBitmap_After.Height;


            //int Filter_Height = Convert.ToInt32(Math.Truncate(Image_Height / 42.0));
            //int Filter_Width = Convert.ToInt32(Math.Truncate(Image_Width / 41.0));

            int Before_H = Convert.ToInt32(Math.Truncate(Image_Height_Before /11.0));
            int Before_W = Convert.ToInt32(Math.Truncate(Image_Width_Before / 12.0));

            int After_H = Convert.ToInt32(Math.Truncate(Image_Height_After / 11.0));
            int After_W = Convert.ToInt32(Math.Truncate(Image_Width_After / 12.0));

            string[] recordRed=new string[11*12];
            string[] recordGreen = new string[11 * 12];
            string[] recordBlue = new string[11 * 12];

            /*  ImageProcessing */
            for (int height = 0; height < 11; height++)
            {
                for (int width = 0; width < 12; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < Before_W; i++)
                    {
                        for (int j = 0; j < Before_H; j++)
                        {
                            tempR += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).R;
                            tempG += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).G;
                            tempB += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).B;

                        }
                    }

                    Array.Resize(ref White_.trans_RedData_Before, White_.trans_RedData_Before.Length + 1);

                    White_.trans_RedData_Before[White_.trans_RedData_Before.Length - 1] = tempR;

                    Array.Resize(ref White_.trans_GreenData_Before, White_.trans_GreenData_Before.Length + 1);

                    White_.trans_GreenData_Before[White_.trans_GreenData_Before.Length - 1] = tempG;

                    Array.Resize(ref White_.trans_BlueData_Before, White_.trans_BlueData_Before.Length + 1);

                    White_.trans_BlueData_Before[White_.trans_BlueData_Before.Length - 1] = tempB;


                }
            }



            for (int height = 0; height < 11; height++)
            {
                for (int width = 0; width <12; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < After_W; i++)
                    {
                        for (int j = 0; j < After_H; j++)
                        {
                            tempR += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).R;
                            tempG += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).G;
                            tempB += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).B;

                        }
                    }

                    Array.Resize(ref White_.trans_RedData_After, White_.trans_RedData_After.Length + 1);

                    White_.trans_RedData_After[White_.trans_RedData_After.Length - 1] = tempR;

                    Array.Resize(ref White_.trans_GreenData_After, White_.trans_GreenData_After.Length + 1);

                    White_.trans_GreenData_After[White_.trans_GreenData_After.Length - 1] = tempG;

                    Array.Resize(ref White_.trans_BlueData_After, White_.trans_BlueData_After.Length + 1);

                    White_.trans_BlueData_After[White_.trans_BlueData_After.Length - 1] = tempB;
                }
            }


            
            White_.deltaRGB = new double[11 * 12];

            for (int i = 0; i < 11 * 12; i++)
            {
                double tempR = White_.trans_RedData_After[i] - White_.trans_RedData_Before[i];
                double tempG = White_.trans_GreenData_After[i] - White_.trans_GreenData_Before[i];
                double tempB = White_.trans_BlueData_After[i] - White_.trans_BlueData_Before[i];
               
                if(tempR > 0)//開關
                {
                    recordRed[i] = "T";
                }
                else if (tempR < 0)
                {
                    recordRed[i] = "F";
                }
                if (tempG > 0)//開關
                {
                    recordGreen[i] = "T";
                }
                else if (tempG < 0)
                {
                    recordGreen[i] = "F";
                }
                if (tempB > 0)//開關
                {
                    recordBlue[i] = "T";
                }
                else if (tempB < 0)
                {
                    recordBlue[i] = "F";
                }



                White_.deltaRGB[i] =Math.Abs( tempR+ tempG+ tempB)%14;
              
            }



            string[] split1 = new string[] { "</note>" };
            string[] split2 = new string[] { "<part id=\"P4\">", "<part id=\"P2\">", "<part id=\"P3\">", "<part id=\"P1\">", "<part id=\"P5\">", "<part id=\"P7\">", "<part id=\"P6\">" };
         
            string[] split3 = new string[] { "\n" };


            string[] temp_string; string[] temp_string2;
            int index_ = 0;
          
            for (int i = 0; i < White_.original_content_parts.Length; i++)
            {
                index_ = 0;
                if (i == 0)
                {
                    temp_string = White_.original_content_parts[i].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                    for(int j = 0; j < temp_string.Length; j++)
                    {
                        if (temp_string[j].Contains("Bach Minuet in G Major"))
                        {
                            White_.trans_content += $"\n <credit-words default-x=\"600.241\" default-y=\"1611.21\" justify=\"center\" valign=\"top\" font-size=\"22\">{FileName}</credit-words>";
                        }
                        else if (temp_string[j].Contains("J. S. Bach") && temp_string[j].Contains("<credit-words"))
                        {
                            White_.trans_content += $"\n <credit-words default-x=\"1114.76\" default-y=\"1511.21\" justify=\"right\" valign=\"bottom\">{Composer}</credit-words>";
                        }
                        else if(temp_string[j].Contains("id=\"P5\""))
                        {
                            White_.trans_content += $">\n {temp_string[j]}";
                        }
                        else
                        {
                            if (j == 0)
                            {
                                White_.trans_content +=  temp_string[j];
                            }else
                            {
                                White_.trans_content += "\n " + temp_string[j];
                            }
                        }
                    }


                    //White_.trans_content += White_.original_content_parts[i];
                } 
                else if (i==7)
                {
                    int high_index = 0;
                    string parameter_step = "";
                    string parameter_alter = "";
                    string parameter_octave = "";
                    string parameter_accidental = "";

                    


                    temp_string = White_.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);
                 
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                       

                       
                        if (j == 0)
                        {

                            White_.trans_content += split2[i - 1] + "\n";

                        }
                        if (temp_string[j].Contains("<pitch>"))
                        {



                            switch ((ImageProcessType)ImageProcess)
                            {
                                case ImageProcessType.Horizontal:
                                    parameter_step = White_.Pitch_List_Step[Convert.ToInt32(White_.deltaRGB[White_.index_horizontal[high_index%11*12]])];
                                    parameter_alter = White_.Pitch_List_Alter[Convert.ToInt32(White_.deltaRGB[White_.index_horizontal[high_index % 11 * 12]])];
                                    parameter_octave = White_.Pitch_List_Octave[Convert.ToInt32(White_.deltaRGB[White_.index_horizontal[high_index % 11 * 12]])];
                                    parameter_accidental = White_.Pitch_List_Accidental[Convert.ToInt32(White_.deltaRGB[White_.index_horizontal[high_index % 11 * 12]])];
                                    break;
                                case ImageProcessType.Vertical:
                                    parameter_step = White_.Pitch_List_Step[Convert.ToInt32(White_.deltaRGB[White_.index_vertical[high_index % 11 * 12]])];
                                    parameter_alter = White_.Pitch_List_Alter[Convert.ToInt32(White_.deltaRGB[White_.index_vertical[high_index % 11 * 12]])];
                                    parameter_octave = White_.Pitch_List_Octave[Convert.ToInt32(White_.deltaRGB[White_.index_vertical[high_index % 11 * 12]])];
                                    parameter_accidental = White_.Pitch_List_Accidental[Convert.ToInt32(White_.deltaRGB[White_.index_vertical[high_index % 11 * 12]])];
                                    break;
                                case ImageProcessType.Clockwise:
                                    parameter_step = White_.Pitch_List_Step[Convert.ToInt32(White_.deltaRGB[White_.index_clockwise[high_index % 11 * 12]])];
                                    parameter_alter = White_.Pitch_List_Alter[Convert.ToInt32(White_.deltaRGB[White_.index_clockwise[high_index % 11 * 12]])];
                                    parameter_octave = White_.Pitch_List_Octave[Convert.ToInt32(White_.deltaRGB[White_.index_clockwise[high_index % 11 * 12]])];
                                    parameter_accidental = White_.Pitch_List_Accidental[Convert.ToInt32(White_.deltaRGB[White_.index_clockwise[high_index % 11 * 12]])];
                                    break;
                                case ImageProcessType.Counterclockwise:
                                    parameter_step = White_.Pitch_List_Step[Convert.ToInt32(White_.deltaRGB[White_.index_counterclockwise[high_index % 11 * 12]])];
                                    parameter_alter = White_.Pitch_List_Alter[Convert.ToInt32(White_.deltaRGB[White_.index_counterclockwise[high_index % 11 * 12]])];
                                    parameter_octave = White_.Pitch_List_Octave[Convert.ToInt32(White_.deltaRGB[White_.index_counterclockwise[high_index % 11 * 12]])];
                                    parameter_accidental = White_.Pitch_List_Accidental[Convert.ToInt32(White_.deltaRGB[White_.index_counterclockwise[high_index % 11 * 12]])];
                                    break;
                            }

                            /*
                            parameter_step = White_.Pitch_List_Step[Convert.ToInt32(White_.deltaRGB[high_index])];
                            parameter_alter = White_.Pitch_List_Alter[Convert.ToInt32(White_.deltaRGB[high_index])];
                            parameter_octave = White_.Pitch_List_Octave[Convert.ToInt32(White_.deltaRGB[high_index])];
                            parameter_accidental = White_.Pitch_List_Accidental[Convert.ToInt32(White_.deltaRGB[high_index])];
                            */

                                 high_index++;
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                           
                            for(int k=0; k < temp_string2.Length; k++)
                            {
                                if (temp_string2[k].Contains("<step>"))
                                {
                                    White_.trans_content += $"<step>{parameter_step}</step>\n";
                                    
                                }
                                else if (temp_string2[k].Contains("<octave>"))
                                {
                                    White_.trans_content += $"<octave>{parameter_octave}</octave>\n";
                                }
                                else
                                {
                                    White_.trans_content += temp_string2[k] + "\n";
                                }
                              
                            }
                           

                          
                            index_++;
                        }
                        else
                        {

                            White_.trans_content += temp_string[j];
                        }

                        if (j != temp_string.Length - 1)
                        {
                            White_.trans_content += "</note>";
                        }

                    }


                  
                   
                

                   // White_.trans_content += split2[i - 1] ;
                    //White_.trans_content += White_.original_content_parts[i];
                }
                else
                {
                    index_ = 0;
                    temp_string = White_.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                        if (j == 0)
                        {
                            
                            White_.trans_content += split2[i-1] + "\n";
                           
                        }
                        if (temp_string[j].Contains("<unpitched>"))
                        {
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                            bool state = false;
                            


                            for (int k = 0; k < temp_string2.Length; k++)
                            {
                                state = false;
                                if (i == 1)
                                {
                                    if (recordRed[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }
                                    
                                }
                                if (i == 2)
                                {
                                    if (recordRed[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                   
                                }
                                if (i == 3)
                                {
                                    if (recordGreen[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }
                                   
                                }
                                if (i == 4)
                                {
                                    if (recordGreen[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }
                                if (i == 5)
                                {
                                    if (recordBlue[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 6)
                                {
                                    if (recordBlue[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }

                                if (state)
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }
                                    else
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }
                                }
                                else
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        White_.trans_content += "<note>\n";
                                    }
                                    else if (temp_string2[k].Contains("<unpitched>"))
                                    {
                                        White_.trans_content += "<rest/>\n";
                                    }
                                    else if (temp_string2[k].Contains("<<duration>"))
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<voice>"))
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<type>"))
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }else if (temp_string2[k].Contains("<display-step>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<display-octave>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("</unpitched>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<instrument id=>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<stem>"))
                                    {

                                    }
                                    else
                                    {
                                        White_.trans_content += temp_string2[k] + "\n";
                                    }

                                }


                            }
                            index_++;
                        }
                        else
                        {
                           
                            White_.trans_content += temp_string[j];
                        }

                        if(j!= temp_string.Length - 1)
                        {
                            White_.trans_content += "</note>";
                        }
                       
                    }

                }


            }


            string tempfilename = FileName;
            string tempfilename_ = tempfilename;
            while (File.Exists($"{Savepath}\\{tempfilename}.musicxml"))
            {
                int n = 0;
                if (int.TryParse(tempfilename.Substring(tempfilename.Length - 1, 1), out n))
                {
                    tempfilename_ = tempfilename;
                    tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
                }
                else
                {
                    tempfilename += " v2";
                }
            }


            using (StreamWriter SW = new StreamWriter($"{Savepath}\\{tempfilename}.musicxml"))
            {
                SW.Write(White_.trans_content);
            }



            /* animated over */
            show_animated = false;

            this.Cursor = Cursors.Arrow;
            Rectangle_Export.IsEnabled = true;
            label_Export.IsEnabled = true;
            textBox_afterlink.IsEnabled = true;
            textBox_beforelink.IsEnabled = true;
            // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
            if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
                $"File Location : {Savepath}\\{tempfilename}.musicxml\r\n", "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (Process P = new Process())
                {
                    P.StartInfo.FileName = "explorer.exe";
                    P.StartInfo.Arguments = Savepath;
                    P.Start();
                }
            }



        }

        private void Asian_Music(string FileName, string Composer, int ImageProcess)
        {

            Asian.trans_RedData_Before = new float[0];
            Asian.trans_GreenData_Before = new float[0];
            Asian.trans_BlueData_Before = new float[0];

            Asian.trans_RedData_After = new float[0];
            Asian.trans_GreenData_After = new float[0];
            Asian.trans_BlueData_After = new float[0];
            Asian.trans_content = "";





            int Image_Width_Before = LoadBitmap_Before.Width;
            int Image_Height_Before = LoadBitmap_Before.Height;


            int Image_Width_After = LoadBitmap_After.Width;
            int Image_Height_After = LoadBitmap_After.Height;


            int Before_H = Convert.ToInt32(Math.Truncate(Image_Height_Before / 30.0));
            int Before_W = Convert.ToInt32(Math.Truncate(Image_Width_Before / 30.0));

            int After_H = Convert.ToInt32(Math.Truncate(Image_Height_After / 30.0));
            int After_W = Convert.ToInt32(Math.Truncate(Image_Width_After / 30.0));

            string[] recordRed = new string[30 *30];
            string[] recordGreen = new string[30 * 30];
            string[] recordBlue = new string[30 * 30];

            /*  ImageProcessing */
            for (int height = 0; height < 30; height++)
            {
                for (int width = 0; width < 30; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < Before_W; i++)
                    {
                        for (int j = 0; j < Before_H; j++)
                        {
                            tempR += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).R;
                            tempG += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).G;
                            tempB += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).B;

                        }
                    }

                    Array.Resize(ref Asian.trans_RedData_Before, Asian.trans_RedData_Before.Length + 1);

                    Asian.trans_RedData_Before[Asian.trans_RedData_Before.Length - 1] = tempR;

                    Array.Resize(ref Asian.trans_GreenData_Before, Asian.trans_GreenData_Before.Length + 1);

                    Asian.trans_GreenData_Before[Asian.trans_GreenData_Before.Length - 1] = tempG;

                    Array.Resize(ref Asian.trans_BlueData_Before, Asian.trans_BlueData_Before.Length + 1);

                    Asian.trans_BlueData_Before[Asian.trans_BlueData_Before.Length - 1] = tempB;


                }
            }



            for (int height = 0; height < 30; height++)
            {
                for (int width = 0; width < 30; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < After_W; i++)
                    {
                        for (int j = 0; j < After_H; j++)
                        {
                            tempR += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).R;
                            tempG += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).G;
                            tempB += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).B;

                        }
                    }

                    Array.Resize(ref Asian.trans_RedData_After, Asian.trans_RedData_After.Length + 1);

                    Asian.trans_RedData_After[Asian.trans_RedData_After.Length - 1] = tempR;

                    Array.Resize(ref Asian.trans_GreenData_After, Asian.trans_GreenData_After.Length + 1);

                    Asian.trans_GreenData_After[Asian.trans_GreenData_After.Length - 1] = tempG;

                    Array.Resize(ref Asian.trans_BlueData_After, Asian.trans_BlueData_After.Length + 1);

                    Asian.trans_BlueData_After[Asian.trans_BlueData_After.Length - 1] = tempB;
                }
            }



            Asian.deltaRGB = new double[30 * 30];

            for (int i = 0; i < 30 * 30; i++)
            {
                double tempR = Asian.trans_RedData_After[i] - Asian.trans_RedData_Before[i];
                double tempG = Asian.trans_GreenData_After[i] - Asian.trans_GreenData_Before[i];
                double tempB = Asian.trans_BlueData_After[i] - Asian.trans_BlueData_Before[i];

                if (tempR > 0)//開關
                {
                    recordRed[i] = "T";
                }
                else if (tempR < 0)
                {
                    recordRed[i] = "F";
                }
                if (tempG > 0)//開關
                {
                    recordGreen[i] = "T";
                }
                else if (tempG < 0)
                {
                    recordGreen[i] = "F";
                }
                if (tempB > 0)//開關
                {
                    recordBlue[i] = "T";
                }
                else if (tempB < 0)
                {
                    recordBlue[i] = "F";
                }



                Asian.deltaRGB[i] = Math.Abs(tempR + tempG + tempB) % 12;

            }



            string[] split1 = new string[] { "</note>" };
            string[] split2 = new string[] { "<part id=\"P4\">", "<part id=\"P2\">", "<part id=\"P3\">", "<part id=\"P1\">", "<part id=\"P5\">", "<part id=\"P7\">", "<part id=\"P6\">" };

            string[] split3 = new string[] { "\n" };


            string[] temp_string; string[] temp_string2;
            int index_ = 0;
            string temp_vlolume;
            for (int i = 0; i < Asian.original_content_parts.Length; i++)
            {
                index_ = 0;
                if (i == 0)
                {
                    temp_string = Asian.original_content_parts[i].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                        if (temp_string[j].Contains("Demo") && temp_string[j].Contains("<credit-words"))
                        {
                            Asian.trans_content += $"\n <credit-words default-x=\"600.241\" default-y=\"1611.21\" justify=\"center\" valign=\"top\" font-size=\"22\">{FileName}</credit-words>";
                        }
                        else if (temp_string[j].Contains("Composer") && temp_string[j].Contains("<credit-words"))
                        {
                            Asian.trans_content += $"\n <credit-words default-x=\"1114.76\" default-y=\"1511.21\" justify=\"right\" valign=\"bottom\">{Composer}</credit-words>";
                        }
                        else if (temp_string[j].Contains("id=\"P5\""))
                        {
                            White_.trans_content += $">\n {temp_string[j]}";
                        }
                        else
                        {
                            if (j == 0)
                            {
                                Asian.trans_content += temp_string[j];
                            }
                            else
                            {
                                Asian.trans_content += "\n " + temp_string[j];
                            }
                        }
                    }


                    //Asian.trans_content += Asian.original_content_parts[i];
                }
                else if (i == 7)
                {
                    int high_index = 0;
                    string parameter_step = "";
                    string parameter_alter = "";
                    string parameter_octave = "";
                    string parameter_accidental = "";

                    string Original_Pitch = "";
                    string Placed_Pitch = "";





                    temp_string = Asian.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < temp_string.Length; j++)
                    {


                        

                        if (j == 0)
                        {

                            Asian.trans_content += split2[i - 1] + "\n";

                        }
                        if (temp_string[j].Contains("<pitch>"))
                        {


                            switch ((ImageProcessType)ImageProcess)
                            {
                                case ImageProcessType.Horizontal:
                                    parameter_step = Asian.Pitch_List_Step[Convert.ToInt32(Asian.deltaRGB[Asian.index_horizontal[high_index % 30 * 30]])];
                                    parameter_alter = Asian.Pitch_List_Alter[Convert.ToInt32(Asian.deltaRGB[Asian.index_horizontal[high_index % 30 * 30]])];
                                    parameter_octave = Asian.Pitch_List_Octave[Convert.ToInt32(Asian.deltaRGB[Asian.index_horizontal[high_index % 30 * 30]])];
                                    parameter_accidental = Asian.Pitch_List_Accidental[Convert.ToInt32(Asian.deltaRGB[Asian.index_horizontal[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Vertical:
                                    parameter_step = Asian.Pitch_List_Step[Convert.ToInt32(Asian.deltaRGB[Asian.index_vertical[high_index % 30 * 30]])];
                                    parameter_alter = Asian.Pitch_List_Alter[Convert.ToInt32(Asian.deltaRGB[Asian.index_vertical[high_index % 30 * 30]])];
                                    parameter_octave = Asian.Pitch_List_Octave[Convert.ToInt32(Asian.deltaRGB[Asian.index_vertical[high_index % 30 * 30]])];
                                    parameter_accidental = Asian.Pitch_List_Accidental[Convert.ToInt32(Asian.deltaRGB[Asian.index_vertical[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Clockwise:
                                    parameter_step = Asian.Pitch_List_Step[Convert.ToInt32(Asian.deltaRGB[Asian.index_clockwise[high_index % 30 * 30]])];
                                    parameter_alter = Asian.Pitch_List_Alter[Convert.ToInt32(Asian.deltaRGB[Asian.index_clockwise[high_index % 30 * 30]])];
                                    parameter_octave = Asian.Pitch_List_Octave[Convert.ToInt32(Asian.deltaRGB[Asian.index_clockwise[high_index % 30 * 30]])];
                                    parameter_accidental = Asian.Pitch_List_Accidental[Convert.ToInt32(Asian.deltaRGB[Asian.index_clockwise[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Counterclockwise:
                                    parameter_step = Asian.Pitch_List_Step[Convert.ToInt32(Asian.deltaRGB[Asian.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_alter = Asian.Pitch_List_Alter[Convert.ToInt32(Asian.deltaRGB[Asian.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_octave = Asian.Pitch_List_Octave[Convert.ToInt32(Asian.deltaRGB[Asian.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_accidental = Asian.Pitch_List_Accidental[Convert.ToInt32(Asian.deltaRGB[Asian.index_counterclockwise[high_index % 30 * 30]])];
                                    break;
                            }
                            /*
                            parameter_step = Asian.Pitch_List_Step[Convert.ToInt32(Asian.deltaRGB[high_index])];
                            parameter_alter = Asian.Pitch_List_Alter[Convert.ToInt32(Asian.deltaRGB[high_index])];
                            parameter_octave = Asian.Pitch_List_Octave[Convert.ToInt32(Asian.deltaRGB[high_index])];
                            parameter_accidental = Asian.Pitch_List_Accidental[Convert.ToInt32(Asian.deltaRGB[high_index])];
                            */

                            high_index++;
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);

                            for (int k = 0; k < temp_string2.Length; k++)
                            {
                                if (temp_string2[k].Contains("<step>"))
                                {
                                    Asian.trans_content += $"<step>{parameter_step}</step>\n";

                                }
                                else if (temp_string2[k].Contains("<octave>"))
                                {
                                    Asian.trans_content += $"<octave>{parameter_octave}</octave>\n";
                                }
                                else
                                {
                                    Asian.trans_content += temp_string2[k] + "\n";
                                }

                            }



                            index_++;
                        }
                        else
                        {

                            Asian.trans_content += temp_string[j];
                        }

                        if (j != temp_string.Length - 1)
                        {
                            Asian.trans_content += "</note>";
                        }

                    }






                    // Asian.trans_content += split2[i - 1] ;
                    //Asian.trans_content += Asian.original_content_parts[i];
                }
                else
                {
                    index_ = 0;
                    temp_string = Asian.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                        if (j == 0)
                        {

                            Asian.trans_content += split2[i - 1] + "\n";

                        }
                        if (temp_string[j].Contains("<unpitched>"))
                        {
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                            bool state = false;



                            for (int k = 0; k < temp_string2.Length; k++)
                            {
                                state = false;
                                if (i == 1)
                                {
                                    if (recordRed[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 2)
                                {
                                    if (recordRed[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 3)
                                {
                                    if (recordGreen[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 4)
                                {
                                    if (recordGreen[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }
                                if (i == 5)
                                {
                                    if (recordBlue[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 6)
                                {
                                    if (recordBlue[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }

                                if (state)
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }
                                    else
                                    {
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }
                                }
                                else
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        Asian.trans_content += "<note>\n";
                                    }
                                    else if (temp_string2[k].Contains("<unpitched>"))
                                    {
                                        Asian.trans_content += "<rest/>\n";
                                    }
                                    else if (temp_string2[k].Contains("<<duration>"))
                                    {
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<voice>"))
                                    {
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<type>"))
                                    {
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<display-step>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<display-octave>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("</unpitched>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<instrument id=>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<stem>"))
                                    {

                                    }
                                    else
                                    {
                                        Asian.trans_content += temp_string2[k] + "\n";
                                    }

                                }


                            }
                            index_++;
                        }
                        else
                        {

                            Asian.trans_content += temp_string[j];
                        }

                        if (j != temp_string.Length - 1)
                        {
                            Asian.trans_content += "</note>";
                        }

                    }

                }


            }


            string tempfilename = FileName;
            string tempfilename_ = tempfilename;
            while (File.Exists($"{Savepath}\\{tempfilename}.musicxml"))
            {
                int n = 0;
                if (int.TryParse(tempfilename.Substring(tempfilename.Length - 1, 1), out n))
                {
                    tempfilename_ = tempfilename;
                    tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
                }
                else
                {
                    tempfilename += " v2";
                }
            }


            using (StreamWriter SW = new StreamWriter($"{Savepath}\\{tempfilename}.musicxml"))
            {
                SW.Write(Asian.trans_content);
            }



            /* animated over */
            show_animated = false;

            this.Cursor = Cursors.Arrow;
            Rectangle_Export.IsEnabled = true;
            label_Export.IsEnabled = true;
            textBox_afterlink.IsEnabled = true;
            textBox_beforelink.IsEnabled = true;
            // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
            if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
                $"File Location : {Savepath}\\{tempfilename}.musicxml\r\n", "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (Process P = new Process())
                {
                    P.StartInfo.FileName = "explorer.exe";
                    P.StartInfo.Arguments = Savepath;
                    P.Start();
                }
            }



        }
        private void Africa_Music(string FileName, string Composer, int ImageProcess)
        {

            Africa.trans_RedData_Before = new float[0];
            Africa.trans_GreenData_Before = new float[0];
            Africa.trans_BlueData_Before = new float[0];

            Africa.trans_RedData_After = new float[0];
            Africa.trans_GreenData_After = new float[0];
            Africa.trans_BlueData_After = new float[0];
            Africa.trans_content = "";





            int Image_Width_Before = LoadBitmap_Before.Width;
            int Image_Height_Before = LoadBitmap_Before.Height;


            int Image_Width_After = LoadBitmap_After.Width;
            int Image_Height_After = LoadBitmap_After.Height;


            int Before_H = Convert.ToInt32(Math.Truncate(Image_Height_Before / 30.0));
            int Before_W = Convert.ToInt32(Math.Truncate(Image_Width_Before / 30.0));

            int After_H = Convert.ToInt32(Math.Truncate(Image_Height_After / 30.0));
            int After_W = Convert.ToInt32(Math.Truncate(Image_Width_After / 30.0));

            string[] recordRed = new string[30 * 30];
            string[] recordGreen = new string[30 * 30];
            string[] recordBlue = new string[30 * 30];

            /*  ImageProcessing */
            for (int height = 0; height < 30; height++)
            {
                for (int width = 0; width < 30; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < Before_W; i++)
                    {
                        for (int j = 0; j < Before_H; j++)
                        {
                            tempR += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).R;
                            tempG += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).G;
                            tempB += LoadBitmap_Before.GetPixel(width * Before_W + i, height * Before_H + j).B;

                        }
                    }

                    Array.Resize(ref Africa.trans_RedData_Before, Africa.trans_RedData_Before.Length + 1);

                    Africa.trans_RedData_Before[Africa.trans_RedData_Before.Length - 1] = tempR;

                    Array.Resize(ref Africa.trans_GreenData_Before, Africa.trans_GreenData_Before.Length + 1);

                    Africa.trans_GreenData_Before[Africa.trans_GreenData_Before.Length - 1] = tempG;

                    Array.Resize(ref Africa.trans_BlueData_Before, Africa.trans_BlueData_Before.Length + 1);

                    Africa.trans_BlueData_Before[Africa.trans_BlueData_Before.Length - 1] = tempB;


                }
            }



            for (int height = 0; height < 30; height++)
            {
                for (int width = 0; width < 30; width++)
                {
                    float tempR = 0;
                    float tempG = 0;
                    float tempB = 0;
                    for (int i = 0; i < After_W; i++)
                    {
                        for (int j = 0; j < After_H; j++)
                        {
                            tempR += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).R;
                            tempG += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).G;
                            tempB += LoadBitmap_After.GetPixel(width * After_W + i, height * After_H + j).B;

                        }
                    }

                    Array.Resize(ref Africa.trans_RedData_After, Africa.trans_RedData_After.Length + 1);

                    Africa.trans_RedData_After[Africa.trans_RedData_After.Length - 1] = tempR;

                    Array.Resize(ref Africa.trans_GreenData_After, Africa.trans_GreenData_After.Length + 1);

                    Africa.trans_GreenData_After[Africa.trans_GreenData_After.Length - 1] = tempG;

                    Array.Resize(ref Africa.trans_BlueData_After, Africa.trans_BlueData_After.Length + 1);

                    Africa.trans_BlueData_After[Africa.trans_BlueData_After.Length - 1] = tempB;
                }
            }



            Africa.deltaRGB = new double[30 * 30];

            for (int i = 0; i < 30 * 30; i++)
            {
                double tempR = Africa.trans_RedData_After[i] - Africa.trans_RedData_Before[i];
                double tempG = Africa.trans_GreenData_After[i] - Africa.trans_GreenData_Before[i];
                double tempB = Africa.trans_BlueData_After[i] - Africa.trans_BlueData_Before[i];

                if (tempR > 0)//開關
                {
                    recordRed[i] = "T";
                }
                else if (tempR < 0)
                {
                    recordRed[i] = "F";
                }
                if (tempG > 0)//開關
                {
                    recordGreen[i] = "T";
                }
                else if (tempG < 0)
                {
                    recordGreen[i] = "F";
                }
                if (tempB > 0)//開關
                {
                    recordBlue[i] = "T";
                }
                else if (tempB < 0)
                {
                    recordBlue[i] = "F";
                }



                Africa.deltaRGB[i] = Math.Abs(tempR + tempG + tempB) % 12;

            }



            string[] split1 = new string[] { "</note>" };
            string[] split2 = new string[] { "<part id=\"P4\">", "<part id=\"P2\">", "<part id=\"P3\">", "<part id=\"P1\">", "<part id=\"P5\">", "<part id=\"P7\">", "<part id=\"P6\">" };

            string[] split3 = new string[] { "\n" };


            string[] temp_string; string[] temp_string2;
            int index_ = 0;
            string temp_vlolume;
            for (int i = 0; i < Africa.original_content_parts.Length; i++)
            {
                index_ = 0;
                if (i == 0)
                {
                    temp_string = Africa.original_content_parts[i].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                        if (temp_string[j].Contains("Mack The Knife") && temp_string[j].Contains("<credit-words"))
                        {
                            Africa.trans_content += $"\n <credit-words default-x=\"600.241\" default-y=\"1611.21\" justify=\"center\" valign=\"top\" font-size=\"22\">{FileName}</credit-words>";
                        }
                        else if (temp_string[j].Contains("Composer") && temp_string[j].Contains("<credit-words"))
                        {
                            Africa.trans_content += $"\n <credit-words default-x=\"1114.76\" default-y=\"1511.21\" justify=\"right\" valign=\"bottom\">{Composer}</credit-words>";
                        }
                        else if (temp_string[j].Contains("id=\"P5\""))
                        {
                            White_.trans_content += $">\n {temp_string[j]}";
                        }
                        else
                        {
                            if (j == 0)
                            {
                                Africa.trans_content += temp_string[j];
                            }
                            else
                            {
                                Africa.trans_content += "\n " + temp_string[j];
                            }
                        }
                    }


                    //Africa.trans_content += Africa.original_content_parts[i];
                }
                else if (i == 7)
                {
                    int high_index = 0;
                    string parameter_step = "";
                    string parameter_alter = "";
                    string parameter_octave = "";
                    string parameter_accidental = "";

                    string Original_Pitch = "";
                    string Placed_Pitch = "";





                    temp_string = Africa.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < temp_string.Length; j++)
                    {


                       

                        if (j == 0)
                        {

                            Africa.trans_content += split2[i - 1] + "\n";

                        }
                        if (temp_string[j].Contains("<pitch>"))
                        {

                            switch ((ImageProcessType)ImageProcess)
                            {
                                case ImageProcessType.Horizontal:
                                    parameter_step = Africa.Pitch_List_Step[Convert.ToInt32(Africa.deltaRGB[Africa.index_horizontal[high_index % 30 * 30]])];
                                    parameter_alter = Africa.Pitch_List_Alter[Convert.ToInt32(Africa.deltaRGB[Africa.index_horizontal[high_index % 30 * 30]])];
                                    parameter_octave = Africa.Pitch_List_Octave[Convert.ToInt32(Africa.deltaRGB[Africa.index_horizontal[high_index % 30 * 30]])];
                                    parameter_accidental = Africa.Pitch_List_Accidental[Convert.ToInt32(Africa.deltaRGB[Africa.index_horizontal[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Vertical:
                                    parameter_step = Africa.Pitch_List_Step[Convert.ToInt32(Africa.deltaRGB[Africa.index_vertical[high_index % 30 * 30]])];
                                    parameter_alter = Africa.Pitch_List_Alter[Convert.ToInt32(Africa.deltaRGB[Africa.index_vertical[high_index % 30 * 30]])];
                                    parameter_octave = Africa.Pitch_List_Octave[Convert.ToInt32(Africa.deltaRGB[Africa.index_vertical[high_index % 30 * 30]])];
                                    parameter_accidental = Africa.Pitch_List_Accidental[Convert.ToInt32(Africa.deltaRGB[Africa.index_vertical[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Clockwise:
                                    parameter_step = Africa.Pitch_List_Step[Convert.ToInt32(Africa.deltaRGB[Africa.index_clockwise[high_index % 30 * 30]])];
                                    parameter_alter = Africa.Pitch_List_Alter[Convert.ToInt32(Africa.deltaRGB[Africa.index_clockwise[high_index % 30 * 30]])];
                                    parameter_octave = Africa.Pitch_List_Octave[Convert.ToInt32(Africa.deltaRGB[Africa.index_clockwise[high_index % 30 * 30]])];
                                    parameter_accidental = Africa.Pitch_List_Accidental[Convert.ToInt32(Africa.deltaRGB[Africa.index_clockwise[high_index % 30 * 30]])];
                                    break;
                                case ImageProcessType.Counterclockwise:
                                    parameter_step = Africa.Pitch_List_Step[Convert.ToInt32(Africa.deltaRGB[Africa.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_alter = Africa.Pitch_List_Alter[Convert.ToInt32(Africa.deltaRGB[Africa.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_octave = Africa.Pitch_List_Octave[Convert.ToInt32(Africa.deltaRGB[Africa.index_counterclockwise[high_index % 30 * 30]])];
                                    parameter_accidental = Africa.Pitch_List_Accidental[Convert.ToInt32(Africa.deltaRGB[Africa.index_counterclockwise[high_index]])];
                                    break;
                            }
                            /*
                            parameter_step = Africa.Pitch_List_Step[Convert.ToInt32(Africa.deltaRGB[high_index])];
                            parameter_alter = Africa.Pitch_List_Alter[Convert.ToInt32(Africa.deltaRGB[high_index])];
                            parameter_octave = Africa.Pitch_List_Octave[Convert.ToInt32(Africa.deltaRGB[high_index])];
                            parameter_accidental = Africa.Pitch_List_Accidental[Convert.ToInt32(Africa.deltaRGB[high_index])];
                            */

                            high_index++;
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);

                            for (int k = 0; k < temp_string2.Length; k++)
                            {
                                if (temp_string2[k].Contains("<step>"))
                                {
                                    Africa.trans_content += $"<step>{parameter_step}</step>\n";

                                }
                                else if (temp_string2[k].Contains("<octave>"))
                                {
                                    Africa.trans_content += $"<octave>{parameter_octave}</octave>\n";
                                }
                                else
                                {
                                    Africa.trans_content += temp_string2[k] + "\n";
                                }

                            }



                            index_++;
                        }
                        else
                        {

                            Africa.trans_content += temp_string[j];
                        }

                        if (j != temp_string.Length - 1)
                        {
                            Africa.trans_content += "</note>";
                        }

                    }






                    // Africa.trans_content += split2[i - 1] ;
                    //Africa.trans_content += Africa.original_content_parts[i];
                }
                else
                {
                    index_ = 0;
                    temp_string = Africa.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < temp_string.Length; j++)
                    {
                        if (j == 0)
                        {

                            Africa.trans_content += split2[i - 1] + "\n";

                        }
                        if (temp_string[j].Contains("<unpitched>"))
                        {
                            temp_string2 = temp_string[j].Split(split3, StringSplitOptions.RemoveEmptyEntries);
                            bool state = false;



                            for (int k = 0; k < temp_string2.Length; k++)
                            {
                                state = false;
                                if (i == 1)
                                {
                                    if (recordRed[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 2)
                                {
                                    if (recordRed[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 3)
                                {
                                    if (recordGreen[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 4)
                                {
                                    if (recordGreen[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }
                                if (i == 5)
                                {
                                    if (recordBlue[index_ % 132] == "T")
                                    {
                                        state = true;
                                    }

                                }
                                if (i == 6)
                                {
                                    if (recordBlue[index_ % 132] == "F")
                                    {
                                        state = true;
                                    }
                                }

                                if (state)
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }
                                    else
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }
                                }
                                else
                                {
                                    if (temp_string2[k].Contains("<note default"))
                                    {
                                        Africa.trans_content += "<note>\n";
                                    }
                                    else if (temp_string2[k].Contains("<unpitched>"))
                                    {
                                        Africa.trans_content += "<rest/>\n";
                                    }
                                    else if (temp_string2[k].Contains("<<duration>"))
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<voice>"))
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<type>"))
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }
                                    else if (temp_string2[k].Contains("<display-step>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<display-octave>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("</unpitched>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<instrument id=>"))
                                    {

                                    }
                                    else if (temp_string2[k].Contains("<stem>"))
                                    {

                                    }
                                    else
                                    {
                                        Africa.trans_content += temp_string2[k] + "\n";
                                    }

                                }


                            }
                            index_++;
                        }
                        else
                        {

                            Africa.trans_content += temp_string[j];
                        }

                        if (j != temp_string.Length - 1)
                        {
                            Africa.trans_content += "</note>";
                        }

                    }

                }

                Rectangle_Export.IsEnabled = true;
                label_Export.IsEnabled = true;
            }


            string tempfilename = FileName;
            string tempfilename_ = tempfilename;
            while (File.Exists($"{Savepath}\\{tempfilename}.musicxml"))
            {
                int n = 0;
                if (int.TryParse(tempfilename.Substring(tempfilename.Length - 1, 1), out n))
                {
                    tempfilename_ = tempfilename;
                    tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
                }
                else
                {
                    tempfilename += " v2";
                }
            }


            using (StreamWriter SW = new StreamWriter($"{Savepath}\\{tempfilename}.musicxml"))
            {
                SW.Write(Africa.trans_content);
            }



            /* animated over */
            show_animated = false;

            this.Cursor = Cursors.Arrow;
            Rectangle_Export.IsEnabled = true;
            label_Export.IsEnabled = true;
            textBox_afterlink.IsEnabled = true;
            textBox_beforelink.IsEnabled = true;

            // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
            if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
                $"File Location : {Savepath}\\{tempfilename}.musicxml\r\n", "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (Process P = new Process())
                {
                    P.StartInfo.FileName = "explorer.exe";
                    P.StartInfo.Arguments = Savepath;
                    P.Start();
                }
            }



        }



        enum ChannelEnum
        {
            Red = 0,
            Green = 1,
            Blue = 2,
            Cyan = 3,
            Magenta = 4,
            Yellow = 5
        }

        enum ImageProcessType
        {
            Horizontal,
            Vertical,
            Clockwise,
            Counterclockwise
        }

        string Title_xml = "";
        string Satellite_xml = "";
        string FileName_xml = "";
        string composer_xml = "";
        int icbo_Treble = 0;
        int icbo_Bass = 1;
        int icbo_BassToTreble = 2;
        int icbo_Musician = 0;
        int icbo_ImageProcess = 0;
        int icbo_Instrument = 0;





        //private void Taiwan_Music(string Title, string FileName, int ImageProcess)
        //{




        //    DateTime start = DateTime.Now;

        //    Taiwan.trans_RedData = new float[0];
        //    Taiwan.trans_GreenData = new float[0];
        //    Taiwan.trans_BlueData = new float[0];


        //    Taiwan.trans_content = "";


        //    int Image_Width = LoadBitmap_Before.Width;
        //    int Image_Height = LoadBitmap_Before.Height;



        //    int HighPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 23.0));
        //    int HighPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 22.0));



        //    /* Taiwan  ImageProcessing */
        //    for (int height = 0; height < 23; height++)
        //    {
        //        for (int width = 0; width < 22; width++)
        //        {
        //            float tempR = 0;


        //            for (int i = 0; i < HighPitch_W; i++)
        //            {
        //                for (int j = 0; j < HighPitch_H; j++)
        //                {
        //                    tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R;//Red_2DData[];
        //                    tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G;//Red_2DData[];
        //                    tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];

        //                }
        //            }
        //            Array.Resize(ref Taiwan.trans_RedData, Taiwan.trans_RedData.Length + 1);

        //            Taiwan.trans_RedData[Taiwan.trans_RedData.Length - 1] = tempR % 17;

        //        }
        //    }







        //    string time1 = ((DateTime.Now - start).TotalMilliseconds / 1000).ToString();

        //    int high_index = 0;
        //    int low_index = 0;
        //    int lowtohigh_index = 0;

        //    string[] temp_string;
        //    string[] split1 = new string[] { "\n" };
        //    bool IsLowToHigh = false;

        //    string Original_Pitch = "";
        //    string Placed_Pitch = "";

        //    DateTime start2 = DateTime.Now;

        //    for (int i = 0; i < Taiwan.original_content_parts.Length; i++)
        //    {
        //        if (Taiwan.original_content_parts[i].Contains("<octave>")) //此字串確認是否為音符
        //        {
        //            temp_string = Taiwan.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);



        //            int Index_ = Taiwan.original_content_parts[i].IndexOf("<pitch>");
        //            string Pitch_StartString = Taiwan.original_content_parts[i].Substring(Index_, Taiwan.original_content_parts[i].Length - Index_);

        //            string V = Pitch_StartString.Substring(Pitch_StartString.IndexOf("<staff>") + 7, 1);
        //            string parameter_step = "";
        //            string parameter_alter = "";
        //            string parameter_octave = "";
        //            string parameter_accidental = "";


        //            /** Pitch參數 -- START  **/

        //            if (V == "1") //高音
        //            {

        //                if (Taiwan.original_content_parts[i].Contains("<attributes>") && i > 0)
        //                {
        //                    IsLowToHigh = Taiwan.original_content_parts[i].Contains("<sign>G</sign>");
        //                }



        //                switch ((ImageProcessType)ImageProcess)
        //                {
        //                    case ImageProcessType.Horizontal:
        //                        parameter_step = Taiwan.HighPitch_Step[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_horizontal[high_index]])];
        //                        parameter_alter = Taiwan.HighPitch_Alter[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_horizontal[high_index]])];
        //                        parameter_octave = Taiwan.HighPitch_Octave[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_horizontal[high_index]])];
        //                        parameter_accidental = Taiwan.HighPitch_Accidental[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_horizontal[high_index]])];
        //                        break;
        //                    case ImageProcessType.Vertical:
        //                        parameter_step = Taiwan.HighPitch_Step[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_vertical[high_index]])];
        //                        parameter_alter = Taiwan.HighPitch_Alter[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_vertical[high_index]])];
        //                        parameter_octave = Taiwan.HighPitch_Octave[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_vertical[high_index]])];
        //                        parameter_accidental = Taiwan.HighPitch_Accidental[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_vertical[high_index]])];
        //                        break;
        //                    case ImageProcessType.Clockwise:
        //                        parameter_step = Taiwan.HighPitch_Step[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_clockwise[high_index]])];
        //                        parameter_alter = Taiwan.HighPitch_Alter[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_clockwise[high_index]])];
        //                        parameter_octave = Taiwan.HighPitch_Octave[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_clockwise[high_index]])];
        //                        parameter_accidental = Taiwan.HighPitch_Accidental[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_clockwise[high_index]])];
        //                        break;
        //                    case ImageProcessType.Counterclockwise:
        //                        parameter_step = Taiwan.HighPitch_Step[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_alter = Taiwan.HighPitch_Alter[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_octave = Taiwan.HighPitch_Octave[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_accidental = Taiwan.HighPitch_Accidental[Convert.ToInt32(Taiwan.trans_RedData[Taiwan.highpitch_index_counterclockwise[high_index]])];
        //                        break;
        //                }






        //                high_index += 1;

        //            }
        //            else if (V == "2") //低音
        //            {
        //                if (Taiwan.original_content_parts[i].Contains("<attributes>"))
        //                {
        //                    IsLowToHigh = Taiwan.original_content_parts[i].Contains("<sign>G</sign>");
        //                }
        //                if (IsLowToHigh)
        //                {
        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = Taiwan.LowToHighPitch_Step[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_alter = Taiwan.LowToHighPitch_Alter[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_octave = Taiwan.LowToHighPitch_Octave[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_accidental = Taiwan.LowToHighPitch_Accidental[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = Taiwan.LowToHighPitch_Step[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_alter = Taiwan.LowToHighPitch_Alter[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_octave = Taiwan.LowToHighPitch_Octave[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_accidental = Taiwan.LowToHighPitch_Accidental[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = Taiwan.LowToHighPitch_Step[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_alter = Taiwan.LowToHighPitch_Alter[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_octave = Taiwan.LowToHighPitch_Octave[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_accidental = Taiwan.LowToHighPitch_Accidental[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = Taiwan.LowToHighPitch_Step[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_alter = Taiwan.LowToHighPitch_Alter[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_octave = Taiwan.LowToHighPitch_Octave[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_accidental = Taiwan.LowToHighPitch_Accidental[Convert.ToInt32(Taiwan.trans_BlueData[Taiwan.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            break;
        //                    }
        //                    /*  parameter_step = Taiwan.LowToHighPitch_Step[Convert.ToInt32(Taiwan.trans_BlueData[lowtohigh_index])];
        //                      parameter_alter = Taiwan.LowToHighPitch_Alter[Convert.ToInt32(Taiwan.trans_BlueData[lowtohigh_index])];
        //                      parameter_octave = Taiwan.LowToHighPitch_Octave[Convert.ToInt32(Taiwan.trans_BlueData[lowtohigh_index])];
        //                      parameter_accidental = Taiwan.LowToHighPitch_Accidental[Convert.ToInt32(Taiwan.trans_BlueData[lowtohigh_index])];*/
        //                    lowtohigh_index += 1;
        //                }
        //                else
        //                {

        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = Taiwan.LowPitch_Step[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_horizontal[low_index]])];
        //                            parameter_alter = Taiwan.LowPitch_Alter[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_horizontal[low_index]])];
        //                            parameter_octave = Taiwan.LowPitch_Octave[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_horizontal[low_index]])];
        //                            parameter_accidental = Taiwan.LowPitch_Accidental[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_horizontal[low_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = Taiwan.LowPitch_Step[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_vertical[low_index]])];
        //                            parameter_alter = Taiwan.LowPitch_Alter[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_vertical[low_index]])];
        //                            parameter_octave = Taiwan.LowPitch_Octave[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_vertical[low_index]])];
        //                            parameter_accidental = Taiwan.LowPitch_Accidental[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_vertical[low_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = Taiwan.LowPitch_Step[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_clockwise[low_index]])];
        //                            parameter_alter = Taiwan.LowPitch_Alter[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_clockwise[low_index]])];
        //                            parameter_octave = Taiwan.LowPitch_Octave[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_clockwise[low_index]])];
        //                            parameter_accidental = Taiwan.LowPitch_Accidental[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_clockwise[low_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = Taiwan.LowPitch_Step[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_alter = Taiwan.LowPitch_Alter[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_octave = Taiwan.LowPitch_Octave[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_accidental = Taiwan.LowPitch_Accidental[Convert.ToInt32(Taiwan.trans_GreenData[Taiwan.lowpitch_index_counterclockwise[low_index]])];
        //                            break;
        //                    }
        //                    /*
        //                    parameter_step = Taiwan.LowPitch_Step[Convert.ToInt32(Taiwan.trans_GreenData[low_index])];
        //                    parameter_alter = Taiwan.LowPitch_Alter[Convert.ToInt32(Taiwan.trans_GreenData[low_index])];
        //                    parameter_octave = Taiwan.LowPitch_Octave[Convert.ToInt32(Taiwan.trans_GreenData[low_index])];
        //                    parameter_accidental = Taiwan.LowPitch_Accidental[Convert.ToInt32(Taiwan.trans_GreenData[low_index])];*/
        //                    low_index += 1;
        //                }

        //            }

        //            /** Pitch參數 -- END  **/

        //            if (Taiwan.original_content_parts[i].Contains("<accidental>"))
        //            {


        //                for (int j = 0; j < temp_string.Length; j++)
        //                {

        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        Taiwan.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                Taiwan.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            Taiwan.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        Taiwan.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else if (temp_string[j].Contains("<accidental>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<accidental>") + 12, 1) + "\r\n";

        //                        if (!String.IsNullOrEmpty(parameter_accidental))
        //                        {
        //                            Taiwan.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += "," + parameter_accidental + "\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += ",\r\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            Taiwan.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Clap Song"))
        //                        {
        //                            Taiwan.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Statellite"))
        //                        {
        //                            Taiwan.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            Taiwan.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            Taiwan.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            Taiwan.trans_content += "\n" + temp_string[j];
        //                        }

        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            Taiwan.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                for (int j = 0; j < temp_string.Length; j++)
        //                {
        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        Taiwan.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                Taiwan.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            Taiwan.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        Taiwan.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<type"))
        //                    {

        //                        Taiwan.trans_content += $"\n {temp_string[j]}";

        //                        Original_Pitch += ",\r\n";
        //                        if (!String.IsNullOrWhiteSpace(parameter_accidental))
        //                        {
        //                            Taiwan.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += $",{parameter_accidental}\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",\r\n";
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            Taiwan.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Clap Song"))
        //                        {
        //                            Taiwan.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Statellite"))
        //                        {
        //                            Taiwan.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            Taiwan.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            Taiwan.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            Taiwan.trans_content += "\n" + temp_string[j];
        //                        }
        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            Taiwan.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            if (i != Taiwan.original_content_parts.Length - 1)
        //            {
        //                Taiwan.trans_content += Taiwan.original_content_parts[i] + "\n        " + "</note>";
        //            }
        //            else
        //            {
        //                Taiwan.trans_content += Taiwan.original_content_parts[i];
        //            }

        //        }
        //    }

        //    string time2 = ((DateTime.Now - start2).TotalMilliseconds / 1000).ToString();
        //    /*
        //    SaveFileDialog SF=new SaveFileDialog();
        //    SF.Filter = "musicxml|*.musicxml";
        //    SF.Title = "Export Music File";
        //    SF.InitialDirectory = Savepath;
        //    SF.FileName = FileName;

        //    if (SF.ShowDialog()==true)
        //    {
        //        using (StreamWriter SW = new StreamWriter(SF.FileName))
        //        {
        //            SW.Write(Taiwan.trans_content);
        //        }
        //    }*/


        //    string tempfilename = FileName;
        //    string tempfilename_ = tempfilename;
        //    while (File.Exists($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        int n = 0;
        //        if(int.TryParse(tempfilename.Substring(tempfilename.Length - 1,1),out n))
        //        {
        //            tempfilename_ = tempfilename;
        //            tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
        //        }
        //        else
        //        {
        //            tempfilename += " v2";
        //        }
        //    }


        //    using (StreamWriter SW = new StreamWriter($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        SW.Write(Taiwan.trans_content);
        //    }



        //    /* animated over */
        //    show_animated = false;

        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {

        //        Rectangle_Export.IsEnabled = true;
        //        label_Export.IsEnabled = true;
        //    }));
        //    // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
        //    if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
        //        $"File Location : {Savepath}\\{Title}_{tempfilename}(Chopin).musicxml\r\n" , "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //    {
        //        using (Process P = new Process())
        //        {
        //            P.StartInfo.FileName = "explorer.exe";
        //            P.StartInfo.Arguments = Savepath;
        //            P.Start();
        //        }
        //    }
        //}


        //private void White__Music(string Title, string Satellite, string FileName, int HighChannel, int LowChannel, int LowToHighChannel, int ImageProcess, string Musical_Instrument = "Piano")
        //{




        //    DateTime start = DateTime.Now;

        //    White_.trans_RedData = new float[0];
        //    White_.trans_GreenData = new float[0];
        //    White_.trans_BlueData = new float[0];


        //    White_.trans_content = "";


        //    int Image_Width = LoadBitmap_Before.Width;
        //    int Image_Height = LoadBitmap_Before.Height;


        //    //int Filter_Height = Convert.ToInt32(Math.Truncate(Image_Height / 42.0));
        //    //int Filter_Width = Convert.ToInt32(Math.Truncate(Image_Width / 41.0));

        //    int HighPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 29.0));
        //    int HighPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 29.0));

        //    int LowPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 25.0));
        //    int LowPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 25.0));

        //    int LowToHighPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 16.0));
        //    int LowToHighPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 16.0));


        //    /* JAPAN  ImageProcessing */
        //    for (int height = 0; height < 29; height++)
        //    {
        //        for (int width = 0; width < 29; width++)
        //        {
        //            float tempR = 0;

        //            for (int i = 0; i < HighPitch_W; i++)
        //            {
        //                for (int j = 0; j < HighPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)HighChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }

        //                }
        //            }


        //            Array.Resize(ref White_.trans_RedData, White_.trans_RedData.Length + 1);

        //            White_.trans_RedData[White_.trans_RedData.Length - 1] = tempR % 29;

        //        }
        //    }


        //    for (int height = 0; height < 25; height++)
        //    {
        //        for (int width = 0; width < 25; width++)
        //        {

        //            float tempG = 0;

        //            for (int i = 0; i < LowPitch_W; i++)
        //            {
        //                for (int j = 0; j < LowPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)LowChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }

        //                }
        //            }

        //            Array.Resize(ref White_.trans_GreenData, White_.trans_GreenData.Length + 1);

        //            White_.trans_GreenData[White_.trans_GreenData.Length - 1] = tempG % 17;

        //        }
        //    }

        //    for (int height = 0; height < 16; height++)
        //    {
        //        for (int width = 0; width < 16; width++)
        //        {

        //            float tempB = 0;
        //            for (int i = 0; i < LowToHighPitch_W; i++)
        //            {
        //                for (int j = 0; j < LowToHighPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)LowToHighChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }
        //                }
        //            }

        //            Array.Resize(ref White_.trans_BlueData, White_.trans_BlueData.Length + 1);

        //            White_.trans_BlueData[White_.trans_BlueData.Length - 1] = tempB % 13;


        //        }
        //    }

        //    string time1 = ((DateTime.Now - start).TotalMilliseconds / 1000).ToString();

        //    int high_index = 0;
        //    int low_index = 0;
        //    int lowtohigh_index = 0;

        //    string[] temp_string;
        //    string[] split1 = new string[] { "\n" };
        //    bool IsLowToHigh = false;

        //    string Original_Pitch = "";
        //    string Placed_Pitch = "";

        //    DateTime start2 = DateTime.Now;

        //    for (int i = 0; i < White_.original_content_parts.Length; i++)
        //    {
        //        if (White_.original_content_parts[i].Contains("<octave>")) //此字串確認是否為音符
        //        {
        //            temp_string = White_.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);



        //            int Index_ = White_.original_content_parts[i].IndexOf("<pitch>");
        //            string Pitch_StartString = White_.original_content_parts[i].Substring(Index_, White_.original_content_parts[i].Length - Index_);

        //            string V = Pitch_StartString.Substring(Pitch_StartString.IndexOf("<staff>") + 7, 1);
        //            string parameter_step = "";
        //            string parameter_alter = "";
        //            string parameter_octave = "";
        //            string parameter_accidental = "";


        //            /** Pitch參數 -- START  **/

        //            if (V == "1") //高音
        //            {

        //                if (White_.original_content_parts[i].Contains("<attributes>") && i > 0)
        //                {
        //                    IsLowToHigh = White_.original_content_parts[i].Contains("<sign>G</sign>");
        //                }



        //                switch ((ImageProcessType)ImageProcess)
        //                {
        //                    case ImageProcessType.Horizontal:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        break;
        //                    case ImageProcessType.Vertical:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        break;
        //                    case ImageProcessType.Clockwise:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        break;
        //                    case ImageProcessType.Counterclockwise:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        break;
        //                }






        //                high_index += 1;

        //            }
        //            else if (V == "2") //低音
        //            {
        //                if (White_.original_content_parts[i].Contains("<attributes>"))
        //                {
        //                    IsLowToHigh = White_.original_content_parts[i].Contains("<sign>G</sign>");
        //                }
        //                if (IsLowToHigh)
        //                {
        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            break;
        //                    }
        //                    /*  parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];*/
        //                    lowtohigh_index += 1;
        //                }
        //                else
        //                {

        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            break;
        //                    }
        //                    /*
        //                    parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[low_index])];*/
        //                    low_index += 1;
        //                }

        //            }

        //            /** Pitch參數 -- END  **/

        //            if (White_.original_content_parts[i].Contains("<accidental>"))
        //            {


        //                for (int j = 0; j < temp_string.Length; j++)
        //                {

        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        White_.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        White_.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else if (temp_string[j].Contains("<accidental>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<accidental>") + 12, 1) + "\r\n";

        //                        if (!String.IsNullOrEmpty(parameter_accidental))
        //                        {
        //                            White_.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += "," + parameter_accidental + "\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += ",\r\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            White_.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Sakura "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Composer "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            White_.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            White_.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            White_.trans_content += "\n" + temp_string[j];
        //                        }

        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            White_.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                for (int j = 0; j < temp_string.Length; j++)
        //                {
        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        White_.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        White_.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<type"))
        //                    {

        //                        White_.trans_content += $"\n {temp_string[j]}";

        //                        Original_Pitch += ",\r\n";
        //                        if (!String.IsNullOrWhiteSpace(parameter_accidental))
        //                        {
        //                            White_.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += $",{parameter_accidental}\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",\r\n";
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            White_.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Sakura "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Composer "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            White_.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            White_.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            White_.trans_content += "\n" + temp_string[j];
        //                        }
        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            White_.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            if (i != White_.original_content_parts.Length - 1)
        //            {
        //                White_.trans_content += White_.original_content_parts[i] + "\n        " + "</note>";
        //            }
        //            else
        //            {
        //                White_.trans_content += White_.original_content_parts[i];
        //            }

        //        }
        //    }

        //    string time2 = ((DateTime.Now - start2).TotalMilliseconds / 1000).ToString();
        //    /*
        //    SaveFileDialog SF=new SaveFileDialog();
        //    SF.Filter = "musicxml|*.musicxml";
        //    SF.Title = "Export Music File";
        //    SF.InitialDirectory = Savepath;
        //    SF.FileName = FileName;

        //    if (SF.ShowDialog()==true)
        //    {
        //        using (StreamWriter SW = new StreamWriter(SF.FileName))
        //        {
        //            SW.Write(Chopin.trans_content);
        //        }
        //    }*/

        //    string tempfilename = FileName;
        //    string tempfilename_ = tempfilename;
        //    while (File.Exists($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        int n = 0;
        //        if (int.TryParse(tempfilename.Substring(tempfilename.Length - 1, 1), out n))
        //        {
        //            tempfilename_ = tempfilename;
        //            tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
        //        }
        //        else
        //        {
        //            tempfilename += " v2";
        //        }
        //    }

        //    using (StreamWriter SW = new StreamWriter($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        SW.Write(White_.trans_content);
        //    }



        //    /* animated over */
        //    show_animated = false;

        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {

        //        Rectangle_Export.IsEnabled = true;
        //        label_Export.IsEnabled = true;
        //    }));
        //    // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
        //    if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
        //    $"File Location : {Savepath}\\{Title}_{tempfilename}.musicxml\r\n", "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //    {
        //        using (Process P = new Process())
        //        {
        //            P.StartInfo.FileName = "explorer.exe";
        //            P.StartInfo.Arguments = Savepath;
        //            P.Start();
        //        }
        //    }
        //}

        //private void White__Music(string Title, string Satellite, string FileName, int HighChannel, int LowChannel, int LowToHighChannel, int ImageProcess, string Musical_Instrument = "Piano")
        //{




        //    DateTime start = DateTime.Now;

        //    White_.trans_RedData = new float[0];
        //    White_.trans_GreenData = new float[0];
        //    White_.trans_BlueData = new float[0];


        //    White_.trans_content = "";


        //    int Image_Width = LoadBitmap_Before.Width;
        //    int Image_Height = LoadBitmap_Before.Height;


        //    //int Filter_Height = Convert.ToInt32(Math.Truncate(Image_Height / 42.0));
        //    //int Filter_Width = Convert.ToInt32(Math.Truncate(Image_Width / 41.0));

        //    int HighPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 29.0));
        //    int HighPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 29.0));

        //    int LowPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 25.0));
        //    int LowPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 25.0));

        //    int LowToHighPitch_H = Convert.ToInt32(Math.Truncate(Image_Height / 16.0));
        //    int LowToHighPitch_W = Convert.ToInt32(Math.Truncate(Image_Width / 16.0));


        //    /* JAPAN  ImageProcessing */
        //    for (int height = 0; height < 29; height++)
        //    {
        //        for (int width = 0; width < 29; width++)
        //        {
        //            float tempR = 0;

        //            for (int i = 0; i < HighPitch_W; i++)
        //            {
        //                for (int j = 0; j < HighPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)HighChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempR += LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * HighPitch_W + i, height * HighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }

        //                }
        //            }


        //            Array.Resize(ref White_.trans_RedData, White_.trans_RedData.Length + 1);

        //            White_.trans_RedData[White_.trans_RedData.Length - 1] = tempR % 29;

        //        }
        //    }


        //    for (int height = 0; height < 25; height++)
        //    {
        //        for (int width = 0; width < 25; width++)
        //        {

        //            float tempG = 0;

        //            for (int i = 0; i < LowPitch_W; i++)
        //            {
        //                for (int j = 0; j < LowPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)LowChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempG += LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowPitch_W + i, height * LowPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }

        //                }
        //            }

        //                Array.Resize(ref White_.trans_GreenData, White_.trans_GreenData.Length + 1);

        //                White_.trans_GreenData[White_.trans_GreenData.Length - 1] = tempG % 17;

        //        }
        //    }

        //    for (int height = 0; height < 16; height++)
        //    {
        //        for (int width = 0; width < 16; width++)
        //        {

        //            float tempB = 0;
        //            for (int i = 0; i < LowToHighPitch_W; i++)
        //            {
        //                for (int j = 0; j < LowToHighPitch_H; j++)
        //                {
        //                    switch ((ChannelEnum)LowToHighChannel)
        //                    {
        //                        case ChannelEnum.Red:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Green:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Blue:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Cyan:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Magenta:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).B;//Red_2DData[];
        //                            break;
        //                        case ChannelEnum.Yellow:
        //                            tempB += LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).G + LoadBitmap_Before.GetPixel(width * LowToHighPitch_W + i, height * LowToHighPitch_H + j).R;//Red_2DData[];
        //                            break;
        //                    }
        //                }
        //            }

        //                Array.Resize(ref White_.trans_BlueData, White_.trans_BlueData.Length + 1);

        //                White_.trans_BlueData[White_.trans_BlueData.Length - 1] = tempB % 13;


        //        }
        //    }

        //    string time1 = ((DateTime.Now - start).TotalMilliseconds / 1000).ToString();

        //    int high_index = 0;
        //    int low_index = 0;
        //    int lowtohigh_index = 0;

        //    string[] temp_string;
        //    string[] split1 = new string[] { "\n" };
        //    bool IsLowToHigh = false;

        //    string Original_Pitch = "";
        //    string Placed_Pitch = "";

        //    DateTime start2 = DateTime.Now;

        //    for (int i = 0; i < White_.original_content_parts.Length; i++)
        //    {
        //        if (White_.original_content_parts[i].Contains("<octave>")) //此字串確認是否為音符
        //        {
        //            temp_string = White_.original_content_parts[i].Split(split1, StringSplitOptions.RemoveEmptyEntries);



        //            int Index_ = White_.original_content_parts[i].IndexOf("<pitch>");
        //            string Pitch_StartString = White_.original_content_parts[i].Substring(Index_, White_.original_content_parts[i].Length - Index_);

        //            string V = Pitch_StartString.Substring(Pitch_StartString.IndexOf("<staff>") + 7, 1);
        //            string parameter_step = "";
        //            string parameter_alter = "";
        //            string parameter_octave = "";
        //            string parameter_accidental = "";


        //            /** Pitch參數 -- START  **/

        //            if (V == "1") //高音
        //            {

        //                if (White_.original_content_parts[i].Contains("<attributes>") && i > 0)
        //                {
        //                    IsLowToHigh = White_.original_content_parts[i].Contains("<sign>G</sign>");
        //                }



        //                switch ((ImageProcessType)ImageProcess)
        //                {
        //                    case ImageProcessType.Horizontal:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_horizontal[high_index]])];
        //                        break;
        //                    case ImageProcessType.Vertical:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_vertical[high_index]])];
        //                        break;
        //                    case ImageProcessType.Clockwise:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_clockwise[high_index]])];
        //                        break;
        //                    case ImageProcessType.Counterclockwise:
        //                        parameter_step = White_.HighPitch_Step[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_alter = White_.HighPitch_Alter[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_octave = White_.HighPitch_Octave[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        parameter_accidental = White_.HighPitch_Accidental[Convert.ToInt32(White_.trans_RedData[White_.highpitch_index_counterclockwise[high_index]])];
        //                        break;
        //                }






        //                high_index += 1;

        //            }
        //            else if (V == "2") //低音
        //            {
        //                if (White_.original_content_parts[i].Contains("<attributes>"))
        //                {
        //                    IsLowToHigh = White_.original_content_parts[i].Contains("<sign>G</sign>");
        //                }
        //                if (IsLowToHigh)
        //                {
        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_horizontal[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_vertical[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_clockwise[lowtohigh_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[White_.lowtohighpitch_index_counterclockwise[lowtohigh_index]])];
        //                            break;
        //                    }
        //                    /*  parameter_step = White_.LowToHighPitch_Step[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_alter = White_.LowToHighPitch_Alter[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_octave = White_.LowToHighPitch_Octave[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];
        //                      parameter_accidental = White_.LowToHighPitch_Accidental[Convert.ToInt32(White_.trans_BlueData[lowtohigh_index])];*/
        //                    lowtohigh_index += 1;
        //                }
        //                else
        //                {

        //                    switch ((ImageProcessType)ImageProcess)
        //                    {
        //                        case ImageProcessType.Horizontal:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_horizontal[low_index]])];
        //                            break;
        //                        case ImageProcessType.Vertical:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_vertical[low_index]])];
        //                            break;
        //                        case ImageProcessType.Clockwise:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_clockwise[low_index]])];
        //                            break;
        //                        case ImageProcessType.Counterclockwise:
        //                            parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[White_.lowpitch_index_counterclockwise[low_index]])];
        //                            break;
        //                    }
        //                    /*
        //                    parameter_step = White_.LowPitch_Step[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_alter = White_.LowPitch_Alter[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_octave = White_.LowPitch_Octave[Convert.ToInt32(White_.trans_GreenData[low_index])];
        //                    parameter_accidental = White_.LowPitch_Accidental[Convert.ToInt32(White_.trans_GreenData[low_index])];*/
        //                    low_index += 1;
        //                }

        //            }

        //            /** Pitch參數 -- END  **/

        //            if (White_.original_content_parts[i].Contains("<accidental>"))
        //            {


        //                for (int j = 0; j < temp_string.Length; j++)
        //                {

        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        White_.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        White_.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else if (temp_string[j].Contains("<accidental>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<accidental>") + 12, 1) + "\r\n";

        //                        if (!String.IsNullOrEmpty(parameter_accidental))
        //                        {
        //                            White_.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += "," + parameter_accidental + "\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += ",\r\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            White_.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Sakura "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Composer "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            White_.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            White_.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            White_.trans_content += "\n" + temp_string[j];
        //                        }

        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            White_.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                for (int j = 0; j < temp_string.Length; j++)
        //                {
        //                    if (temp_string[j].Contains("<step>"))
        //                    {

        //                        Original_Pitch += temp_string[j].Substring(temp_string[j].IndexOf("<step>") + 6, 1);
        //                        Placed_Pitch += parameter_step;
        //                        White_.trans_content += $"\n          <step>{parameter_step}</step>";
        //                        if (!temp_string[j + 1].Contains("<alter>"))
        //                        {
        //                            Original_Pitch += ",";
        //                            if (parameter_alter != "")
        //                            {
        //                                White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";

        //                                Placed_Pitch += $",{parameter_alter}";
        //                            }
        //                            else
        //                            {
        //                                Placed_Pitch += $",";
        //                            }
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<alter>"))
        //                    {

        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<alter>") + 7, 2);
        //                        if (parameter_alter != "")
        //                        {
        //                            White_.trans_content += $"\n          <alter>{parameter_alter}</alter>";
        //                            Placed_Pitch += $",{parameter_alter}";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",";
        //                        }
        //                    }
        //                    else if (temp_string[j].Contains("<octave>"))
        //                    {
        //                        Original_Pitch += "," + temp_string[j].Substring(temp_string[j].IndexOf("<octave>") + 8, 1);
        //                        Placed_Pitch += $",{parameter_octave}";
        //                        White_.trans_content += $"\n          <octave>{parameter_octave}</octave>";
        //                    }
        //                    else if (temp_string[j].Contains("<type"))
        //                    {

        //                        White_.trans_content += $"\n {temp_string[j]}";

        //                        Original_Pitch += ",\r\n";
        //                        if (!String.IsNullOrWhiteSpace(parameter_accidental))
        //                        {
        //                            White_.trans_content += $"\n        <accidental>{parameter_accidental}</accidental>";
        //                            Placed_Pitch += $",{parameter_accidental}\r\n";
        //                        }
        //                        else
        //                        {
        //                            Placed_Pitch += $",\r\n";
        //                        }

        //                    }
        //                    else if (temp_string[j].Contains("<dot/>"))
        //                    {

        //                    }
        //                    else
        //                    {
        //                        if (i == 0 && j == 0)
        //                        {
        //                            White_.trans_content += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        //                        }
        //                        else if (temp_string[j].Contains("Sakura "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"595.4\" default-y=\"1627.12\" justify=\"center\" valign=\"top\" font-family=\"Times New Roman\" font-size=\"22\">{Title}</credit-words>";

        //                        }
        //                        else if (temp_string[j].Contains("Composer "))
        //                        {
        //                            White_.trans_content += $"\n    <credit-words default-x=\"1134.11\" default-y=\"1527.12\" justify=\"right\" valign=\"top\" font-family=\"Times New Roman\">{Satellite}</credit-words>";
        //                        }
        //                        else if (temp_string[j].Contains("<part-name>"))
        //                        {
        //                            White_.trans_content += $"\n      <part-name>{Musical_Instrument}</part-name>";
        //                        }
        //                        else if (temp_string[j].Contains("<instrument-name>"))
        //                        {
        //                            White_.trans_content += $"\n        <instrument-name>{Musical_Instrument}</instrument-name>";
        //                        }
        //                        else
        //                        {
        //                            White_.trans_content += "\n" + temp_string[j];
        //                        }
        //                        if (j == temp_string.Length - 1)
        //                        {
        //                            White_.trans_content += "        </note>";
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            if (i != White_.original_content_parts.Length - 1)
        //            {
        //                White_.trans_content += White_.original_content_parts[i] + "\n        " + "</note>";
        //            }
        //            else
        //            {
        //                White_.trans_content += White_.original_content_parts[i];
        //            }

        //        }
        //    }

        //    string time2 = ((DateTime.Now - start2).TotalMilliseconds / 1000).ToString();
        //    /*
        //    SaveFileDialog SF=new SaveFileDialog();
        //    SF.Filter = "musicxml|*.musicxml";
        //    SF.Title = "Export Music File";
        //    SF.InitialDirectory = Savepath;
        //    SF.FileName = FileName;

        //    if (SF.ShowDialog()==true)
        //    {
        //        using (StreamWriter SW = new StreamWriter(SF.FileName))
        //        {
        //            SW.Write(Chopin.trans_content);
        //        }
        //    }*/

        //    string tempfilename = FileName;
        //    string tempfilename_ = tempfilename;
        //    while (File.Exists($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        int n = 0;
        //        if (int.TryParse(tempfilename.Substring(tempfilename.Length - 1, 1), out n))
        //        {
        //            tempfilename_ = tempfilename;
        //            tempfilename = tempfilename_.Substring(0, tempfilename_.Length - 1) + (n + 1).ToString();
        //        }
        //        else
        //        {
        //            tempfilename += " v2";
        //        }
        //    }

        //    using (StreamWriter SW = new StreamWriter($"{Savepath}\\{Title}_{tempfilename}.musicxml"))
        //    {
        //        SW.Write(White_.trans_content);
        //    }



        //    /* animated over */
        //    show_animated = false;

        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {

        //        Rectangle_Export.IsEnabled = true;
        //        label_Export.IsEnabled = true;
        //    }));
        //    // + $"first:  {time1} ; second: {time2} ; Total : {((DateTime.Now - start).TotalMilliseconds / 1000).ToString()}"
        //    if (MessageBox.Show($"Do you want to direct to the file location?\r\n" +
        //    $"File Location : {Savepath}\\{Title}_{tempfilename}.musicxml\r\n", "Conversion Succeeded", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //    {
        //        using (Process P = new Process())
        //        {
        //            P.StartInfo.FileName = "explorer.exe";
        //            P.StartInfo.Arguments = Savepath;
        //            P.Start();
        //        }
        //    }
        //}

        int Image_Index = 1;

        private void UpdateImageBrush(string PicName)
        {
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/Resources/{PicName}.jpg"));
            imageBrush.Stretch = Stretch.UniformToFill;
        }


     
       

    
        

        private void Rectangle_Export_MouseDown(object sender, MouseButtonEventArgs e)
        {
            show_animated = false;
            Rectangle_Export.Fill = new  System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)150,(byte)150,(byte)150));
            this.Cursor = Cursors.Wait;
        }
        public static bool show_animated = false;

        private void Loading_Animation()
        {
            /*
            Dispatcher.BeginInvoke(new Action(() =>
            {
                

                image_Mozart.Visibility = Visibility.Visible;
                image_Mozart.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/M1.png"));

                image_Beethoven.Visibility = Visibility.Visible;
                image_Beethoven.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/B1.png"));

                image_Chopin.Visibility = Visibility.Visible;
                image_Chopin.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/C1.png"));


                image_Sakura.Visibility = Visibility.Visible;
                image_Sakura.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/S1.png"));

                image_Clap.Visibility = Visibility.Visible;
                image_Clap.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/K1.png"));


                label_Loading.Visibility = Visibility.Visible;


                label_Loading.Content = "Loading";

            }));
            int index = 1;
            
            
            switch (icbo_Musician)
            {
                case 0:
                    while (show_animated)
                    {
                        index++;
                        if (index > 16)
                        {
                            index = 1;
                        }

                        Thread.Sleep(500);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            image_Chopin.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/C{index}.png"));

                            switch (index%7)
                            {
                                case 1:
                                    label_Loading.Content = "Loading";
                                    break;
                                case 2:
                                    label_Loading.Content = "Loading .";
                                    break;
                                case 3:
                                    label_Loading.Content = "Loading . .";
                                    break;
                                case 4:
                                    label_Loading.Content = "Loading . . .";
                                    break;
                                case 5:
                                    label_Loading.Content = "Loading . . . .";
                                    break;
                                case 6:
                                    label_Loading.Content = "Loading . . . . .";
                                    break;

                            }
                            


                        }));



                    }
                    break;
                case 1:
                    while (show_animated)
                    {
                        index++;
                        if (index > 17)
                        {
                            index = 1;
                        }

                        Thread.Sleep(500);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            image_Beethoven.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/B{index}.png"));
                            switch (index % 7)
                            {
                                case 1:
                                    label_Loading.Content = "Loading";
                                    break;
                                case 2:
                                    label_Loading.Content = "Loading .";
                                    break;
                                case 3:
                                    label_Loading.Content = "Loading . .";
                                    break;
                                case 4:
                                    label_Loading.Content = "Loading . . .";
                                    break;
                                case 5:
                                    label_Loading.Content = "Loading . . . .";
                                    break;
                                case 6:
                                    label_Loading.Content = "Loading . . . . .";
                                    break;

                            }
                        }));

                    }
                   
                    break;
                case 2:
                    while (show_animated)
                    {
                        index++;
                        if (index > 17)
                        {
                            index = 1;
                        }

                        Thread.Sleep(500);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            image_Mozart.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/M{index}.png"));
                            switch (index % 7)
                            {
                                case 1:
                                    label_Loading.Content = "Loading";
                                    break;
                                case 2:
                                    label_Loading.Content = "Loading .";
                                    break;
                                case 3:
                                    label_Loading.Content = "Loading . .";
                                    break;
                                case 4:
                                    label_Loading.Content = "Loading . . .";
                                    break;
                                case 5:
                                    label_Loading.Content = "Loading . . . .";
                                    break;
                                case 6:
                                    label_Loading.Content = "Loading . . . . .";
                                    break;

                            }
                        }));
                    }
                    break;
                case 3:
                    while (show_animated)
                    {
                        index++;
                        if (index > 17)
                        {
                            index = 1;
                        }

                        Thread.Sleep(500);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            image_Sakura.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/S{index}.png"));

                            switch (index % 7)
                            {
                                case 1:
                                    label_Loading.Content = "Loading";
                                    break;
                                case 2:
                                    label_Loading.Content = "Loading .";
                                    break;
                                case 3:
                                    label_Loading.Content = "Loading . .";
                                    break;
                                case 4:
                                    label_Loading.Content = "Loading . . .";
                                    break;
                                case 5:
                                    label_Loading.Content = "Loading . . . .";
                                    break;
                                case 6:
                                    label_Loading.Content = "Loading . . . . .";
                                    break;

                            }



                        }));



                    }
                    break;
                case 4:
                    while (show_animated)
                    {
                        index++;
                        if (index > 16)
                        {
                            index = 1;
                        }

                        Thread.Sleep(500);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            image_Clap.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/K{index}.png"));

                            switch (index % 7)
                            {
                                case 1:
                                    label_Loading.Content = "Loading";
                                    break;
                                case 2:
                                    label_Loading.Content = "Loading .";
                                    break;
                                case 3:
                                    label_Loading.Content = "Loading . .";
                                    break;
                                case 4:
                                    label_Loading.Content = "Loading . . .";
                                    break;
                                case 5:
                                    label_Loading.Content = "Loading . . . .";
                                    break;
                                case 6:
                                    label_Loading.Content = "Loading . . . . .";
                                    break;

                            }



                        }));



                    }
                    break;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                image_Mozart.Visibility = Visibility.Hidden;
                
                image_Beethoven.Visibility = Visibility.Hidden;
                
                image_Chopin.Visibility = Visibility.Hidden;

                image_Clap.Visibility = Visibility.Hidden;

                image_Sakura.Visibility = Visibility.Hidden;

                label_Loading.Visibility = Visibility.Hidden;


                ShowMainArea(Visibility.Visible);
            }));
            */
        }

        

        private  void Rectangle_Export_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Export_Gif_Media_Mozart.Visibility = Visibility.Visible;


            //export_function();
            if (LoadBitmap_Before == null || LoadBitmap_After == null || Rectangle_After.Fill == null || Rectangle_Before.Fill == null)
            {
                Rectangle_Export.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)223, (byte)227, (byte)232));

                Rectangle_Export.IsEnabled = true;
                label_Export.IsEnabled = true;
                MessageBox.Show("Please check images and \"File Name\"");
                return;
            }
            if (textBox_Filename.Text != "" && textBox_Composer.Text != "")
            {
                show_animated = true;


                icbo_Musician = Cbo_Musician.SelectedIndex;
                icbo_ImageProcess = Cbo_ImageProcess.SelectedIndex;
                icbo_Instrument = Cbo_Instrument.SelectedIndex;
                FileName_xml = textBox_Filename.Text;
                //ShowMainArea(Visibility.Hidden);
/*
                Loading_Thread = new Thread(Loading_Animation);


                Loading_Thread.Start();*/


                Rectangle_Export.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)223, (byte)227, (byte)232));

                Rectangle_Export.IsEnabled = false;
                label_Export.IsEnabled = false;

                textBox_afterlink.IsEnabled = false;
                textBox_beforelink.IsEnabled = false;
                //DispatcherHelper.DoEvents();
                switch (icbo_Musician)
                {
                    case 0:
                        White_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;
                    case 1:
                        Africa_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;
                    case 2:
                        Asian_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;

                }
            }

            
            //Thread s2 = new Thread(export_function);
            //s2.Start();

        }

        private void label_Export_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            show_animated = false;
            //Export_Gif_Media_Mozart.Visibility = Visibility.Hidden;
            Rectangle_Export.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)150, (byte)150, (byte)150));

        }

        private void label_Export_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LoadBitmap_Before == null || LoadBitmap_After == null || Rectangle_After.Fill == null || Rectangle_Before.Fill == null)
            {
                Rectangle_Export.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)223, (byte)227, (byte)232));
                this.Cursor = Cursors.Arrow;
                Rectangle_Export.IsEnabled = true;
                label_Export.IsEnabled = true;
                MessageBox.Show("Please check images, \"File Name\"");
                return;
            }
            if (textBox_Filename.Text != "" && textBox_Composer.Text != "")
            {
                show_animated = true;


                icbo_Musician = Cbo_Musician.SelectedIndex;
                icbo_ImageProcess = Cbo_ImageProcess.SelectedIndex;
                
                //ShowMainArea(Visibility.Hidden);
/*
                Loading_Thread = new Thread(Loading_Animation);


                Loading_Thread.Start();*/


                Rectangle_Export.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)223, (byte)227, (byte)232));

                Rectangle_Export.IsEnabled = false;
                label_Export.IsEnabled = false;
                textBox_afterlink.IsEnabled = false;
                textBox_beforelink.IsEnabled = false;
                
                //DispatcherHelper.DoEvents();
                switch (icbo_Musician)
                {
                    case 0:
                        White_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;
                    case 1:
                        Africa_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;
                    case 2:
                        Asian_Music(textBox_Filename.Text, textBox_Composer.Text, icbo_ImageProcess);
                        break;

                }
               
            }

        }

      


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            show_animated = false;
           
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                image_Clap.Margin = new Thickness(0, 120, 220 + (e.NewSize.Width - 1000) / 2, 0);
                image_Sakura.Margin = new Thickness(220 + (e.NewSize.Width - 1000) / 2, 120, 0, 0);
            }
           
        }

        private void textBox_beforelink_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //LoadBeforeImage(textBox_beforelink.Text);
                LoadBeforeImage(textBox_beforelink.Text);

            }
            catch (Exception)
            {
                LoadBitmap_Before = null;
                Rectangle_Before.Fill = null;
                MessageBox.Show("Couldn't Load Image From This URL.");
            }
        }

        private void textBox_beforelink_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                LoadAfterImage(textBox_afterlink.Text);


            }
            catch (Exception)
            {
                LoadBitmap_After = null;
                Rectangle_After.Fill = null;
                MessageBox.Show("Couldn't Load Image From This URL.");
            }
        }

        private void button_after_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void button_before_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public static class DispatcherHelper
    {
       
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,new DispatcherOperationCallback(ExitFrames), frame);
            try
            {
                Dispatcher.PushFrame(frame);
            }
            catch (InvalidOperationException) { }
        }
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }

      
    }
}
