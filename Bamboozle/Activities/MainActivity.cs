using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace Bamboozle
{
	[Activity(Label = "Bamboozle", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private List<MessageContent> _chatList = new List<MessageContent>();
		private ListView lstChats;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			// SetContentView (Resource.Layout.Main);
		}
	}
}

