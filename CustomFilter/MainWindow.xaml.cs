using CustomFilter.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace CustomFilter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //Запуск

        List<CheckBox> CheckBoxClassArmor = new List<CheckBox>();
        List<CheckBox> CheckBoxClassWeaBij = new List<CheckBox>();
        List<CheckBox> CheckBoxBaseType = new List<CheckBox>();
        List<CheckBox> CheckBoxGlobal = new List<CheckBox>();
        List<Banner> bannerlist = new List<Banner>();
        List<StackPanel> stacklist = new List<StackPanel>();
        List<CheckBox> CheckBoxOther = new List<CheckBox>();
        string savefilter = "SaveOption.cfs";
        string savefolder = "Saves Custom Filter";
        string savefavorite = "SaveFavotite.cfs";
        string saveglobal = "SaveGlobal.cfs";
        string filefilter = "CustomFilter.filter";
        string filepath = "config.cff";
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();
        SolidColorBrush Normal = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        SolidColorBrush Magic = new SolidColorBrush(Color.FromArgb(255, 136, 136, 255));
        SolidColorBrush Rare = new SolidColorBrush(Color.FromArgb(255, 255, 255, 119));
        SolidColorBrush Unique = new SolidColorBrush(Color.FromArgb(255, 180, 96, 0));
        string[] separators = { " " };
        string levelcheck;
        int t = 1;
        int protCount = 0;
        int armorCount = 0;

        int items = 4;
        int qua = 2;
        int flas = 18;

        public MainWindow()
        {
            InitializeComponent();

            string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            //Загрузка пути к папке
            {
                if (Settings.Default.folder == "")
                {
                    folderbox.Text = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\Path of Exile\\";
                }
                else
                {
                    folderbox.Text = Settings.Default.folder;
                }
            }
            CheckBoxGlobal.Add(fullscreen);
            CheckBoxGlobal.Add(maxsize);
            CheckBoxGlobal.Add(shapersound);
            CheckBoxGlobal.Add(rayshow);
            CheckBoxGlobal.Add(icoshow);

            CheckBoxGlobal.Add(setshelmets);
            CheckBoxGlobal.Add(setsarmors);
            CheckBoxGlobal.Add(setsgloves);
            CheckBoxGlobal.Add(setsboots);
            CheckBoxGlobal.Add(setsweapons);
            CheckBoxGlobal.Add(setsbelts);
            CheckBoxGlobal.Add(setsamulets);
            CheckBoxGlobal.Add(setsrings);


            foreach (CheckBox chb in prot.Children)
            {
                CheckBoxGlobal.Add(chb);
                CheckBoxBaseType.Add(chb);
            }

            foreach (CheckBox chb in armor.Children)
            {
                CheckBoxGlobal.Add(chb);
                CheckBoxClassArmor.Add(chb);
            }

            foreach (CheckBox chb in bija.Children)
            {
                CheckBoxGlobal.Add(chb);
                CheckBoxClassWeaBij.Add(chb);
            }

            foreach (CheckBox chb in weapon.Children)
            {
                CheckBoxGlobal.Add(chb);
                CheckBoxClassWeaBij.Add(chb);
            }

            if (qualityflasksslider.Value == 0)
            {
                qualityflasks.Content = "Качество флаконов: любое";
            }


            //Разделение по банерам
            for (int i = 0; i<lines.Length; i++)
            {
                if (lines[i].Contains("#") && lines[i].Contains(":"))
                {
                    if (lines[i+1].Length == 0)
                    {
                        bannerlist.Add(new Banner(i, lines));
                    }
                }
            }
            //Отрисовка чекбоксов
            for (int i = 0; i < bannerlist.Count; i++)
            {
                stacklist.Add(new StackPanel() { Background = new SolidColorBrush(Color.FromArgb(255, 41, 41, 41)), Margin = new Thickness(0, 0, 5, 5), Width = 200 });
                stacklist[i].Children.Add(new Label() { Content = bannerlist[i].bannername.Remove(0, 2) });
                foreach (Group g in bannerlist[i].grouplist)
                {
                    string ray = "none";
                    string icocolor = "none";
                    string icoform = "none";
                    foreach (string elem in g.regionlist[0].elementlist)
                    {
                        if (elem.Contains("PlayEffect"))
                        {
                            string[] rayelem = elem.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            ray = rayelem[1];
                        }
                        if (elem.Contains("MinimapIcon"))
                        {
                            string[] icoelem = elem.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            icocolor = icoelem[2];
                            icoform = icoelem[3];
                        }
                    }
                    
                    bannerlist[i].checklist.Add(new CheckBoxOptions(true, g.groupname.Remove(0, 2), g.groupcomment.Remove(0, 2), g.regionlist[0].PlayAlertSound[1],
                        Byte.Parse(g.regionlist[0].SetBorderColor[4]), Byte.Parse(g.regionlist[0].SetBorderColor[1]), Byte.Parse(g.regionlist[0].SetBorderColor[2]), Byte.Parse(g.regionlist[0].SetBorderColor[3]),
                        Byte.Parse(g.regionlist[0].SetBackgroundColor[4]), Byte.Parse(g.regionlist[0].SetBackgroundColor[1]), Byte.Parse(g.regionlist[0].SetBackgroundColor[2]), Byte.Parse(g.regionlist[0].SetBackgroundColor[3]),
                        Byte.Parse(g.regionlist[0].SetTextColor[4]), Byte.Parse(g.regionlist[0].SetTextColor[1]), Byte.Parse(g.regionlist[0].SetTextColor[2]), Byte.Parse(g.regionlist[0].SetTextColor[3]),
                        ray, icocolor, icoform, Checked, Unchecked, Setschecked, Setsunchecked, "rare", "magic", "normal",
                        Itemschecked, Itemsunchecked, Gemschecked, Gemsunchecked, Quachecked, Quaunchecked, Flaskchecked, Flaskunchecked, Hightlevelchecked, Hightlevelunchecked));
                }
                foreach (CheckBoxOptions ch in bannerlist[i].checklist)
                {
                    stacklist[i].Children.Add(ch.st);
                }
                //stacklist[i].Children.Add(new StackPanel());
                stacklist[i].Children.Add(new Label());

                wrappanel.Children.Add(stacklist[i]);
            }

            //Загрузка избранного
            string fav;
            if (File.Exists(folderbox.Text + savefolder + "\\" + savefavorite))
            {
                fav = folderbox.Text + savefolder + "\\" + savefavorite;
            }
            else
            {
                fav = "BaseTypes\\BaseFavotite.cfs";
            }
            string[] favoriteline = File.ReadAllLines(fav);
            for (int s = 0; s < favoriteline.Length; s++)
            {
                if (favoriteline[s].Contains("#"))
                {
                    if (favoriteline[s + 2].Contains("Any"))
                    {
                        favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Brushes.Gray, "Any", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                    }
                    if (favoriteline[s + 2].Contains("Normal"))
                    {
                        favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Normal, "Normal", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                    }
                    if (favoriteline[s + 2].Contains("Magic"))
                    {
                        favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Magic, "Magic", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                    }
                    if (favoriteline[s + 2].Contains("Rare"))
                    {
                        favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Rare, "Rare", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                    }
                    if (favoriteline[s + 2].Contains("Unique"))
                    {
                        favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Unique, "Unique", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                    }
                }
            }

            //Загрузка уровней
            if (File.Exists(folderbox.Text + savefolder + "\\" + saveglobal))
            {
                string[] globalline = File.ReadAllLines(folderbox.Text + savefolder + "\\" + saveglobal);
                foreach(string s in globalline)
                {
                    if (s.Contains("itemlevelfrom"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        itemlevelfrom.Text = editline[2];
                    }
                    if (s.Contains("itemlevelto"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        itemlevelto.Text = editline[2];
                    }
                    if (s.Contains("bijlevelfrom"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        bijlevelfrom.Text = editline[2];
                    }
                    if (s.Contains("bijlevelto"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        bijlevelto.Text = editline[2];
                    }
                    if (s.Contains("hightlevel"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        hightlevel.Text = editline[2];
                    }
                }
            }
            //Загрузка слайдеров
            if (File.Exists(folderbox.Text + savefolder + "\\" + saveglobal))
            {
                string[] sliderline = File.ReadAllLines(folderbox.Text + savefolder + "\\" + saveglobal);
                foreach (string s in sliderline)
                {
                    if (s.Contains("qualityVolume"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        allvolumeslider.Value = Convert.ToDouble(editline[2]);
                    }
                    if (s.Contains("qualityFlasks"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        qualityflasksslider.Value = Convert.ToDouble(editline[2]);
                    }
                    if (s.Contains("qualityGems"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        qualitygemsslider.Value = Convert.ToDouble(editline[2]);
                    }
                    if (s.Contains("qualityItems"))
                    {
                        string[] editline = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        qualityitemsslider.Value = Convert.ToDouble(editline[2]);
                    }
                }
            }
            //Загрузка чекбоксов
            if (File.Exists(folderbox.Text + savefolder + "\\" + savefilter))
            {
                string[] savelines = File.ReadAllLines(folderbox.Text + savefolder + "\\" + savefilter);
                foreach (Banner b in bannerlist)
                {
                    foreach (CheckBoxOptions c in b.checklist)
                    {
                        foreach (string s in savelines)
                        {
                            if (s.Contains(c.chechbox.Content.ToString()))
                            {
                                if (s.Contains("True"))
                                {
                                    c.chechbox.IsChecked = true;
                                    c.chechbox.Opacity = 1;
                                }
                                else
                                {
                                    c.chechbox.IsChecked = false;
                                    c.chechbox.Opacity = 0.5;
                                }
                            }
                        }
                    }
                }

                foreach (CheckBox c in CheckBoxGlobal)
                {
                    foreach (string s in savelines)
                    {
                        if (s.Contains(c.Content.ToString()))
                        {
                            if (s.Contains("True"))
                            {
                                c.IsChecked = true;
                            }
                            else
                            {
                                c.IsChecked = false;
                            }
                        }
                    }
                }
            }
            //Загрузка глобальных настроек
            if (File.Exists(folderbox.Text + savefolder + "\\" + saveglobal))
            {
                string[] globaleline = File.ReadAllLines(folderbox.Text + savefolder + "\\" + saveglobal);
                foreach(CheckBox c in CheckBoxGlobal)
                {
                    foreach (string s in globaleline)
                    {
                        if (s.Contains(c.Name.ToString()))
                        {
                            if (s.Contains("True"))
                            {
                                c.IsChecked = true;
                            }
                            else
                            {
                                c.IsChecked = false;
                            }
                        }
                    }
                }
            }

            foreach (CheckBox chb in prot.Children)
            {
                if (chb.IsChecked == true)
                {
                    protCount++;
                }
            }
            Console.WriteLine(protCount);

            foreach (CheckBox chb in armor.Children)
            {
                if (chb.IsChecked == true)
                {
                    armorCount++;
                }
            }
            Console.WriteLine(armorCount);

            dispatcherTimer.Tick += new EventHandler(Run);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();
        }


        //Сохранение
        List<string> write = new List<string>();
        List<string> save = new List<string>();
        List<string> saveglob = new List<string>();
        List<string> savefav = new List<string>();

        void Run(object sender, EventArgs e)
        {
            if (t == 1)
            {
                money.Opacity = money.Opacity - 0.01;
                if (money.Opacity <= 0.05)
                {
                    t = 0;
                }
            }
            else if (t == 0)
            {
                money.Opacity = money.Opacity + 0.01;
                if (money.Opacity >= 0.5)
                {
                    t = 1;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Проверка бижутерии на сеты
            if (setsbelts.IsChecked == false && setsrings.IsChecked == false && setsamulets.IsChecked == false)
            {
                foreach (Banner b in bannerlist)
                {
                    foreach (CheckBoxOptions check in b.checklist)
                    {
                        if (check.chechbox.Content.ToString().Contains("Бижутерия на сеты") && check.chechbox.IsChecked == true)
                        {
                            MessageBox.Show("Так как отображение амулетов, колец и поясов на сеты отключены, пункт \"Бижутерия на сеты\" будет автоматически отключен.");
                            check.chechbox.IsChecked = false;
                        }
                    }
                }
            }
            //Проверка брони на сеты
            if (setshelmets.IsChecked == false && setsarmors.IsChecked == false && setsgloves.IsChecked == false && setsboots.IsChecked == false && setsweapons.IsChecked == false)
            {
                foreach (Banner b in bannerlist)
                {
                    foreach (CheckBoxOptions check in b.checklist)
                    {
                        if (check.chechbox.Content.ToString().Contains("Предметы на сеты") && check.chechbox.IsChecked == true)
                        {
                            MessageBox.Show("Так как отображение шлемов, брони, перчаток, сапогов и оружия на сеты отключены, пункт \"Предметы на сеты\" будет автоматически отключен.");
                            check.chechbox.IsChecked = false;
                        }
                    }
                }
            }

            write.Clear();
            saveglob.Clear();
            save.Clear();
            savefav.Clear();

            write.Add("# Фильтр создан с помощью программы CustomFilter  -  https://ru.pathofexile.com/forum/view-thread/27551");
            write.Add("");
            //Запись избранных предметов
            foreach (CheckFavoriteBox item in favoritelist.Items)
            {
                write.Add("# " + item.Content);
                if (item.IsChecked == true)
                {
                    write.Add("Show");
                    foreach (string fel in item.favoriteitemlist)
                    {
                        write.Add(fel);
                    }
                }
                else if (item.IsChecked == false)
                {
                    write.Add("Hide");
                    foreach (string fel in item.favoriteitemlist)
                    {
                        if (fel.Contains("PlayAlertSound"))
                        {

                        }
                        else if (fel.Contains("PlayEffect"))
                        {

                        }
                        else if (fel.Contains("MinimapIcon"))
                        {

                        }
                        else
                        {
                            write.Add(fel);
                        }
                    }
                }

                write.Add("");
            }
            //Запись предметов на сеты
            foreach (Banner b in bannerlist)
            {
                foreach (Group g in b.grouplist)
                {
                    if (g.groupname.Contains("Предметы на сеты"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int el = 0; el < r.elementlist.Count; el++)
                            {
                                if (r.elementlist[el].Contains("ItemLevel >="))
                                {
                                    string[] edit = r.elementlist[el].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    r.elementlist[el] = "    " + edit[0] + " " + edit[1] + " " + itemlevelfrom.Text;
                                }
                                if (r.elementlist[el].Contains("ItemLevel <="))
                                {
                                    string[] edit = r.elementlist[el].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    r.elementlist[el] = "    " + edit[0] + " " + edit[1] + " " + itemlevelto.Text;
                                }
                                if (r.elementlist[el].Contains("Class"))
                                {
                                    r.elementlist[el] = "    Class ";
                                    if (setshelmets.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Helmets\""; }
                                    if (setsarmors.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Body Armours\""; }
                                    if (setsgloves.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Gloves\""; }
                                    if (setsboots.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Boots\""; }
                                    if (setsweapons.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Two Hand Axes\" \"Two Hand Maces\" \"Two Hand Swords\""; }
                                }
                            }
                        }
                    }

                    if (g.groupname.Contains("Бижутерия на сеты"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int el = 0; el < r.elementlist.Count; el++)
                            {
                                if (r.elementlist[el].Contains("ItemLevel >="))
                                {
                                    string[] edit = r.elementlist[el].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    r.elementlist[el] = "    " + edit[0] + " " + edit[1] + " " + bijlevelfrom.Text;
                                }
                                if (r.elementlist[el].Contains("ItemLevel <="))
                                {
                                    string[] edit = r.elementlist[el].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    r.elementlist[el] = "    " + edit[0] + " " + edit[1] + " " + bijlevelto.Text;
                                }
                                if (r.elementlist[el].Contains("Class"))
                                {
                                    r.elementlist[el] = "    Class ";
                                    if (setsrings.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Rings\""; }
                                    if (setsamulets.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Amulets\""; }
                                    if (setsbelts.IsChecked == true) { r.elementlist[el] = r.elementlist[el] + " " + "\"Belts\""; }
                                }
                            }
                        }
                    }

                    if (g.groupname.Contains("Высокоуровневые предметы"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int el = 0; el < r.elementlist.Count; el++)
                            {
                                if (r.elementlist[el].Contains("ItemLevel >="))
                                {
                                    string[] edit = r.elementlist[el].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    r.elementlist[el] = "    " + edit[0] + " " + edit[1] + " " + hightlevel.Text;
                                }
                            }
                        }
                    }
                }
            }
            //Запись слайдеров
            foreach (Banner b in bannerlist)
            {
                foreach (Group g in b.grouplist)
                {
                    if (g.groupname.Contains("лакон"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int i = 0; i < r.elementlist.Count; i++)
                            {
                                if (r.elementlist[i].Contains("Quality"))
                                {
                                    r.elementlist[i] = "    Quality >= " + qualityflasksslider.Value.ToString();
                                }
                            }
                        }
                    }

                    if (g.groupname.Contains("Камни с качеством"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int i = 0; i < r.elementlist.Count; i++)
                            {
                                if (r.elementlist[i].Contains("Quality"))
                                {
                                    r.elementlist[i] = "    Quality >= " + qualitygemsslider.Value.ToString();
                                }
                            }
                        }
                    }

                    if (g.groupname.Contains("Оружие с качеcтвом"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int i = 0; i < r.elementlist.Count; i++)
                            {
                                if (r.elementlist[i].Contains("Quality"))
                                {
                                    r.elementlist[i] = "    Quality >= " + qualityitemsslider.Value.ToString();
                                }
                            }
                        }
                    }

                    if (g.groupname.Contains("Броня с качеством"))
                    {
                        foreach (Region r in g.regionlist)
                        {
                            for (int i = 0; i < r.elementlist.Count; i++)
                            {
                                if (r.elementlist[i].Contains("Quality"))
                                {
                                    r.elementlist[i] = "    Quality >= " + qualityitemsslider.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }
            //Запись генерации предметов
            string classitem = "    Class";
            string classweapons = "";
            string classarmors = "";
            string classbija = "";
            string baseitem = "    BaseType";
            {
                if (rarhelmets.IsChecked == true)
                {
                    classarmors = classarmors + " \"Helmets\"";
                }
                if (rararmors.IsChecked == true)
                {
                    classarmors = classarmors + " \"Body Armours\"";
                }
                if (rargloves.IsChecked == true)
                {
                    classarmors = classarmors + " \"Gloves\"";
                }
                if (rarboots.IsChecked == true)
                {
                    classarmors = classarmors + " \"Boots\"";
                }



                if (protstrength.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protstrength.txt");
                }
                if (protdexterity.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protdexterity.txt");;
                }
                if (protintelligence.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protintelligence.txt");
                }
                if (protstr_dex.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protstrength_dexterity.txt");
                }
                if (protstr_int.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protstrength_intelligence.txt");
                }
                if (protdex_int.IsChecked == true)
                {
                    baseitem = baseitem + File.ReadAllText("BaseTypes\\protdexterity_intelligence.txt");
                }



                if (rarbelts.IsChecked == true)
                {
                    classbija = classbija + " \"Belts\"";
                }
                if (raramulets.IsChecked == true)
                {
                    classbija = classbija + " \"Amulets\"";
                }
                if (rarrings.IsChecked == true)
                {
                    classbija = classbija + " \"Rings\"";
                }



                if (rarclaws.IsChecked == true)
                {
                    classweapons = classweapons + " \"Claws\"";
                }
                if (rardaggers.IsChecked == true)
                {
                    classweapons = classweapons + " \"Daggers\"";
                }
                if (rarswords.IsChecked == true)
                {
                    classweapons = classweapons + " \"One Hand Swords\"";
                }
                if (rarsword2h.IsChecked == true)
                {
                    classweapons = classweapons + " \"Two Hand Swords\"";
                }
                if (rarsceptre.IsChecked == true)
                {
                    classweapons = classweapons + " \"Sceptres\"";
                }
                if (rarmaces.IsChecked == true)
                {
                    classweapons = classweapons + " \"One Hand Maces\"";
                }
                if (rarmace2h.IsChecked == true)
                {
                    classweapons = classweapons + " \"Two Hand Maces\"";
                }
                if (raraxes.IsChecked == true)
                {
                    classweapons = classweapons + " \"One Hand Axes\"";
                }
                if (raraxe2h.IsChecked == true)
                {
                    classweapons = classweapons + " \"Two Hand Axes\"";
                }
                if (rarstaves.IsChecked == true)
                {
                    classweapons = classweapons + " \"Staves\"";
                }
                if (rarwands.IsChecked == true)
                {
                    classweapons = classweapons + " \"Wands\"";
                }
                if (rarshields.IsChecked == true)
                {
                    classweapons = classweapons + " \"Shields\"";
                }
                if (rarbows.IsChecked == true)
                {
                    classweapons = classweapons + " \"Bows\"";
                }
                if (rarquivers.IsChecked == true)
                {
                    classweapons = classweapons + " \"Quivers\"";
                }
            }
            //Запись основного фильтра
            foreach (Banner b in bannerlist)
            {
                for (int i = 0; i < b.grouplist.Count; i++)
                {
                    if (b.checklist[i].chechbox.IsChecked == true)
                    {
                        write.Add(b.grouplist[i].groupname + "\n\r");
                        foreach (Region r in b.grouplist[i].regionlist)
                        {
                            write.Add("Show");
                            if (b.grouplist[i].groupname.Contains("# Высокоуровневые предметы"))
                            {
                                foreach (CheckBox chb in CheckBoxClassArmor)
                                {
                                    if (chb.IsChecked == true)
                                    {
                                        write.Add(classitem + classarmors);
                                        break;
                                    }
                                }
                                foreach (CheckBox chb in CheckBoxBaseType)
                                {
                                    if (chb.IsChecked == true)
                                    {
                                        if (armorCount != 0)
                                        {
                                            write.Add(baseitem);
                                            break;
                                        }
                                    }
                                }
                                if (armorCount != 0)
                                {
                                    foreach (string el in r.elementlist)
                                    {
                                        write.Add(el);
                                    }
                                    write.Add("Show");
                                }

                                if (classbija != "" || classweapons != "")
                                {
                                    //write.Add("Show");
                                    foreach (CheckBox chb in CheckBoxClassWeaBij)
                                    {
                                        if (chb.IsChecked == true)
                                        {
                                            write.Add(classitem + classbija + classweapons);
                                            break;
                                        }
                                    }
                                    foreach (string el in r.elementlist)
                                    {
                                        write.Add(el);
                                    }
                                }

                                write.Add("Hide");
                            }

                            if (b.grouplist[i].groupname.Contains("# Вещи редкого качества") || b.grouplist[i].groupname.Contains("# Вещи магического качества") || b.grouplist[i].groupname.Contains("# Вещи нормального качества"))
                            {
                                foreach(CheckBox chb in CheckBoxClassArmor)
                                {
                                    if(chb.IsChecked == true)
                                    {
                                        write.Add(classitem + classarmors);
                                        break;
                                    }
                                }
                                foreach (CheckBox chb in CheckBoxBaseType)
                                {
                                    if (chb.IsChecked == true)
                                    {
                                        if (armorCount != 0)
                                        {
                                            write.Add(baseitem);
                                            break;
                                        }
                                    }
                                }

                                if (armorCount != 0)
                                {
                                    foreach (string el in r.elementlist)
                                    {
                                        write.Add(el);
                                    }
                                    write.Add("Show");
                                }

                                if(classbija != "" || classweapons != "")
                                {
                                    //write.Add("Show");
                                    foreach (CheckBox chb in CheckBoxClassWeaBij)
                                    {
                                        if (chb.IsChecked == true)
                                        {
                                            write.Add(classitem + classbija + classweapons);
                                            break;
                                        }
                                    }
                                    foreach (string el in r.elementlist)
                                    {
                                        write.Add(el);
                                    }
                                }

                                write.Add("Hide");
                            }
                            foreach (string el in r.elementlist)
                            {
                                write.Add(el);
                            }
                        }
                    }
                    else
                    {
                        if (b.grouplist[i].groupcomment.Contains("При отключении будут удалены"))
                        {

                        }
                        else
                        {
                            write.Add(b.grouplist[i].groupname + "\n\r");
                            foreach (Region r in b.grouplist[i].regionlist)
                            {
                                write.Add("Hide");
                                foreach (string el in r.elementlist)
                                {
                                    if (el.Contains("PlayAlertSound"))
                                    {

                                    }
                                    else if (el.Contains("MinimapIcon"))
                                    {

                                    }
                                    else if (el.Contains("PlayEffect"))
                                    {

                                    }
                                    else
                                    {
                                        write.Add(el);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Запись глобальных настроек
            for (int w = 0; w < write.Count; w++)
            {
                if (shapersound.IsChecked == false)
                {
                    if (write[w].Contains("Sound Sh"))
                    {
                        write[w] = "PlayAlertSound 3 300";
                    }
                }
                if (maxsize.IsChecked == true)
                {
                    if (write[w].Contains("SetFontSize"))
                    {
                        write[w] = "    SetFontSize 45";
                    }
                }

                if (rayshow.IsChecked == false)
                {
                    if (write[w].Contains("PlayEffect"))
                    {
                        write.RemoveAt(w);
                    }
                }
                if (icoshow.IsChecked == false)
                {
                    if (write[w].Contains("MinimapIcon"))
                    {
                        write.RemoveAt(w);
                    }
                }
                if (write[w].Contains("PlayAlertSound"))
                {
                    string[] thisline = write[w].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    write[w] = "    " + thisline[0] + " " + thisline[1] + " " + (allvolumeslider.Value * 3).ToString();
                }

            }

            Directory.CreateDirectory(folderbox.Text + savefolder);
            File.WriteAllLines(folderbox.Text + filefilter, write);


            //Сохранение Избранного
            foreach (CheckFavoriteBox item in favoritelist.Items)
            {
                savefav.Add("#");
                savefav.Add(item.Content.ToString());
                savefav.Add(item.rar);
                savefav.Add(item.IsChecked.ToString());
                if (item.comment.ToString().Contains("Уровень"))
                {
                    savefav.Add("true");
                }
                else
                {
                    savefav.Add("false");
                }
                savefav.Add(item.lvlfrom);
                savefav.Add(item.lvlto);
                //savefav.Add("false");
            }
            //Сохранение основных настроек
            foreach (Banner b in bannerlist)
            {
                foreach (CheckBoxOptions c in b.checklist)
                {
                    save.Add(c.chechbox.Content.ToString() + " = " + c.chechbox.IsChecked.ToString());
                }
            }
            //Сохранение глобальных настроек
            foreach (CheckBox c in CheckBoxGlobal)
            {
                saveglob.Add(c.Name.ToString() + " = " + c.IsChecked.ToString());
            }

            saveglob.Add("qualityVolume = " + allvolumeslider.Value.ToString());
            saveglob.Add("qualityFlasks = " + qualityflasksslider.Value.ToString());
            saveglob.Add("qualityGems = " + qualitygemsslider.Value.ToString());
            saveglob.Add("qualityItems = " + qualityitemsslider.Value.ToString());

            saveglob.Add("itemlevelfrom = " + itemlevelfrom.Text);
            saveglob.Add("itemlevelto = " + itemlevelto.Text);
            saveglob.Add("bijlevelfrom = " + bijlevelfrom.Text);
            saveglob.Add("bijlevelto = " + bijlevelto.Text);

            saveglob.Add("hightlevel = " + hightlevel.Text);

            File.WriteAllLines(folderbox.Text + savefolder + "\\" + savefilter, save);
            File.WriteAllLines(folderbox.Text + savefolder + "\\" + savefavorite, savefav);
            File.WriteAllLines(folderbox.Text + savefolder + "\\" + saveglobal, saveglob);

            createfilter.Content = "Сохранено!";
            Settings.Default.folder = folderbox.Text;
            Settings.Default.Save();
        }

        //Включить галочки
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach(Banner b in bannerlist)
            {
                foreach(CheckBoxOptions check in b.checklist)
                {
                    check.chechbox.IsChecked = true;
                }
            }
        }
        //Выключить галочки
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (Banner b in bannerlist)
            {
                foreach (CheckBoxOptions check in b.checklist)
                {
                    check.chechbox.IsChecked = false;
                }
            }
        }
        //Добавить избранный предмет
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (checkfavlevel.IsChecked == true)
            {
                levelcheck = "true";
            }
            else
            {
                levelcheck = "false";
            }
            if (basetype.Text == "")
            {
                MessageBox.Show("Введите базовый тип предмета.");
            }
            else
            {
                if (radioany.IsChecked == true)
                {
                    favoritelist.Items.Add(new CheckFavoriteBox(basetype.Text, Brushes.Gray, "Any", forevlevelfrom.Text, forevlevelto.Text, levelcheck) { IsChecked = true});
                }
                if (radionormal.IsChecked == true)
                {
                    favoritelist.Items.Add(new CheckFavoriteBox(basetype.Text, Normal, "Normal", forevlevelfrom.Text, forevlevelto.Text, levelcheck) { IsChecked = true });
                }
                if (radiomagic.IsChecked == true)
                {
                    favoritelist.Items.Add(new CheckFavoriteBox(basetype.Text, Magic, "Magic", forevlevelfrom.Text, forevlevelto.Text, levelcheck) { IsChecked = true });
                }
                if (radiorare.IsChecked == true)
                {
                    favoritelist.Items.Add(new CheckFavoriteBox(basetype.Text, Rare, "Rare", forevlevelfrom.Text, forevlevelto.Text, levelcheck) { IsChecked = true });
                }
                if (radiouniq.IsChecked == true)
                {
                    favoritelist.Items.Add(new CheckFavoriteBox(basetype.Text, Unique, "Unique", forevlevelfrom.Text, forevlevelto.Text, levelcheck) { IsChecked = true });
                }

                basetype.Text = "";
                radioany.IsChecked = true;
                checkfavlevel.IsChecked = false;
            }
        }
        //Удалить избранный предмет
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            favoritelist.Items.Remove(favoritelist.SelectedItem);
        }
        //Выбор папки
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderbox.Text = dialog.SelectedPath + "\\";
                Settings.Default.folder = folderbox.Text;
                Settings.Default.Save();
            }
        }
        //Общая громкость
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            allvolume.Content = "Общая громкость: " + allvolumeslider.Value.ToString() + " %";
            if (allvolumeslider.Value == 0)
            {
                allvolume.Content = "Общая громкость: выключено";
            }
        }
        //Качество флаконов
        private void qualityflasksslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            qualityflasks.Content = "Качество флаконов: от " + qualityflasksslider.Value.ToString() + " %";
            if (qualityflasksslider.Value == 0)
            {
                qualityflasks.Content = "Качество флаконов: любое";
            }
        }
        //Качество камней умений
        private void qualitygemsslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            qualitygems.Content = "Качество камней: от " + qualitygemsslider.Value.ToString() + " %";
            if (qualitygemsslider.Value == 0)
            {
                qualitygems.Content = "Качество камней: любое";
            }
        }
        //Качество экипировки
        private void qualityitemsslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            qualityitems.Content = "Качество экипировки: от " + qualityitemsslider.Value.ToString() + " %";
            if (qualityitemsslider.Value == 0)
            {
                qualityitems.Content = "Качество экипировки: любое";
            }
        }
        //Если внесены изменения
        private void createfilter_LostFocus(object sender, RoutedEventArgs e)
        {
            createfilter.Content = "Сохранить фильтр";
        }
        //Сбросить путь к папке с фильтрами
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            folderbox.Text = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\Path of Exile\\";
            Settings.Default.Reset();
        }
        //Открыть папку с фильтрами
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Process.Start(folderbox.Text);
        }

        //Кнопка инфо
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("CustomFilter\r\n\r\nНазвание проекта: CustomFilter\r\nАвтор проекта: mr_mark_ru\r\nДата основания проекта: 11.11.2017\r\n\r\nПроект созданный для игры Path of Exile. Является пользовательской программой для индивидуальной настройки фильтра предметов в игре.");
        }
        //Кнопка поддержка
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://ru.pathofexile.com/private-messages/compose/to/BeardedMark");
        }
        //Кнопка доната
        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://money.yandex.ru/to/410016079273316");
        }
        //Кнопка форум
        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://ru.pathofexile.com/forum/view-thread/27551");
        }
        //Кнопка отзыв
        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://ru.pathofexile.com/forum/post-reply/27551");
        }
        //Кнопка ВК
        private void Image_MouseDown_6(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://vk.com/customfilter");
        }
        //Обзор избранных предметов
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Сейчас вы будете перенаправленны на официальный сайт PoE WiKi.\r\nЧто бы узнать базовый тип предмета:\r\n1. Найдите его на сайте\r\n2. Скопируйте название на английском языке\r\n3. Вставте название в поле ввода в программе\r\n4. Введите настройки редкости\r\n5. Нажмите кнопку \"Добавить\"\r\n\r\nВнимание! За один раз можно добавить не более одного предмета. Если предмет несуществует в игре, то при загрузке фильтра будет ошибка.");
            Process.Start("https://pathofexile-ru.gamepedia.com/%D0%9F%D1%80%D0%B5%D0%B4%D0%BC%D0%B5%D1%82%D1%8B");

        }
        //Информация о сетах
        private void Image_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Для получения сфер, вам нужно собрать сет вещей определенного уровня: \r\nСфера Удачи - от 1 до 59 уровня.\r\nСфера Хаоса - от 60 до 74 уровня. \r\nСфера Царей - от 75 до 100 уровня.\r\nИнтервал уровня можете выбрать на свое усмотрение.");
        }

        //Картинка под курсором
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image pb = new Image();
            pb = (Image)e.OriginalSource;

            pb.Opacity = 1;
        }
        //Картинка не под курсором
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image pb = new Image();
            pb = (Image)e.OriginalSource;

            pb.Opacity = 0.5;
        }
        //Кнопка под курсором
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button pb = new Button();
            pb = (Button)e.OriginalSource;

            pb.Opacity = 1;
        }
        //Кнопка не под курсором
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button pb = new Button();
            pb = (Button)e.OriginalSource;

            pb.Opacity = 0.5;
        }

        public void Checked(object sender, RoutedEventArgs e)
        {
            blockbij.Visibility = Visibility.Hidden;
        }
        public void Unchecked(object sender, RoutedEventArgs e)
        {
            blockbij.Visibility = Visibility.Visible;
        }

        public void Setschecked(object sender, RoutedEventArgs e)
        {
            blockonsets.Visibility = Visibility.Hidden;
        }
        public void Setsunchecked(object sender, RoutedEventArgs e)
        {
            blockonsets.Visibility = Visibility.Visible;
        }

        public void Itemschecked(object sender, RoutedEventArgs e)
        {
            items = items + 1;
            if (items > 0)
            {
                blockanyarmor.Visibility = Visibility.Hidden;
                blockanybija.Visibility = Visibility.Hidden;
                blockanyweapon.Visibility = Visibility.Hidden;
                blockanyprotection.Visibility = Visibility.Hidden;
            }
        }
        public void Itemsunchecked(object sender, RoutedEventArgs e)
        {
            items = items - 1;
            if (items == 0)
            {
                blockanyarmor.Visibility = Visibility.Visible;
                blockanybija.Visibility = Visibility.Visible;
                blockanyweapon.Visibility = Visibility.Visible;
                blockanyprotection.Visibility = Visibility.Visible;
            }
        }

        public void Hightlevelchecked(object sender, RoutedEventArgs e)
        {
            items = items + 1;
            if (items > 0)
            {
                blockanyarmor.Visibility = Visibility.Hidden;
                blockanybija.Visibility = Visibility.Hidden;
                blockanyweapon.Visibility = Visibility.Hidden;
                blockanyprotection.Visibility = Visibility.Hidden;
            }
            blockhightlevel.Visibility = Visibility.Hidden;
        }
        public void Hightlevelunchecked(object sender, RoutedEventArgs e)
        {
            items = items - 1;
            if (items == 0)
            {
                blockanyarmor.Visibility = Visibility.Visible;
                blockanybija.Visibility = Visibility.Visible;
                blockanyweapon.Visibility = Visibility.Visible;
                blockanyprotection.Visibility = Visibility.Visible;
            }
            blockhightlevel.Visibility = Visibility.Visible;
        }

        public void Gemschecked(object sender, RoutedEventArgs e)
        {
            blockgems.Visibility = Visibility.Hidden;
        }
        public void Gemsunchecked(object sender, RoutedEventArgs e)
        {
            blockgems.Visibility = Visibility.Visible;
        }

        public void Quachecked(object sender, RoutedEventArgs e)
        {
            qua = qua + 1;
            if (qua > 0)
            {
                blockqua.Visibility = Visibility.Hidden;
            }
        }
        public void Quaunchecked(object sender, RoutedEventArgs e)
        {
            qua = qua - 1;
            if (qua == 0)
            {
                blockqua.Visibility = Visibility.Visible;
            }
        }

        public void Flaskchecked(object sender, RoutedEventArgs e)
        {
            flas = flas + 1;
            if (flas > 0)
            {
                blockflask.Visibility = Visibility.Hidden;
            }
        }
        public void Flaskunchecked(object sender, RoutedEventArgs e)
        {
            flas = flas - 1;
            if (flas == 0)
            {
                blockflask.Visibility = Visibility.Visible;
            }
        }

        //Увеличеная шапка
        private void fullscreen_Checked(object sender, RoutedEventArgs e)
        {
            head.Height = new GridLength(250);
            Height = Height + 180;
        }
        private void fullscreen_Unchecked(object sender, RoutedEventArgs e)
        {
            head.Height = new GridLength(70);
            Height = Height - 180;
        }
        //Счетчик включеной брони базы
        private void protdex_int_MouseDown(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                protCount++;
                Console.WriteLine(protCount);
            }
            else if(protCount == 1)
            {
                MessageBox.Show("Должен быть активным хотя бы 1 пункт");
                ((CheckBox)sender).IsChecked = true;
            }
            else
            {
                protCount--;
                Console.WriteLine(protCount);
            }
        }
        //Счетчик включеной брони классы
        private void rarhelmets_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                armorCount++;
                Console.WriteLine(armorCount);
            }
            else
            {
                armorCount--;
                Console.WriteLine(armorCount);
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (File.Exists("BaseTypes\\BaseFavotite.cfs"))
            {
                string[] favoriteline = File.ReadAllLines("BaseTypes\\BaseFavotite.cfs");
                for (int s = 0; s < favoriteline.Length; s++)
                {
                    if (favoriteline[s].Contains("#"))
                    {
                        if (favoriteline[s + 2].Contains("Any"))
                        {
                            favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Brushes.Gray, "Any", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                        }
                        if (favoriteline[s + 2].Contains("Normal"))
                        {
                            favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Normal, "Normal", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                        }
                        if (favoriteline[s + 2].Contains("Magic"))
                        {
                            favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Magic, "Magic", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                        }
                        if (favoriteline[s + 2].Contains("Rare"))
                        {
                            favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Rare, "Rare", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                        }
                        if (favoriteline[s + 2].Contains("Unique"))
                        {
                            favoritelist.Items.Add(new CheckFavoriteBox(favoriteline[s + 1], Unique, "Unique", favoriteline[s + 5], favoriteline[s + 6], favoriteline[s + 4]) { IsChecked = Convert.ToBoolean(favoriteline[s + 3]) });
                        }
                    }
                }
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            favoritelist.Items.Clear();
        }
    }
}

class CheckFavoriteBox : CheckBox
{
    public List<string> favoriteitemlist = new List<string>();
    public string rar;
    public string lvlto;
    public string lvlfrom;
    public string comment;
    public CheckFavoriteBox(string basetype, SolidColorBrush color, string rarity, string levelfrom, string levelto, string levelcheck)
    {
        Margin = new Thickness(2, 1, 2, 1);
        rar = rarity;
        lvlfrom = levelfrom;
        lvlto = levelto;
        Content = basetype;
        VerticalAlignment = VerticalAlignment.Center;
        Foreground = color;
        BorderBrush = Brushes.Transparent;
        Cursor = Cursors.Hand;
        Background = new SolidColorBrush(Color.FromArgb(255, 99, 73, 40));
        comment = "Тип предмета: " + basetype + "\r\nРедкость предмета: " + rarity;
        //ToolTip = "Тип предмета: " + basetype + "\r\nРедкость предмета: " + rarity;

        if (rarity == "Any")
        {

        }
        else
        {
            favoriteitemlist.Add("    Rarity = " + rarity);
        }
        if (levelcheck.Contains("rue"))
        {
            favoriteitemlist.Add("    ItemLevel >= " + lvlfrom);
            favoriteitemlist.Add("    ItemLevel <= " + lvlto);
            comment = comment + "\r\nУровень от " + lvlfrom + " до " + lvlto;
        }
        favoriteitemlist.Add("    BaseType \"" + basetype + "\"");
        favoriteitemlist.Add("    SetTextColor 255 255 255");
        favoriteitemlist.Add("    SetBackgroundColor 128 128 128");
        favoriteitemlist.Add("    SetBorderColor 255 255 255 150");
        favoriteitemlist.Add("    SetFontSize 45");
        favoriteitemlist.Add("    PlayAlertSound 13 300");
        favoriteitemlist.Add("    PlayEffect White");
        favoriteitemlist.Add("    MinimapIcon 0 White Star");

        ToolTip = new ToolTipOptions("Пример отображения", comment, 255, 255, 255, 255, 255, 128, 128, 128, 150, 255, 255, 255, "White", "White", "Star").grid;
    }
}

class CheckBoxOptions
{
    string playsound;
    StackPanel stp = new StackPanel();
    public CheckBox chechbox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center};
    Image image = new Image() { VerticalAlignment = VerticalAlignment.Center, Height = 10, Width = 10, Margin = new Thickness(2,0,2,0), Source = new BitmapImage(new Uri("pack://application:,,,/Ico/микро.png")), Cursor = Cursors.Hand };
    public StackPanel st = new StackPanel() { Orientation = Orientation.Horizontal };    

    public CheckBoxOptions(bool checkmark, string content, string comment, string sound, byte a1, byte r1, byte g1, byte b1, byte a2, byte r2, byte g2, byte b2, byte a3, byte r3, byte g3, byte b3,
                           string ray, string icocolor, string icoform, RoutedEventHandler Checked, RoutedEventHandler Unchecked, RoutedEventHandler Setschecked, RoutedEventHandler Setsunchecked,
                           string rare, string magic, string normal, RoutedEventHandler Itemschecked, RoutedEventHandler Itemsunchecked, RoutedEventHandler Gemschecked, RoutedEventHandler Gemsunchecked,
                           RoutedEventHandler Quachecked, RoutedEventHandler Quaunchecked, RoutedEventHandler Flaskchecked, RoutedEventHandler Flaskunchecked,
                           RoutedEventHandler Hightlevelchecked, RoutedEventHandler Hightlevelunchecked)
    {
        image.Opacity = 0.2;
        image.MouseDown += PlaySound;
        image.MouseEnter += Image_MouseEnter;
        image.MouseLeave += Image_MouseLeave;
        chechbox.MouseEnter += Checkbox_MouseEnter;
        chechbox.MouseLeave += Checkbox_MouseLeave;
        chechbox.IsChecked = checkmark;
        chechbox.Content = content;
        chechbox.ToolTip = new ToolTipOptions("Пример отображения", comment, a1, r1, g1, b1, a2, r2, g2, b2, a3, r3, g3, b3, ray, icocolor, icoform).grid;
        playsound = sound;
        chechbox.Checked += CChecked;
        chechbox.Unchecked += CUnchecked;
        chechbox.Opacity = 1;
        if (playsound == "null")
        {
            image.Source = null;
        }
        if (content.Contains("Бижутерия на сеты"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Checked;
            chechbox.Unchecked += Unchecked;
        }
        if (content.Contains("Предметы на сеты"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Setschecked;
            chechbox.Unchecked += Setsunchecked;
        }
        if (content.Contains("Вещи редкого качества"))
        {
            //chechbox.IsChecked = false;
            chechbox.Name = rare;
            chechbox.Checked += Itemschecked;
            chechbox.Unchecked += Itemsunchecked;
        }
        if (content.Contains("Вещи магического качества"))
        {
            //chechbox.IsChecked = false;
            chechbox.Name = magic;
            chechbox.Checked += Itemschecked;
            chechbox.Unchecked += Itemsunchecked;
        }
        if (content.Contains("Вещи нормального качества"))
        {
            //chechbox.IsChecked = false;
            chechbox.Name = normal;
            chechbox.Checked += Itemschecked;
            chechbox.Unchecked += Itemsunchecked;
        }

        if (content.Contains("Высокоуровневые предметы"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Hightlevelchecked;
            chechbox.Unchecked += Hightlevelunchecked;
        }

        if (content.Contains("Камни с качеством"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Gemschecked;
            chechbox.Unchecked += Gemsunchecked;
        }
        if (content.Contains("Оружие с качеcтвом"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Quachecked;
            chechbox.Unchecked += Quaunchecked;
        }
        if (content.Contains("Броня с качеством"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Quachecked;
            chechbox.Unchecked += Quaunchecked;
        }
        if (content.Contains("лаконы"))
        {
            //chechbox.IsChecked = false;
            chechbox.Checked += Flaskchecked;
            chechbox.Unchecked += Flaskunchecked;
        }

        st.Children.Add(image);
        st.Children.Add(chechbox);
    }
    private void Image_MouseLeave(object sender, MouseEventArgs e)
    {
        image.Opacity = 0.2;
    }
    private void Image_MouseEnter(object sender, MouseEventArgs e)
    {
        image.Opacity = 1;
    }
    private void Checkbox_MouseLeave(object sender, MouseEventArgs e)
    {
        if(chechbox.IsChecked == false)
        {
            chechbox.Opacity = 0.5;
        }
    }
    private void Checkbox_MouseEnter(object sender, MouseEventArgs e)
    {
        chechbox.Opacity = 1;
    }
    private void PlaySound(object sender, MouseButtonEventArgs e)
    {
        //MessageBox.Show(playsound);
        SoundPlayer simpleSound = new SoundPlayer(@"AlertSounds\" + playsound + ".wav");
        simpleSound.Play();
    }

    public void CChecked(object sender, RoutedEventArgs e)
    {
        chechbox.Opacity = 1;
    }
    public void CUnchecked(object sender, RoutedEventArgs e)
    {
        chechbox.Opacity = 0.5;
    }
}

class ToolTipOptions
{
    Rectangle rect = new Rectangle();

    StackPanel stp = new StackPanel();
    Grid gridray = new Grid();
    public Grid grid = new Grid();
    public ToolTipOptions(string text, string comment, byte a1, byte r1, byte g1, byte b1, byte a2, byte r2, byte g2, byte b2, byte a3, byte r3, byte g3, byte b3, string ray, string icocolor, string icoform)
    {
        Image icoout = new Image() { Height = 24, Width = 24, Source = new BitmapImage(new Uri("pack://application:,,,/IcoMinimap/" + icoform + "_" + icocolor + ".png")) };
        Image rayout = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Rays/" + ray + ".png")) };

        Image rayimg = new Image();
        Image icoimg = new Image();
      
        grid.Width = 250;
        grid.Children.Add(stp);
        stp.Children.Add(new TextBox()
        {
            FontFamily = new FontFamily("FrizQuadrataC"),
            //FontWeight = FontWeights.Bold,
            Text = text,
            FontSize = 18,
            BorderBrush = new SolidColorBrush(Color.FromArgb(a1, r1, g1, b1)),
            Background = new SolidColorBrush(Color.FromArgb(a2, r2, g2, b2)),
            Foreground = new SolidColorBrush(Color.FromArgb(a3, r3, g3, b3)),
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(10)
        });
        stp.Children.Add(gridray);
        gridray.Children.Add(rayout);
        gridray.Children.Add(icoout);
        stp.Children.Add(new TextBlock() {
            Text = comment,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(10),
            Foreground = new SolidColorBrush(Color.FromArgb(255, 163, 139, 99))
        });
        WrapPanel wrp = new WrapPanel() { /*Margin = new Thickness(10, 0, 10, 0),*/ HorizontalAlignment = HorizontalAlignment.Center};
        if (comment.Contains("высших"))
        {
            stp.Children.Add(wrp);
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyAddModToRare.png"))});
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "ExaltedShard.png")) });
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyDuplicate.png"))});
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "MirrorShard.png")) });
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyModValues.png"))});
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AncientOrb.png"))});
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AncientShard.png")) });
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AnnullOrb.png"))});
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AnnullShard.png")) });
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "HarbingerOrb.png"))});
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "HarbingerShard.png")) });
            wrp.Children.Add(new Image() {Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyImprintOrb.png"))});
        }

        if (comment.Contains("важных"))
        {
            stp.Children.Add(wrp);
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyPassiveSkillRefund.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "HorizonOrb.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyRerollRare.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyImplicitMod.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyUpgradeMagicToRare.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyVaal.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyUpgradeToRare.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyRerollSocketLinks.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyConvertToNormal.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyMapQuality.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyGemQuality.png")) });

            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "HorizonShard.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "ChaosShard.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "RegalShard.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AlchemyShard.png")) });
        }

        if (comment.Contains("нужных"))
        {
            stp.Children.Add(wrp);
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyRerollMagic.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyRerollSocketColours.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyRerollSocketNumbers.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "EngineersOrb.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyUpgradeRandomly.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "BindingOrb.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyCoin.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "SilverObol.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyFlaskQuality.png")) });

            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "BindingShard.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "EngineersShard.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "AlterationShard.png")) });
        }

        if (comment.Contains("изменения"))
        {
            stp.Children.Add(wrp);
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyAddModToMagic.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyUpgradeToMagic.png")) });

            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "TransmutationShard.png")) });
        }

        if (comment.Contains("точильных"))
        {
            stp.Children.Add(wrp);
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyWeaponQuality.png")) });
            wrp.Children.Add(new Image() { Width = 35, Height = 35, Source = new BitmapImage(new Uri("pack://application:,,,/Orbs/" + "CurrencyArmourQuality.png")) });
        }
    }
}

class Banner
{
    public List<CheckBoxOptions> checklist = new List<CheckBoxOptions>();

    public List<Group> grouplist = new List<Group>();
    public string bannername;
    public Banner(int index, string[] lines)
    {
        bannername = lines[index];
        for (int i = index + 1; i < lines.Length; i++)
        {
            if (lines[i].Contains("#"))
            {
                if (lines[i].Contains(":"))
                {
                    break;
                }
                else if (lines[i].Contains("."))
                {

                }
                else
                {
                    grouplist.Add(new Group(i, lines));
                }
            }
        }
    }
}

class Group
{
    public List<Region> regionlist = new List<Region>();
    public string groupname;
    public string groupcomment;
    public Group(int index, string[] lines)
    {
        groupname = lines[index];
        groupcomment = "  ";
        for (int i = index + 1; i < lines.Length; i++)
        {
            if (lines[i].Contains("#") && lines[i].Contains("."))
            {
                groupcomment = lines[i];
            }
            else if (lines[i].Contains("Show") || lines[i].Contains("Hide"))
            {
                regionlist.Add(new Region(i, lines));
            }
            else if (lines[i].Contains("#"))
            {
                break;
            }
        }
    }
}

class Region
{
    public List<string> elementlist = new List<string>();
    public string visibility;
    public string[] SetTextColor;
    public string[] SetBackgroundColor;
    public string[] SetBorderColor;
    public string[] PlayAlertSound;
    string[] separators = { " " };
    double procent;
    public Region(int index, string[] lines)
    {
        visibility = lines[index];
        for (int i = index + 1; i < lines.Length; i++)
        {
            if (lines[i].Contains("#") || lines[i].Contains("Show") || lines[i].Contains("Hide"))
            {
                break;
            }
            else
            {
                elementlist.Add(lines[i]);
            }
        }

        SetTextColor = Bynar("SetTextColor", SetTextColor);
        SetBackgroundColor = Bynar("SetBackgroundColor", SetBackgroundColor);
        SetBorderColor = Bynar("SetBorderColor", SetBorderColor);
        foreach (string el in elementlist)
        {
            if (el.Contains("PlayAlertSound"))
            {
                PlayAlertSound = el.Remove(0, 4).Split(separators, StringSplitOptions.RemoveEmptyEntries);
                break;
            }
            else
            {
                PlayAlertSound = "0 null".Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        string[] Bynar(string element, string[] array)
        {
            foreach (string el in elementlist)
            {
                if (el.Contains(element))
                {
                    array = el.Remove(0, 4).Split(separators, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            if (array.Length == 4)
            {
                Array.Resize(ref array, array.Length + 1);
                array[4] = "300";
            }
            procent = Convert.ToDouble(array[4]) / 3;
            procent = Math.Truncate(procent * 2.55);
            array[4] = procent.ToString();

            return array;
        }
    }
}
