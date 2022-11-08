using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Kalkulacka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string displ = "";
		string operace = "";
		double? cislo1 = null;
		double cislo2 = 0;
		bool smazat = false;
		bool opakovaneRovnaSe = false;

		public MainWindow()
		{
			InitializeComponent();
			AddKeyEvents();
		}

		private void AddKeyEvents()
		{
			KeyDown += (sender, args) =>
			{
				switch (args.Key)
				{
					case Key.NumPad1:
						ZpracujNumKlavesu("1");
						break;
					case Key.NumPad2:
						ZpracujNumKlavesu("2");
						break;
					case Key.NumPad3:
						ZpracujNumKlavesu("3");
						break;
					case Key.NumPad4:
						ZpracujNumKlavesu("4");
						break;
					case Key.NumPad5:
						ZpracujNumKlavesu("5");
						break;
					case Key.NumPad6:
						ZpracujNumKlavesu("6");
						break;
					case Key.NumPad7:
						ZpracujNumKlavesu("7");
						break;
					case Key.NumPad8:
						ZpracujNumKlavesu("8");
						break;
					case Key.NumPad9:
						ZpracujNumKlavesu("9");
						break;
					case Key.NumPad0:
						ZpracujNumKlavesu("0");
						break;
					case Key.Back:
						ZpracujNumKlavesu("<<");
						break;
					case Key.Add:
						ZpracujOperKlavesu("+");
						break;
					case Key.Subtract:
						ZpracujOperKlavesu("-");
						break;
					case Key.Multiply:
						ZpracujOperKlavesu("*");
						break;
					case Key.Divide:
						ZpracujOperKlavesu("/");
						break;
					case Key.Enter:
						ZpracujRovnaSeKlavesu();
						break;
					case Key.Decimal:
						ZpracujNumKlavesu(",");
						break;
					case Key.Escape:
						displ = "";
						displayTextBlock.Text = "0";
						break;
				}
			};
		}

		private void cisloButton_Click(object sender, RoutedEventArgs e)
		{
			string cisloStr = ((Button)sender).Content.ToString();
			ZpracujNumKlavesu(cisloStr);
		}

		private void ZpracujNumKlavesu(string cisloStr)
		{
			opakovaneRovnaSe = false;
			if (smazat)
			{
				displ = "";
				smazat = false;
			}
			if (cisloStr == "0" && displayTextBlock.Text == "0")
			{
				return;
			}
			if (displayTextBlock.Text.Length == 18 && cisloStr != "<<" & cisloStr != "+/-")
			{
				return;
			}

			if (cisloStr == "<<")
			{
				if (displayTextBlock.Text.Contains("E+"))
				{
					return;
				}
				if (displayTextBlock.Text.Contains("E-"))
				{
					return;
				}
				if (displayTextBlock.Text.Length == 2 && displayTextBlock.Text[0] == '-' || displayTextBlock.Text.Length == 3 && displayTextBlock.Text[0] == '-'
					&& displayTextBlock.Text[1] == '0' && displayTextBlock.Text[2] == ',' || displayTextBlock.Text.Length == 1)
				{
					displ = "";
					displayTextBlock.Text = "0";
					return;
				}
				else if (displayTextBlock.Text.Length > 1)
				{
					displayTextBlock.Text = displayTextBlock.Text.Remove(displayTextBlock.Text.Length - 1);
					displ = displayTextBlock.Text;
					return;
				}
				else
				{
					return;
				}
			}
			
			if (displ.Contains(",") && cisloStr == ",")
			{
				return;
			}
			if (cisloStr == "," && displ == "")
			{
				displ = "0";
			}
			if (cisloStr == "+/-")
			{
				displ = (displayTextBlock.Text == "0") ? displ : displayTextBlock.Text;
				displ = (displ == "") ? "" : (-(double.Parse(displ))).ToString();
				displayTextBlock.Text = (displ == "") ? "0" : displ;
				return;
			}
			displ += cisloStr;
			displayTextBlock.Text = displ;

			Keyboard.Focus(rovnitko); // Stisknutí Enteru neopakuje to,
									  // co jsem stiskl pomocí myši, např. číslo
		}

		private void operaceButton_Click(object sender, RoutedEventArgs e)
		{
			operace = ((Button)sender).Content.ToString();
			ZpracujOperKlavesu(operace);
		}

		private void ZpracujOperKlavesu(string oper)
		{
			operace = oper;
			opakovaneRovnaSe = false;
			try
			{
				cislo1 = double.Parse(displayTextBlock.Text, CultureInfo.GetCultureInfo("cs"));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			smazat = true;
		}

		private void rovnaSeButton_Click(object sender, RoutedEventArgs e)
		{
			ZpracujRovnaSeKlavesu();
		}

		private void ZpracujRovnaSeKlavesu()
		{
			if (cislo1 == null)
			{
				return;
			}
			if (opakovaneRovnaSe)
			{
				try
				{
					cislo1 = double.Parse(displayTextBlock.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				try
				{
					cislo2 = double.Parse(displayTextBlock.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

			try
			{
				displayTextBlock.Text = Vypocet.Spocitej(cislo1, cislo2, operace).ToString();
				displayTextBlock.Text = (displayTextBlock.Text == "NaN") ? "0" : displayTextBlock.Text;
				smazat = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			opakovaneRovnaSe = true;
		}

		private void ceButton_Click(object sender, RoutedEventArgs e)
		{
			displ = "";
			displayTextBlock.Text = "0";
		}

		private void cButton_Click(object sender, RoutedEventArgs e)
		{
			displ = "";
			operace = "";
			cislo1 = null;
			cislo2 = 0;
			displayTextBlock.Text = "0";
		}
	}
}
