using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace LoD
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public class Example
		{
			[JsonProperty("title")]
			public string title { get; set; }

			[JsonProperty("href")]
			public string href { get; set; }

			[JsonProperty("ingredients")]
			public string ingredients { get; set; }

			[JsonProperty("thumbnail")]
			public string thumbnail { get; set; }
		}

		public class Example1
		{
			[JsonProperty("title")]
			public string title { get; set; }

			[JsonProperty("version")]
			public string version { get; set; }

			[JsonProperty("href")]
			public string href { get; set; }

			[JsonProperty("results")]
			public List<Example> results { get; set; }
		}

		private void button1_Click(object sender, EventArgs e)
		{

			string result = "";
			string result1 = "";
			int ingcount = 0;
			int i = 0;
			do
			{
				i++;

				string url = "http://www.recipepuppy.com/api/?i=" + textBox1.Text + "&p=" + i;

				var json = new WebClient().DownloadString(url);
				Example1 j1 = JsonConvert.DeserializeObject<Example1>(json);

				if (j1.results.Count() == 0) break;

				foreach (Example ex in j1.results)
				{
					if (result == "" && checkRadio(ex.ingredients.Split(new Char[] { ' ', ',' })))
					{
						result = ex.href;
						result1 = ex.title;
						ingcount = ex.ingredients.Split(new Char[] { ' ', ',' }).Count();
					}
					else if (ex.ingredients.Split(new Char[] { ' ', ',' }).Count() < ingcount && checkRadio(ex.ingredients.Split(new Char[] { ' ', ',' })))
					{
						result = ex.href;
						result1 = ex.title;
						ingcount = ex.ingredients.Split(new Char[] { ' ', ',' }).Count();
					}
				}

			} while (i <= numericUpDown1.Value);

			if (result != "")
			{
				MessageBox.Show("Название блюда - " + result1 + "; ссылка на него - " + result);
			}
			else
			{
				MessageBox.Show("К сожалению, по введенным вами пораметрам не было найдено ни одного рецепта");
			}
		}

		public bool checkRadio(string[] s)
		{
			if (radioButton2.Checked || !radioButton1.Checked) return true;

			string[] a = textBox1.Text.Split(new Char[] { ' ', ',' });

			for (int i = 0; i < s.Count(); i++)
			{
				for (int j = 0; j < a.Count(); j++)
				{
					if (s[i] == a[j]) return false;
				}
			}

			return true;

		}
	}
}
