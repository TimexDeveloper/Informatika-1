using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AlphabetForKids
{
    public partial class MainWindow : Window
    {
        private string userName = "Друг";
        private char currentLetter = 'А';

        private int mathNum1 = 0;
        private int mathNum2 = 0;
        private string mathOp = "+";
        private int correctMathAnswer = 0;
        private Random random = new Random();

        private MediaPlayer bgPlayer = new MediaPlayer();

        private readonly List<char> alphabet = new List<char>
        {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й',
            'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф',
            'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я'
        };

        private readonly Dictionary<char, string> writingInstructions = new Dictionary<char, string>
        {
            {'А', "Заглавная: ведём наклонную линию вверх, затем вниз и делаем поясок. Строчная: пишем кружок и хвостик справа."},
            {'Б', "Заглавная: прямая спинка вниз, овал внизу и шляпка вверху. Строчная: овал и вверх плавный крючок."},
            {'В', "Заглавная: линия вниз и два полуовала справа. Строчная: петелька вверх, овал внизу."},
            {'Г', "Заглавная: палочка вниз и козырёк вправо. Строчная: плавный крючок сверху вниз."},
            {'Д', "Заглавная: шапочка, ножка вниз, петелька и подставка. Строчная: овал и петелька уходит вниз."},
            {'Е', "Заглавная: дуга и две петельки. Строчная: маленькая плавная петелька."},
            {'Ё', "Пишется как буква Е, но сверху обязательно добавляем две яркие точки!"},
            {'Ж', "Заглавная и строчная: наклонная линия, полуовал слева, крестик в центре и полуовал справа."},
            {'З', "Заглавная: верхний полуовал и нижний полуовал. Строчная: маленький верхний овал и нижняя петелька."},
            {'И', "Заглавная: ножка, диагональ снизу вверх и ножка вниз. Строчная: две палочки с закруглением снизу."},
            {'Й', "Пишется точно как буква И, а сверху добавляем маленькую галочку-птичку!"},
            {'К', "Заглавная: прямая спинка и две наклонные лапки. Строчная: уменьшенная копия заглавной."},
            {'Л', "Заглавная: начинаем с крючка снизу, подъем вверх и спуск направо. Строчная: точно такая же, но поменьше."},
            {'М', "Заглавная: две верхушки-уголка соединены в центре. Строчная: повторяет форму заглавной."},
            {'Н', "Заглавная: две вертикальные палочки, соединённые мостиком. Строчная: аналогично."},
            {'О', "Заглавная: большой ровный овал против часовой стрелки. Строчная: маленький аккуратный овал."},
            {'П', "Заглавная: две ножки и ровная перекладина сверху. Строчная: палочка, мостик и крючок."},
            {'Р', "Заглавная: длинная спинка и овал в верхней части. Строчная: ножка уходит вниз, сверху овал."},
            {'С', "Заглавная: большой открытый полуовал влево. Строчная: маленький полуовал."},
            {'Т', "Заглавная: шляпка сверху и ножка по центру. Строчная: палочка с закруглением или три ножки."},
            {'У', "Заглавная: глубокий крючок и длинная ножка влево. Строчная: закругление и петелька вниз."},
            {'Ф', "Заглавная и строчная: центральная палочка и два овала по бокам."},
            {'Х', "Заглавная и строчная: две пересекающиеся крест-накрест линии."},
            {'Ц', "Заглавная: два крючка снизу и маленькая петелька справа. Строчная: аналогично в строке."},
            {'Ч', "Заглавная: закругление сверху и ножка вниз. Строчная: чашечка и палочка с хвостиком."},
            {'Ш', "Заглавная: три вертикальные ножки, соединенные снизу. Строчная: такая же, меньше размером."},
            {'Щ', "Пишется как Ш, но в правом нижнем углу добавляется маленькая петелька!"},
            {'Ъ', "Заглавная и строчная: козырёк слева, спинка вниз и маленький животик."},
            {'Ы', "Заглавная и строчная: палочка с овалом внизу и соседняя прямая ножка."},
            {'Ь', "Заглавная и строчная: прямая вертикальная палочка и аккуратный овал снизу."},
            {'Э', "Заглавная: полуовал, открытый вправо, и язычок посередине. Строчная: маленькая э."},
            {'Ю', "Заглавная: палочка, перекладина и большой овал справа. Строчная: уменьшенная копия."},
            {'Я', "Заглавная: закруглённая головка, спинка и ножка вправо. Строчная: малая я."}
        };

        public MainWindow()
        {
            InitializeComponent();
            GenerateAlphabetButtons();
            PlayBackgroundMusic();
            GenerateNewMathProblem();
        }

        private void PlayBackgroundMusic()
        {
            try
            {
                string musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PROMISED FUTURE.mp3");
                if (File.Exists(musicPath))
                {
                    bgPlayer.Open(new Uri(musicPath, UriKind.Absolute));
                    bgPlayer.Volume = VolumeSlider.Value;
                    bgPlayer.MediaEnded += (s, e) =>
                    {
                        bgPlayer.Position = TimeSpan.Zero;
                        bgPlayer.Play();
                    };
                    bgPlayer.Play();
                }
            }
            catch { }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (bgPlayer != null)
            {
                bgPlayer.Volume = e.NewValue;
            }
        }

        private void StartApp_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtName.Text))
            {
                userName = TxtName.Text.Trim();
            }

            WelcomeScreen.Visibility = Visibility.Collapsed;
            SubjectSelectionScreen.Visibility = Visibility.Visible;
            TxtSubjectGreeting.Text = $"Отлично, {userName}! Что будем изучать сегодня?";
        }

        private void SelectAlphabet_Click(object sender, RoutedEventArgs e)
        {
            SubjectSelectionScreen.Visibility = Visibility.Collapsed;
            MainAppScreen.Visibility = Visibility.Visible;
            AlphabetContainer.Visibility = Visibility.Visible;
            MathContainer.Visibility = Visibility.Collapsed;

            SelectLetter('А');
        }

        private void SelectMath_Click(object sender, RoutedEventArgs e)
        {
            SubjectSelectionScreen.Visibility = Visibility.Collapsed;
            MainAppScreen.Visibility = Visibility.Visible;
            MathContainer.Visibility = Visibility.Visible;
            AlphabetContainer.Visibility = Visibility.Collapsed;

            TxtGreeting.Text = $"{userName}, давай решать веселые примеры по математике!";
        }

        private void BackToSubjectSelection_Click(object sender, RoutedEventArgs e)
        {
            MainAppScreen.Visibility = Visibility.Collapsed;
            SubjectSelectionScreen.Visibility = Visibility.Visible;
        }

        private void GenerateAlphabetButtons()
        {
            AlphabetPanel.Children.Clear();

            foreach (char letter in alphabet)
            {
                Button btn = new Button
                {
                    Content = letter.ToString(),
                    Style = (Style)FindResource("AnimatedLetterButton")
                };

                char l = letter;
                btn.Click += (s, e) => SelectLetter(l);

                AlphabetPanel.Children.Add(btn);
            }
        }

        private void SelectLetter(char letter)
        {
            currentLetter = char.ToUpper(letter);
            char lower = char.ToLower(letter);

            TxtSelectedTitle.Text = $"Буква {currentLetter} (Заглавная и строчная)";
            TxtTemplate.Text = $"{currentLetter} {lower}";
            TxtAnimated.Text = $"{currentLetter} {lower}";

            if (writingInstructions.ContainsKey(currentLetter))
            {
                TxtGreeting.Text = $"{userName}, вот как пишется «{currentLetter}»: {writingInstructions[currentLetter]}";
            }

            StartWritingAnimation();
        }

        private void StartWritingAnimation()
        {
            WritingClip.BeginAnimation(RectangleGeometry.RectProperty, null);
            WritingClip.Rect = new Rect(0, 0, 0, 500);

            RectAnimation anim = new RectAnimation
            {
                From = new Rect(0, 0, 0, 500),
                To = new Rect(0, 0, 1000, 500),
                Duration = TimeSpan.FromSeconds(2.5),
                FillBehavior = FillBehavior.HoldEnd
            };

            WritingClip.BeginAnimation(RectangleGeometry.RectProperty, anim);
        }

        // --- МАТЕМАТИКА ---
        private void GenerateNewMathProblem()
        {
            mathOp = random.Next(0, 2) == 0 ? "+" : "-";

            if (mathOp == "+")
            {
                mathNum1 = random.Next(1, 10);
                mathNum2 = random.Next(1, 10);
                correctMathAnswer = mathNum1 + mathNum2;
            }
            else
            {
                mathNum1 = random.Next(2, 15);
                mathNum2 = random.Next(1, mathNum1);
                correctMathAnswer = mathNum1 - mathNum2;
            }

            TxtMathProblem.Text = $"{mathNum1} {mathOp} {mathNum2} = ";
            TxtMathAnswer.Text = "";
            TxtMathResult.Text = "";
        }

        private void CheckMathAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtMathAnswer.Text.Trim(), out int userAnswer))
            {
                if (userAnswer == correctMathAnswer)
                {
                    TxtMathResult.Foreground = new SolidColorBrush(Colors.Green);
                    TxtMathResult.Text = $"🎉 Молодец, {userName}! Всё верно!";

                    var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        GenerateNewMathProblem();
                    };
                    timer.Start();
                }
                else
                {
                    TxtMathResult.Foreground = new SolidColorBrush(Colors.Red);
                    TxtMathResult.Text = "❌ Неправильно. Попробуй ещё раз!";
                }
            }
            else
            {
                TxtMathResult.Foreground = new SolidColorBrush(Colors.Orange);
                TxtMathResult.Text = "Введи число!";
            }
        }
    }
}