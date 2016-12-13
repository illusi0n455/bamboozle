using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Xamarin.Database;
using Firebase.Database;
using Firebase.Xamarin.Database.Query;
using Firebase.Auth;

namespace Bamboozle
{
	[Activity(Label = "ChatActivity",Theme = "@android:style/Theme.Holo.Light", Icon = "@drawable/icon")]
	public class ChatActivity : Activity,IValueEventListener
	{
		private List<MessageContent> _messageList = new List<MessageContent>();
		private FirebaseClient firebase;
		private ListView lstMessages;
		private Button btnSendMessage;
		private EditText txtMessage;
		private string chatKey;
		public int MyResultCode = 1;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Chat);
			firebase = FirebaseService.Client;
			btnSendMessage = FindViewById<Button>(Resource.Id.btnSendMessage);
			txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);
			lstMessages = FindViewById<ListView>(Resource.Id.lstMessages);
			chatKey = Intent.GetStringExtra("chatkey") ?? "Data not available"; ;
			FirebaseDatabase.Instance.GetReference("messages").Child(chatKey).AddValueEventListener(this);
			btnSendMessage.Click += delegate
			{
				PostMessage();
			};
			//if (FirebaseAuth.Instance.CurrentUser == null)
			//	StartActivityForResult(new Android.Content.Intent(this, typeof(Login)), MyResultCode);
		}
		private async void PostMessage()
		{
			var item = await firebase.Child("messages").Child(chatKey).PostAsync(new MessageContent(FirebaseAuth.Instance.CurrentUser.Email, txtMessage.Text));
		}
		private async void DisplayChatMessage()
		{
			_messageList.Clear();

			var items = await firebase.Child("messages").Child(chatKey)
				.OnceAsync<MessageContent>();

			foreach (var item in items)
				_messageList.Add(item.Object);
			MessageAdapter chatAdapter = new MessageAdapter(this, _messageList);
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