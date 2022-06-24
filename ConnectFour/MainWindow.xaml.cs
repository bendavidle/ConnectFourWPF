using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ConnectFour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle[,] rect2D = new Rectangle[6, 7];
        Brush defaultColor = Brushes.Black;
        Brush p1Color = Brushes.Red;
        Brush p2Color = Brushes.Yellow;
        private int _turn;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Game.Background = new SolidColorBrush(Colors.Blue);

            StartGame();
        }

        private void StartGame()
        {
            Game.Children.Clear();
            for (int l = 0; l < rect2D.GetLength(1); l++)
            {
                for (int k = 0; k < rect2D.GetLength(0); k++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 90,
                        Height = 90,
                        Fill = defaultColor,
                        Tag = new Position(new[] { k, l })
                    };
                    rect.MouseLeftButtonDown += OnRectClick;

                    rect2D[k, l] = rect;
                }
            }


            for (int k = 0; k < rect2D.GetLength(0); k++)
            {
                for (int l = 0; l < rect2D.GetLength(1); l++)
                {
                    Rectangle? rectangle = rect2D[k, l];
                    Game.Children.Add(rectangle);
                    Canvas.SetTop(rectangle, 10 + k * Height / 6);
                    Canvas.SetLeft(rectangle, 50 + l * Width / 7);
                }
            }
        }


        private void OnRectClick(object sender, MouseButtonEventArgs e)
        {
            Rectangle? cell = e.OriginalSource as Rectangle;
            Position? cellPos = cell.Tag as Position;

            if (cell.Fill != defaultColor) return;

            Brush fill = _turn == 0 ? p1Color : p2Color;

            if (cellPos.X is 5)
            {
                cell.Fill = fill;
            }
            else if (cellPos.X is 4 && rect2D[cellPos.X + 1, cellPos.Y].Fill == defaultColor)
            {

                rect2D[cellPos.X + 1, cellPos.Y].Fill = fill;
            }
            else
            {

                Rectangle? rectUnder = rect2D[cellPos.X + 1, cellPos.Y];
                Position? rectUnderPos = rectUnder.Tag as Position;
                while (rectUnder.Fill == defaultColor)
                {
                    rectUnder = rect2D[rectUnderPos.X + 1, rectUnderPos.Y];
                    rectUnderPos = rectUnder.Tag as Position;
                    if (rectUnderPos.X == rect2D.GetLength(0) - 1)
                    {
                        break;
                    }

                }

                if (rect2D[rectUnderPos.X, rectUnderPos.Y].Fill == defaultColor)
                {
                    rect2D[rectUnderPos.X, rectUnderPos.Y].Fill = fill;

                }
                else
                {
                    rect2D[rectUnderPos.X - 1, rectUnderPos.Y].Fill = fill;

                }
            }

            _turn = (_turn + 1) % 2;


            int winner = chkWinner();
            bool draw = rect2D.Cast<Rectangle>().All(re => re.Fill == Brushes.Red || re.Fill == Brushes.Yellow);


            if (winner is 1 or 2 or 0)
            {
                if (!draw && winner is 0)
                {
                    return;
                }
                string winnerString = draw ? "Draw" : "Winnner is " + (winner == 1 ? "Red" : "Yellow");
                Color winnerColor = fill == Brushes.Red ? Colors.Red : Colors.Yellow;
                Game.Children.Clear();


                TextBlock textBlock = new TextBlock();
                textBlock.Text = winnerString;
                textBlock.Foreground = new SolidColorBrush(winnerColor);
                textBlock.FontSize = 60;
                textBlock.FontFamily = new FontFamily("Segoe UI Black");

                TextBlock resetText = new TextBlock();
                resetText.Text = "Reset";
                resetText.Foreground = new SolidColorBrush(Colors.White);
                resetText.FontSize = 50;
                resetText.FontFamily = new FontFamily("Segoe UI Black");

                Rectangle resetButton = new Rectangle()
                {
                    Width = 180,
                    Height = 90,
                    Fill = Brushes.Navy,
                };



                resetButton.MouseDown += (o, args) => StartGame();
                resetText.MouseDown += (o, args) => StartGame();

                Canvas.SetLeft(textBlock, Width / 2 - textBlock.Text.Length * textBlock.FontSize / 4);
                Canvas.SetTop(textBlock, Height / 2 - textBlock.FontSize);



                Canvas.SetLeft(resetButton, Width / 2 - resetButton.Width / 2);
                Canvas.SetTop(resetButton, Height / 2 - textBlock.FontSize + resetButton.Height * 2);




                Canvas.SetLeft(resetText, Width / 2 - resetButton.Width / 2 + 25);
                Canvas.SetTop(resetText, Height / 2 + resetButton.Height + 35);

                Game.Children.Add(textBlock);
                Game.Children.Add(resetButton);
                Game.Children.Add(resetText);

            }


            //https://stackoverflow.com/questions/33181356/connect-four-game-checking-for-wins-js
            bool checkLine(Rectangle a, Rectangle b, Rectangle c, Rectangle d)
            {
                // Check first cell non-zero and all cells match
                return ((a.Fill != defaultColor) && (a.Fill == b.Fill) && (a.Fill == c.Fill) && (a.Fill == d.Fill));
            }

            int chkWinner()
            {
                // Check down
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 7; c++)
                        if (checkLine(rect2D[r, c], rect2D[r + 1, c], rect2D[r + 2, c], rect2D[r + 3, c]))
                            return rect2D[r, c].Fill == p1Color ? 1 : 2;

                // Check right
                for (int r = 0; r < 6; r++)
                    for (int c = 0; c < 4; c++)
                        if (checkLine(rect2D[r, c], rect2D[r, c + 1], rect2D[r, c + 2], rect2D[r, c + 3]))
                            return rect2D[r, c].Fill == p1Color ? 1 : 2;

                // Check down-right
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 4; c++)
                        if (checkLine(rect2D[r, c], rect2D[r + 1, c + 1], rect2D[r + 2, c + 2], rect2D[r + 3, c + 3]))
                            return rect2D[r, c].Fill == p1Color ? 1 : 2;

                // Check down-left
                for (int r = 3; r < 6; r++)
                    for (int c = 0; c < 4; c++)
                        if (checkLine(rect2D[r, c], rect2D[r - 1, c + 1], rect2D[r - 2, c + 2], rect2D[r - 3, c + 3]))
                            return rect2D[r, c].Fill == p1Color ? 1 : 2;

                return 0;
            }
        }
    }
}
