using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Xamarin.Database;
using Firebase.Database;
using Firebase.Xamarin.Database.Query;
using Firebase.Auth;
using Plugin.Media;
using Firebase.Storage;
using Android.Gms.Tasks;
using Java.Lang;

namespace Bamboozle
{
	[Activity(Label = "ChatActivity",Theme = "@android:style/Theme.Holo.Light", Icon = "@drawable/icon")]
	public class ChatActivity : Activity,IValueEventListener
	{
		private List<MessageContent> _messageList = new List<MessageContent>();
		private FirebaseClient firebase;
		private ListView lstMessages;
		private Button btnSendMessage;
		private ImageButton btnAddPhoto;
		private EditText txtMessage;
		private string photoUrl="none";
		private string chatKey;
		public int MyResultCode = 1;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Chat);
			firebase = FirebaseService.Client;
			btnSendMessage = FindViewById<Button>(Resource.Id.btnSendMessage);
			//btnAddPhoto = FindViewById<ImageButton>(Resource.Id.btnAddPhoto);
			txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);
			lstMessages = FindViewById<ListView>(Resource.Id.lstMessages);
			chatKey = Intent.GetStringExtra("chatkey") ?? "Data not available"; ;
			FirebaseDatabase.Instance.GetReference("messages").Child(chatKey).AddValueEventListener(this);
			btnSendMessage.Click += delegate
			{
				PostMessage();
			};
			//btnAddPhoto.Click += delegate
			//{
			//	AddPhoto();
			//};
			//if (FirebaseAuth.Instance.CurrentUser == null)
			//	StartActivityForResult(new Android.Content.Intent(this, typeof(Login)), MyResultCode);
		}
		private async void AddPhoto()
		{
			var file = await CrossMedia.Current.PickPhotoAsync();
			var stream = file.GetStream();
			file.Dispose();
			StorageReference storageReference= FirebaseStorage.Instance.GetReference("Images").Child("testphoto");
			storageReference.PutStream(stream);
			photoUrl = FirebaseService.Photo;
		}
		private async void PostMessage()
		{
			var item = await firebase.Child("messages").Child(chatKey).PostAsync(new MessageContent(FirebaseAuth.Instance.CurrentUser.Email, txtMessage.Text,photoUrl));
			photoUrl = "none";
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
			lstMessages.StackFromBottom = true;
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