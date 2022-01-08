using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JustAbove
{
    public partial class MainPage : ContentPage
    {
        public List<int> TestInts { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            TestInts = new List<int>() { 1, 2, 3 };
        }
    }
}
