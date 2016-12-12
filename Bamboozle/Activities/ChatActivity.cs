using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Xamarin.Database;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Xamarin.Database.Query;

namespace Bamboozle
{
	[Activity(Label = "ChatActivity")]
	public class ChatActivity : Activity
	{
		private List<MessageContent> _messageList = new List<MessageContent>();
		private ListView lstMessages;
		private Button btnSendMessage;
		private EditText txtMessage;
		private FirebaseClient firebase;
		private string chatKey;
		public int MyResultCode = 1;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			firebase = new FirebaseClient("https://bamboozle-c07fb.firebaseio.com/");
			btnSendMessage = FindViewById<Button>(Resource.Id.btnSendMessage);
			txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);
			lstMessages = FindViewById<ListView>(Resource.Id.lstMessages);
			chatKey = "testchat";
			FirebaseDatabase.Instance.GetReference("messages").Child(chatKey).AddValueEventListener(this);
			btnSendMessage.Click += delegate
			{
				PostMessage();
			};
			if (FirebaseAuth.Instance.CurrentUser == null)
				StartActivityForResult(new Android.Content.Intent(this, typeof(Login)), MyResultCode);
		}
		private async void PostMessage()
		{
			var item = await firebase.Child("messages").Child(chatKey).PostAsync(new MessageContent("testuser", txtMessage.Text));
		}
		private async void DisplayChatMessage()
		{
			_messageList.Clear();

			var items = await firebase.Child("messages").Child(chatKey)
				.OnceAsync<MessageContent>();

			foreach (var item in items)
				_messageList.Add(item.Object);
			ChatAdapter chatAdapter = new ChatAdapter(this, _messageList);
			lstMessages.Adapter = chatAdapter;
		}

		public void OnCancelled(DatabaseError error)
		{
			throw new NotImplementedException();
		}

		public void OnDataChange(DataSnapshot snapshot)
		{
			DisplayChatMessage();
		}
	}
}