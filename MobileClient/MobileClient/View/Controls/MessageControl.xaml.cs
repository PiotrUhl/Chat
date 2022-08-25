using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient.Controls {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Message : Xamarin.Forms.Frame {
		public Message() {
			InitializeComponent();
		}
		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			propertyName: "Text",
			returnType: typeof(string),
			declaringType: typeof(Message),
			defaultValue: default(string),
			defaultBindingMode: Xamarin.Forms.BindingMode.OneWay);
        public string Text {
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly BindableProperty SentProperty = BindableProperty.Create(
				propertyName: "Sent",
				returnType: typeof(bool),
				declaringType: typeof(Message),
				defaultValue: false
			);
		public bool Sent {
			get => (bool)GetValue(SentProperty);
			set => SetValue(SentProperty, value);
		}

		protected override void OnPropertyChanged(string propertyName = null) {
			base.OnPropertyChanged(propertyName);

			if (propertyName == TextProperty.PropertyName) {
				MessageText.Text = Text;
			}
			else if (propertyName == SentProperty.PropertyName) {
				if (Sent == true) {
					this.Style = (Style)Application.Current.Resources["MessageFrameSentStyle"];
					MessageText.Style = (Style)Application.Current.Resources["MessageTextSentStyle"];
				}
				else {
					this.Style = (Style)Application.Current.Resources["MessageFrameReceivedStyle"];
					MessageText.Style = (Style)Application.Current.Resources["MessageTextReceivedStyle"];
				}
			}
		}
	}
}