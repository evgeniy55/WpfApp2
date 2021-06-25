using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;


namespace WpfApp2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var files = ScanFiles(directoryPath.Text);

			var dllMembers = ScanDll(files);

			string strmembers = "";
			foreach (var item in dllMembers)
			{
				strmembers += string.Format(item + "\n");
			}

			textBl.Text = strmembers;
		}

		private IEnumerable<string> ScanFiles(string path)
		{
			return Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
		}

		private IEnumerable<string> ScanDll(IEnumerable<string> files)
		{
			var listNames = new List<string>();

			foreach (var file in files)
			{
				Assembly a = Assembly.LoadFile(file);
				Type[] types = a.GetTypes();
				foreach (Type type in types)
				{
					if (!type.IsPublic)
					{
						continue;
					}
					listNames.Add(type.Name);

					//PropertyInfo[] properties = type.GetProperties(BindingFlags.Public);
					//foreach (var property in properties)
					//{
					//	listNames.Add("- " + property.Name);
					//}

					MethodInfo[] publicMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
					foreach (MethodInfo method in publicMethods)
					{
						listNames.Add("- " + method.Name);
					}

					MethodInfo[] nonpublicMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
					foreach (MethodInfo method in nonpublicMethods)
					{
						listNames.Add("- " + method.Name);
					}
				}
			}

			return listNames;
		}

	}
}
