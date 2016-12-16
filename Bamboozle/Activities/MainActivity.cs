﻿using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Database;
using System;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Plugin.Permissions;
using System.Net.Mail;
using Firebase.Xamarin.Database.Query;

namespace Bamboozle
{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light", Icon = "@drawable/icon")]
	public class MainActivity : Activity, IValueEventListener
	{
		private Dictionary<string, ChatContent> _chatList = new Dictionary<string, ChatContent>();
		private ListView lstChats;
		private Button btnAddChat;
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			if (FirebaseAuth.Instance.CurrentUser == null)
			{
				StartActivity(typeof(LoginActivity));
				Finish();
			}
			SetContentView(Resource.Layout.Main);
			lstChats = FindViewById<ListView>(Resource.Id.lstChats);
			btnAddChat = FindViewById<Button>(Resource.Id.btnAddChat);
			FirebaseDatabase.Instance.GetReference("chats").AddValueEventListener(this);
			lstChats.ItemClick += (sender, e) =>
			{
				var chatActivity = new Intent(this, typeof(ChatActivity));
				chatActivity.PutExtra("chatkey", _chatList.Keys.ToList()[e.Position]);
				StartActivity(chatActivity);
			};
			btnAddChat.Click += delegate
				 {
					 StartActivity(typeof(AddChatActivity));
				 };
			btnAddChat.LongClick += delegate
			{
				FirebaseAuth.Instance.SignOut();
				StartActivity(typeof(LoginActivity));
				Finish();
			};
		}
		private async void DisplayChats()
		{

			_chatList.Clear();

			var items = await FirebaseService.Client.Child("chats")
				.OnceAsync<ChatContent>();

			foreach (var item in items)
				_chatList.Add(item.Key, item.Object);
			ChatAdapter chatAdapter = new ChatAdapter(this, _chatList.Values.ToList());
			lstChats.Adapter = chatAdapter;
		}

		public void OnCancelled(DatabaseError error)
		{
			throw new NotImplementedException();
		}

		public void OnDataChange(DataSnapshot snapshot)
		{
			DisplayChats();
		}
	}
}

