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
using System.Windows.Threading; //DispactherTimerı kullanabilmek için eklediğimiz kütüphane

namespace PacmanHomework
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer(); //timer oluşturma karakterin otomatik ilerlemesi için
        bool soladon, sagadon, asagıdon, yukarıdon; //yön komut degiskenleri true false seklinde kullanılacak
        bool soladonme, sagadonme, asagıdonme, yukarıdonme; //o yöne gitmemesini durdurmak icin kulladıgımız degiskenler
        int hiz = 8; //hiz degiskeni karakterin hizini belirlemek icin

        Rect pacmanHitBox; //pacmanin hitboxu tanımlandı bir yere dokundugununda algılaması icin kullanacağız
        int dusmanHizi = 10; //hayaletlerin hizi
        int dusmanHareketi = 130; //hayaletlerin ne kadar gideceginin limiti
        int mevcutDusmanHareketi; //hayaletlerin o anki limiti, limite ne kadar uzak yakın nerede limite ulasıcagını kontrol etmemiz icin gereken degisken
        int skor = 0; //skor degiskeni tanımlandı

         

        public MainWindow()
        {
            InitializeComponent(); //initilaze edildi ekranımız
            GameSetup(); // işlemlerin oldugu gamesetup fonksiyonu cagrıldı
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && soladonme == false)
            {
                sagadon = yukarıdon = asagıdon = false;
                sagadonme = yukarıdonme = asagıdonme = false;

                soladon = true;
                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Right && sagadonme == false)
            {
                soladon = yukarıdon = asagıdon = false;
                soladonme = yukarıdonme = asagıdonme = false;

                sagadon = true;
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Up && yukarıdonme == false)
            {
                sagadon = soladon = asagıdon = false;
                sagadonme = soladonme = asagıdonme = false;

                yukarıdon = true;
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Down && asagıdonme == false)
            {
                sagadon = yukarıdon = soladon = false;
                sagadonme = yukarıdonme = soladonme = false;

                asagıdon = true;
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);

            }

        }
        private void GameSetup() //program yüklemelerinin yapıldıgı fonksiyon diyebiliriz
        {
            MyCanvas.Focus(); //canvasa focuslandı
            timer.Tick += GameLoop; //zaman her arttıgında oyunun islemlerinin oldugu gameloop fonkisyonu cagırılır
            timer.Interval = TimeSpan.FromMilliseconds(20); //20msde bir oldugunu ifade eder zaman degiskenin
            timer.Start(); //zamanı baslatır
            mevcutDusmanHareketi = dusmanHareketi;

            ImageBrush pacmanImage = new ImageBrush(); //Image nesnesi olusturur
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg")); //olustulan nesneyi bitmapImage ile resimin konumunu yazıp jpg dosyasını baglarız.
            pacman.Fill = pacmanImage; //resmi oyundaki nesnemizin üstüne doldurur

            ImageBrush kirmiziImage = new ImageBrush();
            kirmiziImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            kirmizi.Fill = kirmiziImage;

            ImageBrush turuncuImage = new ImageBrush();
            turuncuImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            turuncu.Fill = turuncuImage;

            ImageBrush pembeImage = new ImageBrush();
            pembeImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pembe.Fill = pembeImage;

        }

        private void GameLoop(object sender, EventArgs e) //oyun icindeki hareketlerin etkinliklerin gerceklestigi kontrol edildigi fonksiyon
        {
            Score.Content = "Skor: " + skor; //skoru ekranda gösterir
            //Hareket kosulları
            if (sagadon)
            {
                Canvas.SetLeft(pacman,Canvas.GetLeft(pacman)+hiz); //eğer sağa gitme booleını doğru ise hizi sola ekleyerek karakterin sağa gitmesini sağlar
            }
            if (soladon)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - hiz); //eğer sola gitme booleını doğru ise hizi soldan cıkartarak karakterin sola gitmesini sağlar
            }
            if (yukarıdon)
            {
                Canvas.SetTop(pacman,Canvas.GetTop(pacman)-hiz); //eğer yukarı gitme booleını doğru ise hizi yukarıdan cıkartarak karakterin yukarı gitmesini sağlar
            }
            if (asagıdon)
            {
                Canvas.SetTop(pacman,Canvas.GetTop(pacman)+hiz); //eğer asagı gitme booleını doğru ise hizi yukarı ekleyerek karakterin asagı gitmesini sağlar
            }
            //Hareketi kısıtlama kosulları
            if(asagıdon && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height) //pacman asagı hareket ediyorsa pencere yüksekligini asmaması acısından asagı hareketi durdurur.
            {
                asagıdonme = true;
                asagıdon = false;
            }
            if(yukarıdon && Canvas.GetTop(pacman) < 1) //pacman hakeret ediyorsa ve pacmanin konumu 1 den küçükse hareketi dururur.
            {
                yukarıdonme = false;
                yukarıdon = false;
            }
            if(soladon && Canvas.GetLeft(pacman) -10 < 1) //pacman sola hareket ediyorsa ve pozisyon konumu 1den küçükse hareketi durdurur.
            {
                soladonme = true;
                soladon = false;    
            }
            if(sagadon && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width) //pacman saga hareket ediyorsa ve pozisyon konumu anapencereden büyükse hareketi durdurur.
            {
                sagadonme = true;
                sagadon = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); //pacman hitboxunı pacman dikdörtgenine atanması

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())// oyunun içindeki tüm dikdörtgenleri tarayacak olan ana oyun döngüsü
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x),Canvas.GetTop(x),x.Width,x.Height); //oyunun icindeki tüm dikdörgenler icin hitbox olusturur

                if((string)x.Tag == "engel") //eğer tagi engelse
                {
                    if (soladon == true && pacmanHitBox.IntersectsWith(hitBox)) //sola giderken engele carpıp carpmadıgı kontol eder carparsa durur
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        soladonme = true;
                        soladon = false;
                    }
                    if (sagadon == true && pacmanHitBox.IntersectsWith(hitBox)) //saga giderken engele carpıp carpmadıgı kontol eder carparsa durur
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        sagadonme = true;
                        sagadon = false;
                    }
                    if (asagıdon == true && pacmanHitBox.IntersectsWith(hitBox)) //asagı giderken engele carpıp carpmadıgı kontol eder carparsa durur
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        asagıdonme = true;
                        asagıdon = false;
                    }
                    if (yukarıdon == true && pacmanHitBox.IntersectsWith(hitBox)) //yukarı giderken engele carpıp carpmadıgı kontol eder carparsa durur
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        yukarıdonme = true;
                        yukarıdon = false;
                    }
                }
                if ((string)x.Tag == "altin") //eger tagi altınsa
                {
                    
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible) //pacman altınlardan herhangi biriyle çarpışırsa ve altınlar ekranda hala görünür durumdaysa
                    {
                        
                        x.Visibility = Visibility.Hidden; //altınları gorunmez yapar
                       
                        skor++; //skoru arttırır
                    }
                }
                if ((string)x.Tag == "dusman") //eger tagi dusmansa
                {
                    
                    if (pacmanHitBox.IntersectsWith(hitBox)) //pacmanın hayaletle carpısıp carpısmadıgını kontrol eder
                    {
                       
                        GameOver("Dusman tarafından yakalandın,Yeniden oynamak icin tıklayın.."); //eger carpısma olmussa gameover fonksiyopnu cagırılır.
                    }

                    
                    if (x.Name.ToString() == "turuncu")
                    {
                       
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - dusmanHizi); //turuncu hayaleti sola hareket ettirir

                    }
                    else
                    {
                       
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + dusmanHizi); //digerlerini saga hareket ettirir
                    }

                    
                    mevcutDusmanHareketi--; //hayaletin mevcut konumunu azaltır her seferinde limite gelip gelmedigini kontrol etmek icin

                    
                    if (mevcutDusmanHareketi < 1) //1in altına düserse 
                    {
                        
                        mevcutDusmanHareketi = dusmanHareketi; //resetlemis olur
                        
                        dusmanHizi = -dusmanHizi; //ters yönde vermis olur
                    }
                }
            }
            if(skor == 36) //kac altin varsa ona ulastıgında oyunu sonlandırır
            {
                GameOver("Kazandınız..");
            }

        
        }

        private void GameOver(string mesaj) //oyun sonlandıgındaki bölüm
        {
            timer.Stop();
            MessageBox.Show(mesaj, "Pacman Odevi");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }
    }
}
