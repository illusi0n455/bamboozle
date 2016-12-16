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
			//lstUsers = FindViewById<ListView>(Resource.Id.lstUsers);
			btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
			etxtSearch = FindViewById<EditText>(Resource.Id.etxtSearch);
			//TODO get chats from user
			btnSearch.Click += delegate
			{
				CreateChat(etxtSearch.Text);
			};
			//lstUsers.ItemClick += (sender, e) =>
			//{
			//	var chatActivity = new Intent(this, typeof(ChatActivity));
			//	chatActivity.PutExtra("chatkey",CreateChat(user).Result);
			//	StartActivity(chatActivity);
			//	Finish();
			//};
		}
		//private async Task<string> CreateChat(string username)
		//{
		//	var chat = await FirebaseService.Client.Child("chats").PostAsync(new ChatContent(username));
		//	var user = await FirebaseService.Client.Child("users").Child(username).Child("chats").Child(chat.Key).PostAsync(new ChatContent(username));
		//	return chat.Key; 
		//}
		private async void CreateChat(string title)
		{
			var chat = await FirebaseService.Client.Child("chats").PostAsync(new ChatContent(title));
		}
		private void DisplayUsers()
		{
			usersList.Clear();
			//var users = await FirebaseService.Client.Child("users").Child(etxtSearch.Text).Child("Name").OnceAsync<string>();
			//foreach (var user in users)
			//{
			//	usersList.Add(user.Key);
			//}
			usersList.Add(etxtSearch.Text);
			lstUsers.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, usersList);
		}
	}
}