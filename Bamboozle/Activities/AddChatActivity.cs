using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Xamarin.Database.Query;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Net.Mail;

namespace Bamboozle
{
	[Activity(Label = "CreateChatActivty", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
	public class AddChatActivity : Activity
	{
		private ListView lstUsers;
		private Button btnSearch;
		private EditText etxtSearch;
		private List<string> usersList = new List<string>();
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			string user = new MailAddress(FirebaseAuth.Instance.CurrentUser.Email).User;
			SetContentView(Resource.Layout.AddChat);

			btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
			etxtSearch = FindViewById<EditText>(Resource.Id.etxtSearch);

			btnSearch.Click += delegate
			{
				CreateChat(etxtSearch.Text);
			};

		}

		private async void CreateChat(string title)
		{
			var chat = await FirebaseService.Client.Child("chats").PostAsync(new ChatContent(title));
		}
		private void DisplayUsers()
		{
			usersList.Clear();
			usersList.Add(etxtSearch.Text);
			lstUsers.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, usersList);
		}
	}
}